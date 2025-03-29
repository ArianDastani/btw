

using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Carts;
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Finances
{
    public class RequestPay : BaseEntity
    {
        #region Properties

        public Guid Guid { get; set; }
        public long? UserId { get; set; }
        public long? CartId { get; set; }
        public int Amount { get; set; }
        public bool IsPay { get; set; }
        public DateTime? PayDate { get; set; }
        public string? Authority { get; set; }
        public long RefId { get; set; } = 0;
        public string? PayImage { get; set; }
        #endregion

        #region Relation

        public virtual User User { get; set; }
        public virtual Cart Cart { get; set; }

        #endregion
    }
}
