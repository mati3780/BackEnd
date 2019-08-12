using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace NetCore.ApiGateway
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var sso = Configuration.GetSection("SSO");
			services.AddAuthentication()
					.AddIdentityServerAuthentication("idsrv", options =>
															  {
																  options.Authority = sso["Authority"];
																  options.ApiName = sso["Scope"];
																  options.ApiSecret = sso["ScopeSecret"];
																  options.EnableCaching = sso["EnableCaching"].ToLower() == "true";
															  });

			services.AddOcelot()
					.AddCacheManager(x => x.WithDictionaryHandle())
					.AddAdministration("/administration", options =>
													  {
														  options.Authority = sso["Authority"];
														  options.ApiName = sso["ScopeAdmin"];
														  options.ApiSecret = sso["ScopeSecretAdmin"];
														  options.EnableCaching = sso["EnableCachingAdmin"].ToLower() == "true";
													  });

			services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
																				   .AllowAnyHeader()
																				   .AllowAnyMethod()
																				   .AllowCredentials()));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseCors()
			   .UseHttpsRedirection();

			app.UseOcelot().Wait();
		}
	}
}
