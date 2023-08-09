using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{

    public class Item
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [Display(Name = "Description")]

        public string Description { get; set; }

        public int Inventory { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0.")]
        [Display(Name = "Price")]

        public decimal Price { get; set; }
     //   [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0.")]
        public decimal Parcentage { get; set; }

        [Display(Name = "Image Name")]
        public string ImageURL { get; set; }
        [Display(Name = "Available")]

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]

        public Guid CategoryId { get; set; }
        [NotMapped]
        [Display(Name = "Uploaded Image")]
        public IFormFile ImageFile { get; set; }
         public OrderDetail orderDetails { get; set; }
        public CartItem cartItems { get; set; }
        public Category Category { get; set; }

    }
}
