using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace NetCore.Customers.API.Infrastructure
{
	public class CustomerContext : DbContext
	{
		public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
										  .Where(t => t.GetInterfaces()
													   .Any(gi => gi.IsGenericType && 
																  gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
										  .ToList();
			typesToRegister.ForEach(t => modelBuilder.ApplyConfiguration((dynamic)Activator.CreateInstance(t)));
		}
	}
}