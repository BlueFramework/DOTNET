using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using HrServiceCenterWeb.Models;
using HrServiceCenterWeb.Manager;
using BlueFramework.User;
using BlueFramework.Common.Excel;

namespace HrServiceCenterWeb.Controllers
{
    public class PaymentController : Controller
    {
        [HttpPost]
        //  Payment/CreatePayment
        public ActionResult CreatePayment(Payment payment)
        {
            payment.CreateTime = DateTime.Now;
            payment.CreatorId = BlueFramework.User.UserContext.CurrentUser.UserId;

            PaymentManager pm = new PaymentManager();
            pm.CreatePayment(payment);
            Object result = new
            {
                success = payment.PayId>0?true:false,
                data = payment.PayId
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult LoadPayment(int payId)
        {
            PaymentManager pm = new PaymentManager();
            Payment payment = pm.LoadPayment(payId);
            payment.DataSource = null;
            string json = JsonConvert.SerializeObject(payment);
            ContentResult contentReuslt = Content(json);
            return contentReuslt;
        }

        public ActionResult SavePayment(Payment payment)
        {
            return null;
        }

        public ActionResult Export(int payId)
        {
            PaymentManager pm = new PaymentManager();
            Payment payment = pm.LoadPayment(payId);
            #region 动态模板
            TTemplate template = new TTemplate();
            TRow R1 = new TRow();
            TRow R2 = new TRow();
            int index1 = 3;
            int index2 = 3;
            TCell A = new TCell("PersonId", "ID", 0, 0); A.RowSpan = 2;A.Width = 1;
            TCell B = new TCell("PersonName", "姓名", 0, 1);B.RowSpan = 2;
            TCell C = new TCell("PersonCode", "身份证", 0, 2); C.RowSpan = 2;
            R1.Cells.Add(A);
            R1.Cells.Add(B);
            R1.Cells.Add(C);
            foreach (PayItemDO item in payment.Items)
            {
                TCell cell = new TCell();
                cell.Name = item.ItemName;
                cell.Caption = item.ItemCaption;
                if (item.IsLeaf==true)
                {
                    //cell.Width = 50;
                }
                if (item.ParentId == 0)
                {
                    cell.RowIndex = 0;
                    cell.ColumnIndex = index1;
                    int colspan = 1;
                    foreach(PayItemDO pi in payment.Items)
                    {
                        if (pi.ParentId == item.ItemId)
                        {
                            colspan++;
                        }
                    }
                    if(colspan==1)
                    {
                        cell.RowSpan = 2;
                        index1++;
                        index2++;
                    }
                    else
                    {
                        cell.ColSpan = colspan-1;
                        index1 = index1 + cell.ColSpan;
                    }
                    R1.Cells.Add(cell);
                }
                else
                {
                    cell.RowIndex = 1;
                    cell.ColumnIndex = index2;
                    index2 = index2 + 1;
                    R2.Cells.Add(cell);
                }
            }

            TSheet tsheet = new TSheet();
            tsheet.Name = "Sheet1";
            tsheet.Title = "Sheet1";
            tsheet.Head.Rows.Add(R1);
            tsheet.Head.Rows.Add(R2);
            template.Sheets.Add(tsheet);
            #endregion

            IExcel excel = ExcelFactory.CreateDefault();
            DataSet ds = new DataSet();
            payment.DataSource.TableName = "Sheet1";
            ds.Tables.Add(payment.DataSource);
            POIStream stream = new POIStream();
            stream.AllowClose = false;
            excel.Write(stream, template,ds, ExcelExtendType.XLSX);
            stream.AllowClose = true;
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            HttpResponse context = System.Web.HttpContext.Current.Response;
            try
            {
                context.ContentType = "application/ms-excel";
                context.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode("工资表", System.Text.Encoding.UTF8)));
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

        public ActionResult Import(int payId)
        {
            if(Request.Files.Count == 0)
            {
                Object failResult = new
                {
                    success = false,
                    data = ""
                };
                JsonResult failJsonResult = Json(failResult, JsonRequestBehavior.AllowGet);
                return failJsonResult;
            }

            IExcel excel = ExcelFactory.CreateDefault();
            DataSet ds = excel.Read(Request.Files[0].InputStream,2);
            DataTable dt = ds.Tables[0];
            string message;
            bool pass = new PaymentManager().ImportPayment(payId,dt, out message);
            Object result = new
            {
                success = pass,
                data = message
            };
            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}