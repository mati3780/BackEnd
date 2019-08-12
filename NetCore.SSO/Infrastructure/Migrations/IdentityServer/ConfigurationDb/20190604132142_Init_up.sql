--CLIENTS
INSERT [dbo].[Clients] ([Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], [AllowAccessTokensViaBrowser], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [BackChannelLogoutSessionRequired], [AllowOfflineAccess], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [Created], [Updated], [LastAccessed], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable]) 
VALUES
(1, N'client', N'oidc', 1, NULL, NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'', NULL, CAST(N'2019-06-04T17:19:29.4892327' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0),
(1, N'angularApp', N'oidc', 1, N'Cliente Angular', NULL, NULL, NULL, 0, 1, 0, 0, 0, 1, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-06-04T17:19:29.7154936' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0),
(1, N'customersApiSwaggerUI', N'oidc', 1, N'Customers API - Swagger UI', NULL, NULL, NULL, 0, 1, 0, 0, 0, 1, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-06-04T17:19:29.7536331' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0),
(1, N'ro.client', N'oidc', 1, NULL, NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-06-04T17:19:29.7541442' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0),
(1, N'mvc', N'oidc', 1, N'MVC Client', NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 1, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-06-04T17:19:29.7545051' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0)
GO

INSERT [dbo].[ClientRedirectUris] ([RedirectUri], [ClientId]) 
VALUES 
(N'https://localhost:44391/swagger/oauth2-redirect.html', 3),
(N'http://localhost:4200/redirect', 2),
(N'http://localhost:4200/silent-renew.html', 2),
(N'http://localhost:5002/signin-oidc', 5)
GO

INSERT [dbo].[ClientScopes] ([Scope], [ClientId]) 
VALUES 
(N'api1', 5),
(N'profile', 5),
(N'openid', 5),
(N'customersApi', 1),
(N'api1', 1),
(N'api1', 4),
(N'apiGateway', 1),
(N'customersApi', 2),
(N'apiGatewayAdmin', 1),
(N'customersApi', 3),
(N'roles', 3),
(N'profile', 3),
(N'openid', 3),
(N'openid', 2),
(N'profile', 2),
(N'roles', 2),
(N'apiGateway', 2),
(N'api1', 2)
GO

INSERT [dbo].[ClientSecrets] ([Description], [Value], [Expiration], [Type], [Created], [ClientId]) 
VALUES 
(NULL, N'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:29.7541496' AS DateTime2), 4), --hashed value = "secret"
(NULL, N'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:29.7545072' AS DateTime2), 5),
(NULL, N'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:29.4902318' AS DateTime2), 1)
GO

INSERT [dbo].[ClientClaims] ([Type], [Value], [ClientId]) 
VALUES 
(N'role', N'google', 1),
(N'role', N'test-role', 1)
GO

INSERT [dbo].[ClientGrantTypes] ([GrantType], [ClientId]) 
VALUES 
(N'client_credentials', 1),
(N'client_credentials', 5),
(N'hybrid', 5),
(N'password', 4),
(N'implicit', 2),
(N'implicit', 3)
GO

INSERT [dbo].[ClientPostLogoutRedirectUris] ([PostLogoutRedirectUri], [ClientId]) 
VALUES 
(N'https://localhost:44391/swagger', 3),
(N'http://localhost:5002/signout-callback-oidc', 5),
(N'http://localhost:4200/logoutredirect', 2)
GO

--API SCOPES
INSERT [dbo].[ApiResources] ([Enabled], [Name], [DisplayName], [Description], [Created], [Updated], [LastAccessed], [NonEditable]) 
VALUES 
(1, N'customersApi', N'Customers API', NULL, CAST(N'2019-06-04T17:19:31.9396295' AS DateTime2), NULL, NULL, 0),
(1, N'api1', N'My API', NULL, CAST(N'2019-06-04T17:19:32.0341394' AS DateTime2), NULL, NULL, 0),
(1, N'apiGateway', N'API Gateway', NULL, CAST(N'2019-06-04T17:19:32.0345406' AS DateTime2), NULL, NULL, 0),
(1, N'apiGatewayAdmin', N'API Gateway Administration', NULL, CAST(N'2019-06-04T17:19:32.0348460' AS DateTime2), NULL, NULL, 0)
GO

INSERT [dbo].[ApiClaims] ([Type], [ApiResourceId]) 
VALUES 
(N'role', 1),
(N'role', 2),
(N'role', 3),
(N'role', 4)
GO

INSERT [dbo].[ApiScopes] ([Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [ApiResourceId]) 
VALUES 
(N'customersApi', N'Customers API', NULL, 0, 0, 1, 1),
(N'api1', N'My API', NULL, 0, 0, 1, 2),
(N'apiGateway', N'API Gateway', NULL, 0, 0, 1, 3),
(N'apiGatewayAdmin', N'API Gateway Administration', NULL, 0, 0, 1, 4)
GO

INSERT [dbo].[ApiSecrets] ([Description], [Value], [Expiration], [Type], [Created], [ApiResourceId]) 
VALUES 
(NULL, N'AUwkP/lg6Hr8hIJkj0HiCE3OdlqgYtzcv04OQ8TbikE=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:31.9566326' AS DateTime2), 1), --hashed value = "api-secret"
(NULL, N'AUwkP/lg6Hr8hIJkj0HiCE3OdlqgYtzcv04OQ8TbikE=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:32.0341556' AS DateTime2), 2),
(NULL, N'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:32.0345469' AS DateTime2), 3), --hashed value = "1"
(NULL, N'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=', NULL, N'SharedSecret', CAST(N'2019-06-04T17:19:32.0348501' AS DateTime2), 4)
GO

--IDENTITY SCOPES
INSERT [dbo].[IdentityResources] ([Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [Updated], [NonEditable]) 
VALUES 
(1, N'openid', N'Your user identifier', NULL, 1, 0, 1, CAST(N'2019-06-04T17:19:31.3272887' AS DateTime2), NULL, 0),
(1, N'profile', N'User profile', N'Your user profile information (first name, last name, etc.)', 0, 1, 1, CAST(N'2019-06-04T17:19:31.3857004' AS DateTime2), NULL, 0),
(1, N'address', N'Your postal address', NULL, 0, 1, 1, CAST(N'2019-06-04T17:19:31.4063804' AS DateTime2), NULL, 0),
(1, N'email', N'Your email address', NULL, 0, 1, 1, CAST(N'2019-06-04T17:19:31.4271974' AS DateTime2), NULL, 0),
(1, N'phone', N'Your phone number', NULL, 0, 1, 1, CAST(N'2019-06-04T17:19:31.4473120' AS DateTime2), NULL, 0),
(1, N'roles', N'Roles', NULL, 1, 0, 1, CAST(N'2019-06-04T17:19:31.4643444' AS DateTime2), NULL, 0)
GO

INSERT [dbo].[IdentityClaims] ([Type], [IdentityResourceId]) 
VALUES 
(N'sub', 1),
(N'phone_number', 5),
(N'email_verified', 4),
(N'email', 4),
(N'address', 3),
(N'updated_at', 2),
(N'locale', 2),
(N'zoneinfo', 2),
(N'birthdate', 2),
(N'phone_number_verified', 5),
(N'gender', 2),
(N'picture', 2),
(N'profile', 2),
(N'preferred_username', 2),
(N'nickname', 2),
(N'middle_name', 2),
(N'given_name', 2),
(N'family_name', 2),
(N'name', 2),
(N'website', 2),
(N'role', 6)
GO