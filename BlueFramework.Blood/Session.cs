using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueFramework.Blood.DataAccess;
using BlueFramework.Blood.Config;

namespace BlueFramework.Blood
{
    /// <summary>
    /// the session of database's operation
    /// </summary>
    public class Session
    {
        private static bool loaded = false;

        /// <summary>
        /// initialize configs
        /// </summary>
        public static void Init()
        {
            if (!loaded)
            {
                LoadConfigs();
                loaded = true;
            }
        }

        /// <summary>
        /// load mapping configs
        /// </summary>
        static void LoadConfigs()
        {
            Config.ConfigManagent.Init();
        }

        public static object GetObject<T>(string selectId,object objectId)
        {
            EntityConfig config = ConfigManagent.Configs[selectId];
            Command command = new Command();
            object o = command.Select<T>(config, objectId);
            if (o == null)
                return null;
            else
                return (T)o;
        }
    }
}
