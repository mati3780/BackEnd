using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCore.SSO.Infrastructure;
using NetCore.SSO.Model;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace NetCore.SSO
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("SSOConnection");
			var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			services.AddCors(options => options.AddDefaultPolicy(policyBuilder => policyBuilder.AllowAnyHeader()
																							   .AllowAnyMethod()
																							   .AllowAnyOrigin()
																							   .AllowCredentials()));

			services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString))
					.AddIdentity<User, Role>()
					.AddEntityFrameworkStores<IdentityContext>()
					.AddDefaultTokenProviders();

			services.AddMvc()
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			
			var builder = services.AddIdentityServer(options =>
													 {
														 options.Events.RaiseErrorEvents = true;
														 options.Events.RaiseInformationEvents = true;
														 options.Events.RaiseFailureEvents = true;
														 options.Events.RaiseSuccessEvents = true;
													 })
								  .AddConfigurationStore(options =>
														 {
															 options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
																											  sql => sql.MigrationsAssembly(migrationAssembly));
														 })
								  .AddOperationalStore(options =>
													   {
														   options.ConfigureDbContext = b => b.UseSqlServer(connectionString, 
																											sql => sql.MigrationsAssembly(migrationAssembly));
														   options.EnableTokenCleanup = true;
													   })
								  .AddAspNetIdentity<User>()
								  .AddSigningCredential(new X509Certificate2(Configuration.GetSection("Certificates:Signing")["Path"],
																			 Configuration.GetSection("Certificates:Signing")["Password"], X509KeyStorageFlags.MachineKeySet))
								  .AddValidationKeys(GetValidationKeys());


			services.AddHealthChecks()
					.AddSqlServer(connectionString, name: "DB");
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment() || env.IsEnvironment("Testing"))
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHealthChecks("/health", new HealthCheckOptions
										   {
											   Predicate = _ => true,
											   ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
										   })
			   .UseCors()
			   .UseHttpsRedirection()
			   .UseStaticFiles()
			   .UseIdentityServer()
			   .UseMvcWithDefaultRoute();
			
			//SeedData.IdentitySeedData(app);
		}

		private AsymmetricSecurityKey[] GetValidationKeys()
		{
			var keys = Configuration.GetSection("Certificates:Validation").Get<string[]>()
									 .Select(path => (AsymmetricSecurityKey) new X509SecurityKey(new X509Certificate2(path)))
									 .ToArray();
			return keys;
		}
	}
}
