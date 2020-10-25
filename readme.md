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
            City.CityList.Remove(cityToBeDeleted);
            return NoContent();
        }    

        [HttpPost("posthotel")]
        public IActionResult PostHotel([FromBody] Hotel hotel)
        {
            City.CityList[0].HotelList.Add(hotel);
            return Created("null", City.CityList);
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