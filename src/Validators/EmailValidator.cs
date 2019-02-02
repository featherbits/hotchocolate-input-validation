using System.ComponentModel.DataAnnotations;
using Featherbits.HotChocolate.Validation.Directives;
using HotChocolate.Resolvers;

namespace Featherbits.HotChocolate.Validation.Validators
{
    public class EmailValidator : IValidator<EmailDirective>
    {
        public ValidationResult Validate(EmailDirective directive, object value)
        {
            return new EmailAddressAttribute().IsValid(value)
                ? ValidationResult.ValidResult
                : new ValidationResult($"Email address is not of valid format");
        }
    }
}