using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.Blood.Config
{
    /// <summary>
    /// entity xml config
    /// </summary>
    public class EntityConfig
    {
        /// <summary>
        /// entity action's full id,as namespace+id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// sql
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// the type of input parameter
        /// </summary>
        public string InputParameterType { get; set; }
    }
}
