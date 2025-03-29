using Microsoft.IdentityModel.Tokens;
using SharghPc.DataLayer.DTOs.Contact;
using SharghPc.DataLayer.Entites.Contact;
using SharghPc.DataLayer.Repository;


namespace SharghPc.Application.Services.Contact
{
    public class ContactServices : IContactServices
    {
        #region Ctor

        private IGenericRepository<Ticket> _ticketRepository;
        private IGenericRepository<TicketMessage> _ticketMessageRepository;
        private IGenericRepository<ContactUs> _contactUsRepository;

        public ContactServices(IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository, IGenericRepository<ContactUs> contactUsRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _contactUsRepository = contactUsRepository;
        }

        #endregion

        public async Task<AddTicketResult> AddUserTicket(AddTicketDTO addTicketDTO, long userId)
        {
            if (addTicketDTO.Text.IsNullOrEmpty()) return AddTicketResult.Error;

            var newticket = new Ticket
            {
                OwnerId = userId,
                CreateDate = DateTime.Now,
                IsReadByOwner = true,
                IsReadByAdmin = false,
                Title = addTicketDTO.Title,
                TicketPriority = addTicketDTO.TicketPriority,
                TicketSection = addTicketDTO.TicketSection,
                TicketState = TicketState.pending
            };

            await _ticketRepository.AddEntity(newticket);
            await _ticketRepository.SaveChanges();

            var message = new TicketMessage
            {
                TicketId = newticket.Id,
                Text = addTicketDTO.Text,
                SenderId = userId,
            };

            await _ticketMessageRepository.AddEntity(message);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        public async Task<bool> CreateContactUs(CreatContactUsDto ContactUsDto, long? userId, string userIp)
        {
            try
            {
                var newcontact = new ContactUs()
                {
                    UserId = userId != null && userId.Value != 0 ? userId.Value : (long?)null,
                    UserIp = userIp,
                    Subject = ContactUsDto.Subject,
                    CreateDate = DateTime.Now,
                    Text = ContactUsDto.Text,
                    Email = ContactUsDto.Email,
                    FullName = ContactUsDto.FullName,
                    IsDelete = false,
                    Mobile = ContactUsDto.Mobile,

                };

                await _contactUsRepository.AddEntity(newcontact);
                await _contactUsRepository.SaveChanges();


                return true;

            }
            catch
            {
                return false;

            }

        }

        public async ValueTask DisposeAsync()
        {
            await _ticketMessageRepository.DisposeAsync();
            await _ticketRepository.DisposeAsync();
            await _contactUsRepository.DisposeAsync();
        }
    }
}
