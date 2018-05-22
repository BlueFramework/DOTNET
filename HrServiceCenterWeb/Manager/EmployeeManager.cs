using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueFramework.Blood;
using HrServiceCenterWeb.Models;

namespace HrServiceCenterWeb.Manager
{
    public class EmployeeManager
    {
        public List<PositionInfo> GetPositions()
        {
            EntityContext context = Session.CreateContext();
            List<PositionInfo> list = context.SelectList<PositionInfo>("hr.position.findPositions", null);
            return list;
        }

        public List<CompanyInfo> GetCompanies(string query)
        {
            EntityContext context = Session.CreateContext();
            List<CompanyInfo> list = context.SelectList<CompanyInfo>("hr.company.findCompanys", query);
            return list;
        }

        public CompanyInfo GetCompany(int companyId)
        {
            EntityContext context = Session.CreateContext();
            CompanyInfo companyInfo = context.Selete<CompanyInfo>("hr.company.findCompanyById", companyId);
            return companyInfo;
        }

        public CompanyInfo SaveCompany(CompanyInfo companyInfo)
        {
            EntityContext context = Session.CreateContext();
            if(companyInfo.CompanyId>0)
                context.Save<CompanyInfo>("hr.company.updateCompany", companyInfo);
            else
                context.Save<CompanyInfo>("hr.company.insertCompany", companyInfo);
            return companyInfo;
        }

        public bool DeleteCompany(int companyId)
        {
            EntityContext context = Session.CreateContext();
            bool pass = context.Delete("hr.company.deleteCompany", companyId);
            return pass;
        }


        public List<object> GetEmployees()
        {
            DataAccess.EmployeeAccess db = new DataAccess.EmployeeAccess();
            System.Data.DataTable dt = db.GetEmployees();
            return null;
        }
    }
}