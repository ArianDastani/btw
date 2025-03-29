using SharghPc.DataLayer.Entites.Common;
using SharghPc.DataLayer.Entites.ProductOrder;
using SharghPc.DataLayer.Entites.Products;
using System.ComponentModel.DataAnnotations;


namespace SharghPc.DataLayer.Entites.Product
{
    public class Product : BaseEntity
    {
        #region properties

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "موجودی محصول")]
        public int Inventory { get; set; }

        public string? ViewConter { get; set; }

        [Display(Name = " محصول ویژه")]
        public bool IsSpecial { get; set; }


        #endregion

        #region relations

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }


        #endregion
    }


}
