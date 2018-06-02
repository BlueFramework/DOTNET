﻿using BlueFramework.Blood;
using BlueFramework.User;
using BlueFramework.User.Models;
using HrServiceCenterWeb.DataAccess;
using HrServiceCenterWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HrServiceCenterWeb.Manager
{
    public class PayManager
    {

        public List<Models.TemplateInfo> GetTemplateList()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.TemplateInfo> list = context.SelectList<Models.TemplateInfo>("hr.template.findTempList", null);
            return list;
        }

        public Models.TemplateInfo GetTemplate(int id)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            Models.TemplateInfo temp = context.Selete<Models.TemplateInfo>("hr.template.findTemplateById", id);
            return temp;
        }

        /// <summary>
        /// 获取模版树结构
        /// </summary>
        /// <returns></returns>
        public List<VEasyUiTree> GetTree()
        {
            List<VEasyUiTree> list = new List<VEasyUiTree>();
            DataTable dt = toDataTable<SalaryItemInfo>(GetTemplateTree());
            DataRow[] drs = dt.Select("ParentId = '0'");
            if (drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    list.Add(GetTree(dr, dt));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取树形菜单的子菜单
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public VEasyUiTree GetTree(DataRow dr, DataTable dt)
        {
            VEasyUiTree tree = new VEasyUiTree();
            tree.text = dr["Name"].ToString();
            tree.id = dr["ItemId"].ToString();
            DataRow[] drs = dt.Select("ParentId = '" + dr["ItemId"].ToString() + "'");
            if (drs.Length > 0)
            {
                tree.children = new List<VEasyUiTree>();
                foreach (DataRow mdr in drs)
                {
                    //递归子节点
                    tree.children.Add(GetTree(mdr, dt));
                }
            }
            return tree;
        }

        public List<SalaryItemInfo> GetTemplateTree()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<SalaryItemInfo> list = context.SelectList<SalaryItemInfo>("hr.template.findSalaryItem", null);
            return list;
        }

        /// <summary>
        /// 转换DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        private DataTable toDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        public int[] GetTemplateByTable(int tempId)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<TemplateDetailInfo> detail = context.SelectList<TemplateDetailInfo>("hr.template.getTemplateByTable", tempId);
            if (detail != null && detail.Count > 0)
            {
                int[] str = new int[detail.Count];
                for (int i = 0; i < detail.Count; i++)
                {
                    str[i] = detail[i].ItemId;
                }
                return str;
            }
            else
                return null;
        }

        public bool SaveTemplateForTable(int tempId, int[] tempItems)
        {
            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    context.Delete("hr.template.deleteTempDetial", tempId);

                    if (tempItems != null)
                    {
                        int[] temp = tempItems.Select(i => i).ToArray();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            TemplateDetailInfo tdi = new TemplateDetailInfo();
                            tdi.TemplateId = tempId;
                            tdi.ItemId = temp[i];
                            context.Save<TemplateDetailInfo>("hr.template.insertTemplateDetail", tdi);
                        }
                    }
                    TemplateInfo ti = new TemplateInfo();
                    ti.UpdateTime = DateTime.Now.ToShortDateString();
                    ti.TemplateId = tempId;
                    context.Save<TemplateInfo>("hr.template.updateTemplate", ti);

                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    return false;
                }
            }
        }

        public bool SaveTemplateMsg(int cmpId, int[] tempItems)
        {
            EmployeeManager em = new EmployeeManager();
            CompanyInfo cmp = em.GetCompany(cmpId);
            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    TemplateInfo tp = new TemplateInfo(cmp.CompanyId, cmp.Name + "-薪酬模版", UserContext.CurrentUser.UserId, DateTime.Now.ToShortDateString());
                    context.Save<TemplateInfo>("hr.template.insertTemplate", tp);

                    if (tempItems != null)
                    {
                        int[] temp = tempItems.Select(i => i).ToArray();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            TemplateDetailInfo tdi = new TemplateDetailInfo();
                            tdi.TemplateId = tp.TemplateId;
                            tdi.ItemId = temp[i];
                            context.Save<TemplateDetailInfo>("hr.template.insertTemplateDetail", tdi);
                        }
                    }
                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    return false;
                }
            }
        }

        public bool DeleteTemplate(int tempId)
        {
            try
            {
                EntityContext context = BlueFramework.Blood.Session.CreateContext();
                context.Delete("hr.template.deleteTemp", tempId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<InsuranceInfo> QueryImportorList(string query)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<InsuranceInfo> list = context.SelectList<InsuranceInfo>("hr.insurance.findInsurance", query);
            return list;
        }

        public bool DeleteInsurance(int id)
        {
            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    context.Delete("hr.insurance.deleteInsurance", id);

                    context.Delete("hr.insurance.deleteInsuranceDetail", id);
                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    return false;
                }
            }
        }

        public bool Import(DataTable dt, string fileName,ref string outmsg)
        {
            Dictionary<string, int> cardIds = getItemCardId();
            Dictionary<string, int> titles = getItemTitle();

            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();

                    //判断是否已经入库
                    //add code

                    InsuranceInfo ii = new InsuranceInfo();
                    ii.Title = fileName;
                    ii.CreatorId = UserContext.CurrentUser.UserId;
                    ii.CreateTime = DateTime.Now.ToShortDateString();
                    context.Save<InsuranceInfo>("hr.insurance.insertInsurance", ii);

                    foreach (DataRow row in dt.Rows)
                    {
                        if (cardIds.ContainsKey(row["身份证号码"].ToString()) && titles.ContainsKey(row["险种"].ToString()))
                        {
                            InsuranceDetailInfo idi = new InsuranceDetailInfo();
                            idi.ImportId = ii.ImportId;
                            idi.PersonId = cardIds[row["身份证号码"].ToString()];
                            idi.PayMonth = row["账户月度"].ToString();
                            idi.ItemId = titles[row["险种"].ToString()];
                            idi.ItemValue = decimal.Parse(row["缴存值"].ToString());
                            context.Save<InsuranceDetailInfo>("hr.insurance.insertInsuranceDetail", idi);
                        }
                    }
                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    outmsg += "服务器内部错误，请联系管理员";
                    context.Rollback();
                    return false;
                }
            }
        }


        private Dictionary<string, int> getItemCardId()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<EmployeeInfo> list = context.SelectList<EmployeeInfo>("hr.employee.findEmployeesCardId", null);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (EmployeeInfo ei in list)
            {
                if (!dic.ContainsKey(ei.CardId))
                    dic.Add(ei.CardId, ei.PersonId);
            }
            return dic;
        }

        private Dictionary<string, int> getItemTitle()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<SalaryItemInfo> list = context.SelectList<SalaryItemInfo>("hr.template.findSalaryItemTitle", null);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (SalaryItemInfo si in list)
            {
                if (!dic.ContainsKey(si.Name))
                    dic.Add(si.Name, si.ItemId);
            }
            return dic;
        }
    }
}