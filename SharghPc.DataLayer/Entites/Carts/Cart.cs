using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;
using SharghPc.DataLayer.Entites.Finances;

namespace SharghPc.DataLayer.Entites.Carts
{
    public class Cart:BaseEntity
    {
        #region prop

        public long? UserId { get; set; }
        public long? RequestPayId { get; set; }
        public Guid BrowserId { get; set; }
        public bool Finished { get; set; }

        #endregion

        #region Relation

        public virtual User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        public ICollection<RequestPay> RequestPays { get; set; }

        #endregion

    }
}
