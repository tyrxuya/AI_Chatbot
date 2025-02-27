﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIChatbot.API
{
    public interface IUserBusiness : IBusiness<User>
    {
        public bool FindByUsername(string username);
    }
}
