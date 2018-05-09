using BlueFramework.User.DataAccess;
using BlueFramework.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlueFramework.User
{
    /// <summary>
    /// 用户基础功能
    /// </summary>
    public class UserProvider:IUserProvider
    {
        public UserAccess ua = new UserAccess();

        public static IUserProvider Instance
        {
            get
            {
                UserProvider user = new UserProvider();
                return user;
            }
        }

        public int AddAccount(UserInfo user)
        {
            int re = -1;
            user.Password = MD5Encrypt64(user.Password);
            if (GetUserByName(user.UserName).UserName != null)
            {
                re = 3;
                return re;
            }
            if (ua.AddAccount(user))
            {
                re = 4;
            }
            else
            {
                re = -4;
            }
            return re;

        }

        /// <summary>
        /// 删除用户，返回信息
        /// </summary>
        /// <returns>0：失败，1：成功，2：不能删除当前登录用户</returns>
        public int DeleteUser(UserInfo user)
        {
            if (user.UserId == UserContext.CurrentUser.UserId)
                return 2;
            if (ua.Delete(user))
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>返回用户信息</returns>
        public UserInfo GetUserByName(string userName)
        {
            UserInfo ui = ua.GetUserByName(userName);
            return ui;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Id">用户ID</param>
        /// <returns>返回用户信息</returns>
        public UserInfo QueryUserById(int userId)
        {
            UserInfo ui = ua.QueryUserById(userId);
            return ui;
        }

        /// <summary>
        /// 获取用户信息（包括权限）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public UserInfo GetFullUserInfo(string userName)
        {
            SysAccess sa = new SysAccess();
            UserInfo ui = ua.GetUser(userName);
            if (ui.UserName != "")
            {
                //ui.Roles = dataAccess.GetRoles(ui.UserId);
                ////ui.AreaName = sysAccess.GetAreaname(ui.GroupId);
                //ui.MenuRights = dataAccess.GetMenuRights(ui.UserId);
                //ui.DataRights = dataAccess.GetDataRights(ui.UserId);
            }
            return ui;
        }

        /// <summary>
        /// 修改账号密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool ModifyPassword(int userId, string password)
        {
            string pwd = MD5Encrypt64(password);
            return ua.ChangePwd(userId, pwd);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="oldName">原用户名</param>
        /// <returns>成功返回true</returns>
        public bool UpdateAccount(UserInfo user, string oldName)
        {
            if (GetUserByName(user.UserName).UserName != "" && user.UserName == oldName)
            {
                return false;
            }
            return ua.UpdateUser(user);
        }

        /// <summary>
        /// 条件查询用户列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserInfo> GetUsers(UserInfo user)
        {
            return ua.GetUsers(user);
        }

        /// <summary>
        /// 64位的MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            MD5 md5 = MD5.Create();//实例化一个md5对象
            //加密后是一个字节类型的数组，需要注意编码的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
    }
}
