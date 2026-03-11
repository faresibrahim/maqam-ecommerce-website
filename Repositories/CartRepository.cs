using Microsoft.AspNetCore.Identity;
using MusicShoppingCartMvcUI.Models;
using MusicShoppingCartMvcUI.Data;
using Microsoft.EntityFrameworkCore;


namespace MusicShoppingCartMvcUI.Repositories 
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, 
            UserManager<IdentityUser> userManager) {

            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(userId))

                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId //When creating a new shopping cart for the user, set the userId of
                                        //the shopping cart to the userId of the logged-in user,
                                        //so that we can associate the shopping cart with the user
                    };
                    _db.ShoppingCarts.Add(cart);
                    await _db.SaveChangesAsync();
                }
                var cartItem = _db.CartDetails.FirstOrDefault
                (a => a.ShoppingCartId == cart.ShoppingCartId && a.ProductId == productId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.ShoppingCartId,
                        Quantity = qty,
                        UnitPrice = product.Price
                    };
                    _db.CartDetails.Add(cartItem);

                }
                _db.SaveChanges();
                transaction.Commit(); //ask AI about this
                
            }

            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            //we comment the below line out because we are not making changes in multiple tables in the database
            //using var transaction = _db.Database.BeginTransaction();
            try
            {
               
                if (string.IsNullOrEmpty(userId))

                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null) //if there is no cart then there is nothing to be removed
                {
                    throw new InvalidOperationException("Invalid Cart");
                }
                //cart detail section
                var cartItem = _db.CartDetails.FirstOrDefault
                (a => a.ShoppingCartId == cart.ShoppingCartId && a.ProductId == productId);


                if (cartItem is null) //same here, if there are no items in the cart, there are no items to be removed
                {
                    throw new InvalidOperationException("No items found in cart");
                }
                else if (cartItem.Quantity ==1) //If there is an item with 1 quantity, then removing it means removing the whole item from the list
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1;
                }

                _db.SaveChanges();
                //transaction.Commit();
                
            }

            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new InvalidOperationException("Invalid userid");
            }
            var shoppingCart = await _db.ShoppingCarts
                            .Include(a => a.CartDetails)
                            .ThenInclude(a=>a.Product)
                            .ThenInclude(a=>a.Stocks)
                            .Include(a => a.CartDetails)
                            .ThenInclude(a => a.Product)
                            .ThenInclude(a => a.Category)
                            .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        { 
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId="")
        {
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts //Grab all shopping carts from the database
                        join cartDetail in _db.CartDetails //join the shopping carts with the cart details table to get the details of the items in the cart
                        on cart.ShoppingCartId equals cartDetail.ShoppingCartId // connect the foreign key(the middle man)
                        where cart.UserId==userId
                        select new { cartDetail.CartDetailId } //select the cart detail id from the cart details table, we only need the cart detail id to count the number of items in the cart
                        ).ToListAsync();

            return data.Count; //Count the number of items in the cart and return it. It
                               //counts the number elements that are returned from the above query,
                               //which is the number of items in the cart
        }

        //assign type later
        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction(); //I don't understand this
            try
            {
                //move data from cartDetail to order and order detail
                var userId = GetUserId();
                if(string.IsNullOrEmpty(userId)) //check if the user is logged in
                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }
                var cart = await GetCart(userId);
                if(cart is null) //Does the cart exist?
                {
                    throw new InvalidOperationException("Invalid cart");
                }
                //get all the cart details for the cart and store them in var cartDetail
                var cartDetail = _db.CartDetails 
                                .Where(a=>a.ShoppingCartId == cart.ShoppingCartId).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is Empty"); //if count is 0 then the cart is empty
                var pendingRecord =_db.OrderStatuses.FirstOrDefault
                    (s=>s.StatusName == "Pending"); //check if there is a pending record in the order status table, if not then create one)
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have pending status");
                var order = new Order //Otherwise, create a new order for the current user
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.StatusId
                };
                _db.Orders.Add(order); //add the new order to the database and save changes
                _db.SaveChanges();
                foreach (var item in cartDetail) //for each in the cart detail, create a new order detail and add it to the database
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.OrderId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                    var stock = await _db.Stocks.FirstOrDefaultAsync(a=>a.ProductId == item.ProductId);
                    if(stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }

                    if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} item(s) are available in the stock");
                    }
                    stock.Quantity -= item.Quantity;
                }
                _db.SaveChanges();
                //removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
