using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.User.Models
{
    /// <summary>
    /// user infomation and user action
    /// </summary>
    public class UserInfo : IUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string TrueName { get; set; }

        public int IsAdmin { get; set; }

        public DateTime CreatTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int State { get; set; }

    }
}
