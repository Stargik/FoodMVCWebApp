using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMVCWebApp.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
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
            ViewData["ApiKey"] = await mapsService.GetKey();
            return View();
        }

    }
}