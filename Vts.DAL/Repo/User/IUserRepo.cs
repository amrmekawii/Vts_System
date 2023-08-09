using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{
    public interface IUserRepo
    {
        List<User> GetAllUser();
        User? GetUserById(string id);
    }
}
