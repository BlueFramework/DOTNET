using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using BlueFramework.User.Models;
using BlueFramework.Data;

namespace BlueFramework.User.DataAccess
{
    public class UserAccess
    {
        public UserInfo GetUserInfo(int userId)
        {
            //DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
            //Database database = dbFactory.CreateDefault();
            //string sql = "select * from t_s_user  where userid=@userid";
            //DbCommand dbCommand = database.GetSqlStringCommand(sql);
            //database.AddInParameter(dbCommand, "userid", DbType.Int32, userId);
            //DataSet dataSet = database.ExecuteDataSet(dbCommand);
            Models.UserInfo userInfo = new Models.UserInfo();
            userInfo.UserId = 1;
            userInfo.UserName = "admin";
            return userInfo;
            
        }
    }
}
