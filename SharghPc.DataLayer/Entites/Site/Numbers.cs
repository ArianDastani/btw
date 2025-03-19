using System.ComponentModel.DataAnnotations;
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Site
{
    public class Numbers : BaseEntity
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا شماره موبایل را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumbers { get; set; } = null!;

        [Display(Name = "کد تخفیف")]
        [DataType(DataType.Text)]
        public string? DiscountCode { get; set; }
    }
}
