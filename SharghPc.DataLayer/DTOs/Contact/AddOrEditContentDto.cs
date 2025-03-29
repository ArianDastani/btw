using System.ComponentModel.DataAnnotations;


namespace SharghPc.DataLayer.DTOs.Contact
{
    public class AddOrEditContentDto
    {
        public long? Id { get; set; }

        public string? Title { get; set; }

        public string? Value { get; set; }
        public string? Link { get; set; }

        public long? ContactTypeId { get; set; }
    }

    public enum ContactTypeId
    {
        [Display(Name = "تلفن همراه")]
        Mobile = 1,
        [Display(Name = "تلفن")]
        Phone = 2,
        [Display(Name = "ایمیل")]
        Email = 3,
        [Display(Name = "متن فوتر")]
        FooterText = 4,
        [Display(Name = "متن هدر")]
        HeaderText = 5,
        [Display(Name = "متن کپی رایت")]
        CopyRight = 6,
        [Display(Name = "آدرس نقشه")]
        MapScript = 7,
        [Display(Name = "آدرس")]
        Address = 8,
        [Display(Name = "درباره ما")]
        AboutUs = 9,
        [Display(Name = "پیوند های سریع")]
        QuickLinks = 10,
        [Display(Name = "پیوندهای برتر")]
        TopLinks = 11,
        [Display(Name = "ساعات کاری")]
        WorkTime = 12,
        [Display(Name = "مرکز پشتیبانی")]
        Support = 13,




    }
}
