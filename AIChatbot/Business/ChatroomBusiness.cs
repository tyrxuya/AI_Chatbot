using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.API;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIChatbot.Business
{
    public class ChatroomBusiness(ChatbotDbContext _dbContext) : IChatroomBusiness
    {
        private readonly ChatbotDbContext dbContext = _dbContext;

        public void Add(Chatroom chatroom)
        {
            if (dbContext.Chatrooms.Find(chatroom.Id) != null)
            {
                return;
            }

            dbContext.Chatrooms.Add(chatroom);
            dbContext.SaveChanges();
        }

        public Chatroom? Find(Chatroom chatroom)
        {
            return dbContext.Chatrooms.Find(chatroom.Id);
        }

        public List<Chatroom> FindByUser(User user)
        {
            return dbContext.Chatrooms
                .Include(ch => ch.User)
                .Where(ch => ch.UserId == user.Id)
                .ToList();
        }

        public List<Chatroom> GetAll()
        {
            return dbContext.Chatrooms
                .Include(ch => ch.User)
                .ToList();
        }

        public Chatroom? FindByUserAndTitle(User user, string title)
        {
            return dbContext.Chatrooms
                .Include(ch => ch.User)
                .Where(ch => ch.User == user && ch.Title == title)
                .FirstOrDefault();
        }

        public void Remove(int id)
        {
            Chatroom? chatroom = dbContext.Chatrooms.Find(id);

            if (chatroom != null)
            {
                dbContext.Chatrooms.Remove(chatroom);
                dbContext.SaveChanges();
            }
        }

        public void UpdateTitle(int id, string title)
        {
            Chatroom? chatroom = dbContext.Chatrooms.Find(id);

            if (chatroom != null)
            {
                dbContext.Entry(chatroom).State = EntityState.Detached;

                chatroom.Title = title;

                dbContext.Attach(chatroom);
                dbContext.Entry(chatroom).State = EntityState.Modified;

                dbContext.SaveChanges();
            }
        }
    }
}
