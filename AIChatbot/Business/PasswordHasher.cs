using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.API;

namespace AIChatbot.Business
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            byte[] digest = SHA256.HashData(data);

            return Encoding.ASCII.GetString(digest);
        }
    }
}
