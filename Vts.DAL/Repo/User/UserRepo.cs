using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{
    public class UserRepo : IUserRepo
    {
        private readonly VtsContext _context;

        public List<User> GetAllUser()
        {
           return _context.Users.ToList();
        }

        public User? GetUserById(string id)
        {
            return _context.Users.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
