using System;
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

        #region

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
            return Json(UserProvider.Instance.GetUsers(userquery));
        }

        #endregion
    }
}