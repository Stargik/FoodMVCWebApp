using System;
namespace FoodMVCWebApp.Models
{
	public class CategoryChartDTO
	{
		public int Id { get; set; }
        public string Title { get; set; }
		public List<DifficultyLevelChartDTO> DifficultyLevelDTOs { get; set; }
    }
}

