using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Vb.Api.olldcontroller;

public class Staff
{
    [Required]
    [StringLength(maximumLength: 250, MinimumLength = 10)]
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Phone is not valid.")]
    public string? Phone { get; set; }

    [Range(minimum: 30, maximum: 400, ErrorMessage = "Hourly salary does not fall within allowed range.")]
    public decimal? HourlySalary { get; set; }
}

[Route("api/[controller]")]
[ApiController]
[NonController]
public class StaffController : ControllerBase
{
    public StaffController()
    {
    }

    [HttpPost]
    public Staff Post([FromBody] Staff value)
    {
        return value;
    }
}

public class StaffValidator : AbstractValidator<Staff>
{

    public StaffValidator()
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





