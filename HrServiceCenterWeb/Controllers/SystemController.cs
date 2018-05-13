﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlueFramework.User;
using BlueFramework.User.Models;

namespace HrServiceCenterWeb.Controllers
{
    public class SystemController : Controller
    {
        //
        // GET: System

        public ActionResult Index()
        {
            return View();
        }

        #region common

        //
        //GET：GetOrgs

        public ActionResult GetOrgs()
        {
            UserManager um = new UserManager();
            List<OrgnizationInfo> orgs = um.GetOrgnizations();
            return Json(orgs);
        }
        #endregion

        #region User

        //
        //GET: 

        public ActionResult UserManage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UsersQuery(UserInfo userquery, string userName)
        {
            userquery.UserName = userName;
            userquery.TrueName = userName;
            return Json(UserManager.Instance.GetUsers(userquery));
        }

        //
        //GET:AddUser

        public ActionResult AddUser(UserInfo user)
        {
            int result = UserManager.Instance.AddAccount(user);
            string backMsg = string.Empty;
            switch (result)
            {
                case 3:
                    backMsg = "该账户已存在";
                    break;
                case 4:
                    backMsg = "新增成功";
                    break;
                case -4:
                    backMsg = "新增失败";
                    break;
            }
            return Json(backMsg);
        }

        public ActionResult UpDateUser(UserInfo user,string oldName)
        {
            int result = UserManager.Instance.UpdateAccount(user,oldName);
            string backMsg = string.Empty;
            switch (result)
            {
                case 1:
                    backMsg = "更新成功";
                    break;
                case 0:
                    backMsg = "更新失败";
                    break;
                case -1:
                    backMsg = "账户名已存在";
                    break;
            }
            return Json(backMsg);
        }

        //
        //GET:LoadUser

        public ActionResult LoadUser(UserInfo user)
        {
            return Json(UserManager.Instance.QueryUserById(user.UserId));
        }

        //
        //GET:InitPwd

        public ActionResult InitPwd(UserInfo user)
        {
            string backMsg = string.Empty;
            if (UserManager.Instance.ModifyPassword(user.UserId, user.Password))
            {
                backMsg = "重置密码成功";
            }
            else
            {
                backMsg = "重置密码失败";
            }
            return Json(backMsg);
        }

        //
        //GET:DeleteUser

        public ActionResult DeleteUser(UserInfo user)
        {
            int result = UserManager.Instance.DeleteUser(user);
            string backMsg = string.Empty;
            switch (result)
            {
                case -1:
                    backMsg = "不能删除当前登录用户";
                    break;
                case 1:
                    backMsg = "删除成功";
                    break;
                case 0:
                    backMsg = "删除失败";
                    break;
            }
            return Json(backMsg);
        }

        #endregion

        #region Role

        //
        // GET:RoleManager

        public ActionResult RoleManage()
        {
            return View();
        }

        //
        //GET:RolesQuery

        public ActionResult RolesQuery(RoleInfo role,string roleName)
        {
            role.RoleName = roleName;
            return Json(RoleManager.Instance.GetRoleList(role));
        }

        #endregion
    }
}