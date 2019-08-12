using NetCore.Common.Domain.Model;

namespace NetCore.Customers.API.Model
{
	public class Customer : Entity
	{
		public string Name { get; set; }
		public string Address { get; set; }

		public long ProvinceId { get; set; }
		public virtual Province Province { get; set; }
	}
}