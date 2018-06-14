using BlueFramework.Blood;
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
using System.Xml;

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
        /// 根据公司ID找发放模版
        /// </summary>
        /// <param name="id">公司ID</param>
        /// <returns></returns>
        public Models.TemplateInfo GetTemplateByCompanyId(int id)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            Models.TemplateInfo temp = context.Selete<Models.TemplateInfo>("hr.template.findTemplateByCompanyId", id);
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

        /// <summary>
        /// 保险导入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public bool Import(DataTable dt, string fileName, ref string outmsg)
        {
            Dictionary<string, int> cardIds = getItemCardId();
            Dictionary<string, int> titles = getItemTitle();
            Dictionary<string, int> thirddic = getThirdItem();
            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    //判断是否已经入库
                    List<InsuranceInfo> list = context.SelectList<InsuranceInfo>("hr.insurance.findInsuranceByTitle", fileName);
                    if (list.Count > 0)
                    {
                        outmsg += "文件：" + fileName + "已上传！<br />";
                        return false;
                    }
                    //入库导入主表
                    InsuranceInfo ii = new InsuranceInfo();
                    ii.Title = fileName;
                    ii.CreatorId = UserContext.CurrentUser.UserId;
                    ii.CreateTime = DateTime.Now.ToShortDateString();
                    context.Save<InsuranceInfo>("hr.insurance.insertInsurance", ii);
                    foreach (DataRow row in dt.Rows)
                    {
                        //入库导入详细表
                        InsuranceDetailInfo idi = new InsuranceDetailInfo();
                        idi.ImportId = ii.ImportId;
                        idi.PayMonth = row["账户月度"].ToString();
                        idi.ItemValue = decimal.Parse(row["缴存值"].ToString());
                        idi.ImportColumnName = row["险种"].ToString();
                        if (cardIds.ContainsKey(row["身份证号码"].ToString()))
                        {
                            idi.PersonId = cardIds[row["身份证号码"].ToString()];
                        }
                        else
                        {
                            //未找到人员，忽略该行
                            outmsg += "第" + dt.Rows.IndexOf(row) + "行人员未知！<br />";
                            dt.Rows.Remove(row);
                            continue;
                        }
                        if (titles.ContainsKey(row["险种"].ToString()))
                        {
                            idi.ItemId = titles[row["险种"].ToString()];
                        }
                        else if (thirddic.ContainsKey(row["险种"].ToString()))
                        {
                            idi.ItemId = thirddic[row["险种"].ToString()];
                        }
                        else
                        {
                            //值错误，不入库
                            outmsg += "第" + dt.Rows.IndexOf(row) + "行险种不匹配！<br />";
                            context.Rollback();
                            return false;
                        }
                        context.Save<InsuranceDetailInfo>("hr.insurance.insertInsuranceDetail", idi);
                    }
                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    outmsg += "服务器内部错误，请联系管理员！<br />";
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

        /// <summary>
        /// 匹配非数据库保险列表
        /// 用XML配置
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, int> getThirdItem()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            XmlDocument doc = new XmlDocument();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Setting/Insurance/InsuranceConfig.xml";
            doc.Load(path);
            XmlNode menus = doc.SelectSingleNode("Root");
            XmlNodeList xn = menus.ChildNodes;
            foreach (XmlNode xmlNode in xn)
            {
                XmlElement xe = (XmlElement)xmlNode;
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType != XmlNodeType.Element) continue;
                    XmlElement nd = (XmlElement)node;
                    dic.Add(nd.GetAttribute("name"), int.Parse(xe.GetAttribute("code")));
                }
            }
            return dic;
        }

        public List<InsuranceDetailInfo> QueryInsuranceDetail(int importId)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<InsuranceDetailInfo> list = context.SelectList<InsuranceDetailInfo>("hr.insurance.findInsuranceDetailById", importId);
            return list;
        }

        public List<PayList> QueryPayList(string query)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<PayList> list = context.SelectList<PayList>("hr.pay.findPayList", query);
            return list;
        }

        public HashSet<int> QueryTemplateDetail(int id)
        {
            HashSet<int> result = new HashSet<int>();
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<TemplateDetailInfo> list = context.SelectList<TemplateDetailInfo>("hr.template.findTemplateDetailByCompanyId", id);
            foreach (TemplateDetailInfo temp in list)
            {
                result.Add(temp.ItemId);
            }
            return result;
        }

        public List<PayDetailInfo> GetPayDetail(int id)
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<PayDetailInfo> list = context.SelectList<PayDetailInfo>("hr.pay.findPayDetail", id);
            Dictionary<int, Dictionary<string, decimal>> dic = GetPersionInsuranceDic();
            foreach (PayDetailInfo info in list)
            {
                if (dic.ContainsKey(info.PersonId))
                {
                    foreach (KeyValuePair<string, decimal> kv in dic[info.PersonId])
                    {
                        Type type = info.GetType();
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(kv.Key);
                        propertyInfo.SetValue(info, kv.Value);
                    }
                }
            }
            return list;
        }

        public Dictionary<int, Dictionary<string, decimal>> GetPersionInsuranceDic()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<InsuranceDetailInfo> list = context.SelectList<InsuranceDetailInfo>("hr.insurance.findInsuranceDetailByNewMonth", null);
            Dictionary<int, string> ItemConfigDic = GetItemConfigDic();
            Dictionary<int, Dictionary<string, decimal>> dic = new Dictionary<int, Dictionary<string, decimal>>();
            foreach (InsuranceDetailInfo info in list)
            {
                if (!dic.ContainsKey(info.PersonId))
                {
                    Dictionary<string, decimal> sdic = new Dictionary<string, decimal>();
                    if (ItemConfigDic.ContainsKey(info.ItemId))
                    {
                        sdic.Add(ItemConfigDic[info.ItemId], info.ItemValue);
                        dic.Add(info.PersonId, sdic);
                    }
                }
                else
                {
                    if (ItemConfigDic.ContainsKey(info.ItemId))
                    {
                        dic[info.PersonId].Add(ItemConfigDic[info.ItemId], info.ItemValue);
                    }
                }
            }
            return dic;
        }

        public Dictionary<int, string> GetItemConfigDic()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            XmlDocument doc = new XmlDocument();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Setting/Pay/PayTableConfig.xml";
            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("Root");
            XmlNodeList xn = root.ChildNodes;
            foreach (XmlNode xmlnode in xn)
            {
                XmlElement xe = (XmlElement)xmlnode;
                if (xe.GetAttribute("isDynamic") == "true")
                {
                    dic.Add(int.Parse(xe.GetAttribute("code")), xe.GetAttribute("fieldName"));
                }
                if (xe.GetAttribute("isLastStage") == "false")
                {
                    XmlNodeList child = xmlnode.ChildNodes;
                    foreach (XmlNode node in child)
                    {
                        XmlElement nd = (XmlElement)node;
                        dic.Add(int.Parse(nd.GetAttribute("code")), nd.GetAttribute("fieldName"));
                    }
                }
            }
            return dic;
        }

        public bool SavePayDetail(List<PayDetailInfo> list, int cmpid, string tname, string time, string count, ref string msg)
        {
            using (EntityContext context = BlueFramework.Blood.Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();

                    Models.TemplateInfo temp = context.Selete<Models.TemplateInfo>("hr.template.findTemplateIdByCompanyId", cmpid);
                    PayList pl = new PayList();
                    pl.CompanyId = cmpid;
                    pl.TemplateId = temp.TemplateId;
                    pl.PayTitle = tname;
                    pl.PayMonth = time;
                    pl.CreatorId = UserContext.CurrentUser.UserId;
                    pl.CreateTime = DateTime.Now.ToShortDateString();
                    context.Save<PayList>("hr.pay.insertPayTable", pl);

                    Dictionary<string, int> dic = GetIdConfigDic();
                    foreach (PayDetailInfo info in list)
                    {
                        foreach (System.Reflection.PropertyInfo p in info.GetType().GetProperties())
                        {
                            if (dic.ContainsKey(p.Name))
                            {
                                PayValueInfo pvi = new PayValueInfo();
                                pvi.PayId = pl.PayId;
                                pvi.ItemId = dic[p.Name];
                                pvi.PersonId = info.PersonId;
                                pvi.PayValue = decimal.Parse(p.GetValue(info).ToString());
                                context.Save<PayValueInfo>("hr.pay.insertPayTableDetail", pvi);
                            }
                        }
                    }

                    CompanyAccountInfo cai = new CompanyAccountInfo();
                    cai.AccountBalance = decimal.Parse(count);
                    cai.CompanyId = cmpid;
                    context.Save<CompanyAccountInfo>("hr.company.updateCompanyBalance", cai);

                    context.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    msg += "服务器内部错误，请联系管理员；";
                    context.Rollback();
                    return false;
                }
            }
        }

        public Dictionary<string, int> GetIdConfigDic()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            XmlDocument doc = new XmlDocument();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Setting/Pay/PayTableConfig.xml";
            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("Root");
            XmlNodeList xn = root.ChildNodes;
            foreach (XmlNode xmlnode in xn)
            {
                XmlElement xe = (XmlElement)xmlnode;
                if (xe.GetAttribute("isDynamic") == "true")
                {
                    dic.Add(xe.GetAttribute("fieldName"), int.Parse(xe.GetAttribute("code")));
                }
                if (xe.GetAttribute("isLastStage") == "false")
                {
                    XmlNodeList child = xmlnode.ChildNodes;
                    foreach (XmlNode node in child)
                    {
                        XmlElement nd = (XmlElement)node;
                        dic.Add(nd.GetAttribute("fieldName"), int.Parse(nd.GetAttribute("code")));
                    }
                }
            }
            return dic;
        }
    }
}