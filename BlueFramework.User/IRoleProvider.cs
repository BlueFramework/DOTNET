using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.User
{
    /// <summary>
    /// 角色接口定义
    /// </summary>
    public interface IRoleProvider
    {
        int[] GetDataRights(int roleId);
        int[] GetMenuRights(int roleId);
    }
}
