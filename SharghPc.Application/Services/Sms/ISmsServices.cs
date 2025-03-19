
namespace SharghPc.Application.Services.Sms
{
    public interface ISmsServices
    {
        Task<bool> SendVerificationSms(string mobile,string activeCode);
        Task<bool> SendUserPasswordSms(string mobile, string Newpassword);
        public Task<bool> SendMessageForMobile(string mobile, string Text);

    }
}
