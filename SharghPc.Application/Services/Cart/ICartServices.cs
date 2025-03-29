
using SharghPc.DataLayer.DTOs.Cart;

namespace SharghPc.Application.Services.Cart
{
    public interface ICartServices : IAsyncDisposable
    {
        public Task<DataLayer.Entites.Carts.Cart> GetLastOpenCart(Guid BrowserId, long? UserId);

        Task<AddToCartResult> AddToCart(long productId, Guid BrowserId, long? UserId);

        Task<bool> RemoveCartItemFromCart(long itemId, Guid BrowserId, long? UserId);

        Task<DataLayer.Entites.Carts.Cart> GetCart(Guid BrowserId, long? UserId);

        public Task<bool> SetCartToUserId(long? userId, Guid browser);

        //Task ChangeCartItemCount(long itemId);

        public Task<bool> AddCount(long cartItemId);

        public Task<bool> LowOffCount(long cartItemId);

    }
}
