using System;

namespace UserContext
{
    public class ClientToken
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SignedUpDate { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public byte[] ClientHash { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ClientGuid { get; set; }
        public string SubDomainName { get; set; }
        public bool IsHashed()
        {
            if (ClientHash != null && ClientHash.Length > 0) return true;
            return false;
        }

        public ClientToken()
        {
            IsAuthenticated = false;
        }        
    }
}
