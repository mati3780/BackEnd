using NetCore.Customers.API.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetCore.Customers.API.DTOs.Extensions
{
	public static class ProvinceExtensions
	{
		public static ProvinceDto Map(this Province value)
		{
			if (value == null)
				return null;

			var dto = new ProvinceDto
					  {
						  Id = value.Id,
						  Name = value.Name
					  };
			return dto;
		}

		public static Dictionary<string, Expression<Func<Province, object>>> GetMappedFields()
		{
			return new Dictionary<string, Expression<Func<Province, object>>>
				   {
					   {nameof(ProvinceDto.Id), x => x.Id},
					   {nameof(ProvinceDto.Name), x => x.Name}
				   };
		}
	}
}