using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using BlueFramework.User;

namespace HrServiceCenterWeb.Controllers
{
    public class AccountController : Controller
    {
        readonly string cookie_name = "UP_TESTANYSIS_NAME";
        readonly string cookie_password = "UP_TESTANYSIS_PASSWORD";
        readonly string cookie_remember = "UP_TESTANYSIS_REMEMBER";

        LoginModel lgmodel = new LoginModel();

        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(lgmodel);
        }

        //
        // POST: /Account/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UserContext.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("ERROR", "服务器内部出错，请联系管理员");
                    return View(lgmodel);
                }
            }
            catch (Exception ex)
            {
                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                ModelState.AddModelError("ERROR", "服务器内部出错，请联系管理员");
                return View(model);
            }
        }

    }
}
