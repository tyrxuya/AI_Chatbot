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
    public class MessageBusiness(ChatbotDbContext _dbContext) : IMessageBusiness
    {
        private readonly ChatbotDbContext dbContext = _dbContext;

        public void Add(Message message)
        {
            if (dbContext.Messages.Find(message.Id) != null)
            {
                return;
            }

            dbContext.Messages.Add(message);
            dbContext.SaveChanges();
        }

        public Message? Find(Message message)
        {
            return dbContext.Messages.Find(message.Id);
        }

        public List<Message> GetAll()
        {
            return dbContext.Messages
                .Include(m => m.Chatroom)
                .ToList();
        }

        public List<Message> GetByChatroom(Chatroom chatroom)
        {
            return dbContext.Messages
                .Include(m => m.Chatroom)
                .Where(m => m.Chatroom == chatroom)
                .ToList();
        }

        public void Remove(int id)
        {
            Message? message = dbContext.Messages.Find(id);

            if (message != null)
            {
                dbContext.Messages.Remove(message);
                dbContext.SaveChanges();
            }
        }
    }
}
