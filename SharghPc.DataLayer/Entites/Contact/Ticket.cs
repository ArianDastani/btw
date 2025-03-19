using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharghPc.DataLayer.Entites.Contact
{
    public class Ticket : BaseEntity
    {
        public long OwnerId { get; set; }

        [Display(Name ="عنوان")]
        [Required(ErrorMessage ="لطفا {} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "بخش ارتباطی")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "وضعیت تیکت")]
        public TicketState TicketState { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }
        public User Owner { get; set; }


        public bool IsReadByOwner { get; set; }
        public bool IsReadByAdmin { get; set; }
    }

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        suppuert,

        [Display(Name = "آموزشی")]
        Academic,

        [Display(Name = "فنی")]
        Technical
    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]
        low,

        [Display(Name = "متوسط")]
        medium,

        [Display(Name = "زیاد")]
        Hight
    }

    public enum TicketState
    {
        [Display(Name = "در حال بررسی")]
        pending,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }
}
