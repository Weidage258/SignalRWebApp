using SignalRWebApp.Models;

namespace SignalRWebApp.Service
{
    public interface IUserService
    {
        bool CodeFirst();
        UserInfo GetUser(string name, string password);

        List<UserMessage> GetMessages(int pageIndex, int pageSize);
    }
}
