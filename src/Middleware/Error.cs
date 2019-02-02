using System;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public class Error
    {
        public Error(string validatorName, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(validatorName))
            {
                throw new ArgumentException(nameof(validatorName));
            }
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(nameof(errorMessage));
            }
            ValidatorName = validatorName;
            ErrorMessage = errorMessage;
        }

        public string ValidatorName { get; }
        public string ErrorMessage { get; }
    }
}