using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Product
{
    public class FilterProductDto
    {
        public string? Title { get; set; }

        public string? Category { get; set; }
        public List<Entites.Product.Product>? Products { get; set; }

        public List<long>? SelectProductCategories { get; set; }
        public FilterProductOrderBy? FilterProductOrderBy { get; set; }

        public FilterProductDto SetProduct(List<Entites.Product.Product> product)
        {
            this.Products = product;
            return this;
        }
    }

    public enum FilterProductOrderBy
    {
        [Display(Name = "جدید ترین")]
        CreateDate_Des,
        [Display(Name = "قدیمی ترین")]
        CreateDate_Asc,
        [Display(Name = "گران ترین")]
        Price_Des,
        [Display(Name = "ارزان ترین")]
        Price_Asc
    }
}
