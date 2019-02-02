using System;
using HotChocolate.Types;

namespace Featherbits.HotChocolate.Validation.Types
{
    public abstract class ValidationDirectiveType<TDirective>
        : DirectiveType<TDirective>, IValidationDirectiveType
        where TDirective : class
    {
        public void Accept(IValidationDirectiveTypeVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void Configure(IDirectiveTypeDescriptor<TDirective> descriptor)
        {
            descriptor.Location(DirectiveLocation.ArgumentDefinition | DirectiveLocation.InputFieldDefinition);
        }
    }
}
