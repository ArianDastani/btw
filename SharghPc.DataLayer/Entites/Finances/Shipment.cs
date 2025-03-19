using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;
using SharghPc.DataLayer.Entites.ProductOrder;
using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.Entites.Finances
{
    public class Shipment : BaseEntity
    {
        [Display(Name = "آدرس کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FullAddress { get; set; } = null!;


        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string City { get; set; } = null!;


        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string State { get; set; } = null!;

        [Display(Name = "کد پستی")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ZipCode { get; set; }


        [Display(Name = "نام تحویل گیرنده")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? RecipientLastName { get; set; }



        [Display(Name = "نام خانوادگی تحویل گیرنده")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? RecipientFirstName { get; set; }


        public long? UserId { get; set; }


        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
