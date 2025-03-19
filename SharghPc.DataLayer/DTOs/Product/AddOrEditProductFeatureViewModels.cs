using SharghPc.DataLayer.DTOs.Product;

namespace SharghPc.Web.Models
{
    public class AddOrEditProductFeatureViewModels
    {
        public List<ProductFeatureDto>? ProductFeatures { get; set; }
        public ProductFeatureDto? ProductFeatureDto { get; set; }

        public long productId { get; set; }
    }
}
