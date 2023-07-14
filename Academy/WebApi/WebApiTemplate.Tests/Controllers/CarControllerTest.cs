using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiTemplate.Contracts.Car;
using WebApiTemplate.Controllers;
using WebApiTemplate.Data;
using WebApiTemplate.Data.Models;
using WebApiTemplate.ServiceErrors;
using WebApiTemplate.Services.Car;
using WebApiTemplate.Tests.Lib;
using Xunit;

namespace WebApiTemplate.Tests.Controllers;

public class CarControllerTest
{
    private readonly CarController _carController;
    private readonly ICarService _carService;
    private readonly Mock<DatabaseContext> _dbContextMock = new();

    public CarControllerTest()
    {
        var cars = new List<Car>
        {
            new Car()
            {
                Id = 1,
                Name = "test111",
                Description = "Test1",
            },
            new Car()
            {
                Id = 2,
                Name = "test2",
                Description = "Test1",
            },
             new Car()
            {
                Id = 10,
                Name = "te",
                Description = "Te",
            }

        }.AsQueryable();

        var mockSet = MockHelper.CreateDbSetMock(cars);
        _dbContextMock.Setup(x => x.Cars).Returns(mockSet.Object);
        _carService = new CarService(_dbContextMock.Object);
        _carController = new CarController(_carService);
    }

    [Fact]
    public void CreateCar_ShouldReturn_Succes()
    {
        // Arrange
        var carRequest = new CreateCarRequest("test1","test1",DateTime.Now,DateTime.Now,new List<string>());
        // Act
        var carTestResult = _carController.CreateCar(carRequest);

        // Assert
        Assert.Equal(typeof(Microsoft.AspNetCore.Mvc.CreatedAtActionResult), carTestResult.GetType());
        Assert.NotNull(carTestResult);
    }

    [Fact]
    public void CreateCar_ShouldReturn_ValidationErrors()
    {
        // Arrange
        var carRequest = new CreateCarRequest("te", "te", DateTime.Now, DateTime.Now, new List<string>());
        // Act
        var carTestResult = _carController.CreateCar(carRequest) as ObjectResult;
        var problemDetails = carTestResult.Value as HttpValidationProblemDetails;
        var hasValidationErrors = problemDetails.Errors.Any(x => x.Key.Equals(Errors.Car.InvalidName.Code) || x.Key.Equals(Errors.Car.InvalidDescription.Code));

        Assert.True(hasValidationErrors);
        Assert.NotNull(carTestResult);
    }

}