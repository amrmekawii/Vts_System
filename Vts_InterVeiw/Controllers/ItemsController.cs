using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vts.DAL;

namespace Vts_InterVeiw.Controllers
{
    [Authorize]

    public class ItemsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly VtsContext _context;
        public ItemsController(VtsContext context, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.Category);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        // GET: Items/Create
       // [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = _context.Categories.Select(x=> new SelectListItem( x.Name, x.Id.ToString())).ToListAsync().GetAwaiter().GetResult();
            return View();
        }    
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Parcentage,ImageFile,IsAvailable,CategoryId")] Item item)

        {
            if (ModelState.IsValid)
            {
                //Save Image On The wwwroot/Image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                string extension = Path.GetExtension(item.ImageFile.FileName);
                item.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }
        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,OldPrice,ImageFile,IsAvailable,CategoryId,ImageURL")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = await _context.Items.FindAsync(id);
                    if (existingItem == null)
                    {
                        return NotFound();
                    }

                    if (item.ImageFile != null && item.ImageFile.Length > 0)
                    {
                        // Delete the old image file (if any)
                        if (!string.IsNullOrEmpty(existingItem.ImageURL))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", existingItem.ImageURL);
                            System.IO.File.Delete(oldImagePath);
                        }

                        // Save the new image file
                        string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                        string extension = Path.GetExtension(item.ImageFile.FileName);
                        item.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(_hostEnvironment.WebRootPath, "Image", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        // Use the existing image file
                        item.ImageURL = existingItem.ImageURL;
                    }

                    // Update the item in the database
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }
        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            //Delete from WWWROOT/Image
            var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", item.ImageURL);
            if (System.IO.File.Exists(ImagePath))
                System.IO.File.Delete(ImagePath);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl) 
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                );
            return LocalRedirect(returnUrl);
        }

    }
}
