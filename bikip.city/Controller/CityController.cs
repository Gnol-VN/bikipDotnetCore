using System.Collections.Generic;
using CityProject.Model;
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

            return Ok(
                new List<City>()
                {
                    new City(1, "Hanoi"),
                    new City(2, "Dublin"),
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