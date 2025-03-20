
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.ProductOrder
{
    public class OrderDetail:BaseEntity
    {
        #region Prop

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public long Count { get; set; }

        public int ProductPrice { get; set; }


        #endregion

        #region Relation

        public Order Order { get; set; }
        public Product.Product Product { get; set; }
        #endregion
    }
}
