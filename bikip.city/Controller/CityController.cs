using System;
using System.Collections.Generic;
using System.Linq;
using CityProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bikip.city.Controller
{
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("getall")]
        public IActionResult GetCities()
        {
            _logger.LogInformation("Custom logging message");
            return Ok(City.CityList);
        }

        [HttpGet("getonebyid/{id}")]
        public IActionResult GetOneCity(long id)
        {
            ObjectResult objectResult = new ObjectResult("ID: " + id);
            objectResult.StatusCode = 201;
            return objectResult;
        }

        [HttpDelete("deletecity/{id}")]
        public IActionResult DeleteOneCity(long id)
        {
            City cityToBeDeleted = City.CityList.FirstOrDefault(elem => elem.id == id);
            cityToBeDeleted.CityName = " a";
            City.CityList.Remove(cityToBeDeleted);
            return NoContent();
        }    

        [HttpPost("posthotel")]
        public IActionResult PostHotel([FromBody] Hotel hotel)
        {
            City.CityList[0].HotelList.Add(hotel);
            return Created("null", City.CityList);
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