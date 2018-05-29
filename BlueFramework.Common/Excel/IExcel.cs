using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace BlueFramework.Common.Excel
{
    public interface IExcel
    {
        DataSet Read(string filePath);

        void Write(StreamWriter stream);
    }
}
