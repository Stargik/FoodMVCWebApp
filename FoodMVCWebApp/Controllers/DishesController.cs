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
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Sheets.v4;

namespace FoodMVCWebApp.Controllers
{
    public class DishesController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly IImageService imageService;
        private readonly StaticFilesSettings imgSettings;
        private readonly BlobStaticFilesSettings blobStaticFilesSettings;
        private readonly IGoogleSheetsService googleSheetsService;
        public DishesController(FoodDbContext context, IImageService imageService, IOptions<StaticFilesSettings> imgSettings, IGoogleSheetsService googleSheetsService)
        {
            _context = context;
            this.imageService = imageService;
            this.imgSettings = imgSettings.Value;
            this.googleSheetsService = googleSheetsService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int? id, int? countryId)
        {
            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();
            var foodDbContext = _context.Dishes.Include(d => d.Category).Include(d => d.CuisineCountryType).Include(d => d.DifficultyLevel);
            var food = await foodDbContext.ToListAsync();
            if (id is not null)
            {
                ViewBag.CategoryId = id;
                food = food.Where(c => c.CategoryId == id).ToList();
            }
            if (countryId is not null)
            {
                ViewBag.CuisineCountryTypeId = id;
                food = food.Where(c => c.CuisineCountryTypeId == countryId).ToList();
            }

            return View(food);
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dishes == null)
            {
                return NotFound();
            }
            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();
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
        [Authorize]
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
        [Authorize]
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
                    var result = _context.Add(dish);
                    await _context.SaveChangesAsync();
                    await googleSheetsService.AddDish(result.Entity);

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
        [Authorize]
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
            DishDTO dishDTO = new DishDTO
            {
                Id = dish.Id,
                Title = dish.Title,
                Recipe = dish.Recipe,
                CategoryId = dish.CategoryId,
                DifficultyLevelId = dish.DifficultyLevelId,
                CuisineCountryTypeId = dish.CuisineCountryTypeId
            };
            ViewData["CategoryTitle"] = (await _context.Categories.FirstOrDefaultAsync(c => c.Id == dishDTO.CategoryId)).Title;
            ViewData["CuisineCountryTypeName"] = (await _context.CuisineCountryTypes.FirstOrDefaultAsync(c => c.Id == dishDTO.CuisineCountryTypeId)).Name;
            ViewData["DifficultyLevelName"] = (await _context.DifficultyLevels.FirstOrDefaultAsync(c => c.Id == dishDTO.DifficultyLevelId)).Name;
            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", dishDTO.CategoryId);
            ViewData["CuisineCountryTypeId"] = new SelectList(_context.CuisineCountryTypes, "Id", "Name", dishDTO.CuisineCountryTypeId);
            ViewData["DifficultyLevelId"] = new SelectList(_context.DifficultyLevels, "Id", "Name", dishDTO.DifficultyLevelId);
            return View(dishDTO);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Recipe,Image,CategoryId,DifficultyLevelId,CuisineCountryTypeId")] DishDTO dishDTO)
        {
            if (id != dishDTO.Id)
            {
                return NotFound();
            }

            try
            {
                Dish dish = new Dish
                {
                    Id = dishDTO.Id,
                    Title = dishDTO.Title,
                    Recipe = dishDTO.Recipe,
                    Image = dishDTO.Image?.FileName,
                    CategoryId = dishDTO.CategoryId,
                    DifficultyLevelId = dishDTO.DifficultyLevelId,
                    CuisineCountryTypeId = dishDTO.CuisineCountryTypeId
                };
                if (dishDTO.Image is null)
                {
                    dish.Image = (await _context.Dishes.AsNoTracking().FirstOrDefaultAsync(dish => dish.Id == dishDTO.Id)).Image;
                }
                if (dishDTO.Image is not null)
                {
                    await imageService.Upload(dishDTO.Image);
                }

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

        // GET: Dishes/Delete/5
        [Authorize]
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
        [Authorize]
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

        [HttpPost]
        public async Task<JsonResult> GetCategoryTitles(string term)
        {
            var categoryTitles = await _context.Categories.Where(c => c.Title.Contains(term)).Select(c => new { c.Id, c.Title }).ToListAsync();
            return Json(categoryTitles);
        }

        [HttpPost]
        public async Task<JsonResult> GetDifficultyLevelsNames(string term)
        {
            var levelsNames = await _context.DifficultyLevels.Where(c => c.Name.Contains(term)).Select(c => new { c.Id, c.Name }).ToListAsync();
            return Json(levelsNames);
        }

        [HttpPost]
        public async Task<JsonResult> GetCuisineCountryTypesNames(string term)
        {
            var countryNames = await _context.CuisineCountryTypes.Where(c => c.Name.Contains(term)).Select(c => new { c.Id, c.Name }).ToListAsync();
            return Json(countryNames);
        }

        private bool DishExists(int id)
        {
            return (_context.Dishes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
