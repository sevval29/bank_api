using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Vb.Api.olldcontroller;

public class Employee : IValidatableObject
{
    [Required]
    [StringLength(maximumLength: 250, MinimumLength = 10, ErrorMessage = "Invalid Name")]
    public string Name { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Phone is not valid.")]
    public string Phone { get; set; }

    [Range(minimum: 50, maximum: 400, ErrorMessage = "Hourly salary does not fall within allowed range.")]
    [MinLegalSalaryRequired(minJuniorSalary: 50, minSeniorSalary: 200)]
    public double HourlySalary { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var minAllowedBirthDate = DateTime.Today.AddYears(-65);
        if (minAllowedBirthDate > DateOfBirth)
        {
            yield return new ValidationResult("Birthdate is not valid.");
        }
    }
}

public class MinLegalSalaryRequiredAttribute : ValidationAttribute
{
    public MinLegalSalaryRequiredAttribute(double minJuniorSalary, double minSeniorSalary)
    {
        MinJuniorSalary = minJuniorSalary;
        MinSeniorSalary = minSeniorSalary;
    }

    public double MinJuniorSalary { get; }
    public double MinSeniorSalary { get; }
    public string GetErrorMessage() => $"Minimum hourly salary is not valid.";

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var employee = (Employee)validationContext.ObjectInstance;
        var dateBeforeThirtyYears = DateTime.Today.AddYears(-30);
        var isOlderThanThirdyYears = employee.DateOfBirth <= dateBeforeThirtyYears;
        var hourlySalary = (double)value;

        var isValidSalary = isOlderThanThirdyYears ? hourlySalary >= MinSeniorSalary : hourlySalary >= MinJuniorSalary;

        return isValidSalary ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
    }
}

[Route("api/[controller]")]
[ApiController]
[NonController]
public class EmployeeController : ControllerBase
{
    public EmployeeController()
    {
    }

    [HttpPost]
    public Employee Post([FromBody] Employee value)
    {
        if (value.DateOfBirth > DateTime.Now.AddYears(-30) && value.HourlySalary < 200)
        {

        }
        return value;
    }
}

public class EmployeeValidator : AbstractValidator<Employee>
{

    public EmployeeValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email address is not valid.");

        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is required.")
            .Length(10, 250)
            .WithMessage("Name length must be between 10 and 250 characters.");
        RuleFor(x => x.Phone)
                    .Custom((phone, context) =>
                    {
                        if (!IsValidPhoneNumber(phone))
                        {
                            context.AddFailure("Phone is not valid.");
                        }
                    });

        RuleFor(x => x.HourlySalary)
            .InclusiveBetween(30, 400)
            .WithMessage("Hourly salary does not fall within allowed range.");

    }

    private bool IsValidPhoneNumber(string phone)
    {

        return !string.IsNullOrEmpty(phone) && phone.Length == 10 && phone.All(char.IsDigit);
    }

}