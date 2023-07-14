using ErrorOr;
using System.ComponentModel.DataAnnotations;
using WebApiTemplate.Contracts.Car;
using WebApiTemplate.ServiceErrors;

namespace WebApiTemplate.Models
{
    public class CarModel
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 50;

        public const int MinDescriptionLength = 5;
        public const int MaxDescriptionLength = 150;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public List<string> Extensions { get; set; }
        private CarModel(
            string name,
            string description,
            DateTime startDateTime,
            DateTime endDateTime,
            DateTime lastModifiedDateTime
            )
        {
            Name = name;
            Description = description;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            LastModifiedDateTime = lastModifiedDateTime;
        }
        private CarModel(
            string name,
            string description,
            DateTime startDateTime,
            DateTime endDateTime,
            DateTime lastModifiedDateTime,
            List<string> extensions
            )
        {
            Name = name;
            Description = description;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            LastModifiedDateTime = lastModifiedDateTime;
            Extensions = extensions;
        }
        public CarModel() { }

        public static ErrorOr<CarModel> Create(
            string name,
            string description,
            DateTime startDateTime,
            DateTime endDateTime,
            List<string> extensions
            )
        {
            List<Error> errors = new();

            if (name.Length is < MinNameLength or > MaxNameLength)
            {
                errors.Add(Errors.Car.InvalidName);
            }

            if (description.Length is < MinDescriptionLength or > MaxDescriptionLength)
            {
                errors.Add(Errors.Car.InvalidDescription);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new CarModel(
                
                name,
                description,
                startDateTime,
                endDateTime,
                DateTime.UtcNow,
                extensions
                );
        }

        public static ErrorOr<CarModel> From(CreateCarRequest request)
        {
            return Create(
                request.Name,
                request.Description,
                request.StartDateTime,
                request.EndDateTime,
                request.Extensions
                );
        }

        public static ErrorOr<CarModel> From(int id, UpsertCarRequest request)
        {
            return Create(
                request.Name,
                request.Description,
                request.StartDateTime,
                request.EndDateTime,
                request.Extensions
                );
        }
    }
}


