using AIChatbot.Data.Models;

namespace AIChatbot.API
{
    public interface IUserBusiness : IBusiness<User>
    {
        public bool FindByUsername(string username);
    }
}
