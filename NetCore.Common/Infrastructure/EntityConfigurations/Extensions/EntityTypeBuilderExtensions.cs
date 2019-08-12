using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Common.Domain.Model;

namespace NetCore.Common.Infrastructure.EntityConfigurations.Extensions
{
	public static class EntityTypeBuilderExtensions
	{
		public static void ConfigureIdWithSequence<T>(this EntityTypeBuilder<T> builder) where T : Entity
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id)
				   .IsRequired()
				   .ForSqlServerUseSequenceHiLo($"{typeof(T).Name}Sequence");
		}

		public static void ConfigureIdWithIdentity<T>(this EntityTypeBuilder<T> builder) where T : Entity
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id)
				   .IsRequired()
				   .UseSqlServerIdentityColumn();
		}
	}
}