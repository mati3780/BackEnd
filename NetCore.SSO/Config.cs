using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace NetCore.SSO
{
	public class Config
	{
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResources.Address(),
				new IdentityResources.Email(),
				new IdentityResources.Phone(),
				new IdentityResource("roles", "Roles", new []{ "role" }) { Required = true }
			};
		}

		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
				   {
					   new ApiResource("customersApi", "Customers API", new []{"role"})
					   {
						   ApiSecrets = { new Secret("api-secret".Sha256()) }
					   },
					   new ApiResource("api1", "My API", new []{"role"})
					   {
						   ApiSecrets = { new Secret("api-secret".Sha256()) }
					   },
					   new ApiResource("apiGateway", "API Gateway", new []{ "role" })
					   {
						   ApiSecrets = { new Secret("1".Sha256())}
					   },
					   new ApiResource("apiGatewayAdmin", "API Gateway Administration", new []{ "role" })
					   {
						   ApiSecrets = { new Secret("1".Sha256())}
					   }
				   };
		}

		// clients want to access resources (aka scopes)
		public static IEnumerable<Client> GetClients()
		{
			// client credentials client
			return new List<Client>
			{
				new Client
				{
					ClientId = "client",
					AllowedGrantTypes = GrantTypes.ClientCredentials,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = { "customersApi", "api1", "apiGateway", "apiGatewayAdmin" },
					Claims = new List<Claim> { new Claim("role", "google"), new Claim("role", "test-role") },
					ClientClaimsPrefix = string.Empty
				},

				new Client
				{
					ClientId = "angularApp",
					ClientName = "Cliente Angular",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,
					RequireConsent = false,
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"roles",
						"apiGateway",
						"customersApi",
						"api1"
					},
					RedirectUris =
					{
						"http://localhost:4200/redirect",
						"http://localhost:4200/silent-renew.html"
					},
					PostLogoutRedirectUris = { "http://localhost:4200/logoutredirect" }
				},

				new Client
				{
					ClientId = "customersApiSwaggerUI",
					ClientName = "Customers API - Swagger UI",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,
					RequireConsent = false,
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"roles",
						"customersApi"
					},
					RedirectUris = { "https://localhost:44391/swagger/oauth2-redirect.html" },
					PostLogoutRedirectUris = { "https://localhost:44391/swagger" }
				},

                // resource owner password grant client
                new Client
				{
					ClientId = "ro.client",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = { "api1" }
				},

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
				{
					ClientId = "mvc",
					ClientName = "MVC Client",
					AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},

					RedirectUris = { "http://localhost:5002/signin-oidc" },
					PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"api1"
					},
					AllowOfflineAccess = true
				}
			};
		}

	}
}