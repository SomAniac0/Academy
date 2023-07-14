namespace WebApiTemplate.ServiceErrors;
using ErrorOr;
using WebApiTemplate.Models;

public static class Errors
{
    public static class Car
    {
        public static Error InvalidName => Error.Validation(
            code: "Car.InvalidName",
            description: $"Car name must be at least {CarModel.MinNameLength}" +
                $" characters long and at most {CarModel.MaxNameLength} characters long.");

        public static Error InvalidDescription => Error.Validation(
            code: "Car.InvalidDescription",
            description: $"Car description must be at least {CarModel.MinDescriptionLength}" +
                $" characters long and at most {CarModel.MaxDescriptionLength} characters long.");

        public static Error NotFound => Error.NotFound(
            code: "Car.NotFound",
            description: "Car not found");
    }
}
