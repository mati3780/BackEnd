using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Common.Infrastructure.EntityConfigurations.Extensions;
using NetCore.Customers.API.Model;

namespace NetCore.Customers.API.Infrastructure.EntityConfigurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ConfigureIdWithSequence();
			
			builder.Property(x => x.Name)
				   .IsRequired()
				   .HasMaxLength(200);

			builder.Property(x => x.Address)
				   .IsRequired()
				   .HasMaxLength(300);

			builder.HasOne(x => x.Province)
				   .WithMany()
				   .HasForeignKey(x => x.ProvinceId)
				   .OnDelete(DeleteBehavior.Restrict)
				   .IsRequired();
		}
	}
}