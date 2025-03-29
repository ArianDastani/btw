using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Contact;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Product;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Site
{
    public class SIteInfoServices : ISiteInfoServices
    {
        private IGenericRepository<DataLayer.Entites.Site.Contact> _contactRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<DataLayer.Entites.ProductOrder.Order> _OrderRepository;
        private IGenericRepository<DataLayer.Entites.Product.Product> _ProductRepository;
        private IGenericRepository<ProductCategory> _CategoryRepository;


        public SIteInfoServices(IGenericRepository<DataLayer.Entites.Site.Contact> contactRepository, IGenericRepository<User> userRepository, IGenericRepository<DataLayer.Entites.ProductOrder.Order> orderRepository, IGenericRepository<DataLayer.Entites.Product.Product> productRepository, IGenericRepository<ProductCategory> categoryRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            _OrderRepository = orderRepository;
            _ProductRepository = productRepository;
            _CategoryRepository = categoryRepository;
        }

        public async ValueTask DisposeAsync()
        {
            await _contactRepository.DisposeAsync();
        }

        public async Task<bool> AddOrEditContact(AddOrEditContentDto contactDto)
        {
            if (contactDto.Id == 0 || contactDto.Id == null)
            {
                if (contactDto == null) return false;

                DataLayer.Entites.Site.Contact contact = new DataLayer.Entites.Site.Contact()
                {
                    CreateDate = DateTime.Now,
                    ContactTypeId = contactDto.ContactTypeId,
                    Title = contactDto.Title,
                    Value = contactDto.Value,
                    IsDelete = false,
                    Link = contactDto.Link,
                };

                await _contactRepository.AddEntity(contact);
                await _contactRepository.SaveChanges();

                return true;
            }
            else
            {
                long id = (long)contactDto.Id;

                var contact = await _contactRepository.GetEntityById(id);

                if (contact == null) return false;

                contact.Title = contactDto.Title;
                contact.Value = contactDto.Value;
                contact.ContactTypeId = contactDto.ContactTypeId;
                contact.Link = contactDto.Link;

                _contactRepository.EditEntity(contact);
                await _contactRepository.SaveChanges();
                return true;

            }

        }

        public async Task<LoadContactDto> GetContactById(long? id)
        {
            if (id == null || id == 0)
            {
                return new LoadContactDto() { Id = 0 };
            }

            var contact = await _contactRepository.GetEntityById((long)id);

            if (contact == null) return null;

            return new LoadContactDto()
            {
                ContactTypeId = contact.ContactTypeId,
                Title = contact.Title,
                Value = contact.Value,
                Id = contact.Id,
                Link = contact.Link

            };

        }

        public async Task<List<DataLayer.Entites.Site.Contact>> GetAllContact()
        {
            return await _contactRepository.GetQuery()
                .Where(x => x.IsDelete == false)
                .Include(x => x.ContactType)
                .ToListAsync();
        }

        public async Task<List<GetContatsDtoForSiteDto>> GetContatsDtoForSite()
        {
            return await _contactRepository.GetQuery()
                .Where(x => x.IsDelete == false)
                .Select(x => new GetContatsDtoForSiteDto()
                {
                    Title = x.Title,
                    Value = x.Value,
                    ContactTypeId = x.ContactTypeId,
                    Link = x.Link
                })
                .ToListAsync();
        }

        public async Task<AdminViewModels> GetAdminViewModels()
        {
            int counter = 0;
            foreach (var product in _ProductRepository.GetQuery().ToList())
            {
                counter = counter + int.Parse(product.ViewConter);
            }

            //var count = _ProductRepository.GetQuery().Sum(x => int.Parse(x.ViewConter));

            var categorycount = await _CategoryRepository.GetQuery().CountAsync();
            var order = await _OrderRepository.GetQuery().CountAsync();
            var customer = await _userRepository.GetQuery().CountAsync();

            var Lastorder = await _OrderRepository.GetQuery()
                .Include(x => x.User)
                .Include(x => x.RequestPay)
                .OrderByDescending(x => x.CreateDate)
                .Where(x => x.IsDelete == false)
                .ToListAsync();

            var topProduct = await _ProductRepository.GetQuery()
                .OrderByDescending(x => x.ViewConter)
                .Where(x => x.IsDelete == false)
                .ToListAsync();

            return new AdminViewModels()
            {
                TotalCategory = categorycount.ToString(),
                TotalOrder = order.ToString(),
                TotalCustomer = customer.ToString(),
                TotalVisit = counter.ToString(),

                LastOrders = Lastorder,
                TopProducts = topProduct

            };
        }

        public async Task<bool> Delete(long Id)
        {
            if (Id == 0 || Id == null) return false;

            var con = await _contactRepository.GetEntityById(Id);

            if (con == null) return false;

            _contactRepository.DeletePermanent(con);
            await _contactRepository.SaveChanges();

            return true;
        }
    }
}
