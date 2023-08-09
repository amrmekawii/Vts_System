using System.ComponentModel.DataAnnotations;

namespace Vts.DAL
{
    public class OrderDetail
    {
        
        public int Id { get; set; }
        public int ItemId { get; set; }

        [Required]

        public int OrderId { get; set; }

        public int Quantity { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
        public Item Items { get; set; }

        public Order Order { get; set; }



    }
}

