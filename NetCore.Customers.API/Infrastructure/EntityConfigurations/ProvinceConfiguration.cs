using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Common.Infrastructure.EntityConfigurations.Extensions;
using NetCore.Customers.API.Model;

namespace NetCore.Customers.API.Infrastructure.EntityConfigurations
{
	public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
	{
		public void Configure(EntityTypeBuilder<Province> builder)
		{
			builder.ConfigureIdWithIdentity();
			
			builder.Property(x => x.Name)
				   .IsRequired()
				   .HasMaxLength(100);
		}
	}
}