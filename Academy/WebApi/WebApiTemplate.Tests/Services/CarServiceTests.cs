using ErrorOr;
using Moq;
using WebApiTemplate.Data;
using WebApiTemplate.Data.Models;
using WebApiTemplate.Models;
using WebApiTemplate.ServiceErrors;
using WebApiTemplate.Services.Car;
using WebApiTemplate.Tests.Lib;

using Xunit;
namespace WebApiTemplate.Tests.Services;
public class CarServiceTests
{
    private readonly ICarService _carService;
    private readonly Mock<DatabaseContext> _dbContextMock = new();

    public CarServiceTests()
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
    }

    [Fact]
    public void GetCar_ShouldReturn_CarModel()
    {
        // Arrange
        int carId = 1;

        // Act
        ErrorOr<CarModel> carTestResult = _carService.GetCar(carId);

        // Assert
        Assert.Equal(typeof(ErrorOr<CarModel>), carTestResult.GetType());
        Assert.NotNull(carTestResult.Value);
        Assert.Equal("test111", carTestResult.Value.Name);
    }

    [Fact]
    public void GetCarById_ShouldReturn_NotFoundError()
    {
        // Arrange
        int carId = 3;

        // Act
        ErrorOr<CarModel> carTestResult = _carService.GetCar(carId);

        // Assert
        Assert.Equal(typeof(ErrorOr<CarModel>), carTestResult.GetType());
        Assert.Equal(Errors.Car.NotFound.Code, carTestResult.FirstError.Code);
    }

    [Fact]
    public void CreateCar_ShouldReturn_ResultCreated()
    {
        // Arrange
        CarModel carModel = new CarModel()
        {
            Name = "TestCardMOdel",
            Description = "Description",
            EndDateTime = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            StartDateTime = DateTime.Now
        };

        // Act
        ErrorOr<Created> carTestResult = _carService.CreateCar(carModel);

        // Assert
        Assert.False(carTestResult.IsError);
        Assert.Equal(typeof(ErrorOr<Created>), carTestResult.GetType());
    }

    [Fact]
    public void DeleteCar_ShouldReturn_CarDeleted()
    {
        // Arrange
        int carId = 3;

        // Act
        ErrorOr<Deleted> carTestResult = _carService.DeleteCar(carId);

        // Assert
        Assert.Equal(typeof(ErrorOr<Deleted>), carTestResult.GetType());
        Assert.True(carTestResult.IsError);
    }

    [Fact]
    public void DeleteCar_ShouldReturn_NotFound()
    {
        // Arrange
        int carId = 2342432;

        // Act
        ErrorOr<Deleted> carTestResult = _carService.DeleteCar(carId);

        // Assert
        Assert.Equal(typeof(ErrorOr<Deleted>), carTestResult.GetType());
        Assert.True(carTestResult.IsError);
        Assert.Equal(Errors.Car.NotFound.Code, carTestResult.FirstError.Code);
    }

    [Fact]
    public void UpsertCar_ShouldReturn_InserResult()
    {
        // Arrange
        CarModel carModel = new CarModel()
        {
            Id = 1043,
            Name = "TestCardMOdel",
            Description = "Description",
            EndDateTime = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            StartDateTime = DateTime.Now
        };

        // Act
        ErrorOr<UpsertedCar> carTestResult = _carService.UpsertCar(carModel);

        // Assert
        Assert.Equal(typeof(ErrorOr<UpsertedCar>), carTestResult.GetType());
        Assert.True(carTestResult.Value.IsNewlyCreated == true);
        Assert.False(carTestResult.IsError);
    }

    [Fact]
    public void UpsertCar_ShouldReturn_UpdateResult()
    {
        // Arrange
        CarModel carModel = new CarModel()
        {
            Id = 10,
            Name = "TestCardMOdel",
            Description = "Description",
            EndDateTime = DateTime.Now,
            LastModifiedDateTime = DateTime.Now,
            StartDateTime = DateTime.Now
        };

        // Act
        ErrorOr<UpsertedCar> carTestResult = _carService.UpsertCar(carModel);
        ;
        // Assert
        Assert.Equal(typeof(ErrorOr<UpsertedCar>), carTestResult.GetType());
        Assert.True(carTestResult.Value.IsNewlyCreated == false);
        //Assert.Equal(Errors.Car.NotFound.Code, carTestResult.FirstError.Code);
    }
}