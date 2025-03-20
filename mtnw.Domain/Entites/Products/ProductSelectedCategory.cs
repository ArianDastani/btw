
using SharghPc.DataLayer.Entites.Common;
using SharghPc.DataLayer.Entites.Product;

namespace SharghPc.DataLayer.Entites.Product
{
    public class ProductSelectedCategory : BaseEntity
    {
        #region properties

        public long ProductId { get; set; }

        public long ProductCategoryId { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        public ProductCategory ProductCategory { get; set; }

        #endregion
    }
}
