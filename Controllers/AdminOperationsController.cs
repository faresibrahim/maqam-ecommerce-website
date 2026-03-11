using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicShoppingCartMvcUI.Constants;
using MusicShoppingCartMvcUI.Models;

namespace MusicShoppingCartMvcUI.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))] //??
    public class AdminOperationsController : Controller
    {

        private readonly IUserOrderRepository _userOrderRepository;
        private readonly ApplicationDbContext _context;

        public AdminOperationsController(IUserOrderRepository userOrderRepository, ApplicationDbContext context)
        {
            _userOrderRepository = userOrderRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardModel result = new DashboardModel
            {
                TotalCategories = _context.Categories.Count(),
                TotalProducts = _context.Products.Count(),
                TotalOrders = _context.Orders.Count(),

            };
            return View(result);
        }

       public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction(nameof(AllOrders));
        }

        public async Task<IActionResult> UpdateOrderStatus(int orderid)
        {
            var order = await _userOrderRepository.GetOrderById(orderid);
            if (order == null)
            {
                throw new Exception($"Order with id: {orderid} is not found");
            }
            var orderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(orderStatus
                =>
            {
                return new SelectListItem
                {
                    Value = orderStatus.Id.ToString(),
                    Text = orderStatus.StatusName,
                    Selected = order.OrderStatusId == orderStatus.Id
                };

            });
            var data = new UpdateOrderStatusModel
            {
                OrderId = orderid,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus (UpdateOrderStatusModel data)
        {
            try
            {
                if ( !ModelState.IsValid )
                {
                    data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select
                    (orderStatus =>
                    {
                        return new SelectListItem
                        {
                            Value = orderStatus.Id.ToString(),

                            Text = orderStatus.StatusName,

                            Selected = orderStatus.Id == data.OrderStatusId
                        };
                    });
                    return View(data);
                }
                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Updated Sucessfully";
            }
            catch
            {
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }

        public async Task<IActionResult> RemoveOrder(Order order)
        {
            var orderid = await _userOrderRepository.GetOrderById(order.OrderId);

            await _userOrderRepository.RemoveOrder(orderid);

            return RedirectToAction("AllOrders");
        }

    }
}
