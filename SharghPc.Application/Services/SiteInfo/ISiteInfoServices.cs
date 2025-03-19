using SharghPc.Application.Services.Order;
using SharghPc.DataLayer.DTOs.Contact;

namespace SharghPc.Application.Services.Site
{
    public interface ISiteInfoServices:IAsyncDisposable
    {
        Task<bool> AddOrEditContact(AddOrEditContentDto contactDto);
        public Task<LoadContactDto> GetContactById(long? id);
        public Task<List<DataLayer.Entites.Site.Contact>> GetAllContact();
        public Task<bool> Delete(long Id);
        public Task<List<GetContatsDtoForSiteDto>> GetContatsDtoForSite();

        public Task<AdminViewModels> GetAdminViewModels();



    }

    public class AdminViewModels
    {
        public string? TotalVisit { get; set; }
        public string? TotalOrder { get; set; }
        public string? TotalCustomer { get; set; }
        public string? TotalCategory { get; set; }

        public List<DataLayer.Entites.ProductOrder.Order>? LastOrders { get; set; }
        public List<DataLayer.Entites.Product.Product>? TopProducts { get; set; }
    }
}
