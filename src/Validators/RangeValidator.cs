using System;
using System.Collections;
using Featherbits.HotChocolate.Validation.Directives;

namespace Featherbits.HotChocolate.Validation.Validators
{
    public class RangeValidator : IValidator<RangeDirective>
    {
        public ValidationResult Validate(RangeDirective directive, object value)
        {
            if (IsValueInRange(directive.From, directive.To, value))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult($"Value not in range from {directive.From} to {directive.To}");
        }

        private static bool IsValueInRange(decimal from, decimal to, object value)
        {
            var number = GetNumberValue(value);

            return from <= number && number <= to;
        }

        private static decimal GetNumberValue(object value)
        {
            switch (value)
            {
                case string chars:
                    return chars.Length;
                case int number:
                    return number;
                case decimal number:
                    return number;
                case ICollection collection:
                    return collection.Count;
                case double number:
                    return (decimal)number;
                case uint number:
                    return (decimal)number;
                case byte number:
                    return (decimal)number;
                default:
                    throw new Exception($"Range value handler for type {value.GetType().Name} not implemented");
            }
        }
    }
}