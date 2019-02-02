using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public interface IFieldValidatorMiddleware
    {
        Task Run(IMiddlewareContext context, FieldDelegate next);
    }
}