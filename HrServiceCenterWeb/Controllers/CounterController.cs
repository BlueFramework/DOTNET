﻿using System;
using System.Collections.Generic;
using System.Linq;
using HrServiceCenterWeb.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using HrServiceCenterWeb.Models;
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
            List<CounterBO> list = manager.GetEmployeeCounts();
            CounterVO vo = fillCounter(list);
            JsonResult json = Json(vo);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetPositionCounts()
        {
            ReportManager manager = new ReportManager();
            List<CounterBO> list = manager.GetPositionCounts();
            CounterVO vo = fillCounter(list);
            JsonResult json = Json(vo);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetEmployeeCounts()
        {
            ReportManager manager = new ReportManager();
            List<CounterBO> list = manager.GetEmployeeCounts();
            CounterVO vo = fillCounter(list);
            JsonResult json = Json(vo);
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
