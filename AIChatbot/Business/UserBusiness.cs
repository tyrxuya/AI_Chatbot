using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data;
using AIChatbot.Data.Models;

namespace AIChatbot.Business
{
    public class UserBusiness
    {
        private ChatbotDbContext dbContext;

        public List<User> GetAll()
        {
            using (dbContext = new ChatbotDbContext())
            {
                return dbContext.Users.ToList();
            }
        }

        public void Add(User user)
        {
            using (dbContext = new ChatbotDbContext())
            {
                if (dbContext.Users.Find(user.Id) != null) 
                {
                    return;
                }

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
        }

        public bool Find(User user)
        {
            using (dbContext = new ChatbotDbContext())
            {
                return dbContext.Users
                    .Where(u => u.Username == user.Username && u.Password == user.Password)
                    .Any();
            }
        }
    }
}
