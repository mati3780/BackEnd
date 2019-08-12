using NetCore.Common.Domain.Contracts;

namespace NetCore.Common.Domain.Model
{
	public abstract class Entity : IEntity
	{
		/// <summary>
		/// Identifier of the entity
		/// </summary>
		public long Id { get; set; }
	}
}