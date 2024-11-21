using System.Security.Claims;
using WebShop.Areas.Customer.Models;

namespace WebShop.Areas.Customer.Repositories.Interface
{
    public interface ICartRepository
    {
        Task<Cart> AddCartAsync(Cart cart);
        Task<Cart> GetCartByUserClaimsAsync(ClaimsPrincipal claims);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);

        Task<IEnumerable<CartItem>> GetCartItemsAsync(ClaimsPrincipal claims);

        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(int cartItemId);
    }
}
