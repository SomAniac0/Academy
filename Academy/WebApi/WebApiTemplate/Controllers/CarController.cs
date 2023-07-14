using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Contracts.Car;
using WebApiTemplate.Models;
//using WebApiTemplate.DModels;

using WebApiTemplate.Services.Car;

namespace WebApiTemplate.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CarController : ApiController
{
    private readonly ICarService _carService;
    
    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [AllowAnonymous]
    // POST api/Car
    [HttpPost]
    public IActionResult CreateCar(CreateCarRequest request)
    {
        ErrorOr<CarModel> requestToCarResult = CarModel.From(request);

        if (requestToCarResult.IsError)
        {
            return Problem(requestToCarResult.Errors);
        }

        var car = requestToCarResult.Value;
        ErrorOr<Created> createCarResult = _carService.CreateCar(car);

        var test = createCarResult.Match(
            created => CreatedAtGetCar(car),
            Problem);
        return createCarResult.Match(
            created => CreatedAtGetCar(car),
            Problem);
    }

    // GET api/Car/1
    [HttpGet("{id}")]
    public IActionResult GetCar(int id)
    {
        ErrorOr<CarModel> getCarResult = _carService.GetCar(id);

        return getCarResult.Match(
            car => Ok(MapCarResponse(car)),
            Problem);
    }

    // PUT api/Car/1
    [HttpPut("{id}")]
    public IActionResult UpsertCar(int id, UpsertCarRequest request)
    {
        ErrorOr<CarModel> requestToCarResult = CarModel.From(id, request);

        if (requestToCarResult.IsError)
        {
            return Problem(requestToCarResult.Errors);
        }

        var car = requestToCarResult.Value;
        car.Id = id;
        ErrorOr<UpsertedCar> upsertCarResult = _carService.UpsertCar(car);

        return upsertCarResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetCar(car) : NoContent(),
            Problem);
    }

    // DELETE api/Car/5
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCar(int id)
    {
        ErrorOr<Deleted> deleteCarResult = _carService.DeleteCar(id);

        return deleteCarResult.Match(
            deleted => NoContent(),
            Problem);
    }

    private static CarResponse MapCarResponse(CarModel car)
    {
        return new CarResponse(
            car.Name,
            car.Description,
            car.StartDateTime,
            car.EndDateTime,
            car.LastModifiedDateTime,
            car.Extensions
            );
    }

    private CreatedAtActionResult CreatedAtGetCar(CarModel car)
    {
        return CreatedAtAction(
            actionName: nameof(GetCar),
            routeValues: new { id = car.Id },
            value: MapCarResponse(car));
    }
}