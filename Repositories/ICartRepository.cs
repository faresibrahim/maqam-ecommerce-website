using System.Threading.Tasks;

namespace MusicShoppingCartMvcUI.Repositories
{
    public interface ICartRepository
    {
        //The first two were returning a boolean value to indicate success or failure.
        //We changed them to integer so we can store the number of items
       // in the cart after adding or removing an item. This way we can update the cart item count in
       // the UI without having to query the database again.
        Task<int> AddItem(int productId, int qty);
        Task<int> RemoveItem(int productId);
        Task<ShoppingCart> GetUserCart();
        Task<ShoppingCart> GetCart(string userId);
        Task<int> GetCartItemCount(string userId = "");

        Task<bool> DoCheckout(CheckoutModel model);
    }
}
