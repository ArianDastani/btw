using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Carts;

public class CartItem:BaseEntity
{

    #region Prop

    public long ProductId { get; set; }

    public int Count { get; set; }
    public int Price { get; set; }

    public long CartId { get; set; }

    #endregion

    #region Relation
    public virtual Product.Product Product { get; set; }

    public virtual Cart Cart { get; set; }


    #endregion

}