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
    public class RangeValidationTests : ValidationTestsBase
    {
        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void InvalidRangeValueTest(int inputValue)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: " + inputValue + ") }");
            AssertError(result, "noop", "input", "range");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidRangeValueTest(int inputValue)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: " + inputValue + ") }");
            AssertValid(result, "noop");
        }

        private static ISchema CreateSchema()
        {
            return SchemaFactory.Create<Mutation, RangeDirective, RangeDirectiveType, RangeValidator>(@"
                type Mutation {
                    noop(input: Int! @range(from: 1, to: 3)): Boolean
                }
            ");
        }
        
        public class Mutation
        {
            public bool Noop(int input) => true;
        }
    }
}
