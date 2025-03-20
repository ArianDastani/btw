using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;
using SharghPc.DataLayer.Entites.Finances;

namespace SharghPc.DataLayer.Entites.ProductOrder
{
    public class Order : BaseEntity
    {
        #region prop

        public long? UserId { get; set; }
        public long? ShipmentId { get; set; }
        public long RequestPayId { get; set; }
        public OrderState OrderState { get; set; }
        
        #endregion

        #region Relation
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Shipment Shipment { get; set; }
        public virtual User User { get; set; }
        public virtual RequestPay RequestPay { get; set; }


        #endregion
    }
}
