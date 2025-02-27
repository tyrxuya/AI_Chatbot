using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data.Models;

namespace AIChatbot.API
{
    public interface IChatroomBusiness : IBusiness<Chatroom>
    {
        public List<Chatroom> FindByUser(User user);

        public Chatroom? FindByUserAndTitle(User user, string title);

        public void UpdateTitle(int id, string title);
    }
}
