using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Entites.Products;
using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Product
{
    public class ProductDetailDto
    {
        #region properties

        public long Id { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Description { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "موجودی محصول")]
        public int? Inventory { get; set; }

        public List<Entites.Product.Product>? RelatedProduct { get; set; }

        #endregion

        #region relations

        public List<ProductCategory>? ProductCategories { get; set; }
        public List<ProductFeature>? ProductFeatures { get; set; }
        public List<ProductGallery>? ProductGalleries { get; set; }



        #endregion
    }
}
