﻿using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;
using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.Entites.Contact
{
    public class TicketMessage : BaseEntity
    {
        public long? TicketId { get; set; }
        public long SenderId { get; set; }

        [Display(Name = "متن تیکت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        public string Text { get; set; } = null!;



        public Ticket Ticket { get; set; }
        public User Sender { get; set; }
    }
}
