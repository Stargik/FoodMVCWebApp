using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FoodMVCWebApp.Controllers
{
    public class CuisineCountryTypesController : Controller
    {
        private readonly FoodDbContext _context;

        public CuisineCountryTypesController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: CuisineCountryTypes
        public async Task<IActionResult> Index()
        {
              return _context.CuisineCountryTypes != null ? 
                          View(await _context.CuisineCountryTypes.ToListAsync()) :
                          Problem("Entity set 'FoodDbContext.CuisineCountryTypes'  is null.");
        }

        // GET: CuisineCountryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CuisineCountryTypes == null)
            {
                return NotFound();
            }

            var cuisineCountryType = await _context.CuisineCountryTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuisineCountryType == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Dishes", new { countryId = cuisineCountryType.Id });
        }

        // GET: CuisineCountryTypes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CuisineCountryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] CuisineCountryType cuisineCountryType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuisineCountryType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuisineCountryType);
        }

        // GET: CuisineCountryTypes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CuisineCountryTypes == null)
            {
                return NotFound();
            }

            var cuisineCountryType = await _context.CuisineCountryTypes.FindAsync(id);
            if (cuisineCountryType == null)
            {
                return NotFound();
            }
            return View(cuisineCountryType);
        }

        // POST: CuisineCountryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CuisineCountryType cuisineCountryType)
        {
            if (id != cuisineCountryType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuisineCountryType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuisineCountryTypeExists(cuisineCountryType.Id))
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
            return View(cuisineCountryType);
        }

        // GET: CuisineCountryTypes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CuisineCountryTypes == null)
            {
                return NotFound();
            }

            var cuisineCountryType = await _context.CuisineCountryTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuisineCountryType == null)
            {
                return NotFound();
            }

            return View(cuisineCountryType);
        }

        // POST: CuisineCountryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CuisineCountryTypes == null)
            {
                return Problem("Entity set 'FoodDbContext.CuisineCountryTypes'  is null.");
            }
            var cuisineCountryType = await _context.CuisineCountryTypes.FindAsync(id);
            if (cuisineCountryType != null)
            {
                _context.CuisineCountryTypes.Remove(cuisineCountryType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuisineCountryTypeExists(int id)
        {
          return (_context.CuisineCountryTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
