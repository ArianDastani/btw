﻿using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Account
{
    public class ActiveMobileDto : CaptchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; } = null!;

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string MobileActiveCode { get; set; } = null!;
    }
}
