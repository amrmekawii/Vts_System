using System.ComponentModel.DataAnnotations;

namespace Vts.DAL 


{
    public class OrderStatus
    {

            public int Id { get; set; }
            [Required]
            public int StatusId { get; set; }
            [Required, MaxLength(50)]
            public string? StatusName { get; set; }
        
    }
}
