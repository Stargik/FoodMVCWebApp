﻿using System;
namespace FoodMVCWebApp.Entities
{
	public class CuisineCountryType
	{
		public int Id { get; set; }
        public string Name { get; set; }

        public List<Dish> Dishes { get; set; }
    }
}

