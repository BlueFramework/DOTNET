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
                user.CreateTime = DateTime.Parse(row["CREATE_TIME"].ToString()).ToShortDateString();
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
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory();
            Database database = dbFactory.CreateDefault();
            string sql = @"SELECT T.*, A.ORG_NAME
                          FROM T_S_USER T
                           LEFT JOIN T_S_ORGANIZATION A
                           ON T.ORG_ID = A.ORG_ID
                           WHERE T.USERNAME <> 'ADMIN' ";
            string whereStr = "";
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.TrueName))
            {
                whereStr += "AND ( T.USERNAME LIKE '%" + user.UserName + "%' OR T.TRUENAME LIKE '%" + user.TrueName + "%') ";
            }
            whereStr += " order by userid";
            sql += whereStr;
            DbCommand dbCommand = database.GetSqlStringCommand(sql);
            //database.AddInParameter(dbCommand, "userName", DbType.String, user.TrueName);
            DataSet dataSet = database.ExecuteDataSet(dbCommand);
            DataTable dt = dataSet.Tables[0];
            List<UserInfo> users = new List<UserInfo>();
            foreach (DataRow row in dt.Rows)
            {
                UserInfo ui = new UserInfo();
                ui.UserId= int.Parse(row["USERID"].ToString());
                ui.UserName=row["UserName"].ToString();
                ui.TrueName = row["TrueName"].ToString();
                ui.CreateTime= DateTime.Parse(row["CREATE_TIME"].ToString()).ToShortDateString();
                ui.OrgName= row["ORG_NAME"].ToString();
                users.Add(ui);
            }
            return users;
        }
    }
}
