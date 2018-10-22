using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using BlueFramework.User;
using BlueFramework.User.Models;
using BlueFramework.Common.Excel;

namespace HrServiceCenterWeb.Controllers
{
    public class EmployeeController : BaseController
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
            ViewBag.PersonCode = new Manager.EmployeeManager().GetMaxPersonCode();
            return View();
        }

        // Employee/GetEmployeeList
        public ActionResult GetEmployeeList(EmployeeInfo employeeInfo)
        {
            List<EmployeeInfo> list = new Manager.EmployeeManager().GetEmployees(employeeInfo);
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        public ActionResult GetContractIsEndEmployeeList()
        {
            List<EmployeeInfo> list = new Manager.EmployeeManager().GetContractBeEndingEmployees();
            JsonResult jsonResult = Json(list);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult GetRetireIsEndEmployeeList()
        {
            List<EmployeeInfo> list = new Manager.EmployeeManager().GetRetireBeEndingEmployees();
            JsonResult jsonResult = Json(list);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
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
                data = pass ? "删除成功" : "删除失败，系统限制不允许删除！"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult ExportPayDetail(EmployeeInfo employeeInfo)
        {
            IExcel excel = ExcelFactory.CreateDefault();
            DataSet ds = new Manager.EmployeeManager().GetEmployees();
            POIStream stream = new POIStream();
            stream.AllowClose = false;
            excel.Write(stream, ds, ExcelExtendType.XLSX);
            stream.AllowClose = true;
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            HttpResponse context = System.Web.HttpContext.Current.Response;
            try
            {
                context.ContentType = "application/ms-excel";
                context.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode( "人员清单", System.Text.Encoding.UTF8)));
                context.BinaryWrite(buffer);
                context.Flush();
                context.End();
            }
            catch (Exception ex)
            {
                context.ContentType = "text/plain";
                context.Write(ex.Message);
            }
            return null;
        }
    }
}
