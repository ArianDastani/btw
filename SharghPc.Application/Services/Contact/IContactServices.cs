using SharghPc.DataLayer.DTOs.Contact;

namespace SharghPc.Application.Services.Contact
{
    public interface IContactServices : IAsyncDisposable
    {
        Task<AddTicketResult> AddUserTicket(AddTicketDTO addTicketDTO, long userId);
        Task<bool> CreateContactUs(CreatContactUsDto ContactUsDto, long? userId, string userIp);
    }
}
