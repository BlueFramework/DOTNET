using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using HrServiceCenterWeb.Manager;
using System.Data;
using System.Web.UI.WebControls;
using BlueFramework.Common.Excel;

namespace HrServiceCenterWeb.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult WxyjYear()
        {
            return View();
        }

        // GET: Reports/GetYears/
        [HttpPost]
        public ActionResult GetYears()
        {
            int start = 2018;
            int end = DateTime.Now.Year + 2;
            List<object> list = new List<object>();
            for(int i = start; i < end; i++)
            {
                var o = new 
                {
                    id = i,
                    text = i+"年度"
                };
                list.Add(o);
            }
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        // GET: Reports/GetWxyjByYear/
        [HttpGet]
        public ActionResult GetWxyjByYear(int year)
        {
            ReportManager manager = new ReportManager();
            DataSet ds = manager.GetWxyjByYear(year);
            DataTable dt = ds.Tables[0];
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            ContentResult contentResult = Content(json);
         
            return contentResult;
        }
        // GET: Reports/GetWxyjByYear/
        public ActionResult DownWxyjByYear(int year)
        {
            ReportManager manager = new ReportManager();
            DataSet ds = manager.GetWxyjByYear(year);
            ds.Tables[0].TableName = "sheet1";

            POIStream stream = new POIStream();
            IExcel excel = ExcelFactory.CreateDefault();
            stream.AllowClose = false;
            //excel.Write(stream, ds, ExcelExtendType.XLSX);
            string templateName = AppDomain.CurrentDomain.BaseDirectory + @"\Setting\Reports\wxyjYear.xml";
            excel.Write(stream, templateName, ds, ExcelExtendType.XLSX);
            stream.AllowClose = true;
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            HttpResponse context = System.Web.HttpContext.Current.Response;
            try
            {
                context.ContentType = "application/ms-excel";
                context.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode("单位五险一金统计", System.Text.Encoding.UTF8)));
                context.BinaryWrite(buffer);
                context.Flush();
                context.End();
            }
            catch (Exception ex)
            {
                context.ContentType = "text/plain";
                context.Write(ex.Message);
            }
            return null;
        }
    }
}
