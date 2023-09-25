using System;
using FoodMVCWebApp.Entities;

namespace FoodMVCWebApp.Interfaces
{
	public interface IGoogleSheetsService
	{
        public Task AddDish(Dish dish);

    }
}

