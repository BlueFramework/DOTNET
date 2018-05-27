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
        // GET: /Employee/EmployeeList
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
            ViewBag.EmployeeId = int.Parse(this.HttpContext.Request.QueryString["id"]);
            if (this.HttpContext.Request.QueryString["companyId"] == null)
                ViewBag.CompanyId = 0;
            else
                ViewBag.CompanyId = int.Parse(this.HttpContext.Request.QueryString["companyId"]); 
            return View();
        }

        // Employee/GetEmployeeList
        public ActionResult GetEmployeeList(EmployeeInfo employeeInfo)
        {
            List<EmployeeInfo> list = new Manager.EmployeeManager().GetEmployees(employeeInfo);
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        public ActionResult GetEmployee(int personId)
        {
            EmployeeInfo employeeInfo = new EmployeeInfo();
            if (personId > 0)
            {
                employeeInfo = new Manager.EmployeeManager().GetEmployee(personId);
            }
            JsonResult jsonResult = Json(employeeInfo, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        // Employee/SaveEmployee
        [HttpPost]
        public ActionResult SaveEmployee(EmployeeInfo employeeInfo)
        {
            bool pass = new Manager.EmployeeManager().SaveEmployee(employeeInfo);
            Object result = new
            {
                success = pass,
                data = pass ? employeeInfo.PersonId : 0
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int personId)
        {
            bool pass = new Manager.EmployeeManager().DeleteEmployee(personId);
            Object result = new
            {
                success = pass,
                data = pass ? "删除成功" : "删除失败"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
