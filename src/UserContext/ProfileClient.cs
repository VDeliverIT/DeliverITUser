using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace UserContext
{
    public class ProfileClient
    {
        private const string _CookieName = "DeliverIT.Profile";

        public void LogIn(ClientToken aClientToken, HttpContext currentContext, string cookieName)
        {
            SetTokenCookie(currentContext, aClientToken, cookieName);
        }

        private bool CreateCookie(HttpContext currentContext, string value, string cookieName, string subdomainName)
        {
            CookieOptions aCookieOptions = new CookieOptions();
            aCookieOptions.HttpOnly = true;
            // aCookieOptions.Domain = "deliverit.com";
            currentContext.Response.Cookies.Append(cookieName, value, aCookieOptions);
            return true;
        }
        private bool SetTokenCookie(HttpContext currentContext, ClientToken aToken, string cookieName)
        {
            ClientHasher.Hash(ref aToken);
            CookieSerializer cs = new CookieSerializer();
            string cookiestring = cs.Serialize(aToken);
            CookieEncrypter cE = new CookieEncrypter();
            byte[] binaryToken = cE.Encrypt(cookiestring);
            string encodedCookie = WebUtility.UrlEncode(Convert.ToBase64String(binaryToken));
            if (currentContext.Items.ContainsKey(cookieName))
            {
                currentContext.Items.Remove(cookieName);
            }
            currentContext.Items.Add(cookieName, encodedCookie);
            CreateCookie(currentContext, encodedCookie, cookieName, aToken.SubDomainName);

            return true;
        }
        public ClientToken GetCurrentUser(HttpContext currentContext, string cookieName)
        {
            return GetCurrentUserToken(currentContext, cookieName);
        }
        public ClientToken GetCurrentUser(HttpContext currentContext)
        {
            return GetCurrentUserToken(currentContext, _CookieName);
        }
        private ClientToken GetCurrentUserToken(HttpContext currentContext, string cookieName)
        {
            ClientToken clientToken = null;
            if (currentContext.Items.ContainsKey(cookieName))
            {
                try
                {
                    string cookieVal = currentContext.Items[cookieName].ToString();
                    string decodedCookie = WebUtility.UrlDecode(cookieVal);

                    CookieEncrypter cE = new CookieEncrypter();

                    string decryptedCookie = cE.Decrypt(Convert.FromBase64String(decodedCookie));
                    CookieSerializer cs = new CookieSerializer();
                    clientToken = cs.Deserialize(decryptedCookie);
                    if (clientToken != null)
                    {
                        return clientToken;
                    }
                    var aCookie = currentContext.Request.Cookies[cookieName];
                    cookieVal = aCookie;
                    decodedCookie = WebUtility.UrlDecode(cookieVal);
                    decryptedCookie = cE.Decrypt(Convert.FromBase64String(decodedCookie));
                    clientToken = cs.Deserialize(decryptedCookie);
                    if (clientToken == null)
                    {
                        currentContext.Items.Remove(cookieName);
                    }
                }
                catch (Exception)
                {
                    currentContext.Items.Remove(cookieName);
                }
            }
            else
            {
                try
                {
                    CookieEncrypter cE = new CookieEncrypter();
                    CookieSerializer cs = new CookieSerializer();
                    var aCookie = currentContext.Request.Cookies[cookieName];
                    if (aCookie == null) return null;
                    string cookieVal = aCookie;
                    string decodedCookie = WebUtility.UrlDecode(cookieVal);
                    string decryptedCookie = cE.Decrypt(Convert.FromBase64String(decodedCookie));
                    clientToken = cs.Deserialize(decryptedCookie);
                    if (clientToken != null)
                    {
                        return clientToken;
                    }
                    currentContext.Items.Remove(cookieName);
                }
                catch (Exception)
                {
                    currentContext.Items.Remove(cookieName);
                }
            }
            return null;
        }
    }
}
