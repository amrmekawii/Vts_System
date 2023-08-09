using System.ComponentModel.DataAnnotations;

namespace Vts.DAL
{
    public class Category
    {


        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}