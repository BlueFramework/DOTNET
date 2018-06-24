using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace BlueFramework.Blood.EntityFramework
{
    public class BehaviorUtils
    {
        public static List<PropertyBehavior> GetBehaviors(DataRow row,List<PropertyInfo> properties)
        {
            List<PropertyBehavior> behaviors = new List<PropertyBehavior>();
            foreach(PropertyInfo pi in properties)
            {
                PropertyBehavior behavior = new PropertyBehavior(pi);
                object o = row[pi.Name];
                behaviors.Add(behavior);
                bool sameType = o.GetType().Equals(pi.PropertyType);
                if(sameType) continue;
                switch (pi.PropertyType.ToString())
                {
                    case "System.Boolean":
                        behavior.Behavior = BehaviorType.IntToBoolean;
                        break;
                    case "System.DateTime":
                        behavior.Behavior = BehaviorType.DateToString;
                        break;
                }
            }

            return behaviors;
        }
    }
}
