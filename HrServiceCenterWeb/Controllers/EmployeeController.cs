﻿using System;
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
        public ActionResult CompanyList()
        {
            return View();
        }

        // GET: /Account/
        public ActionResult EmployeeList()
        {
            return View();
        }

        public ActionResult EmployeePage()
        {
            return View();
        }

    }
}
