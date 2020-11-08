using System;
using System.Collections.Generic;
using System.Linq;
using CityProject.Models;
using CityProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bikip.city.Controller
{
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;
        private readonly IMailService _mailService;

        public CityController(ILogger<CityController> logger, IMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentException(nameof(mailService));
        }

        [HttpGet("getall")]
        public IActionResult GetCities()
        {
            _logger.LogInformation("Custom logging message");
            return Ok(CityDto.CityList);
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
            CityDto cityToBeDeleted = CityDto.CityList.FirstOrDefault(elem => elem.Id == id);
            CityDto.CityList.Remove(cityToBeDeleted);
            _mailService.Send();
            return NoContent();
        }    

        [HttpPost("posthotel")]
        public IActionResult PostHotel([FromBody] HotelDto hotel)
        {
            CityDto.CityList[0].HotelList.Add(hotel);
            return Created("null", CityDto.CityList);
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