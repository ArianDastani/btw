
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;
using SharghPc.DataLayer.Entites.Site;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Sms
{
    public class SmsServices : ISmsServices
    {

        private IGenericRepository<Numbers> _numbersRepository;

        public SmsServices(IGenericRepository<Numbers> numbersRepository)
        {
            _numbersRepository = numbersRepository;
        }
        public async Task<bool> SendVerificationSms(string mobile, string activeCode)
        {
            try
            {
                string apikey = "qqzTgd0BLc7IAEIs2qteOd7A3sXBmCctV7bbWrLq7wOmeHewbywXhaWYZgSmwVnJ";

                SmsIr smsIr = new SmsIr(apikey);

                string Mobile = mobile;
                int templateId = 506132;

                VerifySendParameter[] verifySendParameters =
                {
                    new VerifySendParameter("CODE", activeCode),
                };

                var response = await smsIr.VerifySendAsync(mobile, templateId, verifySendParameters);

                VerifySendResult sendResult = response.Data;
                int messageId = sendResult.MessageId;
                decimal cost = sendResult.Cost;

                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SendUserPasswordSms(string mobile, string Newpassword)
        {
            try
            {
                string apikey = "qqzTgd0BLc7IAEIs2qteOd7A3sXBmCctV7bbWrLq7wOmeHewbywXhaWYZgSmwVnJ";

                SmsIr smsIr = new SmsIr(apikey);

                string Mobile = mobile;
                int templateId = 506132;

                VerifySendParameter[] verifySendParameters =
                {
                    new VerifySendParameter("CODE", Newpassword),
                };

                var response = await smsIr.VerifySendAsync(mobile, templateId, verifySendParameters);

                VerifySendResult sendResult = response.Data;
                int messageId = sendResult.MessageId;
                decimal cost = sendResult.Cost;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public async Task<bool> SendDiscountCode(string mobile,string Discount, string discountCode)
        //{
        //    if (string.IsNullOrEmpty(mobile) || string.IsNullOrWhiteSpace(Discount) ||
        //        string.IsNullOrWhiteSpace(discountCode)) return false;


        //}

        public async Task<bool> SaveNumber(string mobile, string Dsicountcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mobile)) return false;

                Numbers number = new Numbers()
                {
                    MobileNumbers = mobile,
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    DiscountCode = Dsicountcode
                };

                await _numbersRepository.AddEntity(number);
                await _numbersRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SendMessageForMobile(string mobile, string Text)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return false;

            try
            {
                string apikey = "qqzTgd0BLc7IAEIs2qteOd7A3sXBmCctV7bbWrLq7wOmeHewbywXhaWYZgSmwVnJ";

                SmsIr smsIr = new SmsIr(apikey);

                string Mobile = mobile;
                int templateId = 881693;

                VerifySendParameter[] verifySendParameters =
                {
                    new VerifySendParameter("URL", "products"),
                };

                var response = await smsIr.VerifySendAsync(mobile, templateId, verifySendParameters);

                if (response.Status == 1)
                {
                    var saveMobile = await SaveNumber(mobile, DateTime.Now.Ticks.ToString());

                }

                VerifySendResult sendResult = response.Data;
                int messageId = sendResult.MessageId;
                decimal cost = sendResult.Cost;

                return true;
            }
            catch
            {
                return false;
            }


        }


    }
}
