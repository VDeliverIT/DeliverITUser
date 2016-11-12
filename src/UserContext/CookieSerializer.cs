using System;
using System.Text;

namespace UserContext
{
    public class CookieSerializer
    {
        private const char Delimiter = '|';

        private const int ClientHash = 0;
        private const int RememberMe = 1;
        private const int SignedUpDate = 2;
        private const int UserName = 3;
        private const int FirstName = 4;
        private const int LastName = 5;
        private const int Email = 6;
        private const int UserId = 7;
        private const int RoleId = 8;
        private const int ClientGuid = 9;
        private const int SubDomainName = 10;
        public string Serialize(ClientToken token)
        {

            StringBuilder sb = new StringBuilder();
            if (token.IsHashed())
            {
                sb.Append(Convert.ToBase64String(token.ClientHash)).Append(Delimiter);
            }
            else
            {
                sb.Append(Delimiter);
            }
            sb.Append(token.IsAuthenticated.ToString()).Append(Delimiter);
            sb.Append(token.SignedUpDate.Ticks.ToString()).Append(Delimiter);
            sb.Append(token.UserName).Append(Delimiter);
            sb.Append(token.FirstName).Append(Delimiter);
            sb.Append(token.LastName).Append(Delimiter);
            sb.Append(token.Email).Append(Delimiter);
            sb.Append(token.UserId).Append(Delimiter);
            sb.Append(token.RoleId).Append(Delimiter);
            sb.Append(token.ClientGuid).Append(Delimiter);
            sb.Append(token.SubDomainName).Append(Delimiter);

            return sb.ToString();
        }

        public ClientToken Deserialize(string value)
        {
            try
            {
                string[] values = value.Split(Delimiter);
                ClientToken userToken = new ClientToken();
                userToken.SignedUpDate = new DateTime(Convert.ToInt64(values[SignedUpDate]));
                userToken.ClientHash = Convert.FromBase64String(values[ClientHash]);
                userToken.Email = values[Email];
                userToken.FirstName = values[FirstName];
                userToken.LastName = values[LastName];
                userToken.UserName = values[UserName];
                userToken.RoleId = Convert.ToInt32(values[RoleId]);
                userToken.UserId = Convert.ToInt32(values[UserId]);
                userToken.IsAuthenticated = Convert.ToBoolean(values[RememberMe]);
                userToken.ClientGuid = values[ClientGuid];
                userToken.SubDomainName = values[SubDomainName];
                if (ClientHasher.IsValid(userToken))
                {
                    return userToken;
                }
            }
            catch
            {

            }
            return null;
        }
    }
}
