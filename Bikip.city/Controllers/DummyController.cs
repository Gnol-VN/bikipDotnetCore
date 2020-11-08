using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityProject.Contexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        /// <summary>
        /// For DI to CityDbContext, then EnsureCreated and Seeding will be executed
        /// </summary>
        private CityDbContext _context;

        public DummyController(CityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/<DummyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
