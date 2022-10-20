// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticationService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("recommendation-service", "Personas, Interests and Recommendations"),
                new ApiScope("user-profile-service", "User Profile Information"),
                new ApiScope("comment-service", "Leave and Read Comments"),

            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("recommendation-service", "Recomendation service??"){
                    Scopes={ "recommendation-service" },
                    UserClaims =
                    {
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Name,
                    }
                },
                 new ApiResource("comment-service", "Comment service??"){
                    Scopes={ "comment-service" },
                    UserClaims =
                    {
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Name,
                    }
                },
            };


        public static IEnumerable<Client> Clients (IConfiguration conf) =>
            new Client[]
            {
                new Client
                {
                    ClientId = "angular-app",
                    ClientName = "Commendie Web application",

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    RedirectUris = conf.GetSection("Clients:RedirectUris").GetChildren().Select(s => s.Value).ToArray(),
                    PostLogoutRedirectUris = conf.GetSection("Clients:PostLogoutUris").GetChildren().Select(s => s.Value).ToArray(),
                    AllowedCorsOrigins = conf.GetSection("Clients:Cors").GetChildren().Select(s => s.Value).ToArray(),


                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = { "openid", "profile", "recommendation-service", "user-profile-service", "comment-service" }
                },
            };
    }
}