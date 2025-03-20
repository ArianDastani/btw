using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Products
{
    public class ProductGallery : BaseEntity
    {
        #region Prop
        public long ProductId { get; set; }

        public string ImageName { get; set; }



        #endregion

        #region Relation

        public Product.Product Product { get; set; }

        #endregion
    }
}
