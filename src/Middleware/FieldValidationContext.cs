using System.Collections.Generic;
using HotChocolate.Language;
using System;

namespace Featherbits.HotChocolate.Validation.Middleware
{
    public class FieldValidationContext
    {
        private readonly List<Error> errors = new List<Error>();
        private readonly List<FieldValidationContext> fields = new List<FieldValidationContext>();

        public string FieldName { get; }
        public IReadOnlyCollection<Error> Errors { get => errors; }
        public IReadOnlyCollection<FieldValidationContext> Fields { get => fields; }

        public FieldValidationContext(string fieldname)
        {
            if (string.IsNullOrWhiteSpace(fieldname))
            {
                throw new ArgumentException(nameof(fieldname));
            }

            FieldName = fieldname;
        }

        public void Add(Error error)
        {
            errors.Add(error);
        }

        public void AddErrored(FieldValidationContext fieldError)
        {
            if (HasErrors(fieldError))
            {
                fields.Add(fieldError);
            }
        }

        private bool HasErrors(FieldValidationContext fieldError)
        {
            return fieldError.Errors.Count > 0 || fieldError.Fields.Count > 0;
        }

        public bool HasErrors()
        {
            return HasErrors(this);
        } 
    }
}