using BlueFramework.User.Models;
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
        /// <summary>
        /// 获取角色所属的菜单权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        int[] GetDataRights(int roleId);

        /// <summary>
        /// 获取角色所属的数据权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        int[] GetMenuRights(int roleId);

        /// <summary>
        /// 获取角色列表
        /// 支持对角色名称模糊查询
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns></returns>
        List<RoleInfo> GetRoleList(RoleInfo role);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        RoleInfo GetRoleByRoleId(int roleId);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        RoleInfo GetRoleByRoleName(string roleName);

        /// <summary>
        /// 保存菜单权限
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="items">菜单对象</param>
        /// <returns></returns>
        bool SaveMenuRights(int roleId, int[] items);

        /// <summary>
        /// 保存数据权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="items">数据对象</param>
        /// <returns></returns>
        bool SaveDataRights(int roleId, int[] items);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int AddRole(RoleInfo role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool DeleteRole(RoleInfo role);

        /// <summary>
        /// 获取角色对应分组
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        int[] GetGrouping(int roleId);
    }
}
