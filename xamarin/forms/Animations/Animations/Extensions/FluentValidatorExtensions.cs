using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Animations.Extensions
{
    public static class FluentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> ToFieldError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule,
            IErrorMessage errorMessageField)
        {
            return rule
                .WithState(vm => errorMessageField)
                .OnFailure((vm, ctx, message) => errorMessageField.ErrorMessage = message);
        }

        public static void SetErrorFromDictionaryKey(this IErrorMessage errorMessageField, IDictionary<string, string> dict, string key)
        {
            if (dict != null && dict.ContainsKey(key))
                errorMessageField.ErrorMessage = dict[key];
        }

        public static ValidationResult ClearValidFields(this ValidationResult This, params IErrorMessage[] fields)
        {
            if (fields == null || !fields.Any() || This.Errors == null || !This.Errors.Any())
                return This;

            foreach (var field in fields)
            {
                if (This.Errors.All(x => x.CustomState != field))
                {
                    field.ErrorMessage = null;
                }
            }

            return This;
        }
    }
}
