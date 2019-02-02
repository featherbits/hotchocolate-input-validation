using System;
using System.Linq;
using Featherbits.HotChocolate.Validation;
using Featherbits.HotChocolate.Validation.Directives;
using Featherbits.HotChocolate.Validation.Directives.Types;
using Featherbits.HotChocolate.Validation.Middleware;
using Featherbits.HotChocolate.Validation.Validators;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests
{
    public class RangeValidationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void InvalidRangeValueTest(int inputValue)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: " + inputValue + ") }");
            Assert.Contains("noop", result.Data);
            Assert.Null(result.Data["noop"]);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("FieldValidationError", result.Errors.First().Extensions);

            var validationContext = result.Errors.First().Extensions
                .Where(e => e.Key == "FieldValidationError")
                .Select(e => e.Value as FieldValidationContext)
                .First();
            
            Assert.Equal("noop", validationContext.FieldName);
            Assert.Equal(0, validationContext.Errors.Count);
            Assert.Single(validationContext.Fields);
            var input = validationContext.Fields.First();
            Assert.Equal("input", input.FieldName);
            Assert.Single(input.Errors);
            var error = input.Errors.First();
            Assert.Equal("range", error.ValidatorName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidRangeValueTest(int inputValue)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: " + inputValue + ") }");
            Assert.Contains("noop", result.Data);
            Assert.NotNull(result.Data["noop"]);
            Assert.IsType<bool>(result.Data["noop"]);
            Assert.True((bool)result.Data["noop"]);
            Assert.Empty(result.Errors);
        }

        private static ISchema CreateSchema()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IValidator<RangeDirective>, RangeValidator>();
            sc.AddSingleton<IFieldValidatorMiddleware, FieldValidatorMiddleware>();
            var sp = sc.BuildServiceProvider();

            return Schema.Create(@"
                type Mutation {
                    noop(input: Int! @range(from: 1, to: 3)): Boolean
                }
            ", c =>
            {
                c.RegisterExtendedScalarTypes();
                c.RegisterServiceProvider(sp);
                c.Use(next => ctx => ctx.Service<IFieldValidatorMiddleware>().Run(ctx, next));
                c.BindType<Mutation>();
                c.RegisterDirective<RangeDirectiveType>();
            });
        }
        
        public class Mutation
        {
            public bool Noop(int input, IResolverContext ctx)
            {
                return true;
            }
        }
    }
}
