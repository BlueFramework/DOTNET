using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using BlueFramework.Common.CSV;

namespace HrServiceCenterWeb.Controllers
{
    public class PayController : Controller
    {

        // 模板列表
        // VIEW: /Pay/TemplateList
        public ActionResult TemplateList()
        {
            return View();
        }

        //获取所有模板列表
        //VIEW: /Pay/GetTemplateList
        public ActionResult GetTemplateList()
        {
            List<Models.TemplateInfo> list = new Manager.PayManager().GetTemplateList();
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        // 模板编辑器
        // VIEW: /Pay/TemplateEditor
        public ActionResult TemplateEditor()
        {
            ViewBag.TemplateId = int.Parse(this.HttpContext.Request.QueryString["id"]);
            return View();
        }

        public ActionResult QueryTemplate(string id)
        {
            int tempId = int.Parse(id);
            Models.TemplateInfo temp = new Manager.PayManager().GetTemplate(tempId);
            JsonResult jsonResult = Json(temp);
            return jsonResult;
        }

        // 获取模板列表树
        // VIEW: /Pay/TemplateEditor
        public ActionResult GetTemplateTree()
        {
            return Json(new Manager.PayManager().GetTree());
        }

        /// <summary>
        /// 获取数据库中存在的模版列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTemplateByTable(string id)
        {
            int tempId = int.Parse(id);
            int[] itemp = new Manager.PayManager().GetTemplateByTable(tempId);
            string[] stemp = itemp.Select(i => i.ToString()).ToArray();
            string strtemp = string.Join(",", stemp);
            return Json(strtemp);
        }

        public ActionResult SaveTemplateForTable(int id, string temps)
        {
            int[] tempArr = null;
            if (!string.IsNullOrEmpty(temps))
            {
                string strTemp = temps;
                string[] strArr = strTemp.Split(',');
                tempArr = new int[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    tempArr[i] = Convert.ToInt32(strArr[i]);
                }
            }
            string msg = string.Empty;
            if (new Manager.PayManager().SaveTemplateForTable(id, tempArr))
            {
                msg = "保存成功";
            }
            else
            {
                msg = "保存失败";
            }
            return Json(msg);
        }

        public ActionResult SaveTemplateMsg(int id, string temps)
        {
            int[] tempArr = null;
            if (!string.IsNullOrEmpty(temps))
            {
                string strTemp = temps;
                string[] strArr = strTemp.Split(',');
                tempArr = new int[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    tempArr[i] = Convert.ToInt32(strArr[i]);
                }
            }
            string msg = string.Empty;
            if (new Manager.PayManager().SaveTemplateMsg(id, tempArr))
            {
                msg = "保存成功";
            }
            else
            {
                msg = "保存失败";
            }
            return Json(msg);
        }

        public ActionResult DeleteTemplate(int id)
        {
            string msg = string.Empty;
            if (new Manager.PayManager().DeleteTemplate(id))
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return Json(msg);
        }

        // 导入列表
        // VIEW: /Pay/ImportorList
        public ActionResult ImportorList()
        {
            return View();
        }

        //查询保险导入列表
        //VIEW: /Pay/QueryImportorList
        public ActionResult QueryImportorList(string query)
        {
            List<InsuranceInfo> list = new Manager.PayManager().QueryImportorList(query);
            JsonResult jsonResult = Json(list);
            return jsonResult;
        }

        public ActionResult DeleteInsurance(int id)
        {
            string msg = string.Empty;
            if (new Manager.PayManager().DeleteInsurance(id))
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return Json(msg);
        }

        // 导入保险
        // VIEW: /Pay/ImportorEditor
        public ActionResult ImportorEditor()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string outmsg = string.Empty;
            bool success = false;
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file1 = files[i];
                    Stream stream = file1.InputStream;
                    string fileName = System.IO.Path.GetFileName(file1.FileName);
                    string fileType = System.IO.Path.GetExtension(file1.FileName).ToLower();
                    switch (fileType)
                    {
                        case ".csv":
                            CsvFileParser cfp = new CsvFileParser();
                            DataTable dt = cfp.TryParse(stream, out outmsg);
                            success = new Manager.PayManager().Import(dt, fileName,ref outmsg);
                            break;
                        case ".xls":
                            success = false;
                            break;
                        default:
                            success = false;
                            outmsg += "文件：" + fileName +"格式不支持；";
                            break;
                    }
                }
                outmsg += "提交失败，请联系管理员；";
            }
            return View();
        }

        // 发放列表
        // VIEW: /Pay/ImportorList
        public ActionResult PayList()
        {
            return View();
        }

        // 发放编辑器
        // VIEW: /Pay/PayEditor
        public ActionResult PayEditor()
        {
            return View();
        }
    }
}
