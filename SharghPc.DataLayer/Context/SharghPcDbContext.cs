using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Carts;
using SharghPc.DataLayer.Entites.Contact;
using SharghPc.DataLayer.Entites.Finances;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Entites.ProductOrder;
using SharghPc.DataLayer.Entites.Products;
using SharghPc.DataLayer.Entites.Roles;
using SharghPc.DataLayer.Entites.Site;

namespace SharghPc.DataLayer.Context
{
    public class SharghPcDbContext:DbContext
    {
        public SharghPcDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        public DbSet<SiteInfo> SiteInfo { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<RequestPay> RequestPays { get; set; }

        public DbSet<IndexCategoryArea> IndexCategoryArea { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<ContactType> ContactType { get; set; }

        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Numbers> Numbers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var item in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            //{
            //    item.DeleteBehavior = DeleteBehavior.Cascade;
            //}


            base.OnModelCreating(modelBuilder);
        }
    }
}
