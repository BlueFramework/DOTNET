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

        public UserInfo GetUser(UserInfo user)
        {
            return null;
        }

        public bool AddAccount(UserInfo user)
        {
            return false;
        }

        public bool Delete(UserInfo user)
        {
            return false;
        }

        public UserInfo GetUserByName(string userName)
        {
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
            Database database = dbFactory.CreateDefault();
            string sql = "select * from t_s_user t  where username=@userName";
            DbCommand dbCommand = database.GetSqlStringCommand(sql);
            database.AddInParameter(dbCommand, "userName", DbType.String, userName);
            DataSet dataSet = database.ExecuteDataSet(dbCommand);
            DataTable dt = dataSet.Tables[0];
            UserInfo user = new UserInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                user.UserId = int.Parse(row["USERID"].ToString());
                user.UserName = row["USERNAME"].ToString();
                user.Password = row["PASSWORD"].ToString();
                user.OrgName = row["ORG_ID"].ToString();
                user.TrueName = row["TRUENAME"].ToString();
                user.CreatTime = DateTime.Parse(row["CREATE_TIME"].ToString());
                user.IsAdmin = (row["IsAdmin"].ToString() == "1") ? true : false;
            }
            return user;
        }

        public UserInfo QueryUserById(int userId)
        {
            return null;
        }

        public UserInfo GetUser(string userName)
        {
            return null;
        }

        public bool ChangePwd(int userID, string pwd)
        {
            return false;
        }

        public bool UpdateUser(UserInfo user)
        {
            return false;
        }

        public List<UserInfo> GetUsers(UserInfo user)
        {
            return null;
        }
    }
}
