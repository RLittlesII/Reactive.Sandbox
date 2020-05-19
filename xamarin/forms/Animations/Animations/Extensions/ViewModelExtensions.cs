using System;
using System.Text.RegularExpressions;
using Animations.Login;
using FluentValidation.Results;

namespace Animations.Extensions
{
    public static class ViewModelExtensions
    {
        public static string GetReadableName<T>(this T viewModel)
            where T : class
        {
            var name = viewModel
                .GetType()
                .Name
                .Replace("ViewModel", string.Empty);

            var words = Regex.Split(name, @"(?<!^)(?=[A-Z])");

            return string.Join(" ", words);
        }

        public static Validator<TViewModel> GetValidator<TViewModel>(this TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            if (!(viewModel.Validator is Validator<TViewModel>))
            {
                viewModel.Validator = Activator.CreateInstance<Validator<TViewModel>>();
                viewModel.BuildValidationRules();
            }

            return (Validator<TViewModel>)viewModel.Validator;
        }

        public static ValidationResult Validate<TViewModel>(this TViewModel viewModel)
            where TViewModel : ViewModelBase =>
            viewModel.GetValidator().Validate(viewModel);
    }
}
