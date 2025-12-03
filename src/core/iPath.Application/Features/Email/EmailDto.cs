using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace iPath.Application.Features;

public class EmailDto
{
    [EmailAddress, Required]
    public string Address { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class EmailDtoValidator : AbstractValidator<EmailDto>
{
    public EmailDtoValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .Length(1, 200);

        RuleFor(x => x.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<EmailDto>.CreateWithOptions((EmailDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}