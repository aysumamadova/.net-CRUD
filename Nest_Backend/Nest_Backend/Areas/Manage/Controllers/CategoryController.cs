using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        public readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Categories> category = await _context.Categories.Include(c => c.Products).ToListAsync();
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            Categories category = _context.Categories.Find(id);
            category.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            Categories category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Categories category)
        {
            if (_context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim()) != null) return RedirectToAction(nameof(Index));
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Update(int id)
        {
            Categories categories = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categories == null) return NotFound();

            return View(categories);
        }
        [HttpPost]
        public IActionResult Update(Categories categories)
        {
            Categories c = _context.Categories.FirstOrDefault(x => x.Id == categories.Id);
            if (c == null) return NotFound();

            c.Name = categories.Name;
            c.Logo = categories.Logo;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
