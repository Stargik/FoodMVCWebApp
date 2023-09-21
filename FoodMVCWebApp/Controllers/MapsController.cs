using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodMVCWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly IMapsService mapsService;
        public MapsController(IMapsService mapsService)
        {
            this.mapsService = mapsService;
        }

        [HttpGet("JsonAddressList")]
        public async Task<JsonResult> GetAddresses()
        {
            var addressList = await mapsService.GetAddresses();

            return new JsonResult(addressList);
        }
    }
}
