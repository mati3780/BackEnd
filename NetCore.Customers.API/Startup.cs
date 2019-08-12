using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NetCore.Common.Swagger;
using NetCore.Customers.API.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetCore.Customers.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("CustomersContext");
			var ssoSection = Configuration.GetSection("SSO");
			services.AddMvcCore(options => options.Filters
												  .Add(new AuthorizeFilter(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
																		   			.RequireAuthenticatedUser().Build())))
					.AddJsonFormatters()
					.AddAuthorization()
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
					.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());
			
			services.AddAuthorization()
					.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddIdentityServerAuthentication(options =>
													 {
														 options.Authority = ssoSection["Authority"];
														 options.ApiName = ssoSection["ApiName"];
														 options.ApiSecret = ssoSection["ApiSecret"];
														 options.EnableCaching = ssoSection["EnableCaching"].ToLower() == "true";
													 });

			services.AddDbContext<CustomerContext>(options => options.UseSqlServer(connectionString,
																				   sqlOptions =>
																				   {
																					   sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
																					   sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
																				   }));

			// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			// note: the specified format code will format the version as "'v'major[.minor][-status]"
			services.AddVersionedApiExplorer(options =>
											 {
												 options.GroupNameFormat = "'v'VVV";

												 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
												 // can also be used to control the format of the API version in route templates
												 options.SubstituteApiVersionInUrl = true;
											 })
					.AddApiVersioning(options =>
									  {
										  options.Conventions.Add(new VersionByNamespaceConvention());
										  options.ReportApiVersions = true;
									  })
					.AddSwaggerGen(options =>
								   {
									   // resolve the IApiVersionDescriptionProvider service
									   // note: that we have to build a temporary service provider here because one has not been created yet
									   var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
									   
									   //Add security for authenticated APIs
									   options.AddSecurityDefinition("oauth2", 
																	 new OAuth2Scheme
																	 {
																		 Flow = "implicit",
																		 AuthorizationUrl = $"{ssoSection["Authority"]}/connect/authorize",
																		 TokenUrl = $"{ssoSection["Authority"]}/connect/token",
																		 Scopes = new Dictionary<string, string>
																				  {
																					  { ssoSection["ApiName"], "Customers API" }
																				  }
																	 });

									   // add a swagger document for each discovered API version
									   // note: you might choose to skip or document deprecated API versions differently
									   foreach (var description in provider.ApiVersionDescriptions)
										   options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

									   // add a custom operation filter which sets default values
									   options.OperationFilter<SwaggerDefaultValues>();
									   options.OperationFilter<AuthorizeCheckOperationFilter>();
									   
									   // integrate xml comments
									   var xmlFiles = Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.xml");
									   foreach (var xmlFile in xmlFiles)
										   options.IncludeXmlComments(xmlFile);
								   });

			services.AddHealthChecks()
					.AddUrlGroup(new Uri(ssoSection["Authority"]), "SSO")
					.AddSqlServer(connectionString, name: "DB");
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

			app.UseHealthChecks("/health", new HealthCheckOptions
										   {
											   Predicate = _ => true,
											   ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
										   })
			   .UseSwagger()    // Enable middleware to serve generated Swagger as a JSON endpoint.
			   .UseSwaggerUI(options =>
							 {
								 // build a swagger endpoint for each discovered API version
								 foreach (var description in provider.ApiVersionDescriptions)
									 options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
								 options.OAuthClientId(Configuration.GetSection("SSO")["SwaggerUIClient"]);
								 options.OAuthAppName("Customers API - Swagger UI");
							 })
			   .UseHttpsRedirection()
			   .UseAuthentication()
			   .UseMvc();
		}

		private static Info CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new Info
			{
				Title = "Customers API",
				Version = description.ApiVersion.ToString(),
				Description = "API for managing Customers.",
				Contact = new Contact { Name = "NetCore", Email = "info@NetCore.com" },
				TermsOfService = "https://www.NetCore.com"
			};

			if (description.IsDeprecated)
				info.Description += " This API version has been deprecated.";

			return info;
		}
	}
}
