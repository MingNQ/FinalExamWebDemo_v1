using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalExamDemo.Models;

namespace FinalExamDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NewShopContext _context;
        private int pageSize = 6;

        public ProductsController(NewShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ProductByCategory(int? cid, int? pageIndex)
        {
            var products = _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Provider);

            if (cid != null)
            {
				products = _context.Products
						.Where(p => p.CategoryId == cid)
						.Include(p => p.Category)
						.Include(p => p.Provider);
				ViewBag.Cid = cid;
			}

			int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);
			int pageNum = (int)Math.Ceiling(products.Count() / (float)pageSize);
			ViewBag.PageNum = pageNum;
			var result = products.OrderByDescending(p => p.UnitPrice).Skip(pageSize * (page - 1)).Take(pageSize);

			return PartialView("ProductCategory", await result.ToListAsync());
        }

        // GET: Products
        public async Task<IActionResult> Index(int ?pageIndex)
        {
            var newShopContext = _context.Products.Include(p => p.Category).Include(p => p.Provider);

            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);
            int pageNum = (int)Math.Ceiling(newShopContext.Count() / (float)pageSize);
            ViewBag.PageNum = pageNum;
            var result = newShopContext.OrderByDescending(p => p.UnitPrice).Skip(pageSize * (page - 1)).Take(pageSize);

            return View(await result.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitPrice,Image,Available,CategoryId,Description,ProviderId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Id", product.ProviderId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Id", product.ProviderId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UnitPrice,Image,Available,CategoryId,Description,ProviderId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Id", product.ProviderId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (ContainProduct(id))
            {
                return Content("Can't delete this Product");
            }

            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContainProduct(string id)
        {
            if (_context.OrderDetails.Where(o => o.ProductId == id).ToList().Count() > 0 ||
                _context.Comments.Where(c => c.ProductId == id).ToList().Count() > 0)
                return true;
            return false;
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
