﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HrServiceCenterWeb.Models;
using BlueFramework.Blood;
using BlueFramework.Blood.DataAccess;

namespace HrServiceCenterWeb.Manager
{
    public class PaymentManager
    {
        public void CreatePayment(Payment payment)
        {
            EntityContext context = Session.CreateContext();
            TemplateInfo template = context.Selete<TemplateInfo>("hr.payment.findDefaultTemplate", 0);
            List<PayItemDO> items = context.SelectList<PayItemDO>("hr.payment.findTemplateItems", template.TemplateId);
            List<PayObjectDO> objects = context.SelectList<PayObjectDO>("hr.payment.findCompanyPersons", payment.CompanyId);
            CommandParameter[] parameters =
            {
                new CommandParameter("CompanyId",payment.CompanyId),
                new CommandParameter("PayMonth",DateTime.Parse(payment.PayMonth).ToString("yyyyMM") )
            };
            List<PayValueInfo> payValues = context.SelectList<PayValueInfo>("hr.payment.findCompanyPersonsValue", parameters);

            using (context)
            {
                try
                {
                    context.BeginTransaction();
                    payment.TemplateId = template.TemplateId;
                    context.Save<Payment>("hr.payment.insertPayment", payment);
                    foreach(PayItemDO item in items)
                    {
                        item.PaymentId = payment.PayId;
                        context.Save<PayItemDO>("hr.payment.insertPayment.items", item);
                    }
                    foreach (PayObjectDO payObject in objects)
                    {
                        payObject.PaymentId = payment.PayId;
                        context.Save<PayObjectDO>("hr.payment.insertPayment.object", payObject);
                    }
                    foreach (PayValueInfo payValue in payValues)
                    {
                        payValue.PayId = payment.PayId;
                        context.Save<PayValueInfo>("hr.payment.insertPayment.detail", payValue);
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

        public Payment LoadPayment(int paymentId)
        {
            EntityContext context = Session.CreateContext();
            Payment payment = context.Selete<Payment>("hr.payment.findPayment", paymentId) ;
            List<PayItemDO> items = context.SelectList<PayItemDO>("hr.payment.findPaymentItems", paymentId);
            List<PayObjectDO> objects = context.SelectList<PayObjectDO>("hr.payment.findPaymentPersons", paymentId);
            List<PayValueInfo> payValues = context.SelectList<PayValueInfo>("hr.payment.findPaymentValue", paymentId);
            payment.Items = items;
            payment.Objects = objects;
            DataTable dt = new DataTable();
            dt.Columns.Add("PersonId");
            dt.Columns.Add("PersonName");
            dt.Columns.Add("PersonCode");
            foreach(PayItemDO item in items)
            {
                if (item.IsLeaf == false) continue;
                item.ItemName = "f_" + item.ItemId;
                DataColumn column = new DataColumn(item.ItemName, Type.GetType("System.Decimal"));
                column.DefaultValue = 0;
                dt.Columns.Add(column);
                
            }

            Dictionary<int, DataRow> rows = new Dictionary<int, DataRow>();
            foreach(PayObjectDO o in objects)
            {
                DataRow dr = dt.NewRow();
                dr["PersonId"] = o.ObjectId;
                dr["PersonName"] = o.ObjectName;
                dr["PersonCode"] = o.ObjectCode;
                dt.Rows.Add(dr);
                rows.Add(o.ObjectId, dr);
            }
            // fill values by dictionary
            foreach(PayValueInfo pv in payValues)
            {
                DataRow dr = rows[pv.PersonId];
                string columnName = "f_" + pv.ItemId;
                dr[columnName] = pv.PayValue;
            }
 
            // TODO:calculate by formula but this is fix code now
            #region calculate columns
            foreach (DataRow dr in dt.Rows)
            {
                dr["f_199"] =
                    decimal.Parse(dr["f_101"].ToString()) +
                    decimal.Parse(dr["f_102"].ToString()) +
                    decimal.Parse(dr["f_103"].ToString()) +
                    decimal.Parse(dr["f_104"].ToString()) +
                    decimal.Parse(dr["f_105"].ToString()) +
                    decimal.Parse(dr["f_106"].ToString()) +
                    decimal.Parse(dr["f_107"].ToString()) +
                    decimal.Parse(dr["f_108"].ToString()) +
                    decimal.Parse(dr["f_109"].ToString()) +
                    decimal.Parse(dr["f_110"].ToString());
                dr["f_299"] =
                    decimal.Parse(dr["f_201"].ToString()) +
                    decimal.Parse(dr["f_202"].ToString()) +
                    decimal.Parse(dr["f_203"].ToString()) +
                    decimal.Parse(dr["f_204"].ToString()) +
                    decimal.Parse(dr["f_205"].ToString()) +
                    decimal.Parse(dr["f_206"].ToString());
                dr["f_3"] =
                    decimal.Parse(dr["f_199"].ToString()) -
                    decimal.Parse(dr["f_299"].ToString());
                dr["f_7"] =
                    decimal.Parse(dr["f_199"].ToString()) +
                    decimal.Parse(dr["f_4"].ToString()) +
                    decimal.Parse(dr["f_5"].ToString()) +
                    decimal.Parse(dr["f_6"].ToString());
            }
            #endregion

            payment.DataSource = dt;

            payment.Sheet = new PayDetailVO();
            payment.Sheet.rows = dt;
            payment.Sheet.total = dt.Rows.Count;
            payment.Sheet.footer = new List<JObject>();
            JObject joSum = new JObject();
            joSum.Add("PersonCode", "合计");
            payment.Sheet.footer.Add(joSum);
            for(int i = 3; i < dt.Columns.Count; i++)
            {
                string columnName = dt.Columns[i].ColumnName;
                string exp = string.Format("sum({0})", columnName);
                object sum = dt.Compute(exp, "");
                joSum.Add(columnName, sum.ToString());

            }

            return payment;
        }

        public bool ImportPayment(int paymentId,DataTable dt,out string message)
        {
            message = string.Empty;
            EntityContext context = Session.CreateContext();
            List<PayItemDO> items = context.SelectList<PayItemDO>("hr.payment.findPaymentItems", paymentId);
            List<PayObjectDO> objects = context.SelectList<PayObjectDO>("hr.payment.findPaymentPersons", paymentId);
            List<PayValueInfo> payValues = new List<PayValueInfo>();
            Dictionary<string, PayItemDO> dicItems = new Dictionary<string, PayItemDO>();
            foreach(PayItemDO pi in items)
            {
                if (pi.Editable==1)
                    dicItems.Add(pi.ItemCaption, pi);
            }
            int rownum = 1;
            foreach(DataRow dr in dt.Rows)
            {
                int objectId = int.Parse(dr["ID"].ToString());
                foreach(DataColumn column in dt.Columns)
                {
                    bool exist = dicItems.ContainsKey(column.ColumnName);
                    if (!exist) continue;
                    string cellValue = dr[column.ColumnName].ToString();
                    if (string.IsNullOrEmpty(cellValue)) continue;
                    PayItemDO item = dicItems[column.ColumnName];
                    PayValueInfo pv = new PayValueInfo();
                    pv.PayId = paymentId;
                    pv.PersonId = objectId;
                    pv.ItemId = item.ItemId;
                    decimal payValue = 0;
                    bool success = decimal.TryParse(cellValue, out payValue);
                    if (success)
                        pv.PayValue = payValue;
                    else
                        message = message + string.Format("第{0}行{1}数据异常.",rownum, column.ColumnName);
                    payValues.Add(pv);
                    rownum++;
                }
            }

            using (context)
            {
                try
                {
                    context.BeginTransaction();
                    foreach (PayValueInfo payValue in payValues)
                    {
                        context.Save<PayValueInfo>("hr.payment.insertPayment.detail", payValue);
                    }

                    context.Commit();
                }
                catch (Exception ex)
                {
                    BlueFramework.Common.Logger.LoggerFactory.CreateDefault().Info(ex.Message);
                    message += ex.Message;
                    context.Rollback();
                }
            }
            if (string.IsNullOrEmpty(message))
                return true;
            else
                return false;
        }
    }
}