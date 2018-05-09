using BlueFramework.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.User
{
    public interface IUserProvider
    {
        int AddAccount(UserInfo user);
        int DeleteUser(UserInfo user);
        UserInfo GetUserByName(string userName);
        UserInfo QueryUserById(int userId);
        bool ModifyPassword(int userId, string password);
        bool UpdateAccount(UserInfo user, string oldName);
        List<UserInfo> GetUsers(UserInfo user);
    }
}
