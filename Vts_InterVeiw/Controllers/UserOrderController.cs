using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

using System.Security.Claims;
using Vts.DAL;

namespace Vts_InterVeiw.Controllers
{
    public class UserOrderController : Controller
    {
        [BindProperty]
        public Order _orderDetails { get; set; }
        private readonly VtsContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderController(VtsContext db,
            UserManager<IdentityUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrder()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _db.Orders
                            .Include(x => x.orderStatus)
                            .Include(x => x.User)
                            .Include(x => x.OrderDetails)
                            .ThenInclude(x => x.Items)
                            .ThenInclude(x => x.Category)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            return orders;
        }
        public async Task<IActionResult> UserOrders()
        {
            var orders = await UserOrder();
            return View(orders);
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(GetUserOrders));
        }
        public async Task<IEnumerable<Order>> GetUserOrder()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _db.Orders
                            .Include(x => x.orderStatus)
                            .Include(x => x.User)
                            .Include(x => x.OrderDetails)
                            .ThenInclude(x => x.Items)
                            .ThenInclude(x => x.Category)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            return orders;
        }
        public async Task<IActionResult> GetUserOrders()
        {
            var orders = await GetUserOrder();
            return View(orders);
        }

        public  IActionResult CreateOrder(int orderId)
        {
            //var ordera =  _db.Orders.Find(orderId);
            //if (ordera == null)
            //{
            //    return NotFound();
            //}


            Random _random = new Random();
            string transactionId = _random.Next(0, 3000).ToString();
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount",10000); // Convert to int to match API requirement
            input.Add("currency", "USD");
            input.Add("receipt", transactionId);
            return View("Payment", _orderDetails);
        }

      









    }
}
