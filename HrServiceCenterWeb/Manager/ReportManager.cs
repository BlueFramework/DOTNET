using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlueFramework.Blood;

namespace HrServiceCenterWeb.Manager
{
    public class ReportManager
    {
        public List<Models.CounterBO> GetEmployeeCounts()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CounterBO> list = context.SelectList<Models.CounterBO>("hr.chart.employeeCount", null);
            return list;
        }

        public List<Models.CounterBO> GetPositionCounts()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CounterBO> list = context.SelectList<Models.CounterBO>("hr.chart.positionCount", null);
            return list;
        }

        public List<Models.CounterBO> GetDegreeCounts()
        {
            EntityContext context = BlueFramework.Blood.Session.CreateContext();
            List<Models.CounterBO> list = context.SelectList<Models.CounterBO>("hr.chart.degreeCount", null);
            return list;
        }
    }
}