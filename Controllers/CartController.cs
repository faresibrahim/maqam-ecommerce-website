using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicShoppingCartMvcUI.Controllers
{
    [Authorize] //This attribute ensures that only authenticated users can access the
                //CartController and its actions.
    public class CartController : Controller
    {

        private readonly ICartRepository _cartRepo;
        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int productId, int qty=1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");

        }
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);

            return RedirectToAction("GetUserCart");

        }

        

        public async Task<IActionResult> GetUserCart(int productId)
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepo.DoCheckout(model);
            if (!isCheckedOut)
            {
                RedirectToAction(nameof(OrderFailure));
            }
            return RedirectToAction(nameof(OrderSuccess)); //nameof is a C# operator that returns the name of a method
                                                           //as a string. It is used here to avoid hardcoding the action name as a string,
                                                           //which can help prevent errors if the method name changes in the future.

        }

        public IActionResult OrderSuccess()
        {
                  return View();
        }

        public IActionResult OrderFailure()
        {
                  return View();
        }


    }
}
