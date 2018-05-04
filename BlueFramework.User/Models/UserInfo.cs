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
    public class UserInfo:IUser 
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

    }
}
