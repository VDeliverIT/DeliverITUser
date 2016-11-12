using Microsoft.AspNetCore.Http;
using System;
using UserContext;

namespace DeliverITUser.Models
{
    public static class User
    {
        public static ClientToken GetUserDetailsFromCookie(HttpContext currentContext)
        {
            ProfileClient aProfileClient = new ProfileClient();
            return aProfileClient.GetCurrentUser(currentContext, "DeliverIT.Profile");
        }

        public static void LogUserIn(HttpContext currentContext, string userName)
        {
            ClientToken aClientToken = new ClientToken
            {
                Email = "yali.girijaa@gmail.com",
                FirstName = "Girijaa",
                LastName = "Doraiswamy",
                RoleId = 1,
                UserId = 1,
                UserName = userName,
                IsAuthenticated = true,
                SubDomainName = "shopowner",
                ClientGuid = Guid.NewGuid().ToString(),
            };

            ProfileClient aProfileClient = new ProfileClient();
            aProfileClient.LogIn(aClientToken, currentContext, "DeliverIT.Profile");
        }

        public static void SetAnonymousCookie(HttpContext currentContext)
        {
            ClientToken aClientToken = new ClientToken
            {
                Email = "",
                FirstName = "",
                LastName = "",
                RoleId = 0,
                UserId = 0,
                UserName = "",
                IsAuthenticated = true,
                SubDomainName = "user",
                ClientGuid = Guid.NewGuid().ToString(),
            };

            ProfileClient aProfileClient = new ProfileClient();
            aProfileClient.LogIn(aClientToken, currentContext, "DeliverIT.Profile");
        }
        public static void Logout(HttpContext currentContext)
        {
            SetAnonymousCookie(currentContext);
        }
    }
}
