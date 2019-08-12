using NetCore.Common.Domain.Model;

namespace NetCore.Customers.API.DTOs
{
	/// <summary>
	/// Customer
	/// </summary>
	public class CustomerDto : Entity
	{
		/// <summary>
		/// The name of the customer
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The address of the customer
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// The identifier of the province selected for this customer
		/// </summary>
		public long ProvinceId { get; set; }
		/// <summary>
		/// The province where the customer has his address
		/// </summary>
		public ProvinceDto Province { get; set; }
	}
}