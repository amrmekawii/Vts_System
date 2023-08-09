using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.BL
{
    public class DetailsCategoryDto
    {
        public string Name { get; set; }
        public ICollection<ItemDto>? Items { get; set; }

    }
    public class ItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageURL { get; set; }
        public decimal Price { get; set; }


    }


}


