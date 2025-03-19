using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SharghPc.Application.Services.Contact;
using SharghPc.Application.Services.Security;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.Context;
using SharghPc.DataLayer.Repository;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using SharghPc.Application.Services.Cart;
using SharghPc.Application.Services.Category;
using SharghPc.Application.Services.Index;
using SharghPc.Application.Services.Order;
using SharghPc.Application.Services.Site;
using SharghPc.Application.Services.Sms;
using SharghPc.Application.Services.Product;
using SharghPc.Application.Services.RequestPay;
using System.Security.Claims;
using SharghPc.Application.Services.Shipment;
using SharghPc.Application.Services.image;
using SharghPc.Application.Services;

namespace SharghPc.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region IoC

            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
            builder.Services.AddScoped<IContactServices, ContactServices>();
            builder.Services.AddScoped<ISiteInfoServices, SIteInfoServices>();
            builder.Services.AddScoped<ISmsServices, SmsServices>();
            builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICartServices, CartServices>();
            builder.Services.AddScoped<IRequestPayServices, RequestPayServices>();
            builder.Services.AddScoped<IIndexServices, IndexServices>();
            builder.Services.AddScoped<IShipmentServices, ShipmentServices>();
            builder.Services.AddTransient<IImgaeService, ImgaeService>();
            builder.Services.AddTransient<Itestservices, testservices>();




            #endregion

            #region Context

            builder.Services.AddDbContext<SharghPcDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("db"));
            });

            #endregion

            #region Authentication

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(op =>
            {
                op.LoginPath = "/Account/Login";
                op.LogoutPath = "/Account/logout";
                op.ExpireTimeSpan = TimeSpan.FromDays(10);
            });


            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));

                option.AddPolicy("CustomerRole",
                    policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
            });
            #endregion


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Errors

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Request.Path = "/404";
                    await next();
                }

            });

                #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
	            name: "default",
	            pattern: "{controller=Home}/{action=Index}/{id?}",
	            defaults: new { area = "admin" });

            app.Run();
        }
    }
}