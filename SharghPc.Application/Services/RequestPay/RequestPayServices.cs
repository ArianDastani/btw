using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharghPc.Application.Services.Cart;
using SharghPc.DataLayer.DTOs.RequestPay;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.RequestPay
{
    public class RequestPayServices: IRequestPayServices
    {
        #region Ctorf

        private IGenericRepository<DataLayer.Entites.Finances.RequestPay> _financesRepository;
        private ICartServices _cartServices;


        public RequestPayServices(IGenericRepository<DataLayer.Entites.Finances.RequestPay> financesRepository, ICartServices cartServices)
        {
            _financesRepository = financesRepository;
            _cartServices = cartServices;
        }

        #endregion
        public async Task<RequestPayResultDto> AddRequestPay(int amount, long? userId, Guid browserId,IFormFile imagepay)
        {
           
            var getCart = await _cartServices.GetLastOpenCart(browserId, userId);


            string imagename = "";

            if (imagepay != null)
            {
                imagename = Guid.NewGuid().ToString("N") + Path.GetExtension(imagepay.FileName);
                await imagepay.SaveFileToServer($"{Directory.GetCurrentDirectory()}/wwwroot/content/pay/", imagename);
            }

            var newRequestPay = new DataLayer.Entites.Finances.RequestPay()
            {
                Guid = Guid.NewGuid(),
                Amount = amount,
                UserId = userId != 0 ? getCart.UserId : null,
                IsPay = false,
                CreateDate = DateTime.Now,
                IsDelete = false,
                CartId = getCart.Id,
                PayImage = imagename
            };

            await _financesRepository.AddEntity(newRequestPay);
            await _financesRepository.SaveChanges();

            return new RequestPayResultDto()
            {
                guid = newRequestPay.Guid,
                Amount = newRequestPay.Amount,
                RequestPayId = newRequestPay.Id
            };

        }
        public async ValueTask DisposeAsync()
        {
            await _financesRepository.DisposeAsync();
        }
    }
}
