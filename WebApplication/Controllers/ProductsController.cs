using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataContext;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopDbContext _context;
        public ProductsController(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category)
        {
            if (category == null)
                return View(await _context.Products.ToListAsync());
            else
                return View(await _context.Products.Where(p => p.IdCategory.Equals(category)).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var product = await _context.Products.Include(p => p.IdCategory).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCategory,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCategory,Name,Price")] Product product)
        {
            if (id != product.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "Name", product.IdCategory);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var product = await _context.Products.Include(p => p.IdCategory).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}