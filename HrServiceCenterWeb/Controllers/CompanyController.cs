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
using HrServiceCenterWeb.Manager;

namespace HrServiceCenterWeb.Controllers
{
    public class CompanyController : Controller
    {


        // GET: /Company/CompanyList
        public ActionResult CompanyList()
        {
            return View();
        }


        // Company/GetCompanyList?query=
        public ActionResult GetCompanyList(string query)
        {
            List<CompanyInfo> list = new Manager.EmployeeManager().GetCompanies(query);
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        // Company/QueryCompanyList?q=
        public ActionResult QueryCompanyList(string q)
        {
            if (String.IsNullOrEmpty(q))
            {
                return null;
            }
            List<CompanyInfo> list = new Manager.EmployeeManager().GetCompanies(q);
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        public ActionResult CompanyPage()
        {
            ViewBag.CompanyId = int.Parse(this.HttpContext.Request.QueryString["id"]);
            return View();
        }

        // Company/GetCompany?id=
        public ActionResult GetCompany(int id)
        {
            CompanyInfo company = new Manager.EmployeeManager().GetCompany(id);
            JsonResult jsonResult = Json(company,JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        // Company/GetCompany?id=
        [HttpPost]
        public ActionResult SaveCompany(CompanyInfo companyInfo)
        {
            CompanyInfo ci = new Manager.EmployeeManager().SaveCompany(companyInfo);
            Object result = new
            {
                success = ci==null?false:true,
                data = ci==null?0:ci.CompanyId
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult SaveRecharge(int companyId,decimal money)
        {
            EmployeeManager manager = new EmployeeManager();
            CompanyInfo company = manager.GetCompany(companyId);
            CompanyAccountRecordInfo recordInfo = new CompanyAccountRecordInfo()
            {
                AccountId = company.AccountId,
                CompanyId = companyId,
                Creator = BlueFramework.User.UserContext.CurrentUser.UserId,
                Money = money,
                AccountBalance = company.AccountBalance
            };
            bool pass = manager.SaveRecharge(recordInfo);
            Object result = new
            {
                success = pass,
                data = pass?"充值成功！":"充值失败！"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult DeleteCompany(int id)
        {
            bool pass = new Manager.EmployeeManager().DeleteCompany(id);
            Object result = new
            {
                success = pass,
                data = pass?"删除成功": "删除失败，数据可能已经被引用。"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        // Company/SavePosition
        [HttpPost]
        public ActionResult SavePosition(CompanyPositionSetInfo positionSetInfo)
        {
            bool pass = new Manager.EmployeeManager().SavePosition(positionSetInfo);
            Object result = new
            {
                success = pass,
                data = pass?"成功":"失败"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult DeletePosition(CompanyPositionSetInfo positionSetInfo)
        {
            bool pass = new Manager.EmployeeManager().DeletePostion(positionSetInfo);
            Object result = new
            {
                success = pass,
                data = pass ? "成功" : "失败"
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

    }
}
