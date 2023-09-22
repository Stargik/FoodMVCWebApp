using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodMVCWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {

        private readonly FoodDbContext _context;

        public ChartController(FoodDbContext context)
        {
            _context = context;
        }

        [HttpGet("JsonCategoriesList")]
        public async Task<JsonResult> GetCategories()
        {
            var categories = await _context.Categories.Select(c => new CategoryChartDTO
            {
                Id = c.Id,
                Title = c.Title,
                DifficultyLevelDTOs = _context.DifficultyLevels.Include(dl => dl.Dishes)
                    .Select(dl => new DifficultyLevelChartDTO
                    {
                        Id = dl.Id,
                        Name = dl.Name,
                        DishesCount = dl.Dishes.Where(dish => dish.DifficultyLevelId == dl.Id && dish.CategoryId == c.Id).Count()
                    }).ToList()
            }).ToListAsync();
            return new JsonResult(categories);
        }
    }
}
