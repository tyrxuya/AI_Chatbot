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
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=Chatbot; Username=postgres; Password=1234");
        }

        public DbSet<User> Users { get; set; }
    }
}
