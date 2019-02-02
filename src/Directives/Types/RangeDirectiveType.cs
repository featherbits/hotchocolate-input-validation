using Featherbits.HotChocolate.Validation.Types;
using HotChocolate.Types;

namespace Featherbits.HotChocolate.Validation.Directives.Types
{
    public class RangeDirectiveType : ValidationDirectiveType<RangeDirective>
    {
        protected override void Configure(IDirectiveTypeDescriptor<RangeDirective> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("range");
        }
    }
}