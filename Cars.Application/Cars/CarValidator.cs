using Cars.Domain;
using FluentValidation;

namespace Cars.Application.Cars;

public class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        RuleFor(c => c.Brand)
            .NotEmpty()
            .WithMessage("Brand required")
            .MaximumLength(50)
            .WithMessage("Brand cannot be longer than 50 characters");
        
        RuleFor(c => c.Model)
            .NotEmpty()
            .WithMessage("Model required")
            .MaximumLength(50)
            .WithMessage("Model cannot be longer than 50 characters");
        
        RuleFor(c => c.DoorsNumber)
            .InclusiveBetween(2, 5)
            .WithMessage("Doors number must be between 2 and 5");
        
        RuleFor(c => c.LuggageCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Luggage capacity must be greater than or equal to 0");
        
        RuleFor(c => c.EngineCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Engine capacity must be greater than or equal to 0");
        
        RuleFor(c => c.ProductionDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Production date cannot be in the future");

        RuleFor(c => c.AverageFuelConsumption)
            .GreaterThan(0)
            .WithMessage("Average fuel consumption must be greater than 0");
    }
}