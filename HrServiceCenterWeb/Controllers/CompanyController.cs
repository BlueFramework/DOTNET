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
                success = true,
                data = ci.CompanyId
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
