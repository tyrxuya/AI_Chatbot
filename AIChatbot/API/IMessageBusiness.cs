using AIChatbot.Data.Models;

namespace AIChatbot.API
{
    public interface IMessageBusiness : IBusiness<Message>
    {
        public List<Message> GetByChatroom(Chatroom chatroom);
    }
}
