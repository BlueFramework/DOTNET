using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using BlueFramework.Blood.Config;
using BlueFramework.Data;

namespace BlueFramework.Blood.DataAccess
{
    public class Command
    {
        DatabaseProviderFactory factory;
        Database db;
        public Command()
        {
            factory = new DatabaseProviderFactory();
            db = factory.CreateDefault();
        }

        public T LoadEntity<T>(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return default(T);
            T o = System.Activator.CreateInstance<T>();
            Type type = o.GetType();
            DataRow row = dt.Rows[0];
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                string propertyName = property.Name;
                if (dt.Columns.Contains(propertyName))
                {
                    property.SetValue(o, row[propertyName]);
                }
            }
            return o;
        }

        /// <summary>
        /// load entity list from datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> LoadEntities<T>(DataTable dt)
        {
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return new List<T>();
            Type type = typeof(T);
            List<PropertyInfo> properties = type.GetProperties().ToList();
            // remove property where it is not exist
            for(int i = 0; i < properties.Count; i++)
            {

            }
            // freach properties
            List<T> objects = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T o = System.Activator.CreateInstance<T>();
                foreach (PropertyInfo pi in properties)
                {
                    pi.SetValue(o, dr[pi.Name]);
                }
                objects.Add(o);
            }
            return objects;
        }

        public T Select<T>(EntityConfig config, object objectId)
        {
            DbCommand dbCommand = BuildCommand(config, objectId);
            try
            {
                T o = default(T);
                using (DataSet ds = db.ExecuteDataSet(dbCommand))
                {
                    DataTable dt = ds.Tables[0];
                    o = LoadEntity<T>(dt);
                }
                return o;
            }
            catch
            {
                return default(T);
            }

        }

        public object Select<T>(EntityConfig config, CommandParameter[] parameters)
        {
            return null;
        }

        public bool Insert(EntityConfig config)
        {
            return false;
        }

        #region sql format
        private string FormatSql(EntityConfig config)
        {
            string parameterName = db.BuildParameterName("value");
            string sql = config.Sql.Replace("#{value}", parameterName);
            return sql;
        }

        private DbCommand BuildCommand(EntityConfig config, object objectId)
        {
            string sql = FormatSql(config);
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            db.AddInParameter(dbCommand, "value", DbType.String, objectId);
            return dbCommand;
        }

        private DbCommand BuildCommand(EntityConfig config,params CommandParameter[] parameters)
        {
            string sql = FormatSql(config);
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
      
            return dbCommand;
        }
        #endregion
    }
}
