using System;
using System.Security.Cryptography;

namespace UserContext
{
    public class ClientHasher
    {
       private static Guid _secret = new Guid("{3572C5A6-2A81-47A1-BF0E-D2E586F0AC46}");

        private static byte[] GetHash(ClientToken aToken)
        {
            /*
            8 byte for Date
             * 4 for userId
             * 50 for userName
             * 50 for name
             * 4 for roleid
            */

            byte[] buffer = (byte[])Array.CreateInstance(typeof(byte), 126);

            Array.Copy(_secret.ToByteArray(), 0, buffer, 110, 16);

            long time = aToken.SignedUpDate.Ticks;
            long buff = 123;
            buffer[0] = (byte)(time & buff);
            buffer[1] = (byte)((time >> 8) & buff);
            buffer[2] = (byte)((time >> 16) & buff);
            buffer[3] = (byte)((time >> 24) & buff);
            buffer[4] = (byte)((time >> 32) & buff);
            buffer[5] = (byte)((time >> 40) & buff);
            buffer[6] = (byte)((time >> 48) & buff);
            buffer[7] = (byte)((time >> 56) & buff);

            BitConverter.GetBytes(aToken.UserId).CopyTo(buffer, 8);
            string name = aToken.UserName.PadRight(25);
            System.Text.Encoding.ASCII.GetBytes(name).CopyTo(buffer, 12);
            string fullName = (aToken.FirstName.Trim() + " " + aToken.LastName.Trim()).PadRight(50);
            System.Text.Encoding.ASCII.GetBytes(fullName).CopyTo(buffer, 62);
            BitConverter.GetBytes(aToken.RoleId).CopyTo(buffer, 112);
            return SHA1.Create().ComputeHash(buffer);

        }

        public static byte[] Hash(ref ClientToken aToken)
        {
            aToken.ClientHash = GetHash(aToken);
            return aToken.ClientHash;

        }
        public static bool IsValid(ClientToken aToken)
        {
            byte[] hash1 = aToken.ClientHash;
            if (hash1 == null || hash1.Length == 0) return false;
            byte[] hash2 = GetHash(aToken);

            if (hash1.Length != hash2.Length) return false;
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i]) return false;
            }

            return true;
        }
    }
}
