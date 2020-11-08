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

7. Entities  
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

# 8. Database  
1. Database instantiate  
   1.  Create CityDbContext extent DbContext  represent a session with a database.  
   Can have many context for many purposes. 

   For example, `DbSet<City> and DbSet<Hotel>` will create two tablesCities and Hotels (with PK-FK relation).  
The constructor with `Database.EnsureCreated()` to create table if not exist  

    ```
    public class CityDbContext : DbContext 
    {
        public DbSet<City> Cities { get; set; }

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
 
    2. Register CityDbContext to DI container (remember to add Nuget Microsoft.EntityFrameworkCore.SqlServer
)
    ```
        string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CityInfoDB;Trusted_Connection=True;";
        services.AddDbContext<CityDbContext>(builder =>
        {
            builder.UseSqlServer(connString);
        });
    ```
    3. To use ORM method...
  
2 Migration: like Liquibase  
* `Package Manager Console -> Add-migration [migration file name]` this will scan [all context files].OnModelCreating to **generate seeding data**
* `Package Manager Console -> Update-database` apply migration to database

