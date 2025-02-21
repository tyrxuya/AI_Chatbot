using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIChatbot.Data
{
    public class ChatbotDbContext : DbContext
    {
        public ChatbotDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(APIConstants.CONNECTION_STRING);
        }

        public DbSet<User> Users { get; set; }
    }
}
