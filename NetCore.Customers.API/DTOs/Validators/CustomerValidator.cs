using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NetCore.Customers.API.Infrastructure;
using NetCore.Customers.API.Model;

namespace NetCore.Customers.API.DTOs.Validators
{
	public class CustomerValidator : AbstractValidator<CustomerDto>
	{
		public CustomerValidator(CustomerContext dbContext)
		{
			CascadeMode = CascadeMode.StopOnFirstFailure;

			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(200);

			RuleFor(x => x.Address)
				.NotEmpty()
				.MaximumLength(300);

			RuleFor(x => x.ProvinceId)
				.NotEmpty()
				.MustAsync(async (id, ct) => await dbContext.Set<Province>().AnyAsync(x => x.Id == id))
				.WithMessage("The ProvinceId provided doesn't exist");
		}
	}
}