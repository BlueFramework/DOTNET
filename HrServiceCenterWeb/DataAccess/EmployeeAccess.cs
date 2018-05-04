using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BlueFramework.Data;

namespace HrServiceCenterWeb.DataAccess
{
    public class EmployeeAccess
    {
        public DataTable GetEmployees()
        {
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
            Database dataBase = dbFactory.CreateDefault();
            string sql = "select * from HR_EMPLOYEE";
            DataSet ds = dataBase.ExecuteDataSet(CommandType.Text, sql);
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }
    }
}