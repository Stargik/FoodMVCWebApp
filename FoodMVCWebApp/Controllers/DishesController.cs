using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using FoodMVCWebApp.Models;
using FoodMVCWebApp.Interfaces;
using Microsoft.Extensions.Options;

namespace FoodMVCWebApp.Controllers
{
    public class DishesController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly IImageService imageService;
        private readonly StaticFilesSettings imgSettings;
        public DishesController(FoodDbContext context, IImageService imageService, IOptions<StaticFilesSettings> imgSettings)
        {
            _context = context;
            this.imageService = imageService;
            this.imgSettings = imgSettings.Value;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            ViewData["ImageStoragePath"] = "~/" + imgSettings.Path;
            var foodDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.CuisineCountryType).Include(d => d.DifficultyLevel);
            return View(await foodDbContext.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }
            ViewData["ImageStoragePath"] = "~/" + imgSettings.Path;
            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.CuisineCountryType)
                .Include(d => d.DifficultyLevel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            ViewData["CuisineCountryTypeId"] = new SelectList(_context.CuisineCountryTypes, "Id", "Name");
            ViewData["DifficultyLevelId"] = new SelectList(_context.DifficultyLevels, "Id", "Name");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Recipe,Image,CategoryId,DifficultyLevelId,CuisineCountryTypeId")] DishDTO dishDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Dish dish = new Dish
                    {
                        Id = dishDTO.Id,
                        Title = dishDTO.Title,
                        Recipe = dishDTO.Recipe,
                        Image = dishDTO.Image.FileName,
                        CategoryId = dishDTO.CategoryId,
                        DifficultyLevelId = dishDTO.DifficultyLevelId,
                        CuisineCountryTypeId = dishDTO.CuisineCountryTypeId
                    };

                    await imageService.Upload(dishDTO.Image);
                    _context.Add(dish);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", ex.Message);
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", dishDTO.CategoryId);
            ViewData["CuisineCountryTypeId"] = new SelectList(_context.CuisineCountryTypes, "Id", "Name", dishDTO.CuisineCountryTypeId);
            ViewData["DifficultyLevelId"] = new SelectList(_context.DifficultyLevels, "Id", "Name", dishDTO.DifficultyLevelId);
            return View(dishDTO);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["ImageStoragePath"] = "~/" + imgSettings.Path;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", dish.CategoryId);
            ViewData["CuisineCountryTypeId"] = new SelectList(_context.CuisineCountryTypes, "Id", "Name", dish.CuisineCountryTypeId);
            ViewData["DifficultyLevelId"] = new SelectList(_context.DifficultyLevels, "Id", "Name", dish.DifficultyLevelId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Recipe,Image,CategoryId,DifficultyLevelId,CuisineCountryTypeId")] DishDTO dishDTO)
        {
            if (id != dishDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Dish dish = new Dish
                    {
                        Id = dishDTO.Id,
                        Title = dishDTO.Title,
                        Recipe = dishDTO.Recipe,
                        Image = dishDTO.Image.FileName,
                        CategoryId = dishDTO.CategoryId,
                        DifficultyLevelId = dishDTO.DifficultyLevelId,
                        CuisineCountryTypeId = dishDTO.CuisineCountryTypeId
                    };

                    await imageService.Upload(dishDTO.Image);
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dishDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", dishDTO.CategoryId);
            ViewData["CuisineCountryTypeId"] = new SelectList(_context.CuisineCountryTypes, "Id", "Name", dishDTO.CuisineCountryTypeId);
            ViewData["DifficultyLevelId"] = new SelectList(_context.DifficultyLevels, "Id", "Name", dishDTO.DifficultyLevelId);
            return View(dishDTO);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.CuisineCountryType)
                .Include(d => d.DifficultyLevel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dishes == null)
            {
                return Problem("Entity set 'FoodDbContext.Dishes'  is null.");
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
          return (_context.Dishes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
