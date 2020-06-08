using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api", "API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    RedirectUris = { "http://localhost:5084/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5084/signout-callback-oidc" },
                    AllowedScopes = new List<string> {"openid", "profile", "api"},
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Angular Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = new List<string> {"openid", "profile", "api"},
                    RedirectUris = new List<string> { "http://localhost:4200/authentication/login-callback" },
                    PostLogoutRedirectUris = new List<string> { "http://localhost:4200/authentication/logout-callback" },
                    AllowedCorsOrigins = new List<string> {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
