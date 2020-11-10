Ctrl K, V to open markdown tab in VSCode

Use ctrl-shift-P and select "OmniSharp: Select Project" to select the correct project (a .sln file).


>dotnet new sln --name bikipDotnetCore  
dotnet new console --name CityProject --output Bikip.City    
dotnet sln add .\Bikip.City\CityProject.csproj

Program.cs: for configuration (Web host, web server) and start the application

Startup.cs: entry point of the web application

Startup.cs > ConfigureServices method: Dependency Injection: Register services inside 

Startup.cs > Configure method: use the services in ConfigureServices method to response to user requests

Http pipeline contains Middleware(s). Middleware are software components to decorate a request and respond before sending back to client. For example,
    Request -> Logger middleware -> Auth middleware -> Route to /api middleware

2) Add an endpoint   

2.1 In ConfigureServices method, add
> services.AddMvc();

In Configure method, add
```
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
```
2.2 Create CityController.cs, with 
> [ApiController] and [Route("api/[controller]")] attribute at class level  
<b>[controller]</b> will be resolved as 'city' (prefix of CityController file name)

> Remember extending <b>ControllerBase</b> from Microsoft.AspNetCore.Mvc

> [HttpGet("getall")] <b>attribute</b> at method level

For example, GET, POST, DELETE methods
```
namespace Bikip.city.Controller {
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase 
    {
        [HttpGet("getall")]
        public IActionResult GetCities()
        {
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
            return NoContent();
        }    

        [HttpPost("posthotel")]
        public IActionResult PostHotel([FromBody] HotelDto hotel)
        {
            CityDto.CityList[0].HotelList.Add(hotel);
            return Created("null", CityDto.CityList);
        }
    }
}
```
2.3 Inputs of API 
2.3.1 Path Variable
```
[HttpGet ("getonebyid/{id}")]
public string GetOneCity (long id) 
```

2.3.2 Request Body

    public IActionResult PostHotel([FromBody] Hotel hotel)



Other
IActionResult: base class for consumer responses (Json, xml, string ... )

2.4. Status code  
Example of returning 201
```
public IActionResult GetOneCity(long id)
{
    ObjectResult objectResult = new ObjectResult("ID: " + id);
    objectResult.StatusCode = 201;
    return objectResult;
}
```
or, for short, just `return BadRequest("Custom message") or Unauthorized("..."); or NotFound("...")`

`204`: No content (For PUT and DELETE)

2.5. Input / output formatter  
Each consumer can request a different output format, for example, JSON or XML.  
AspNetCore supports Input and Output formatters by adding AddMvcOptions into Startup.cs / ConfigureServices() method
```
 services.AddMvc()
    .AddMvcOptions(options =>
    {
        options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
        // options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());  To accept input as XML
    })
```

So, <b>in request header</b>, consumer can add `Accept:application/xml` to request XML format.
By default, JSON format is return. Notably, `OutputFormatters` array <b>already</b> has JSON format as first element.

* If encounter `Datacontract exception. Cannot be serialized`, add empty constructor in DTO.
* Do not hard code `return new JsonResult()`. Use `return Ok()` instead


3. Model   

3.4. Required and validated fields

```
    [Required(ErrorMessage = "Custom message: email is required")]
    [EmailAddress(ErrorMessage = "Custom message: email is not in correct format")]
    public string Email { get; set; }
``` 

This namespace `System.ComponentModel.DataAnnotations` provides input validation before mapping into models

4. Dependency Injection   
<b>Inversion of control</b>: to delegate [the instantiation a concrete dependent for a class] to an external component
<b>Dependencies Injection</b>: use a central object - the container - to do two things:
* Instantiate the concrete dependant
* Bind this dependant to the class to use (that class needs a backing field of Interface type to hold the dependant)


Example 1, to inject a logger into a class, we declare an Interface typed backing field `_logger`, and inject it by <b>constructor injection</b> 
```
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
```

Example 2, to get properties from `appsettings.[env].json`
```
        private readonly IConfiguration _configuration;
        public LocalMailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }
```
then get it by `string from = _configuration["mailSetting:from"]`  
Notably, to change [env], right click in project file > Debug section > Environment variable

5. Log to a file  
a. Install NLog.Web.AspNetCore from Nuget  
b. Inject NLog to Program.cs: ` webBuilder.UseNLog();`  
c. Create `nlog.config` file at project level.   

```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
	  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

	<targets>
		<target name="logfile" xsi:type="File" fileName="nlog-${shortdate}.log" />
		<target name="logconsole" xsi:type="Console" />
	</targets>

	<rules>
		<!-- <logger name="*" minlevel="Info" writeTo="logconsole" /> -->
		<logger name="*" minlevel="Info" writeTo="logfile" />
	</rules>
</nlog>
```
d. Log is located in `project\bin\Debug\netcoreapp3.1\nlog-2020-10-26.log`


6. Add a service   
For example, create two mail services: <b>Local</b> and <b>Cloud</b>  
a. Create an interface for a service (For Dependency Injection later)   
b. Create two concrete classes `LocalMailService` and `CloudMailService`   
c. In `Startup.cs > ConfigureServices() method`, add 
```
// For flexible type dependant (prefereable)
services.AddTransient<IMailService, LocalMailService>();
// For fixed type dependant
services.AddTransient<LocalMailService>();
```
d. Change constuctor and backing field of `CityController`, so now Controller can call dependant methods   

# 7. Entities  
1. Create entities with **attributes**  
    * [Key]
    * [DatabaseGenerated(DatabaseGeneratedOption.Identity)]: auto increase ID (based on DB provider)
    * [Required], [EmailAddress]: for validation

    For example, One to Many:  City <--> many Hotels
    ```
     public class City
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }

            [Required]
            public string CityName { get; set; }

            // One to Many implicitly
            public List<Hotel> HotelList { get; set; }

            public City()
            {
                HotelList = new List<Hotel>();
            }
        }
    ```

    Notably, we must explicitly define a foreign key in Hotel entity, by steps:  
      1. Define City field with { get; set;}
      2. Define foreign key property CityID { get; set;}
      3. Annotate navigation property with [ForeignKey("CityID")]
    ```
    public class Hotel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }

            [Required(ErrorMessage = "Custom message: hotel name is required")]
            public string HotelName { get; set; }

            [ForeignKey("CityId")] //Match to property CityId
            public City City { get; set; }

            public long CityId { get; set; }
        }
    ```
2. AutoMapper: to map between Entities <--> DTOs (two-way)  
Mapper maps the properties of the 1st object to the **SAME properties** of 2nd object. For unexisting properties in either side, it just ignores
    1. Setup
    * Add nuget AutoMapper.Extensions.Microsoft.DependencyInjection
    * Add DI in Startup.cs `services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());`  
    2. Create MapperProfiles  
    Mapper Profiles will be scanned and auto-included in bootstrap
    ```
    public class CityMapperProfile : Profile
    {
        public CityMapperProfile()
        {
            // To declare mapping method from 1st object to 2nd object. Can have many mapping methods
            // ReverseMap() to have two-way mapping
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityDToWithoutHotel>().ReverseMap();
        }
    }
    
    public class HotelMapperProfile : Profile
    {
        public HotelMapperProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }

    ```
    3. Usage
        1. Add DI to Controller: private interface field & constructor `private readonly IMapper _mapper;`
        2. Map: `.Map<To>(From)`, for example:
           1. For one: `CityDto cityWithHotels = _mapper.Map<CityDto>(city);`
           2. For list: `List<CityDto> cityWithHotels = _mapper.Map<List<CityDto>>(city);`


# 8. Database  
1. Create DbContext
    1.  Create CityDbContext extent DbContext  represent a session with a database.  
    Can have many context for many purposes. 

    For example, `DbSet<City> and DbSet<Hotel>` will create two tablesCities and Hotels (with PK-FK relation).  
    The constructor with `Database.EnsureCreated()` to create table if not exist  

    ```
    public class CityDbContext : DbContext 
    {
        public DbSet<City> Cities { get; set; } // Queries to DbSet are translated into SQL query

        public DbSet<Hotel> Hotels { get; set; }

        public CityDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding data
            modelBuilder.Entity<City>()
                .HasData(
                    new City{Id = 1, CityName = "Hanoi"},
                    new City{Id = 2, CityName = "Dublin"}
                    );
            
            modelBuilder.Entity<Hotel>()
                .HasData(
                    new Hotel{Id = 1, HotelName = "Hilton", CityId = 1, Email = "hilton@gmail.com"},
                    new Hotel{Id = 2, HotelName = "Movenpick", CityId = 2, Email = "movenpick@gmail.com"}
                );
        }      
    }
    ```  
 
    2. Register CityDbContext to DI container (remember to add Nuget Microsoft.EntityFrameworkCore.SqlServer)
    ```
        string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CityInfoDB;Trusted_Connection=True;";
        services.AddDbContext<CityDbContext>(builder =>
        {
            builder.UseSqlServer(connString);
        });
    ```
2. Create Repository with ORM methods  
    1. Create Interface  
    Normally, use IEnumerable to retrive list of objects. We can instead use IQueryable for building complex query.
    ```
     public interface ICityRepository
        {
            IEnumerable<City> GetCities(); 
            City GetCity(long cityId, bool includeHotels);
            IEnumerable<Hotel> GetHotelsInOneCity(long cityId);
            Hotel GetHotel(long cityId, long hotelId);
            bool IsCityExists(long cityId);
            bool Save();
        }

    ```
    2. Create Concrete repository with DbContext injected
    3. Register DI on Startup.cs
    >services.AddScoped<ICityRepository, CityRepository>();  // AddScoped(): for each session  

    This contains concrete ORM methods.  
    Controller -> Service -> Repo -> Db Context. Repo works with DbContext to CRUD entity, **via context.DbSet<>**.  
    * To get one object, use context.dbset.where(column**s**).**FirstOrDefault()**
    * To get all list, use context.dbset.where(column**s**).**ToList()**    
    
    4. Implementation
   
    ```
    public class CityRepository : ICityRepository
    {
        private readonly CityDbContext _cityDbContext; // Concrete type for injection

        public CityRepository(CityDbContext dbContext)
        {
            _cityDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<City> GetCities()
        {
            return _cityDbContext.Cities.ToList();
        }

        public City GetCity(long cityId, bool includeHotels)
        {
            if (includeHotels)
            {
                return _cityDbContext.Cities
                    .Include(city => city.HotelList)  // To include navigational properties (Foreign properties)
                    .FirstOrDefault(city => city.Id == cityId);
            }

            return _cityDbContext.Cities.FirstOrDefault(city => city.Id == cityId);
        }

        public IEnumerable<Hotel> GetHotelsInOneCity(long cityId)
        {
            return _cityDbContext.Hotels.Where(hotel => hotel.CityId == cityId).ToList();
        }

        public Hotel GetHotel(long cityId, long hotelId)
        {
            return _cityDbContext.Hotels.FirstOrDefault(hotel => hotel.CityId == cityId && hotel.Id == hotelId);
        }

        public bool IsCityExists(long cityId)
        {
            return _cityDbContext.Cities.Any(city => city.Id == cityId);
        }
        
        public bool Save()
        {
            return (_cityDbContext.SaveChanges() >= 0);
        }
    }
    ```
    Notably, **Save()** method actually makes change to DB.  
   
3. Example of full flow from Controller -> Repo -> DbContext
```
    [HttpPost("posthoteltocity/{cityId}")]
    public IActionResult PostHotelToCity([FromBody] HotelDto hotelDTO, long cityId)
    {
        City city = _cityRepository.GetCity(cityId, true);
        Hotel hotel = _mapper.Map<Hotel>(hotelDTO);
        city.HotelList.Add(hotel);
        _cityRepository.Save();
        CityDto returnCityDto = _mapper.Map<CityDto>(city);
        return Created("null", returnCityDto);
    }
```
  
# 9. Migration: like Liquibase  
* `Package Manager Console -> Add-migration [migration file name]` this will scan [all context files].OnModelCreating to **generate seeding data**
* `Package Manager Console -> Update-database` apply migration to database

