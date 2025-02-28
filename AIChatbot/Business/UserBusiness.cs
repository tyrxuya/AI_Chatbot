﻿using AIChatbot.API;
using AIChatbot.Data;
using AIChatbot.Data.Models;

namespace AIChatbot.Business
{
    public class UserBusiness(ChatbotDbContext dbContext) : IUserBusiness
    {
        private readonly ChatbotDbContext dbContext = dbContext;

        public List<User> GetAll()
        {
            return dbContext.Users.ToList();
        }

        public void Add(User user)
        {
            if (dbContext.Users.Find(user.Id) != null)
            {
                return;
            }

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public User? Find(User user)
        {
            return dbContext.Users
                    .Where(u => u.Username == user.Username && u.Password == user.Password)
                    .FirstOrDefault();
        }

        public bool FindByUsername(string username)
        {
            return dbContext.Users
                    .Where(u => u.Username == username)
                    .Any();
        }

        public void Remove(int id)
        {
            User? user = dbContext.Users.Find(id);

            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}
