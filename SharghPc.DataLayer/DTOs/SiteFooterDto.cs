
using System.ComponentModel.DataAnnotations;


namespace SharghPc.DataLayer.DTOs
{
    public class SiteFooterDto
    {
        [Display(Name = "تلفن همراه")]
        public string? Mobile { get; set; }

        [Display(Name = "متن فوتر")]
        public string? FooterText { get; set; }

        [Display(Name = "متن کپی رایت")]
        public string? CopyRight { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        public string? About { get; set; }

    }
}
