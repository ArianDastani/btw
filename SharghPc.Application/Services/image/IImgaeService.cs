using Microsoft.AspNetCore.Http;

namespace SharghPc.Application.Services.image
{
    public interface IImgaeService
    {
        Task ProccessImage(IFormFile image, int fullscreenWidths);
    }





    public class ImgaeService: IImgaeService
    {
        public Task ProccessImage(IFormFile image, int fullscreenWidths)
        {
            throw new NotImplementedException();
        }
    }
}
