using SignalRWebApp.Models;
using SqlSugar;
using System.Reflection;

namespace SignalRWebApp.Service
{
    public class UserService : IUserService
    {
        private readonly ISqlSugarClient _db;
        public UserService(ISqlSugarClient db)
        {
            _db = db;
        }
        public bool CodeFirst()
        {
            try
            {
                //创建数据库
                _db.DbMaintenance.CreateDatabase();
                string nspace = "SignalRWebApp.Models";
                Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "SignalRWebApp.dll")
                    .GetTypes().Where(p => p.Namespace == nspace).ToArray();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables(ass);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public UserInfo GetUser(string name, string password)
        {
            //查询用户是否存在
            UserInfo userInfo = _db.Queryable<UserInfo>().First(x => x.Name == name);
            if (userInfo == null)
            {
                userInfo = new UserInfo();


            }
            else if (userInfo.Password != password)
            {
                return new UserInfo();
            }

            return userInfo;
        }

        public List<UserMessage> GetMessages(int pageIndex, int pageSize)
        {
            return _db.Queryable<UserMessage>().ToOffsetPage(pageIndex, pageSize);
        }
    }
}
