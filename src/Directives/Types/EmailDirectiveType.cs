using Featherbits.HotChocolate.Validation.Types;
using HotChocolate.Types;

namespace Featherbits.HotChocolate.Validation.Directives.Types
{
    public class EmailDirectiveType : ValidationDirectiveType<EmailDirective>
    {
        protected override void Configure(IDirectiveTypeDescriptor<EmailDirective> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("email");
        }
    }
}