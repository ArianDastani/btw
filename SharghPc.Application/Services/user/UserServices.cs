using System.ComponentModel.DataAnnotations;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Account;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Repository;
using System.Security.Cryptography.X509Certificates;
using SharghPc.Application.Services.Sms;
using SharghPc.DataLayer.Entites.Roles;
using static SharghPc.DataLayer.DTOs.Account.RegisterUserDTO;

namespace SharghPc.Application.Services.user
{
    public class UserServices : IUserServices
    {
        private IGenericRepository<User> _userRepo;
        private IPasswordHelper _passwordHelper;
        private ISmsServices _smsServices;
        private IGenericRepository<Roles> _rolesRepo;

        public UserServices(IGenericRepository<User> userRepo, IPasswordHelper passwordHelper, ISmsServices smsServices, IGenericRepository<Roles> rolesRepo)
        {
            _userRepo = userRepo;
            _passwordHelper = passwordHelper;
            _smsServices = smsServices;
            _rolesRepo = rolesRepo;
        }
        public async ValueTask DisposeAsync()
        {
            await _userRepo.DisposeAsync();
        }

        public async Task<string> SendCodeForLoginUser(string mobileNumber)
        {
            string VerifecationCode = new Random().Next(88738, 999939).ToString();

            var res = await _smsServices
                .SendVerificationSms(mobileNumber,VerifecationCode);

            if (res)
            {
                return VerifecationCode;
            }
            else
            {
                return null;
            }

            //if (res)
            //{
            //    var user = new User
            //    {
            //        CreateDate = DateTime.Now,
            //        Address = "",
            //        Avatar = "",
            //        Email = "",
            //        EmailActiveCode = "",
            //        FirstName = "کاربر",
            //        LastName = "کامپیوتر شرق",
            //        IsMobileActive = true,
            //        MobileActiveCode = VerifecationCode,
            //        IsDelete = false,
            //        IsBlocked = false,
            //        IsEmailActive = false,
            //        Mobile = mobileNumber,


            //    };
            //}
            //else
            //{
            //    return false;
            //}

            //return false;


        }

        //public async Task<LoginUserResult> GetCodeForLogin(string verificationCode)
        //{
            
        //}

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepo.GetQuery().AsQueryable()
                .Include(x=>x.Roles)
                .SingleOrDefaultAsync(x => x.Mobile == mobile);

        }

        public async Task<ForgetPassResult> RecoveryUserPassword(ForgetPasswordDto forgetPass)
        {
            var user = await _userRepo.GetQuery().SingleOrDefaultAsync(u => u.Mobile == forgetPass.Mobile);

            if (user == null)
            {
                return ForgetPassResult.NotFound;
            }

            var newpass = new Random().Next(10000, 7878788).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newpass);
            _userRepo.EditEntity(user);

            await _smsServices.SendUserPasswordSms(user.Mobile, newpass);

            await _userRepo.SaveChanges();
            return ForgetPassResult.Success;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto, long UserId)
        {

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                return false;
            }

            var user = await _userRepo.GetEntityById(UserId);

            if (user == null)
            {
                return false;
            }

            var newpass = _passwordHelper.EncodePasswordMd5(changePasswordDto.NewPassword);

            if (user.Password != newpass)
            {

                user.Password = newpass;
                _userRepo.EditEntity(user);
                await _userRepo.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> ActiveMobile(ActiveMobileDto activeMobileDto)
        {
            var user = await _userRepo.GetQuery()
                .SingleOrDefaultAsync(x => x.Mobile == activeMobileDto.Mobile);

            if (user != null)
            {
                if (user.MobileActiveCode == activeMobileDto.MobileActiveCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(88738, 999939).ToString();
                    await _userRepo.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<EditUserProfileDto> GetUserProfile(long userId)
        {
            var user = await _userRepo.GetEntityById(userId);
            if (user == null)
            {
                return null;
            }

            return new EditUserProfileDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };


        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto UserProfile,long UserId)
        {
            try
            {
                var user = await _userRepo.GetEntityById(UserId);
                if (user == null)
                {
                    return EditUserProfileResult.NotFound;
                }

                if ((bool)user.IsBlocked)
                {
                    return EditUserProfileResult.Blocked;
                }

                if (!user.IsMobileActive)
                {
                    return EditUserProfileResult.IsNotActiveMobile;
                }

                user.Email = UserProfile.Email;
                user.FirstName = UserProfile.FirstName;
                user.LastName = UserProfile.LastName;
                user.LastUpdateDate = DateTime.Now;

                _userRepo.EditEntity(user);
                await _userRepo.SaveChanges();
                return EditUserProfileResult.Success;
            }
            catch
            {
                return EditUserProfileResult.Error;
            }

        }

        public async Task<List<User>> GetAllUserForAdmin()
        {
            var users = await _userRepo.GetQuery()
                .Include(x=>x.Roles)
                .Where(x => x.IsDelete == false)
                .ToListAsync();

            return users;
        }

        public async Task<AddOrEditUserDto> LoadUserForAdmin(long userId)
        {
            if (userId == 0 || userId == null)
            {
                return new AddOrEditUserDto() { UserId = 0 };
            }

            var user = await _userRepo.GetQuery()
                .Include(x=>x.Roles)
                .FirstOrDefaultAsync(x=>x.Id==userId);

            if (user == null)
            {
                return null;
            }

            return new AddOrEditUserDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = user.Password,

                Role = user.Roles
            };
        }

        public async Task<bool> AddOrEditUserForAdmin(AddOrEditUserDto userDto)
        {
            try
            {
                if (userDto.UserId == 0)
                {
                    var role = await _rolesRepo.GetEntityById(userDto.RoleId.Value);

                    User user = new User()
                    {
                        CreateDate = DateTime.Now,
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Mobile = userDto.Mobile,
                        MobileActiveCode = new Random().Next(88738, 999939).ToString(),
                        EmailActiveCode = Guid.NewGuid().ToString("N"),
                        Password = _passwordHelper.EncodePasswordMd5(userDto.Password),
                        Email = "1",
                        Avatar = "1",
                        IsBlocked = false,
                        IsDelete = false,
                        IsEmailActive = false,
                        IsMobileActive = false,
                        Roles = role

                    };

                    await _userRepo.AddEntity(user);
                    await _userRepo.SaveChanges();
                    return true;
                }
                else
                {
                    var user = await _userRepo.GetEntityById(userDto.UserId.Value);

                    if (user == null) return false;

                    user.RolesId=userDto.RoleId.Value;
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.Mobile = userDto.Mobile;
                    
                    _userRepo.EditEntity(user);
                    await _userRepo.SaveChanges();

                    return true;
                }
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> RemoveUser(long userId)
        {
            if (userId == 0 || userId == null) return false;

            var user=await  _userRepo.GetEntityById(userId);

            if (user == null) return false;

            user.IsDelete = true;

            _userRepo.EditEntity(user);
            await _userRepo.SaveChanges();
            return true;

        }

        public async Task<bool> ActiveMobileUserForAdmin(long userId)
        {
            if (userId == 0 || userId == null) return false;

            var user = await _userRepo.GetEntityById(userId);

            if (user == null) return false;

            if (user.IsMobileActive == true)
            {
                user.IsMobileActive = false;
                _userRepo.EditEntity(user);
                await _userRepo.SaveChanges();
                return true;
            }

            user.IsMobileActive = true;
            _userRepo.EditEntity(user);
            await _userRepo.SaveChanges();
            return true;



        }

        public async Task<bool> IsExitsMobile(string number)
        {
            return await _userRepo.GetQuery().AsQueryable().AnyAsync(x => x.Mobile == number);
        }

        public async Task<LoginUserResult> LoginUser(LoginUserDto loginUserDto)
        {
            User? user = await _userRepo.GetQuery().FirstOrDefaultAsync(x => x.Mobile == loginUserDto.Mobile);

            if (user == null)
            {
                return LoginUserResult.NotFound;
            }

            if (!user.IsMobileActive)
            {
                return LoginUserResult.NotActivated;
            }

            if (user.Password != _passwordHelper.EncodePasswordMd5(loginUserDto.Password))
            {
                return LoginUserResult.NotFound;
            }

            return LoginUserResult.Success;
        }

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            if (await IsExitsMobile(registerUserDTO.Mobile))
            {
                return RegisterUserResult.MobileExits;
            }

            try
            {
                User user = new User
                {
                    CreateDate = DateTime.Now,
                    FirstName = registerUserDTO.FirstName,
                    LastName = registerUserDTO.LastName,
                    Mobile = registerUserDTO.Mobile,
                    MobileActiveCode = new Random().Next(88738, 999939).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N"),
                    Password = _passwordHelper.EncodePasswordMd5(registerUserDTO.Password),
                    Email = "1",
                    Avatar = "1",
                    IsBlocked = false,
                    IsDelete = false,
                    IsEmailActive = false,
                    IsMobileActive = false,
                    RolesId = 4,
                };

                await _userRepo.AddEntity(user);
                await _userRepo.SaveChanges();
                var res = await _smsServices.SendVerificationSms(user.Mobile, user.MobileActiveCode);

                return RegisterUserResult.Success;
            }
            catch (Exception)
            {

                return RegisterUserResult.Errore;

            }



            return RegisterUserResult.Errore;
        }


        public async Task<ResultLoginAndRegisterWithMobileDto> SendSmsForLoginAndRegisterWithMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return null;

            if (mobile.Trim().Length < 11) return null;

            var activeCode = new Random().Next(88738, 999939).ToString();

            var res = await _smsServices.SendVerificationSms(mobile, activeCode);

            //if (res == false) return null;

            var user = await GetUserByMobile(mobile);

            if (user == null)
            {
                User Currentuser = new User
                {
                    CreateDate = DateTime.Now,
                    FirstName = null,
                    LastName = null,
                    Mobile = mobile,
                    MobileActiveCode = activeCode,
                    EmailActiveCode = Guid.NewGuid().ToString("N"),
                    Password = null,
                    Email = "1",
                    Avatar = "1",
                    IsBlocked = false,
                    IsDelete = false,
                    IsEmailActive = false,
                    IsMobileActive = true,
                    RolesId = 4,
                    LastUpdateDate = DateTime.Now,

                };

                await _userRepo.AddEntity(Currentuser);
                await _userRepo.SaveChanges();
            }
            else
            {
                user.MobileActiveCode = activeCode;
                _userRepo.EditEntity(user);
                await _userRepo.SaveChanges();
            }



            return new ResultLoginAndRegisterWithMobileDto()
            {
                Mobile = mobile
            };
        }

        public async Task<LoginUserResult> LoginAndRegisterWithMobile(string mobile, string VerifyCode)
        {
            if (string.IsNullOrWhiteSpace(VerifyCode) || string.IsNullOrWhiteSpace(mobile)) return LoginUserResult.Null;

            var user = await GetUserByMobile(mobile);

            if (user == null) return LoginUserResult.NotFound;

            if (user.MobileActiveCode != VerifyCode)
            {
                return LoginUserResult.TryAgain;
            }

            return LoginUserResult.Success;

        }
    }
}
