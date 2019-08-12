using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetCore.ApiGateway
{
	public static class OcelotExtensions
	{
		public static IConfigurationBuilder AddOcelot(this IConfigurationBuilder builder, string folder, IHostingEnvironment env)
		{
			const string primaryConfigFile = "ocelot.json";
			var globalConfigFile = $"ocelot.global.{env.EnvironmentName}.json";
			var subConfigPattern = $@"^ocelot(\.[a-zA-Z0-9]+)?\.{env.EnvironmentName}\.json$";

			var reg = new Regex(subConfigPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			var fileConfiguration = new FileConfiguration();
			var files = new DirectoryInfo(folder)
						.EnumerateFiles()
						.Where(fi => reg.IsMatch(fi.Name) || fi.Name == globalConfigFile)
						.ToList();

			foreach (var file in files)
			{
				if (files.Count > 1 && file.Name.Equals(primaryConfigFile, StringComparison.OrdinalIgnoreCase))
					continue;

				var lines = File.ReadAllText(file.FullName);
				var config = JsonConvert.DeserializeObject<FileConfiguration>(lines);

				if (file.Name.Equals(globalConfigFile, StringComparison.OrdinalIgnoreCase))
					fileConfiguration.GlobalConfiguration = config.GlobalConfiguration;

				fileConfiguration.Aggregates.AddRange(config.Aggregates);
				fileConfiguration.ReRoutes.AddRange(config.ReRoutes);
			}

			var json = JsonConvert.SerializeObject(fileConfiguration);
			File.WriteAllText(primaryConfigFile, json);
			builder.AddJsonFile(primaryConfigFile, false, false);
			return builder;
		}
	}
}