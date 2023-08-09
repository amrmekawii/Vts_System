using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vts.DAL
{
    public class CatRepo :ICatRepo
    {
        private readonly VtsContext _context;

        CatRepo(VtsContext context)
        {
            _context = context;
        }
        public void Add(Category Cat)
        {
            _context.Categories.Add(Cat);
        }

        public Category? GetById(Guid id)
        {
            return _context.Set<Category>().FirstOrDefault(d => d.Id == id);

        }

        public Category? GetDetailsCat(Guid id)
        {
            return _context.Set<Category>().Include(p => p.Items).FirstOrDefault(p => p.Id == id);
        }

        public void Remove(Category cat)
        {
            _context.Categories.Remove(cat);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
