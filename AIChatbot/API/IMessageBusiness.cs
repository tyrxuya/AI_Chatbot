using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data.Models;

namespace AIChatbot.API
{
    public interface IMessageBusiness : IBusiness<Message>
    {
        public List<Message> GetByChatroom(Chatroom chatroom);
    }
}
