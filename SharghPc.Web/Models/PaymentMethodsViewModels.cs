using SharghPc.DataLayer.Entites.Carts;
using SharghPc.DataLayer.Entites.Finances;

namespace SharghPc.Web.Models
{
    public class PaymentMethodsViewModels
    {
        public Cart? Cart { get; set; }
        public Shipment? Shipment { get; set; }

    }
}
