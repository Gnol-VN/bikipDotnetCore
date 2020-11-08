using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityProject.Entities;
using CityProject.Models;
using CityProject.Repositories;
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
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityController(ILogger<CityController> logger, IMailService mailService, ICityRepository cityRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentException(nameof(mailService));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("getall")]
        public IActionResult GetCities()
        {
            _logger.LogInformation("Custom logging message");
            return Ok(_cityRepository.GetCities());
        }

        [HttpGet("getonebyid/{id}")]
        public IActionResult GetOneCity(long id, bool includeHotels = false)
        {
            City city = _cityRepository.GetCity(id, includeHotels);
            ObjectResult objectResult;
            if (includeHotels)
            {
                CityDto cityWithHotels = _mapper.Map<CityDto>(city);
                objectResult = new ObjectResult(cityWithHotels);
            }
            else
            {
                CityDToWithoutHotel cityWithoutHotel = _mapper.Map<CityDToWithoutHotel>(city);
                objectResult = new ObjectResult(cityWithoutHotel);
            }
            
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