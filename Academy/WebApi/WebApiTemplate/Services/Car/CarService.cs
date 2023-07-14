using ErrorOr;
using WebApiTemplate.Models;
using WebApiTemplate.Services.Car;
using WebApiTemplate.ServiceErrors;
using WebApiTemplate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using WebApiTemplate.Lib;
namespace WebApiTemplate.Services.Car;

public class CarService : ICarService
{
    private readonly DatabaseContext _context;
    private readonly Mapper _mapper;

    public CarService() {}
    public CarService(DatabaseContext ctx)
    {
        _context = ctx;
        _mapper = MapperConfig.InitializeAutomapper();
    }

    public ErrorOr<Created> CreateCar(CarModel car)
    {
        var carModel = _mapper.Map<Data.Models.Car>(car);

        _context.Add(carModel);
        _context.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteCar(int id)
    {
        var carDbResult = _context.Cars.FirstOrDefault(c => c.Id == id);

        if (carDbResult != null)
        {
            _context.Remove(id);
            _context.SaveChanges();
            return Result.Deleted;
        }
        else
        {
            return Errors.Car.NotFound;
        }
    }

    public ErrorOr<CarModel> GetCar(int id)
    {
        var carDbResult = _context.Cars.FirstOrDefault(c => c.Id == id);
        var carModel = _mapper.Map<CarModel>(carDbResult);
        
        if (carDbResult != null)
        {
            return carModel;
        }

        _context.SaveChanges();
        return Errors.Car.NotFound;
    }

    public ErrorOr<UpsertedCar> UpsertCar(CarModel carTest)
    {
        var carDbResult = _context.Cars.FirstOrDefault(c => c.Id == carTest.Id);
        var carDbModel = _mapper.Map<Data.Models.Car>(carTest);

        if (carDbResult != null)
        {
            // The basic idea was: Lets use an auto mapper so i dont need to define
            // every property, but after mapping:
            // carDbResult object != carDbModel object.
            // In that case entity does not recognize as a pending update row
            // carDbResult = carDbModel

            carDbResult.Name = carTest.Name;
            carDbResult.Description = carTest.Description;
            carDbResult.StartDateTime = carTest.StartDateTime;
            carDbResult.EndDateTime = carTest.EndDateTime;
            carDbResult.LastModifiedDateTime = carTest.LastModifiedDateTime;
        }
        else
        {
            carDbModel.Id = default;
            _context.Cars.Add(carDbModel);
        }
        _context.SaveChanges();
        return new UpsertedCar(carDbResult == null);
    }
}

