using SharghPc.DataLayer.DTOs.Product;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Entites.Site;

namespace SharghPc.Web.Models
{
    public class IndexViewModel
    {
        public List<IndexCategoryArea>? CategoryAreas { get; set; }
        public List<ProductForIndexDto>? ProductsForIndex { get; set; }
        public List<ProductForIndexDto>? ProductsStorageForIndex { get; set; }
        public List<ProductForIndexDto>? ProductsCableForIndex { get; set; }

        public Product? SpecialProduct { get; set; }
    }
}
