using DelegateDecompiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Common.Infrastructure.Context.Extensions;
using NetCore.Customers.API.DTOs;
using NetCore.Customers.API.DTOs.Extensions;
using NetCore.Customers.API.Infrastructure;
using NetCore.Customers.API.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.Customers.API.Controllers
{
	[ApiVersion("1")]
    [Route("api/v{apiVersion:ApiVersion}/[controller]")]
	[Produces("application/json")]
    [ApiController]
    public class ProvincesController : ControllerBase
	{
		private readonly CustomerContext _dbContext;

		public ProvincesController(CustomerContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Get all Provinces
		/// </summary>
		/// <param name="sort">Sort the query by the fields specified</param>
		/// <response code="200">Returns the items</response>
		[HttpGet("")]
		[ProducesResponseType(typeof(List<ProvinceDto>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get(string sort = "")
		{
			var items = await _dbContext.Set<Province>()
										.AsNoTracking()
										.ApplyFilter(Request.Query, ProvinceExtensions.GetMappedFields())
										.ApplySort(sort, nameof(ProvinceDto.Id), ProvinceExtensions.GetMappedFields())
										.ToListMapAsync(x => x.Map().Computed());
			return Ok(items);
		}

		/// <summary>
		/// Get Province by id
		/// </summary>
		/// <param name="id">Province id</param>
		/// <response code="200">Returns the item</response>
		/// <response code="404">Item not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ProvinceDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var item = await _dbContext.Set<Province>().AsNoTracking().SingleAsync(x => x.Id == id);
			if (item == null)
				return NotFound();
			return Ok(item.Map());
		}
	}
}