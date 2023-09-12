using System;
namespace FoodMVCWebApp.Entities
{
	public class Dish
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Recipe { get; set; }
		public string Image { get; set; }
		public int CategoryId { get; set; }
		public int DifficultyLevelId { get; set; }
        public int CuisineCountryTypelId { get; set; }

        public Category Category { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public CuisineCountryType CuisineCountryType { get; set; }
    }
}

