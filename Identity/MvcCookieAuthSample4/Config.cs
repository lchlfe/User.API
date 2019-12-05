using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace MvcCookieAuthSample4
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api","My Api")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="mvc",
                    ClientName = "Mvc Client",
                    ClientUri = "http://localhost:5401",
                    //LogoUri = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1535451048122&di=eecda626cfcee796adcaf6eedd35a8a2&imgtype=0&src=http%3A%2F%2Fimg.mukewang.com%2F5a77b61000013ca502560192.jpg",
                    AllowRememberConsent = true,



                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },

                    RequireConsent = true,

                    RedirectUris = {"http://localhost:5401/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5401/signout-callback-oidc"},

                    /*
                     OAuth通常有以下几种endpoint:
                    1. /authorize, 请求token(通过特定的流程flows)
                    2. /token, 请求token(通过特定的流程flows), 刷新token, 使用authorization code来换取token.
                    3. /revocation, 吊销token.

                    OpenId Connect 通常有以下几种 endpoints:
                    1. /userinfo, 获取用户信息
                    2. /checksession, 检查当前用户的session
                    3. /endsession, 终结当前用户的session
                    4. /.well-known/openid-configuration, 提供了authorization server的信息(endpoints列表和配置信息等)
                    5. /.well-known/jwks, 列出了JWT签名key的信息, 它们是用来验证token的.
                     */

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "jesse",
                    Password = "123456"
                }
            };
        }
    }
}
