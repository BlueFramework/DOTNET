using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.Blood.DataAccess
{
    public class CommandParameter
    {
        private string _parameterName;
        private object _parameterValue;

        public string ParameterName { get => _parameterName; set => _parameterName = value; }
        public object ParameterValue { get => _parameterValue; set => _parameterValue = value; }

        public CommandParameter()
        {

        }

        public CommandParameter(string parameterName, object parameterValue)
        {
            _parameterName = parameterName;
            _parameterValue = parameterValue;
        }

    }
}
