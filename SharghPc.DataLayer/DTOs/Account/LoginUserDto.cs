using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Account
{
    public class LoginUserDto : CaptchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Mobile { get; set; }

        [Display(Name = "کلمه ی عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

    public enum LoginUserResult
    {
        Success,
        Null,
        NotFound,
        TryAgain,
        NotActivated
    }
}
