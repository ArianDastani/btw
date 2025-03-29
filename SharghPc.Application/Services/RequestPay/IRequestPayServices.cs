

using Microsoft.AspNetCore.Http;
using SharghPc.DataLayer.DTOs.RequestPay;

namespace SharghPc.Application.Services.RequestPay
{
    public interface IRequestPayServices : IAsyncDisposable
    {
        public Task<RequestPayResultDto> AddRequestPay(int amount, long? userId, Guid browserId, IFormFile imagepay);

    }
}
