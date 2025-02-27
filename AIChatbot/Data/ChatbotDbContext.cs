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

        public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(APIConstants.CONNECTION_STRING);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
