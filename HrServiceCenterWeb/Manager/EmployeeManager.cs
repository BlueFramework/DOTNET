using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Manager
{
    public class EmployeeManager
    {
        public List<object> GetEmployees()
        {
            DataAccess.EmployeeAccess db = new DataAccess.EmployeeAccess();
            System.Data.DataTable dt = db.GetEmployees();
            return null;
        }
    }
}