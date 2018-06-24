using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BlueFramework.Common.Excel
{
    public class TemplateConfig
    {
        private string filePath;
        private TTemplate template;

        public TemplateConfig()
        {
            template = new TTemplate();
            template.Sheets = new List<TSheet>();
        }

        public void Load(string filePath)
        {
            this.filePath = filePath;
        }

        public void Load(DataSet ds)
        {
            foreach(DataTable dt in ds.Tables)
            {
                TSheet sheet = new TSheet();
                sheet.Title = dt.TableName;
                sheet.Name = sheet.Title;
                sheet.Head = new THead();
                TRow row = new TRow();
                sheet.Head.Rows.Add(row);
                int i = 0;
                foreach(DataColumn column in dt.Columns)
                {
                    TCell cell = new TCell();
                    cell.RowIndex = 0;
                    cell.ColumnIndex = i;
                    cell.Name = column.ColumnName;
                    cell.Caption = string.IsNullOrEmpty(column.Caption)?column.ColumnName:column.Caption;
                    row.Cells.Add(cell);
                    i++;
                }
                this.template.Sheets.Add(sheet);
            }
        }

        public TTemplate GetTemplate()
        {
            return this.template;
        }
    }
}
