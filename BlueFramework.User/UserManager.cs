using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueFramework.User.Models;
using BlueFramework.User.DataAccess;

namespace BlueFramework.User
{
    public class UserManager
    {
        public UserInfo GetUser(int userId)
        {
            UserAccess userDao = new UserAccess();
            return userDao.GetUserInfo(userId);
        }

        public UserInfo GetUser(string userName)
        {
            return GetUser(1);
        }

        public bool ValidatePassword(string userName, string password)
        {
            return true;
        }
    }
}
