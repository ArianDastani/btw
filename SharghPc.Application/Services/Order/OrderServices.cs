
using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Order;
using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.ProductOrder;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Order
{
    public class OrderServices : IOrderServices
    {
        #region Ctorf

        private IGenericRepository<DataLayer.Entites.ProductOrder.Order> _OrderRepository;
        private IGenericRepository<DataLayer.Entites.ProductOrder.OrderDetail> _OrderDetailRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<DataLayer.Entites.Finances.RequestPay> _requestpayRepository;
        private IGenericRepository<DataLayer.Entites.Carts.Cart> _cartRepository;
        private IGenericRepository<DataLayer.Entites.Finances.Shipment> _shipmentRepository;


        public OrderServices(IGenericRepository<DataLayer.Entites.ProductOrder.Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository, IGenericRepository<User> userRepository, IGenericRepository<DataLayer.Entites.Finances.RequestPay> requestpayRepository, IGenericRepository<DataLayer.Entites.Carts.Cart> cartRepository, IGenericRepository<DataLayer.Entites.Finances.Shipment> shipmentRepository)
        {
            _OrderRepository = orderRepository;
            _OrderDetailRepository = orderDetailRepository;
            _userRepository = userRepository;
            _requestpayRepository = requestpayRepository;
            _cartRepository = cartRepository;
            _shipmentRepository = shipmentRepository;
        }

        #endregion

        //public async Task<long> AddOrderForUser(long userId)
        //{
        //    DataLayer.Entites.ProductOrder.Order Order = new DataLayer.Entites.ProductOrder.Order()
        //    {
        //        UserId = userId,
        //        CreateDate = DateTime.Now,
        //        IsDelete = false
        //    };

        //    await _OrderRepository.AddEntity(Order);
        //    await _OrderRepository.SaveChanges();

        //    return Order.Id;
        //}

        //public async Task<DataLayer.Entites.ProductOrder.Order> GetUserLastOpenOrder(long userId)
        //{
        //    if (!await _OrderRepository.GetQuery().AnyAsync(s => s.UserId == userId && !s.IsPaid))
        //    {
        //         await AddOrderForUser(userId);
        //    }

        //    var lastOpenOrder = await _OrderRepository.GetQuery()
        //        .Include(x=>x.OrderDetails)
        //        .SingleOrDefaultAsync(x => x.UserId == userId && !x.IsPaid);

        //    return lastOpenOrder;
        //}

        //public async Task<bool> AddProductToOpenOrder(long UserId,AddProductToOrderDto orderDetailDto)
        //{
        //    if (UserId == 0 || UserId == null)
        //    {
        //        return false;
        //    }

        //    var openOrder = await GetUserLastOpenOrder(UserId);

        //    var similarOrder = openOrder.OrderDetails
        //        .SingleOrDefault(x => x.ProductId == orderDetailDto.ProductId);

        //    if (similarOrder==null)
        //    {
        //        var orderDetail = new OrderDetail()
        //        {
        //            CreateDate = DateTime.Now,
        //            IsDelete = false,
        //            OrderId = openOrder.Id,
        //            ProductId = orderDetailDto.ProductId,
        //            Count = orderDetailDto.Count,
        //        };

        //        await _OrderDetailRepository.AddEntity(orderDetail);
        //        await _OrderDetailRepository.SaveChanges();
        //    }
        //    else
        //    {
        //        similarOrder.Count += orderDetailDto.Count;
        //         await _OrderDetailRepository.SaveChanges();
        //    }

        //    return true;



        //}

        public async ValueTask DisposeAsync()
        {
            await _OrderRepository.DisposeAsync();
            await _OrderDetailRepository.DisposeAsync();
        }

        public async Task<bool> AddNewOrder(AddNewOrderDto orderDto)
        {
            var user = await _userRepository.GetEntityById(orderDto.UserId);

            var cart = await _cartRepository.GetQuery()
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.Id == orderDto.CartId)
                .FirstOrDefaultAsync();

            var pay = await _requestpayRepository.GetEntityById(orderDto.RequestpayId);

            var shipment = await _shipmentRepository.GetQuery()
                .Where(x => x.UserId == user.Id).FirstOrDefaultAsync();


            pay.IsPay = true;
            pay.PayDate = DateTime.Now;

            cart.Finished = true;

            var newOrder = new DataLayer.Entites.ProductOrder.Order
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                ShipmentId = shipment.Id,
                OrderState = OrderState.Processing,
                RequestPayId = pay.Id,
                UserId = user.Id,

            };

            await _OrderRepository.AddEntity(newOrder);

            List<OrderDetail> OrderDetail = new List<OrderDetail>();

            foreach (var item in cart.CartItems)
            {
                var newOrderDetial = new OrderDetail()
                {
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    Count = item.Count,
                    Order = newOrder,
                    ProductPrice = item.Product.Price,
                    Product = item.Product,
                };
                OrderDetail.Add(newOrderDetial);
            }

            await _OrderDetailRepository.AddRangeEntity(OrderDetail);

            await _OrderRepository.SaveChanges();





            return true;
        }

        public async Task<List<DataLayer.Entites.ProductOrder.Order>> GetOrdersForUser(long userId)
        {
            if (userId == 0 || userId == null)
            {
                return null;
            }

            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
            {
                return null;
            }

            var orders = await _OrderRepository.GetQuery()
                .Include(x => x.OrderDetails)
                .Where(x => x.UserId == user.Id && x.IsDelete == false)
                .ToListAsync();

            return orders;
        }

        public async Task<GetOrderDetailDto> GetOrderDetailForSite(long OrderId, long UserId)
        {
            if (OrderId == 0 || OrderId == null || UserId == 0 || UserId == null) return null;

            var user = await _userRepository.GetEntityById(UserId);

            if (user == null) return null;

            var order = await _OrderRepository.GetQuery()
                .Include(x => x.RequestPay)
                .Include(x => x.User)
                .Include(x => x.Shipment)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == OrderId);

            if (order == null) return null;

            if (order.UserId != user.Id) return null;

            return new GetOrderDetailDto()
            {
                FullAddress = order.Shipment.FullAddress,
                FullName = order.User.FirstName + " " + order.User.LastName,
                OrderState = order.OrderState,
                Guid = order.RequestPay.Guid,
                Amount = order.RequestPay.Amount,
                IsPay = order.RequestPay.IsPay,
                PayDate = order.RequestPay.PayDate,
                City = order.Shipment.City,
                State = order.Shipment.State,
                ZipCode = order.Shipment.ZipCode,

                OrderDetails = order.OrderDetails.ToList()
            };
        }

        public async Task<List<OrdersDto>> GetOrdersForAdmin(OrderState orderState)
        {
            var orders = await _OrderRepository.GetQuery()
                .Include(p => p.OrderDetails)
                .Include(x => x.User)
                .Where(p => p.OrderState == orderState && p.IsDelete == false)
                .OrderByDescending(p => p.Id)
                .Select(p => new OrdersDto
                {
                    InsetTime = p.CreateDate.Value,
                    OrderId = p.Id,
                    OrderState = p.OrderState,
                    ProductCount = p.OrderDetails.Count(),
                    RequestId = p.RequestPayId,
                    UserId = p.UserId.Value,

                    UserName = p.User.FirstName + " " + p.User.LastName,
                }).ToListAsync();

            return orders;
        }

        public async Task<DataLayer.Entites.ProductOrder.Order> GetOrderForAdmin(long Id)
        {
            if (Id == 0 || Id == null)
            {
                return null;
            }

            var order = await _OrderRepository.GetQuery()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .Include(x => x.Shipment)
                .Include(x => x.RequestPay)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (order == null)
            {
                return null;
            }

            return order;

        }

        public async Task<bool> AddToDelivered(long Id)
        {
            if (Id == 0 || Id == null) return false;

            var order = await _OrderRepository.GetEntityById(Id);

            if (order == null) return false;

            if (order.OrderState == OrderState.Canceled) return false;

            if (order.OrderState == OrderState.Processing)
            {
                order.OrderState = OrderState.Delivered;
                _OrderRepository.EditEntity(order);
                await _OrderRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }

    public class GetOrderDetailDto
    {
        public string FullName { get; set; }
        public OrderState OrderState { get; set; }
        public Guid Guid { get; set; }
        public int Amount { get; set; }
        public bool IsPay { get; set; }
        public DateTime? PayDate { get; set; }
        //public string? Authority { get; set; }
        //public long RefId { get; set; } = 0;
        //public string? PayImage { get; set; }
        public string? FullAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
