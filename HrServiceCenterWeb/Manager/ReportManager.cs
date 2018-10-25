using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlueFramework.Blood;

namespace HrServiceCenterWeb.Manager
{
    public class ReportManager
    {
        /// <summary>
        /// 获取岗位分布饼图数据
        /// </summary>
        /// <returns></returns>
        public List<Models.CountetBase> GetPositionCounts()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CountetBase> list = context.SelectList<Models.CountetBase>("hr.chart.positionCount", null);
            return list;
        }

        /// <summary>
        /// 获取学历分布饼图数据
        /// </summary>
        /// <returns></returns>
        public List<Models.CountetBase> GetDegreeCounts()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CountetBase> list = context.SelectList<Models.CountetBase>("hr.chart.degreeCount", null);
            return list;
        }

        /// <summary>
        /// 获取人员统计柱状图数据
        /// </summary>
        public List<Models.CounterBO> GetBarChartData()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CounterBO> list = context.SelectList<Models.CounterBO>("hr.chart.employeeCount", null);
            return list;
        }

        /// <summary>
        /// 获取保险，应发工资，实发工资统计折线图数据
        /// </summary>
        /// <returns></returns>
        public List<Models.CounterVO> GetLineChartData()
        {
            List<Models.CounterVO> series = new List<Models.CounterVO>();
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CounterBO> insuranceList = context.SelectList<Models.CounterBO>("hr.chart.insuranceCount", null);
            List<Models.CounterBO> shouldPayList = context.SelectList<Models.CounterBO>("hr.chart.shouldPayCount", null);
            List<Models.CounterBO> personPayList = context.SelectList<Models.CounterBO>("hr.chart.personPayCount", null);
            List<Models.CounterBO> truePayList = context.SelectList<Models.CounterBO>("hr.chart.truePayCount", null);
            List<Models.CounterBO> servicePayList = context.SelectList<Models.CounterBO>("hr.chart.servicePayCount", null);

            Dictionary<string, DateTime> months = new Dictionary<string, DateTime>();
            DateTime startDate = DateTime.Parse( DateTime.Now.ToString("yyyy-MM-01") );
            int i = 0;
            for(i = -12; i <=0; i++)
            {
                DateTime date = startDate.AddMonths(i);
                string month = date.ToString("yyyyMM");
                months.Add(month, date);
            }

            Models.CounterVO s1 = new Models.CounterVO();
            Models.CounterVO s2 = new Models.CounterVO();
            Models.CounterVO s3 = new Models.CounterVO();
            Models.CounterVO s4 = new Models.CounterVO();
            Models.CounterVO s5 = new Models.CounterVO();
            s1.DataAxis = s2.DataAxis = s3.DataAxis = months.Keys.ToArray();
            s1.Data = new decimal[13]; s1.Title = "单位社保公积金部分";
            s2.Data = new decimal[13]; s2.Title = "单位工资部分";
            s3.Data = new decimal[13]; s3.Title = "个人社保部分";
            s4.Data = new decimal[13]; s4.Title = "单位+个人费用总额";
            s5.Data = new decimal[13]; s5.Title = "服务费";

            i = 0;
            foreach (string x in months.Keys)
            {
                string month = months[x].ToString("yyyy-MM-dd");
                foreach (Models.CounterBO o in insuranceList)
                {
                    if (o.DataAxis == month)
                    {
                        s1.Data[i] = o.Data;
                        break;
                    }
                }
                foreach (Models.CounterBO o in shouldPayList)
                {
                    if (o.DataAxis == month)
                    {
                        s2.Data[i] = o.Data;
                        break;
                    }
                }
                foreach (Models.CounterBO o in personPayList)
                {
                    if (o.DataAxis == month)
                    {
                        s3.Data[i] = o.Data;
                        break;
                    }
                }
                foreach (Models.CounterBO o in truePayList)
                {
                    if (o.DataAxis == month)
                    {
                        s4.Data[i] = o.Data;
                        break;
                    }
                }
                foreach (Models.CounterBO o in servicePayList)
                {
                    if (o.DataAxis == month)
                    {
                        s5.Data[i] = o.Data;
                        break;
                    }
                }
                i++;
            }
            series.Add(s1);
            series.Add(s2);
            series.Add(s3);
            series.Add(s4);
            series.Add(s5);
            return series;
        }

        private decimal[] getMonthData(List<Models.CountetBase> list)
        {
            if (list == null)
            {
                return new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            string[] month = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            decimal[] data = new decimal[12];
            for (int i = 0; i < month.Length; i++)
            {
                Models.CountetBase obj = new Models.CountetBase();
                obj = list.Where(l => l.name == month[i]).FirstOrDefault();
                if (obj != null)
                    data[i] = obj.moneyValue;
                else
                    data[i] = 0;
            }
            return data;
        }
    }
}