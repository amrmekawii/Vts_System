using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Vts.BL;
using Vts.DAL;

namespace Vts_View.Controllers
{

    [Authorize]

    //public class CategoriesController : Controller
    //{
    //    private readonly ICategoryManger _manger;
    //    private readonly VtsContext _context;

    //    CategoriesController(ICategoryManger manger, VtsContext context)
    //    {
    //        _manger = manger;
    //        _context = context; 
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Index()
    //    {
    //        return _context.Categories != null ?
    //                               View(await _context.Categories.ToListAsync()) :
    //                               Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
    //    }

    //    public IActionResult Create()
    //    {
    //        return View();
    //    }
    //   // [Authorize(Roles = "Admin")]
    //    [HttpPost]
    //    public async Task<IActionResult> Create( AddCategortDto category)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _manger.AddCategory(category);
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(category);
    //    }

    //    [HttpGet]
    //    public IActionResult Edit(Guid id)
    //    {
    //        ReadCategoryDto? category = _manger.GetByIdAsEditViewModel(id);
    //        //Handle Null
    //        return View(category);
    //    }

    //    [HttpPost]
    //    public IActionResult Edit(EditCatDto EditCatDto)
    //    {
    //        _manger.EditUsingViewModel(EditCatDto);
    //        return RedirectToAction(nameof(Index));
    //    }

    //    [HttpGet]
    //    public IActionResult Details(Guid id)
    //    {
    //        DetailsCategoryDto? catVM = _manger.GetDetails(id);
    //        //Handle Null
    //        return View(catVM);
    //    }


    //    public async Task<IActionResult> Delete(Guid? id)
    //    {
    //        if (id == null || _context.Categories == null)
    //        {
    //            return NotFound();
    //        }


    //        var category = await _context.Categories
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (category == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(category);
    //    }
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Categories == null)
    //        {
    //            return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
    //        }
    //        var category = await _context.Categories.FindAsync(id);
    //        if (category != null)
    //        {
    //            _context.Categories.Remove(category);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //}


    public class CategoriesController : Controller
    {
        private readonly VtsContext _context;

        public CategoriesController(VtsContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            ViewBag.role = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //[Authorize(Roles = "Admin")]
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }
       // [Authorize(Roles = "Admin")]
        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //Authorize(Roles = "Admin")]
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //[Authorize(Roles = "Admin")]
        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
      //  [Authorize(Roles = "Admin")]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
      //  [Authorize(Roles = "Admin")]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }


}
