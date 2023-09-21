using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMVCWebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodMVCWebApp.Controllers
{
    public class AdditionalInfoController : Controller
    {
        private readonly IMapsService mapsService;
        public AdditionalInfoController(IMapsService mapsService)
        {
            this.mapsService = mapsService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ApiKey"] = await mapsService.GetKKey();
            return View();
        }
    }
}