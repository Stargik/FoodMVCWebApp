using System;
using FoodMVCWebApp.Entities;

namespace FoodMVCWebApp.Models
{
	public class DishDTO
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Recipe { get; set; }
        public IFormFile Image { get; set; } = null!;
        public int CategoryId { get; set; }
        public int DifficultyLevelId { get; set; }
        public int CuisineCountryTypeId { get; set; }
    }
}

