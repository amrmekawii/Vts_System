using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{
    public interface ICatRepo
    {
        void Add(Category Cat);
        Category GetById(Guid id);
        Category GetDetailsCat(Guid id);
        void Remove(Category cat);
        int Save();
    }
}
