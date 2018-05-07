using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.User
{
    /// <summary>
    /// 角色功能提供
    /// 增删改查等... ...
    /// </summary>
    public class RoleProvider:IRoleProvider
    {
        protected BlueFramework.User.DataAccess.SysAccess sysAccess = new User.DataAccess.SysAccess();

        public static IRoleProvider Instance
        {
            get
            {
                RoleProvider role = new RoleProvider();
                return role;
            }
        }

        /// <summary>
        /// 获取角色所属的菜单权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public int[] GetMenuRights(int roleId)
        {
            return sysAccess.GetMenuRights(roleId);
        }

        /// <summary>
        /// 获取角色所属的数据权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public int[] GetDataRights(int roleId)
        {
            return sysAccess.GetDataRights(roleId);
        }
    }
}
