using System;

namespace Featherbits.HotChocolate.Validation
{
    public class ValidationResult
    {
        private ValidationResult(bool validity)
        {
            Valid = validity;
        }

        public ValidationResult(string errorMessage)
            : this(false)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(nameof(errorMessage));
            }

            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
        public bool Valid { get; }

        public static readonly ValidationResult ValidResult = new ValidationResult(true);
    }
}