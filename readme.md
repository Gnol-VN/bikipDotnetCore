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
> services.AddControllers();

In Configure method, add
```
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
```
2.2 Create CityController.cs, with 
> [ApiController] and [Route("api/[controller]")] attribute at class level  
[controller] will be resolved as 'city' (prefix of CityController file name)

> [HttpGet("getall")] attribute at method level

For example, 
```
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
```
2.3 Inputs of API 
2.3.1 Path Variable
```
[HttpGet ("getonebyid/{id}")]
public string GetOneCity (long id) 
```




Other
IActionResult: base class for consumer responses (Json, xml, string ... )