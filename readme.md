Ctrl K, V to open markdown tab in VSCode

Use ctrl-shift-P and select "OmniSharp: Select Project" to select the correct project (a .sln file).


>dotnet new sln --name bikipDotnetCore  
dotnet new console --name CityProject --output Bikip.City    
dotnet sln add .\Bikip.City\CityProject.csproj

Program.cs: for configuration (Web host, web server) and start the application

Startup.cs: entry point of the web application

Startup.cs > ConfigureServices method: Dependency Injection: Register services inside 

Startup.cs > Configure method: use the services in ConfigureServices method to response to user requests

Http pipeline contains Middleware(s). Middleware are software components that are assembled into an application pipeline to handle requests and responses. For example,
    Request -> Logger middleware -> Auth middleware -> Route to /api middleware

2) Add an endpoint 
In ConfigureServices method, add
> services.AddMvc();

In Configure method, add
> app.UseMvc()

Attribute-based Routing (recommended) vs Convention-based Routing