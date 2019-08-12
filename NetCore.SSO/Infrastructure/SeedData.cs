using Microsoft.AspNetCore.Identity;
using NetCore.SSO.Model;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.SSO.Infrastructure
{
	public class SeedData
	{
		public static void IdentitySeedData(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var configDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

				if (!configDbContext.Clients.Any())
				{
					foreach (var client in Config.GetClients())
						configDbContext.Clients.Add(client.ToEntity());

					configDbContext.SaveChanges();
				}

				if (!configDbContext.IdentityResources.Any())
				{
					foreach (var resource in Config.GetIdentityResources())
						configDbContext.IdentityResources.Add(resource.ToEntity());

					configDbContext.SaveChanges();
				}

				if (!configDbContext.ApiResources.Any())
				{
					foreach (var resource in Config.GetApiResources())
						configDbContext.ApiResources.Add(resource.ToEntity());

					configDbContext.SaveChanges();
				}

				Task.Run(async () =>
						 {
							 if (!await roleManager.RoleExistsAsync("Admin"))
							 {
								 var adminRole = new Role
												 {
													 Name = "Admin",
													 NormalizedName = "Administrator"
												 };

								 await roleManager.CreateAsync(adminRole);
							 }

							 if (!await userManager.Users.AnyAsync(x => x.UserName == "admin"))
							 {
								 var adminUser = new User
												 {
													 UserName = "admin",
													 Email = "info@NetCore.com",
													 EmailConfirmed = true,
													 LockoutEnabled = true,
													 SecurityStamp = Guid.NewGuid().ToString()
												 };

								 var result = await userManager.CreateAsync(adminUser, "Passw0rd.");

								 await userManager.AddToRoleAsync(adminUser, "Admin");
								 await userManager.AddClaimAsync(adminUser, new Claim("claim1", "value1"));
							 }
						 }).Wait();
			}
			
		}
	}
}