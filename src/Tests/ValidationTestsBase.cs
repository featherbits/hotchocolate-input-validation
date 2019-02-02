using System.Linq;
using Featherbits.HotChocolate.Validation.Middleware;
using HotChocolate.Execution;
using Xunit;

namespace Tests
{
    public abstract class ValidationTestsBase
    {
        protected static void AssertValid(ReadOnlyQueryResult result,
            string fieldName)
        {
            Assert.Contains(fieldName, result.Data);
            Assert.NotNull(result.Data[fieldName]);
            Assert.IsType<bool>(result.Data[fieldName]);
            Assert.True((bool)result.Data[fieldName]);
            Assert.Empty(result.Errors);
        }

        protected static void AssertError(ReadOnlyQueryResult result,
            string fieldName, string inputName, string directiveName)
        {
            Assert.Contains(fieldName, result.Data);
            Assert.Null(result.Data[fieldName]);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("FieldValidationError", result.Errors.First().Extensions);

            var validationContext = result.Errors.First().Extensions
                .Where(e => e.Key == "FieldValidationError")
                .Select(e => e.Value as FieldValidationContext)
                .First();
            
            Assert.Equal(fieldName, validationContext.FieldName);
            Assert.Equal(0, validationContext.Errors.Count);
            Assert.Single(validationContext.Fields);
            var input = validationContext.Fields.First();
            Assert.Equal(inputName, input.FieldName);
            Assert.Single(input.Errors);
            var error = input.Errors.First();
            Assert.Equal(directiveName, error.ValidatorName);
        }
    }
}