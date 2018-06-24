using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HrServiceCenterWeb.Models;
using BlueFramework.Blood;
using BlueFramework.Blood.DataAccess;

namespace HrServiceCenterWeb.Manager
{
    public class PaymentManager
    {
        public void CreatePayment(Payment payment)
        {
            payment.CreateTime = DateTime.Now;
            EntityContext context = Session.CreateContext();
            TemplateInfo template = context.Selete<TemplateInfo>("hr.payment.findDefaultTemplate", 0);
            List<PayItemDO> items = context.SelectList<PayItemDO>("hr.payment.findTemplateItems", template.TemplateId);
            List<PayObjectDO> objects = context.SelectList<PayObjectDO>("hr.payment.findCompanyPersons", payment.CompanyId);
            CommandParameter[] parameters =
            {
                new CommandParameter("CompanyId",payment.CompanyId),
                new CommandParameter("PayMonth",payment.PayMonth)
            };
            List<PayValueInfo> payValues = context.SelectList<PayValueInfo>("hr.payment.findCompanyPersonsValue", parameters);

            using (context)
            {
                try
                {
                    context.BeginTransaction();
                    context.Save<Payment>("hr.payment.insertPayment", payment);
                    foreach(PayItemDO item in items)
                    {
                        item.PaymentId = payment.PayId;
                        context.Save<PayItemDO>("insertPayment.items", item);
                    }

                    context.Commit();
                }
                catch (Exception ex)
                {
                    BlueFramework.Common.Logger.LoggerFactory.CreateDefault().Info(ex.Message);
                    context.Rollback();
                }
            }


        }
    }
}