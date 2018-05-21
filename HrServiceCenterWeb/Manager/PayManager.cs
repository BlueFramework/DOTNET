using BlueFramework.Blood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Manager
{
    public class PayManager
    {

        public List<Models.TemplateInfo> GetTemplateList()
        {
            EntityContext context = Session.CreateContext();
            List<Models.TemplateInfo> list = context.SelectList<Models.TemplateInfo>("hr.pay.findTempList",null);
            return list;
        }
    }
}