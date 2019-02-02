using System;
using Featherbits.HotChocolate.Validation.Types;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public class ValidationDirectiveVisitor : IValidationDirectiveTypeVisitor
    {
        private readonly IDirective directive;
        private readonly IServiceProvider services;
        private readonly object value;
        private readonly FieldValidationContext ctx;

        public ValidationDirectiveVisitor(IDirective directive,
            IServiceProvider services, object value, FieldValidationContext ctx)
        {
            this.directive = directive;
            this.services = services;
            this.value = value;
            this.ctx = ctx;
        }

        public void Visit<TDirective>(DirectiveType<TDirective> directiveType)
            where TDirective : class
        {
            // get CLR type that maps to a GraphQL directive
            TDirective directive = this.directive.ToObject<TDirective>();

            if (directive == null)
            {
                throw new Exception("Given GraphQL directive was not declared by validation directive type");
            }

            // get validation handler for validation directive
            IValidator<TDirective> validator = this.services.GetService<IValidator<TDirective>>();

            if (validator == null)
            {
                throw new Exception($"Validator {typeof(IValidator<TDirective>).Name} not registered");
            }

            var result = validator.Validate(directive, this.value);
            
            if ( ! result.Valid)
            {
                var error = new Error(directiveType.Name, result.ErrorMessage);
                ctx.Add(error);
            }
        }
    }
}