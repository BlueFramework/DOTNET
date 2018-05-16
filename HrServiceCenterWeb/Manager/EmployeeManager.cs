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

        public List<CompanyInfo> GetCompanies(string query)
        {
            EntityContext context = Session.CreateContext();
            List<CompanyInfo> list = context.SelectList<CompanyInfo>("hr.company.findCompanys", query);
            return list;
        }

        public List<object> GetEmployees()
        {
            DataAccess.EmployeeAccess db = new DataAccess.EmployeeAccess();
            System.Data.DataTable dt = db.GetEmployees();
            return null;
        }
    }
}