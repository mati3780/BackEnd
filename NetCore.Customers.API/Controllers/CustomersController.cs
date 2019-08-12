using DelegateDecompiler;
using DelegateDecompiler.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Common.Domain.Model;
using NetCore.Common.Infrastructure.Context.Extensions;
using NetCore.Common.Validators;
using NetCore.Customers.API.DTOs;
using NetCore.Customers.API.DTOs.Extensions;
using NetCore.Customers.API.Infrastructure;
using NetCore.Customers.API.Model;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.Customers.API.Controllers
{
	[ApiVersion("1")]
	[Route("api/v{apiVersion:ApiVersion}/[controller]")]
	[Produces("application/json")]
	[ApiController]
    public class CustomersController : ControllerBase
	{
		private readonly CustomerContext _dbContext;
		private readonly IValidator<CustomerDto> _validator;

		public CustomersController(CustomerContext dbContext, IValidator<CustomerDto> validator)
		{
			_dbContext = dbContext;
			_validator = validator;
		}

		/// <summary>
		/// Get all Customers
		/// </summary>
		/// <param name="offset">Record from where to start paging</param>
		/// <param name="limit">Amount of items to page</param>
		/// <param name="sort">Sort the query by the fields specified</param>
		/// <response code="200">Returns the items</response>
		[HttpGet("")]
		[ProducesResponseType(typeof(Page<CustomerDto>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get(int offset = 0, int limit = 10, string sort = "")
		{
			var items = await _dbContext.Set<Customer>()
										.AsNoTracking()
										.ApplyFilter(Request.Query, CustomerExtensions.GetMappedFields())
										.ApplySort(sort, nameof(CustomerDto.Id), CustomerExtensions.GetMappedFields())
										.ToPageComputedAsync(offset, limit, x => x.Map().Computed());
			return Ok(items);
		}

		/// <summary>
		/// Get Customer by id
		/// </summary>
		/// <param name="id">Customer id</param>
		/// <response code="200">Returns the item</response>
		/// <response code="404">Item not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(CustomerDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var item = await _dbContext.Set<Customer>()
									   .AsNoTracking()
									   .Include(x => x.Province)
									   .DecompileAsync()
									   .SingleOrDefaultMapAsync(x => x.Id == id, 
																x => x.Map().Computed());
			if (item == null)
				return NotFound();
			return Ok(item);
		}

		/// <summary>
		/// Create new Customer
		/// </summary>
		/// <param name="dto">Customer model</param>
		/// <param name="apiVersion">The API version.</param>
		/// <response code="201">Returns the newly created item.</response>
		/// <response code="400">Validation errors</response>
		[HttpPost]
		[ProducesResponseType(typeof(CustomerDto), (int)HttpStatusCode.Created)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Post(int apiVersion, [CustomizeValidator]CustomerDto dto)
		{
			var item = dto.Map();
			_dbContext.Set<Customer>().Add(item);
			await _dbContext.SaveChangesAsync();

			return Created($"api/v{apiVersion}/Customers/{item.Id}", dto);
		}

		/// <summary>
		/// Update Customer by id
		/// </summary>
		/// <param name="dto">Customer model</param>
		/// <param name="id">Customer id</param>
		/// <response code="200">Update successful</response>
		/// <response code="400">Validation errors</response>
		/// <response code="404">Item not found</response>
		[HttpPut("{id}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Put(long id, [FromBody] CustomerDto dto)
		{
			var item = await _dbContext.Set<Customer>().FindAsync(id);
			if (item == null)
				return NotFound();

			if (!await _validator.IsValid(dto, ModelState))
				return BadRequest(ModelState);

			dto.Map(item);
			await _dbContext.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// Delete Customer by id
		/// </summary>
		/// <param name="id">Customer id</param>
		/// <response code="200">Delete successful</response>
		/// <response code="404">Item not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete(long id)
		{
			var item = await _dbContext.Set<Customer>().FindAsync(id);
			if (item == null)
				return NotFound();

			_dbContext.Set<Customer>().Remove(item);
			await _dbContext.SaveChangesAsync();

			return Ok();
		}
	}
}