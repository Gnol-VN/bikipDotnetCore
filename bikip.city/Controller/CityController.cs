using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Bikip.city.Controller {
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase 
    {
        [HttpGet("getall")]
        public JsonResult GetCities() 
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