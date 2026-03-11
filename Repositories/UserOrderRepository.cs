using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MusicShoppingCartMvcUI.Repositories
{
    public class UserOrderRepository :IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor; //Used for obtaining the current user's information, such as their user ID,
                                                                    //from the HTTP context. This allows us to associate orders with the correct user.

        public UserOrderRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
        var order = await _db.Orders.FindAsync(data.OrderId);
            if(order == null)
            {
                throw new InvalidOperationException($"Order with ID {data.OrderId} not found.");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _db.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatuses.ToListAsync(); ;
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if(order == null)
            {
                throw new InvalidOperationException($"order with id: {orderId} is not found");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                        .Include(x=>x.OrderStatus)
                        .Include(x=>x.OrderDetail)
                        .ThenInclude(x=>x.Product)
                        .ThenInclude(x=>x.Category).AsQueryable(); //What is AsQueryable()?
            if(!getAll)
            {
                string userId = GetUserId();
                if(string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }
                orders = orders.Where(a=>a.UserId == userId);
                return await orders.ToListAsync();
            }
            return await orders.ToListAsync();
        }
        
        public async Task RemoveOrder(Order order)
        {
            _db.Orders.Remove(order); //we cannot call await on this line because Remove is not async, we remove then save changes
            await _db.SaveChangesAsync();
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
