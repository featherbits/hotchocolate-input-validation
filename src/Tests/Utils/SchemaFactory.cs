using Featherbits.HotChocolate.Validation;
using Featherbits.HotChocolate.Validation.Middleware;
using Featherbits.HotChocolate.Validation.Types;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public static class SchemaFactory
    {
        public static ISchema Create<TMutation, TDirective, TDirectiveType, TValidator>(string schema)
            where TMutation : class
            where TDirective : class
            where TValidator : class, IValidator<TDirective>
            where TDirectiveType : ValidationDirectiveType<TDirective>, new()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IValidator<TDirective>, TValidator>();
            sc.AddSingleton<IFieldValidatorMiddleware, FieldValidatorMiddleware>();
            var sp = sc.BuildServiceProvider();

            return Schema.Create(schema, c =>
            {
                c.RegisterExtendedScalarTypes();
                c.RegisterServiceProvider(sp);
                c.Use(next => ctx => ctx.Service<IFieldValidatorMiddleware>().Run(ctx, next));
                c.BindType<TMutation>();
                c.RegisterDirective<TDirectiveType>();
            });
        }
    }
}