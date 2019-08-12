using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace NetCore.Common.Validators
{
	public static class ValidatorExtensions
	{
		public static async Task<bool> IsValid<T>(this IValidator<T> validator, T instance, ModelStateDictionary modelState)
		{
			var result = await validator.ValidateAsync(instance);
			if (!result.IsValid)
				result.AddToModelState(modelState, null);
			return result.IsValid;
		}
	}
}