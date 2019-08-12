using DelegateDecompiler;
using NetCore.Customers.API.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetCore.Customers.API.DTOs.Extensions
{
	public static class CustomerExtensions
	{
		public static CustomerDto Map(this Customer value)
		{
			if (value == null)
				return null;

			var dto = new CustomerDto
					  {
						  Id = value.Id,
						  Name = value.Name,
						  Address = value.Address,
						  ProvinceId = value.ProvinceId,
						  Province = value.Province.Map().Computed()
					  };
			return dto;
		}

		public static Customer Map(this CustomerDto value, Customer item = null)
		{
			if (value == null)
				return null;

			if (item == null)
				item = new Customer();

			item.Name = value.Name;
			item.Address = value.Address;
			item.ProvinceId = value.ProvinceId;

			return item;
		}

		public static Dictionary<string, Expression<Func<Customer, object>>> GetMappedFields()
		{
			return new Dictionary<string, Expression<Func<Customer, object>>>
				   {
					   { nameof(Customer.Id), x => x.Id },
					   { nameof(Customer.Name), x => x.Name },
					   { nameof(Customer.Address), x => x.Address },
					   { nameof(Customer.ProvinceId), x => x.ProvinceId },
					   { "province.id", x => x.Province.Id },
					   { "province.name", x => x.Province.Name }
				   }; 
		}
	}
}