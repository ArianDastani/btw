using SharghPc.DataLayer.Entites.Contact;
using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Contact
{
    public class AddTicketDTO
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "بخش ارتباطی")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [Display(Name = "متن تیکت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        public string Text { get; set; } = null!;
    }

    public enum AddTicketResult
    {
        Success,
        Error
    }
}
