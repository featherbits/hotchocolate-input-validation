using HotChocolate.Types;

namespace Featherbits.HotChocolate.Validation.Types
{
    public interface IValidationDirectiveTypeVisitor
    {
        void Visit<TDirective>(DirectiveType<TDirective> directiveType)
            where TDirective : class;
    }
}