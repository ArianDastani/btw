using MarketPlace.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SharghPc.Application.Services;
using SharghPc.Application.Services.Cart;
using SharghPc.Application.Services.Category;
using SharghPc.Application.Services.Contact;
using SharghPc.Application.Services.image;
using SharghPc.Application.Services.Index;
using SharghPc.Application.Services.Order;
using SharghPc.Application.Services.Product;
using SharghPc.Application.Services.RequestPay;
using SharghPc.Application.Services.Security;
using SharghPc.Application.Services.Shipment;
using SharghPc.Application.Services.Site;
using SharghPc.Application.Services.Sms;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.Repository;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace mtnw.Infrastructure.Extentions
{
    public static class IServicesCollectionsExtentions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<HtmlEncoder>(HtmlEncoder.Create(new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddScoped<IUserServices, UserServices>()
                .AddScoped<IPasswordHelper, PasswordHelper>()
                .AddScoped<IContactServices, ContactServices>()
                .AddScoped<ISiteInfoServices, SIteInfoServices>()
                .AddScoped<ISmsServices, SmsServices>()
                .AddScoped<IProductServices, ProductServices>()
                .AddScoped<IOrderServices, OrderServices>()
                .AddScoped<ICategoryServices, CategoryServices>()
                .AddScoped<ICartServices, CartServices>()
                .AddScoped<IRequestPayServices, RequestPayServices>()
                .AddScoped<IIndexServices, IndexServices>()
                .AddScoped<IShipmentServices, ShipmentServices>()
                .AddTransient<IImgaeService, ImgaeService>()
                .AddTransient<Itestservices, testservices>();
        }
    }
}
