using SharghPc.DataLayer.DTOs.Order;
using SharghPc.DataLayer.Entites.ProductOrder;

namespace SharghPc.Application.Services.Order
{
    public interface IOrderServices : IAsyncDisposable
    {
        //Task<long> AddOrderForUser(long userId);

        //Task<DataLayer.Entites.ProductOrder.Order> GetUserLastOpenOrder(long userId);

        //Task<bool> AddProductToOpenOrder(long UserId,AddProductToOrderDto orderDetailDto);

        Task<bool> AddNewOrder(AddNewOrderDto orderDto);

        public Task<List<DataLayer.Entites.ProductOrder.Order>> GetOrdersForUser(long userId);

        public Task<GetOrderDetailDto> GetOrderDetailForSite(long OrderId, long UserId);

        public Task<List<OrdersDto>> GetOrdersForAdmin(OrderState orderState);

        public Task<DataLayer.Entites.ProductOrder.Order> GetOrderForAdmin(long Id);

        public Task<bool> AddToDelivered(long Id);
    }

    public class OrdersDto
    {
        public long OrderId { get; set; }
        public DateTime InsetTime { get; set; }
        public long RequestId { get; set; }
        public long UserId { get; set; }

        public string? UserName { get; set; }
        public OrderState OrderState { get; set; }
        public int ProductCount { get; set; }
    }
}
