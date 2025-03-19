using SharghPc.DataLayer.Entites.Carts;
using SharghPc.DataLayer.Entites.Finances;

namespace SharghPc.Web.Models
{
    public class InfoVerifyViewModel
    {
        public Shipment? Shipment { get; set; }
        public Cart? Cart { get; set; }
    }
}
