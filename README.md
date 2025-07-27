

# Test Web API Demo

This is a sample MSTest project using ASP.NET Core. Show that how to test create, read, update and delete (CRUD) todo items APIs. The test agaist on server deployed with github project https://github.com/seymenbahtiyar/Web_API_Demo, following its readme to deploy the server on proper place

## Getting Started

To run this project locally, you will need:

- .NET 9 SDK
- Visual Studio Code or Visual Studio 
- github project https://github.com/seymenbahtiyar/Web_API_Demo running

### Terminal Commands to run Test

Before run test, you need to set BASE_URL environemnt matched your test server url, otherwise it will use default http://localhost:5089
The value of BASE_URL format should be protocol://hostname:port, notice that there is no slash in the ending. 

- Restore the dependencies using `dotnet restore`
- Run the project using `dotnet run`
- Run the project with console log and html report using `dotnet test --logger "html;LogFileName=test-results.html" --logger "console;verbosity=detailed"`


## NuGet Packages

- FluentAssertions            8.5.0      
- Microsoft.NET.Test.Sdk      17.12.0      
- MSTest                      3.6.4            


## License

[MIT] See the details in LICENSE file


