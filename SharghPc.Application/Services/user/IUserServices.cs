using SharghPc.DataLayer.DTOs.Account;
using SharghPc.DataLayer.Entites.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharghPc.DataLayer.Entites.Roles;
using static SharghPc.DataLayer.DTOs.Account.RegisterUserDTO;

namespace SharghPc.Application.Services.user
{
    public interface IUserServices:IAsyncDisposable
    {
        Task<RegisterUserResult> RegisterUser(RegisterUserDTO registerUserDTO);
        Task<bool> IsExitsMobile(string number);
        Task<LoginUserResult> LoginUser(LoginUserDto loginUserDto);
        public Task<string> SendCodeForLoginUser(string mobileNumber);
        Task<User> GetUserByMobile(string mobile);
        Task<ForgetPassResult> RecoveryUserPassword(ForgetPasswordDto forgetPass);
        Task<bool> ChangePassword(ChangePasswordDto changePasswordDto,long UserId);
        Task<bool> ActiveMobile(ActiveMobileDto  activeMobileDto);
        Task<EditUserProfileDto> GetUserProfile(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto UserProfile, long UserId);
        Task<List<User>> GetAllUserForAdmin();
        Task<AddOrEditUserDto> LoadUserForAdmin(long userId);
        Task<bool> AddOrEditUserForAdmin(AddOrEditUserDto userDto);
        Task<bool> RemoveUser(long userId);
        public Task<bool> ActiveMobileUserForAdmin(long userId);
        public Task<ResultLoginAndRegisterWithMobileDto> SendSmsForLoginAndRegisterWithMobile(string mobile);
        public Task<LoginUserResult> LoginAndRegisterWithMobile(string mobile, string VerifyCode);
    }

    public class AddOrEditUserDto
    {
        public long? UserId { get; set; }
        public long? RoleId { get; set; }


        [Display(Name = "ایمیل")]
        public string? Email { get; set; }
        public string? EmailActiveCode { get; set; }

        [Display(Name = "ایمیل فعال / غیرفعال")]
        public bool IsEmailActive { get; set; }

        [Display(Name = "تلفن همراه")]
        public string? Mobile { get; set; }

        public string? MobileActiveCode { get; set; }

        [Display(Name = "موبایل فعال / غیرفعال")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "کلمه ی عبور")]
        public string? Password { get; set; }

        [Display(Name = "نام")]
        public string? FirstName { get; set; } 

        [Display(Name = "نام خانوادگی")]
        public string? LastName { get; set; }

        [Display(Name = "تصویر آواتار")]
        public string? Avatar { get; set; }

        [Display(Name = "بلاک شده / نشده")]
        public bool? IsBlocked { get; set; }

        public Roles Role { get; set; }

    }

    public class ResultLoginAndRegisterWithMobileDto
    {
        public string Mobile { get; set; }
    }

    public class RequestLoginVerifyDto
    {
        public string Mobile { get; set; }
    }
}
