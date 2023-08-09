using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Vts.DAL;


namespace Project_Cate.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly VtsContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartController(VtsContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.Carts
                              join CartItem in
                              _context.CartItems on cart.Id equals CartItem.CartId
                              select new { CartItem.Id }).ToListAsync();
            return data.Count;
        }
        public async Task<IActionResult> GetTotalItemCart()
        {
            int cartItem = await GetCartItemCount();

            return Ok(cartItem);
        }
        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid UserId");
            var shoppingCart = await _context.Carts.Include(a => a.cartItems)
                .ThenInclude(a => a.Item).
                ThenInclude(a => a.Category).
                Where(a => a.UserId == userId)
                .FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<IActionResult> GetUsersCart()
        {
            var cart = await GetUserCart();
            return View(cart);
        }
        public async Task<int> AddItem(int itemId, int Quantity = 1)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    _context.Carts.Add(cart);
                }
                _context.SaveChanges();
                // cart detail section
                var cartItem = _context.CartItems
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ItemId == itemId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += Quantity;
                }
                else
                {
                    var item = _context.Items.Find(itemId);
                    cartItem = new CartItem
                    {
                        ItemId = itemId,
                        CartId = cart.Id,
                        Quantity = Quantity,
                        UnitPrice = item.Price  // it is a new line after update
                    };
                    _context.CartItems.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<IActionResult> AddToItem(int itemId, int Quantity = 1, int redirect = 0)
        {
            var cartCount = await AddItem(itemId, Quantity);
            if (redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction("GetUsersCart", "Cart");
        }
        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;

        }
        public async Task<int> RemoveItem(int itemId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _context.CartItems
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ItemId == itemId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _context.CartItems.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<IActionResult> RemoveItems(int itemId)
        {
            var cartCount = await RemoveItem(itemId);
            return RedirectToAction("GetUsersCart");
        }
        public async Task<bool> DoCheckOut()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartItems
                                    .Where(a => a.CartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    
                    //Id= cart.Id,
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1,//pending,
                    CouponsId = 1,
                    
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ItemId = item.ItemId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,

                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                // removing the cartdetails
                _context.CartItems.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await DoCheckOut();
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            return RedirectToAction("GetUserOrders", "UserOrder");
        }
    }
}