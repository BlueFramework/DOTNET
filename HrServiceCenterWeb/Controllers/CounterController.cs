using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using HrServiceCenterWeb.Manager;


namespace HrServiceCenterWeb.Controllers
{
    public class CounterController : Controller
    {
        // GET: BaseCode/GetPositions/
        [HttpGet]
        public ActionResult GetEmployeeCount()
        {
            ReportManager manager = new ReportManager();
            List<CounterBO> list = manager.GetBarChartData();
            CounterVO vo = fillCounter(list);
            JsonResult json = Json(vo);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetPositionCounts()
        {
            ReportManager manager = new ReportManager();
            List<CountetBase> list = manager.GetPositionCounts();
            JsonResult json = Json(list);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetDegreeCounts()
        {
            ReportManager manager = new ReportManager();
            List<CountetBase> list = manager.GetDegreeCounts();
            JsonResult json = Json(list);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetEmployeeCounts()
        {
            ReportManager manager = new ReportManager();
            List<CounterBO> list = manager.GetBarChartData();
            CounterVO vo = fillCounter(list);
            JsonResult json = Json(vo);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetPayCounts()
        {
            ReportManager manager = new ReportManager();
            List<object> list = manager.GetLineChartData();
            string[] month = new string[] { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
            var dataSource = new {
                xaxis= month,
                data= list
            };
            JsonResult json = Json(dataSource);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        private CounterVO fillCounter(List<CounterBO> list)
        {
            CounterVO vo = new CounterVO();
            vo.Data = new int[list.Count];
            vo.DataAxis = new string[list.Count];
            for(int i = 0; i < list.Count; i++)
            {
                CounterBO bo = list[i];
                vo.Data[i] = bo.Data;
                vo.DataAxis[i] = bo.DataAxis;
            }

            return vo;
        }
    }
}
