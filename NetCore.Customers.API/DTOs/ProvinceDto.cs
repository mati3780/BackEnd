using NetCore.Common.Domain.Model;

namespace NetCore.Customers.API.DTOs
{
	/// <summary>
	/// Province for an address
	/// </summary>
	public class ProvinceDto : Entity
	{
		/// <summary>
		/// The name of the province
		/// </summary>
		public string Name { get; set; }
	}
}