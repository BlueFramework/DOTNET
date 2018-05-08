using BlueFramework.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.User.DataAccess
{
    public class SysAccess
    {
        public int[] GetMenuRights(int roleId)
        {
            return null;
        }

        public int[] GetDataRights(int roleId)
        {
            return null;
        }

        public RoleInfo GetRoleByRoleId(int roleId)
        {
            return null;
        }

        public RoleInfo GetRoleByRoleName(string roleName)
        {
            return null;
        }

        public List<RoleInfo> GetRoles(RoleInfo role)
        {
            return null;
        }

        public bool SaveMenuRights(int roleId, int[] item)
        {
            return false;
        }

        public bool SaveDataRights(int roleId, int[] item)
        {
            return false;
        }

        public int AddRole(RoleInfo role)
        {
            return 0;
        }

        public bool DeleteRole(RoleInfo role)
        {
            return false;
        }

        public int[] GetGrouping(int roleId)
        {
            return null;
        }
    }
}
