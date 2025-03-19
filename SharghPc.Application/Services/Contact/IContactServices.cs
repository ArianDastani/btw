using SharghPc.DataLayer.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharghPc.Application.Services.Contact
{
    public interface IContactServices:IAsyncDisposable
    {
        Task<AddTicketResult> AddUserTicket(AddTicketDTO addTicketDTO,long userId);
        Task<bool> CreateContactUs(CreatContactUsDto ContactUsDto,long? userId,string userIp);
    }
}
