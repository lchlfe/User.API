using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace MvcCookieAuthSample
{
    public class Config
    {
        //所有可以访问的Resource
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api1", "API Application")
            };
        }

        //客户端
        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client()
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,//模式：混合模式
                    ClientSecrets =//私钥
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =//运行访问的资源
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"

                    },
                    RedirectUris = { "http://localhost:5001/signin-oidc" },//跳转登录到的客户端的地址
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },//跳转登出到的客户端的地址
                    RequireConsent=true,//是否需要用户点击确认进行跳转,改为点击确认后进行跳转
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,//允许脱机访问

                    ClientName = "MVC Client",
                    ClientUri = "http://localhost:5001",
                    LogoUri = "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31",
                    AllowRememberConsent = true,
                }
            };
        }

        //测试用户
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser{
                    SubjectId="10000",
                    Username="wyt",
                    Password="password"
                }
            };
        }

        //定义系统中的资源
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                //这里实际是claims的返回资源
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }
    }
}
