using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetCore.Common.Swagger
{
	public class AuthorizeCheckOperationFilter : IOperationFilter
	{
		private readonly IConfiguration _configuration;

		public AuthorizeCheckOperationFilter(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Apply(Operation operation, OperationFilterContext context)
		{
			var allowAnonymous = context.MethodInfo
										.DeclaringType?
										.GetCustomAttributes(true)
										.OfType<AllowAnonymousAttribute>()
										.Any() ?? false;
			if (allowAnonymous)
				return;

			operation.Responses.Add(((int)HttpStatusCode.Unauthorized).ToString(), 
									new Response { Description = HttpStatusCode.Unauthorized.ToString() });
			operation.Responses.Add(((int)HttpStatusCode.Forbidden).ToString(),
									new Response { Description = HttpStatusCode.Forbidden.ToString() });
			operation.Security = new List<IDictionary<string, IEnumerable<string>>>
								 {
									 new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { _configuration.GetSection("SSO")["ApiName"] }}}
								 };
		}
	}
}