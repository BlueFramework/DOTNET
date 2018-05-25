﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Models
{
    public class CompanyInfo
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Representative { get; set; }

        public string Remark { get; set; }

        public int AccountId { get; set; }

        public decimal AccountBalance { get; set; }
    }
}