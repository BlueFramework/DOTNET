using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HrServiceCenterWeb.Models;
using HrServiceCenterWeb.Manager;
using BlueFramework.User;

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
            return null;
        }

        public ActionResult SavePayment(Payment payment)
        {
            return null;
        }
    }
}