using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.Entites.ProductOrder
{
    public enum OrderState
    {
        [Display(Name = "در حال پردازش")]
        Processing = 0,
        [Display(Name = "لغو شده")]
        Canceled = 1,
        [Display(Name = "تحویل شده")]
        Delivered = 2,
        [Display(Name = "در انتظار پرداخت")]
        AwaitingPayment = 3,

    }

}
