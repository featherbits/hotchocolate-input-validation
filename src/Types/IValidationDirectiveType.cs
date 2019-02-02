using HotChocolate.Types;

namespace Featherbits.HotChocolate.Validation.Types
{
    public interface IValidationDirectiveType
    {
        void Accept(IValidationDirectiveTypeVisitor visitor);
    }
}