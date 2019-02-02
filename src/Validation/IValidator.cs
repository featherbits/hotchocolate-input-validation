namespace Featherbits.HotChocolate.Validation
{
    /// <summary>
    /// GraphQL directive based validator
    /// </summary>
    /// <typeparam name="TDirective">
    /// CLR type that maps to a GraphQL directive which
    /// signifies validation purpose
    /// </typeparam>
    public interface IValidator<TDirective>
        where TDirective : class
    {
        /// <summary>
        /// Check if given value is valid based on rules for the directive
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="value"></param>
        ValidationResult Validate(TDirective directive, object value);
    }
}