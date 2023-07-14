using ErrorOr;
using WebApiTemplate.Contracts.Car;
using WebApiTemplate.Models;
namespace WebApiTemplate.Services.Car;

public interface ICarService
{
    ErrorOr<Created> CreateCar(CarModel car);
    ErrorOr<CarModel> GetCar(int id);
    ErrorOr<UpsertedCar> UpsertCar(CarModel car);
    ErrorOr<Deleted> DeleteCar(int id);
}
