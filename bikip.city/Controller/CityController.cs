using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Bikip.city.Controller
{
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase
    {
        [HttpGet("getall")]
        public IActionResult GetCities()
        {

            return new JsonResult(
                new List<object>()
                {
                    new {id = 1, Name = "Hanoi"},
                    new {id = 2, Name = "HCM City"},
                }
            );
        }

        [HttpGet("getonebyid/{id}")]
        public IActionResult GetOneCity(long id)
        {
            ObjectResult objectResult = new ObjectResult("ID: " + id);
            objectResult.StatusCode = 201;
            return objectResult;
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Custom bad request message");
        }
        
        [HttpGet("unauthorized")]
        public IActionResult GetForbidden()
        {
            return Unauthorized("Custom unauthorized message");
        }
    }
}