
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Data;

namespace WebShop.Areas.Customer.Repositories.Implement
{
    public class CartRepository : ICartRepository
    {
        private readonly WebShopContext _context;

        public CartRepository(WebShopContext context)
        {
            _context = context;
        }


        public async Task<Cart> GetCartByUserClaimsAsync(ClaimsPrincipal claims)
        {
            var userIdclaims = claims.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdclaims == null)
            {
                return null;
            }

            var userId = Guid.Parse(userIdclaims.Value);
            var cart = await _context.Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

  
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }


        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }



        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            var existingItem = await _context.CartItems.FindAsync(cartItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity = cartItem.Quantity;
                existingItem.Price = cartItem.Price; 
                await _context.SaveChangesAsync();
                return existingItem;
            }
            return null; 
        }

		public async Task<IEnumerable<CartItem>> GetCartItemsAsync(ClaimsPrincipal claims)
		{
			var userIdclaims = claims.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdclaims == null)
			{
				return Enumerable.Empty<CartItem>();
			}

			var userId = Guid.Parse(userIdclaims.Value);
            var cart = await GetCartByUserClaimsAsync(claims);
            IEnumerable<CartItem> cartItems = await _context.CartItems.Where(i => i.CartId == cart.CartId).ToListAsync(); 
            return cartItems;

		}
	}
}
