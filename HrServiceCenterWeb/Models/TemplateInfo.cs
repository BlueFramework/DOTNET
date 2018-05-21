using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Models
{
    public class TemplateInfo
    {
        public int TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string CompanyName { get; set; }

        public string CreatTime { get; set; }
        
        public string Representative { get; set; }
    }
}