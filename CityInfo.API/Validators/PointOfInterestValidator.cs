using CityInfo.API.Models;
using FluentValidation;

namespace CityInfo.API.Validators
{
    public class PointOfInterestValidator : AbstractValidator<PointOfInterest>
    {
        public PointOfInterestValidator()
        {
            RuleFor(poi => poi.Name)
                .NotNull().WithMessage("Name field cannot be null.")
                .NotEmpty().WithMessage("Name field cannot be empty.")
                .MaximumLength(50).WithMessage("Name field must not be greater than 50 characters")
                .NotEqual(poi => poi.Description).WithMessage("Name and Description fields must be different");

            RuleFor(poi => poi.Description)
                .NotNull().WithMessage("Description field cannot be null.")
                .NotEmpty().WithMessage("Description field cannot be empty.")
                .MaximumLength(300).WithMessage("Description field must not be greater than 300 characters");
        }
    }
} 
