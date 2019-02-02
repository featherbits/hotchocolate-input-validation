using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Featherbits.HotChocolate.Validation.Types;
using HotChocolate.Execution;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public class FieldValidatorMiddleware : IFieldValidatorMiddleware
    {
        private readonly IServiceProvider services;

        public FieldValidatorMiddleware(IServiceProvider services)
        {
            this.services = services;
        }

        public Task Run(IMiddlewareContext context, FieldDelegate next)
        {
            var rootCtx = new FieldValidationContext(context.Field.Name);
            
            foreach (var inputField in context.Field.Arguments)
            {
                var directives = GetValidationDirectives(inputField.Directives);
                var value = context.Argument<object>(inputField.Name);
                var ctx = new FieldValidationContext(inputField.Name);
                HandleInput(value, inputField.Type, directives, ctx);
                rootCtx.AddErrored(ctx);
            }

            if (rootCtx.HasErrors())
            {
                var errorsProperty = new ErrorProperty("FieldValidationError", rootCtx);
                var error = new QueryError("Invalid input data", errorsProperty);

                throw new QueryException(error);
            }

            return next.Invoke(context);
        }

        private void HandleType(object value, NonNullType type,
            IReadOnlyCollection<IDirective> directives, FieldValidationContext ctx)
        {
            // non null type is just a wrapper to other types
            // handle input by its inner type
            HandleInput(value, (IInputType)type.Type, directives, ctx);
        }

        private void HandleType(object value, ScalarType type,
            IReadOnlyCollection<IDirective> directives, FieldValidationContext ctx)
        {
            ValidateValue(value, directives, ctx);
        }

        private void HandleType(object value, InputObjectType type,
            IReadOnlyCollection<IDirective> directives, FieldValidationContext ctx)
        {
            if (value == null)
            {
                return;
            }

            ValidateValue(value, directives, ctx);
            
            foreach (var field in type.Fields)
            {
                var fieldValue = field.GetValue(value);
                var fieldDirectives = GetValidationDirectives(field.Directives);
                var childCtx = new FieldValidationContext(field.Name);
                HandleInput(fieldValue, field.Type, fieldDirectives, childCtx);
                ctx.AddErrored(childCtx);
            }
        }

        private void HandleType(object value, ListType type,
            IReadOnlyCollection<IDirective> directives, FieldValidationContext ctx)
        {
            if (value == null)
            {
                return;
            }

            ValidateValue(value, directives, ctx);

            if (type.ElementType is IInputType)
            {
                var list = (IEnumerable)value;
                var elementType = (IInputType)type.ElementType;
                var elementIndex = 0;
                
                foreach (var element in list)
                {
                    var childCtx = new FieldValidationContext((elementIndex++).ToString());
                    HandleInput(element, elementType, null, childCtx);
                    ctx.AddErrored(childCtx);
                }
            }
        }

        private void HandleInput(object value, IInputType type,
            IReadOnlyCollection<IDirective> directives, FieldValidationContext ctx)
        {
            switch (type)
            {
                case NonNullType nnt:
                    HandleType(value, nnt, directives, ctx);
                    return;
                case ScalarType st:
                    HandleType(value, st, directives, ctx);
                    break;
                case InputObjectType iot:
                    HandleType(value, iot, directives, ctx);
                    break;
                case ListType lt:
                    HandleType(value, lt, directives, ctx);
                    break;
            }
        }

        /// <summary>
        /// Validate value by its validation directives
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="directives">Directives that cause validation</param>
        private void ValidateValue(object value, IReadOnlyCollection<IDirective> directives,
            FieldValidationContext ctx)
        {
            if (value == null || directives == null)
            {
                return;
            }

            foreach (var directive in directives)
            {
                var vd = (IValidationDirectiveType)directive.Type;
                var visitor = new ValidationDirectiveVisitor(directive, this.services, value, ctx);
                vd.Accept(visitor);
            }
        }

        private static IReadOnlyCollection<IDirective> GetValidationDirectives(IDirectiveCollection collection)
        {
            return collection.Where(d => d.Type is IValidationDirectiveType).ToList();
        }
    }
}