using System.ComponentModel.DataAnnotations;
using Vts.DAL;

namespace Vts.DAL
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]

        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User User { get; set; }
        public OrderStatus orderStatus { get; set; }
        public int CouponsId { get; set; }
        public Coupons Coupons { get; set; }
        public string TransactionId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }


    }
}
