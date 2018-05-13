using BlueFramework.Data;
using BlueFramework.User.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public List<RoleInfo> GetRoles(RoleInfo roleinfo)
        {
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
            Database database = dbFactory.CreateDefault();
            string sql = "select * from T_S_ROLE t where 1=1 ";
            string whereStr = "";
            if (!string.IsNullOrEmpty(roleinfo.RoleName))
            {
                whereStr += " and t.name like'%" + roleinfo.RoleName + "%'";
            }
            whereStr += " order by roleid";
            sql += whereStr;
            DbCommand dbCommand = database.GetSqlStringCommand(sql);
            DataSet dataSet = database.ExecuteDataSet(dbCommand);
            DataTable dt = dataSet.Tables[0];
            List<RoleInfo> roles = new List<RoleInfo>();
            foreach (DataRow row in dt.Rows)
            {
                RoleInfo role = new RoleInfo();
                role.RoleId = int.Parse(row["ROLEID"].ToString());
                role.RoleName = row["NAME"].ToString();
                role.Description = row["DESCRIPTION"].ToString();
                roles.Add(role);
            }
            return roles;
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
