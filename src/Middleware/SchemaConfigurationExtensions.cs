using HotChocolate;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public static class SchemaConfigurationExtensions
    {
        public static void UseInputValidation(this ISchemaConfiguration cfg)
        {
            cfg.Use(next => ctx => ctx.Service<IFieldValidatorMiddleware>().Run(ctx, next));
        }
    }
}