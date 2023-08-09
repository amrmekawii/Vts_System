using System.ComponentModel.DataAnnotations;

namespace Vts.DAL
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
        public Item Item { get; set; }
        public Cart Cart { get; set; }
        public int Quantity { get; set; } 
    }
}
