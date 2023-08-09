using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{
    public class User : IdentityUser
    {
        [MaxLength(30)]

        public string FirstName { get; set; } = string.Empty;
        [MaxLength(30)]

        public string LasttName { get; set; } = string.Empty;
        public UserType UserType { get; set; }

        public string About { get; set; } = string.Empty;
        public string UserImage { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public List<Cart> Carts { get; set; }
    }
}
