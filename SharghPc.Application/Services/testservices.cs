using SharghPc.Application.Services.Sms;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.DTOs.Account;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services
{

    public interface Itestservices
    {
        public Task<ResultLoginAndRegisterWithMobileDto> SendSmsForLoginAndRegisterWithMobile(string mobile);
        public Task<LoginUserResult> LoginAndRegisterWithMobile(string mobile, string VerifyCode);

        public void Get();
    }


    //--=-=-=-=-=-==-=-=-=--=-==--==-=-=--==-=-=--=-=-=-=-=-==-=-=-=-=-=--=


    public class testservices : Itestservices
    {
        private ISmsServices _smsServices;
        private IGenericRepository<User> _userRepository;
        private IUserServices _userService;
        private IGenericRepository<DataLayer.Entites.Product.Product> _product;

        public testservices(ISmsServices smsServices, IGenericRepository<User> userRepository, IUserServices userService, IGenericRepository<DataLayer.Entites.Product.Product> product)
        {
            _smsServices = smsServices;
            _userRepository = userRepository;
            _userService = userService;
            _product = product;
        }

        public async Task<ResultLoginAndRegisterWithMobileDto> SendSmsForLoginAndRegisterWithMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return null;

            if (mobile.Trim().Length < 11) return null;

            var activeCode = new Random().Next(88738, 999939).ToString();

            var res = await _smsServices.SendVerificationSms(mobile, activeCode);

            if (res == false) return null;

            var user = await _userService.GetUserByMobile(mobile);

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
                    IsMobileActive = false,
                    RolesId = 4,
                    LastUpdateDate = DateTime.Now,

                };

                await _userRepository.AddEntity(Currentuser);
                await _userRepository.SaveChanges();
            }
            else
            {
                user.MobileActiveCode = activeCode;
                _userRepository.EditEntity(user);
                await _userRepository.SaveChanges();
            }



            return new ResultLoginAndRegisterWithMobileDto()
            {
                Mobile = mobile
            };
        }

        public async Task<LoginUserResult> LoginAndRegisterWithMobile(string mobile, string VerifyCode)
        {
            if (string.IsNullOrWhiteSpace(VerifyCode) || string.IsNullOrWhiteSpace(mobile)) return LoginUserResult.Null;

            var user = await _userService.GetUserByMobile(mobile);

            if (user == null) return LoginUserResult.NotFound;

            if (user.MobileActiveCode != VerifyCode)
            {
                return LoginUserResult.TryAgain;
            }

            return LoginUserResult.Success;

        }

        public void Get()
        {
            //var res = _product.GetQuery().ToList();

            //   var res2= from s in res

            //    where int.Parse(s.ViewConter)>10
            //       join pf in res o
            //                              select s;
        }
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
