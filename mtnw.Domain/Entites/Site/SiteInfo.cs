using System.ComponentModel.DataAnnotations;
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Site
{
    public class SiteInfo:BaseEntity
    {
        #region properties

        [Display(Name = "تلفن همراه")]
        public string? Mobile { get; set; }

        [Display(Name = "تلفن")]
        public string? Phone { get; set; }

        [Display(Name = "آدرس ایمیل")]
        public string? Email { get; set; }

        [Display(Name = "متن فوتر")]
        public string? FooterText { get; set; }

        //[Display(Name = "متن هدر")]
        //public string? HeaderText { get; set; }

        [Display(Name = "متن کپی رایت")]
        public string? CopyRight { get; set; }

        [Display(Name = "آدرس نقشه")]
        public string? MapScript { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "درباره ما")]
        public string? AboutUs { get; set; }


        #endregion
    }
}
