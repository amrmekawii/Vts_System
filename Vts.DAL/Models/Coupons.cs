using System.ComponentModel.DataAnnotations;

namespace Vts.DAL
{
    public class Coupons
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
    }
}
