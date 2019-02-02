using System.Linq;
using Featherbits.HotChocolate.Validation.Directives;
using Featherbits.HotChocolate.Validation.Directives.Types;
using Featherbits.HotChocolate.Validation.Middleware;
using Featherbits.HotChocolate.Validation.Validators;
using HotChocolate;
using Xunit;

namespace Tests
{
    public class EmailValidationTests : ValidationTestsBase
    {
        [Theory]
        [InlineData("dev@integration.test")]
        public void ValidEmailAddressTest(string email)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: \"" + email + "\") }");
            AssertValid(result, "noop");
        }

        [Theory]
        [InlineData("a@")]
        public void InvalidEmailAddressTest(string email)
        {
            var schema = CreateSchema();
            var result = schema.ExecuteQuery("mutation { noop(input: \"" + email + "\") }");
            AssertError(result, "noop", "input", "email");
        }

        private static ISchema CreateSchema()
        {
            return SchemaFactory.Create<Mutation, EmailDirective, EmailDirectiveType, EmailValidator>(@"
                type Mutation {
                    noop(input: String! @email): Boolean
                }
            ");
        }
        
        public class Mutation
        {
            public bool Noop(string input) => true;
        }
    }
}