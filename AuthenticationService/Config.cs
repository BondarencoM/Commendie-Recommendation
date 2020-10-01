// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

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
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("recommendation-service", "Recomendation service??"){ Scopes={ "recommendation-service" } }
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "angular-app",
                    ClientName = "Commendie Web application",

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,
                    
                    RedirectUris = { "http://localhost:4200/auth-callback" },
                    PostLogoutRedirectUris = { "http://localhost:4200/auth-signout-callback" },
                    AllowedCorsOrigins = { "http://localhost:4200" },


                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = { "openid", "profile", "recommendation-service", "user-profile-service" }
                },
            };
    }
}