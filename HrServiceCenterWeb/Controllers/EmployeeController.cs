using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using BlueFramework.User;
using BlueFramework.User.Models;

namespace HrServiceCenterWeb.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: /Account/
        public ActionResult EmployeeList()
        {
            if (this.HttpContext.Request.QueryString["id"] == null)
                ViewBag.CompanyId = 0;
            else
                ViewBag.CompanyId = int.Parse(this.HttpContext.Request.QueryString["id"]);
            return View();
        }

        public ActionResult EmployeePage()
        {
            return View();
        }


    }
}
