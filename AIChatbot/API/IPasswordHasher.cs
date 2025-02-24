﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatbot.API
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
    }
}
