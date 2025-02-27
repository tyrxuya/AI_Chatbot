﻿using System.Security.Cryptography;
using System.Text;
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
