using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Repository;
using System.ComponentModel.DataAnnotations;

namespace SharghPc.Application.Services.Shipment
{
    public interface IShipmentServices : IAsyncDisposable
    {
        public Task<DataLayer.Entites.Finances.Shipment> GetShipment(long UserId);
        public Task<bool> AddUserShipment(AddUserShipmentDto userShipmentDto, long UserId);
        public Task<GetShipmentForEditDto> GetShipmentForEdit(long ShipmentId);
        public Task<bool> EditShipment(GetShipmentForEditDto shipmentForEditDto);

    }

    public class ShipmentServices : IShipmentServices
    {
        #region Ctorf

        private IGenericRepository<DataLayer.Entites.Finances.Shipment> _shipmentRepository;
        private IGenericRepository<User> _userRepository;

        public ShipmentServices(IGenericRepository<DataLayer.Entites.Finances.Shipment> shipmentRepository, IGenericRepository<User> userRepository)
        {
            _shipmentRepository = shipmentRepository;
            _userRepository = userRepository;
        }
        public async ValueTask DisposeAsync()
        {
            await _shipmentRepository.DisposeAsync();
            await _userRepository.DisposeAsync();
        }

        #endregion

        public async Task<DataLayer.Entites.Finances.Shipment> GetShipment(long UserId)
        {
            if (UserId == 0 || UserId == null)
            {
                return null;
            }

            var user = await _userRepository.GetEntityById(UserId);

            if (user == null)
            {
                return null;
            }

            var shipment = await _shipmentRepository.GetQuery()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (shipment == null)
            {
                return null;
            }

            return shipment;
        }

        public async Task<bool> AddUserShipment(AddUserShipmentDto userShipmentDto, long UserId)
        {
            var user = await _userRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
            {
                return false;
            }

            if (_shipmentRepository.GetQuery().Any(x => x.UserId == user.Id))
            {
                return false;
            }

            var newShipment = new DataLayer.Entites.Finances.Shipment()
            {
                CreateDate = DateTime.Now,
                ZipCode = userShipmentDto.ZipCode,
                UserId = user.Id,
                IsDelete = false,
                RecipientFirstName = userShipmentDto.RecipientFirstName,
                RecipientLastName = userShipmentDto.RecipientLastName,
                FullAddress = userShipmentDto.FullAddress,
                City = userShipmentDto.City,
                State = userShipmentDto.State,

            };

            await _shipmentRepository.AddEntity(newShipment);
            await _shipmentRepository.SaveChanges();
            return true;
        }

        public async Task<GetShipmentForEditDto> GetShipmentForEdit(long Id)
        {
            if (Id == 0 || Id == null)
            {
                return null;
            }

            var shipment = await _shipmentRepository.GetEntityById(Id);

            if (shipment == null)
            {
                return null;
            }

            return new GetShipmentForEditDto
            {
                Id = shipment.Id,
                FullAddress = shipment.FullAddress,
                City = shipment.City,
                State = shipment.State,
                ZipCode = shipment.ZipCode,
                RecipientFirstName = shipment.RecipientFirstName,
                RecipientLastName = shipment.RecipientLastName,
                UserId = shipment.UserId,
            };
        }

        public async Task<bool> EditShipment(GetShipmentForEditDto shipmentForEditDto)
        {
            var res = await _shipmentRepository.GetEntityById(shipmentForEditDto.Id.Value);

            if (res == null)
            {
                return false;
            }

            res.City = shipmentForEditDto.City;
            res.State = shipmentForEditDto.State;
            res.FullAddress = shipmentForEditDto.FullAddress;
            res.RecipientFirstName = shipmentForEditDto.RecipientFirstName;
            res.RecipientFirstName = shipmentForEditDto.RecipientFirstName;
            res.ZipCode = shipmentForEditDto.ZipCode;

            _shipmentRepository.EditEntity(res);
            await _shipmentRepository.SaveChanges();
            return true;


        }
    }

    public class AddUserShipmentDto
    {


        [Display(Name = "آدرس کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FullAddress { get; set; } = null!;


        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string City { get; set; } = null!;


        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string State { get; set; } = null!;

        [Display(Name = "کد پستی")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ZipCode { get; set; }


        [Display(Name = "نام تحویل گیرنده")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? RecipientLastName { get; set; }



        [Display(Name = "نام خانوادگی تحویل گیرنده")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? RecipientFirstName { get; set; }
        public long? UserId { get; set; }
    }

    public class GetShipmentForEditDto
    {
        public long? Id { get; set; }
        public string? FullAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? RecipientLastName { get; set; }
        public string? RecipientFirstName { get; set; }
        public long? UserId { get; set; }
    }
}
