using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using mtnw.Infrastructure.Extentions;
using SharghPc.DataLayer.Context;
using System.Security.Claims;

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

            builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            builder.Services.AddInfrastructureLayer();


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