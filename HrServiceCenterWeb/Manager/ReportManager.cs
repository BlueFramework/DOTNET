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
        public List<object> GetLineChartData()
        {
            List<object> objlist = new List<object>();
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CountetBase> insuranceList = context.SelectList<Models.CountetBase>("hr.chart.insuranceCount", null);
            var insuranceobj = new {
                data = getMonthData(insuranceList),
                name = "保险部分"
            };
            objlist.Add(insuranceobj);
            List<Models.CountetBase> shouldPayList = context.SelectList<Models.CountetBase>("hr.chart.shouldPayCount", null);
            var shouldPayobj = new
            {
                data = getMonthData(shouldPayList),
                name = "工资部分"
            };
            objlist.Add(shouldPayobj);
            List<Models.CountetBase> truePayList = context.SelectList<Models.CountetBase>("hr.chart.truePayCount", null);
            var truePayobj = new
            {
                data = getMonthData(truePayList),
                name = "工资总额"
            };
            objlist.Add(truePayobj);
            return objlist;
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