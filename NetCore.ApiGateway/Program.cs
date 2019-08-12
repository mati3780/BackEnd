using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace NetCore.ApiGateway
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}
		
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				   .ConfigureLogging(builder => builder.ClearProviders())
				   .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration))
				   .ConfigureAppConfiguration((builderContext, config) => config.AddOcelot("Ocelot", builderContext.HostingEnvironment))
				   .UseStartup<Startup>();
	}
}