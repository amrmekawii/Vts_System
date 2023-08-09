
namespace Vts.DAL
{
    public class Cart
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int CartItemId { get; set; }
        public ICollection<CartItem> cartItems { get; set; }
        public User User { get; set; }
    }
}