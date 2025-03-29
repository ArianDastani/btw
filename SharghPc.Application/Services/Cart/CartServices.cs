using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.DTOs.Cart;
using SharghPc.DataLayer.Entites.Carts;
using SharghPc.DataLayer.Repository;

namespace SharghPc.Application.Services.Cart
{
    public class CartServices : ICartServices
    {
        #region ctor

        private IGenericRepository<DataLayer.Entites.Carts.Cart> _cartRepository;
        private IGenericRepository<CartItem> _cartItemRepository;
        private IGenericRepository<DataLayer.Entites.Product.Product> _productRepository;

        public CartServices(IGenericRepository<DataLayer.Entites.Carts.Cart> cartRepository, IGenericRepository<CartItem> cartItemRepository, IGenericRepository<DataLayer.Entites.Product.Product> productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        #endregion


        public async Task<DataLayer.Entites.Carts.Cart> GetLastOpenCart(Guid BrowserId, long? UserId)
        {


            var isCart = await _cartRepository.GetQuery()
                .Include(x => x.CartItems)
                .OrderByDescending(x => x.CreateDate)
                .FirstOrDefaultAsync(x => x.UserId == UserId || x.BrowserId == BrowserId);

            if (isCart == null)
            {
                DataLayer.Entites.Carts.Cart newcart = new DataLayer.Entites.Carts.Cart()
                {
                    CreateDate = DateTime.Now,
                    BrowserId = BrowserId,
                    UserId = UserId != 0 ? UserId : null,
                    Finished = false,
                    IsDelete = false
                };


                await _cartRepository.AddEntity(newcart);
                await _cartRepository.SaveChanges();


            }

            var Cart = await _cartRepository.GetQuery()
                .Include(x => x.CartItems)
                .OrderByDescending(x => x.CreateDate)
                .FirstOrDefaultAsync(x => x.UserId == UserId || x.BrowserId == BrowserId);

            if (Cart.Finished == true)
            {
                DataLayer.Entites.Carts.Cart newcart2 = new DataLayer.Entites.Carts.Cart()
                {
                    CreateDate = DateTime.Now,
                    BrowserId = BrowserId,
                    UserId = UserId != 0 ? UserId : null,
                    Finished = false,
                    IsDelete = false
                };


                await _cartRepository.AddEntity(newcart2);
                await _cartRepository.SaveChanges();
            }





            if (UserId == null || UserId == 0)
            {
                var cartbyBrowserId = await _cartRepository.GetQuery()
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                    .OrderByDescending(x => x.CreateDate)
                    .FirstOrDefaultAsync(x => x.BrowserId == BrowserId && x.Finished == false);

                return cartbyBrowserId;
            }
            else
            {
                var cartbyUserId = await _cartRepository.GetQuery()
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                    .OrderByDescending(x => x.CreateDate)
                    .FirstOrDefaultAsync(x => x.Finished == false && x.UserId == UserId);

                return cartbyUserId;
            }
        }

        public async Task<AddToCartResult> AddToCart(long productId, Guid BrowserId, long? UserId)
        {
            try
            {
                var cart = await GetLastOpenCart(BrowserId, UserId);

                var cartItem = await _cartItemRepository.GetQuery()
                    .FirstOrDefaultAsync(x => x.ProductId == productId && x.CartId == cart.Id);

                var product = await _productRepository.GetEntityById(productId);

                if (cartItem == null)
                {
                    CartItem newCartItem = new CartItem()
                    {
                        CartId = cart.Id,
                        IsDelete = false,
                        CreateDate = DateTime.Now,
                        ProductId = productId,
                        Count = 1,
                        Price = product.Price,


                    };

                    await _cartItemRepository.AddEntity(newCartItem);
                    await _cartItemRepository.SaveChanges();
                }
                else
                {
                    cartItem.Count++;
                    await _cartItemRepository.SaveChanges();
                }

                return AddToCartResult.success;

            }
            catch
            {
                return AddToCartResult.error;
            }
        }

        public async Task<bool> RemoveCartItemFromCart(long itemId, Guid BrowserId, long? UserId)
        {
            var cart = await GetLastOpenCart(BrowserId, UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return false;
            }

            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == itemId && x.IsDelete == false);

            if (cartItem == null)
            {
                return false;
            }


            _cartItemRepository.DeletePermanent(cartItem);
            await _cartItemRepository.SaveChanges();

            return true;

        }

        public async Task<DataLayer.Entites.Carts.Cart> GetCart(Guid BrowserId, long? UserId)
        {
            if (UserId != null || UserId != 0)
            {
                var getCartByUserId = await _cartRepository.GetQuery()
                    .Include(x => x.CartItems)
                    .OrderByDescending(x => x.CreateDate)
                    .FirstOrDefaultAsync(x => x.UserId == UserId && x.Finished == false);

                return getCartByUserId;
            }
            else
            {
                var getCartByBrowserId = await _cartRepository.GetQuery()
                    .Include(x => x.CartItems)
                    .FirstOrDefaultAsync(x => x.UserId == UserId);

                return getCartByBrowserId;
            }
        }

        public async Task<bool> SetCartToUserId(long? userId, Guid browser)
        {
            if (userId != null)
            {
                //var cart = await _cartRepository.GetQuery().FirstOrDefaultAsync(x => x.BrowserId == browser);
                var cart = await GetLastOpenCart(browser, 0);
                if (cart != null)
                {
                    cart.UserId = userId;
                    await _cartRepository.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> AddCount(long cartItemId)
        {
            if (cartItemId != null || cartItemId != 0)
            {
                var res = await _cartItemRepository.GetEntityById(cartItemId);

                if (res != null)
                {
                    res.Count = res.Count + 1;
                    _cartItemRepository.EditEntity(res);
                    await _cartItemRepository.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> LowOffCount(long cartItemId)
        {
            if (cartItemId != null || cartItemId != 0)
            {
                var res = await _cartItemRepository.GetEntityById(cartItemId);

                if (res != null)
                {
                    res.Count--;
                    _cartItemRepository.EditEntity(res);
                    await _cartItemRepository.SaveChanges();
                    return true;
                }

                return false;
            }
            return false;
        }

        //async Task ChangeCartItemCount(long itemId)
        //{

        //}

        public async ValueTask DisposeAsync()
        {
            await _cartRepository.DisposeAsync();
            await _cartItemRepository.DisposeAsync();
        }
    }
}
