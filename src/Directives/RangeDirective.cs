using Featherbits.HotChocolate.Validation.Types;

namespace Featherbits.HotChocolate.Validation.Directives
{
    public class RangeDirective
    {
        public decimal From { get; set; }
        public decimal To { get; set; }
    }
}