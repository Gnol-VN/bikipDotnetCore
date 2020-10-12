using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Bikip.city.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase 
    {
        [HttpGet]
        public JsonResult GetCities () 
        {
            return new JsonResult(
                new List<object>() 
                {
                    new {id = 1, Name = "Hanoi"},
                    new {id = 2, Name = "HCM City"},
                }
            );
        }
    }
}