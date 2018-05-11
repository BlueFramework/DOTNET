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
            UserAccess userDao = new UserAccess();
            return userDao.GetUserByName(userName);
        }

        public bool ValidatePassword(string userName, string password)
        {
            UserAccess userDao = new UserAccess();
            UserInfo user= userDao.GetUserByName(userName);
            if(user.Password== password)
            { }
            return true;
        }
    }
}
