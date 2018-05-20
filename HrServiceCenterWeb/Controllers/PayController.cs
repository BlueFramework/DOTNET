using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using BlueFramework.User;
using BlueFramework.User.Models;

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

        // 模板编辑器
        // VIEW: /Pay/TemplateEditor
        public ActionResult TemplateEditor()
        {
            return View();
        }

        // 导入列表
        // VIEW: /Pay/ImportorList
        public ActionResult ImportorList()
        {
            return View();
        }

        // 导入保险编辑器
        // VIEW: /Pay/ImportorEditor
        public ActionResult ImportorEditor()
        {
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
