using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Models
{
    /// <summary>
    /// 发放列表实体
    /// </summary>
    public class PayMent
    {
        public int PayId { get; set; }

        public int TemplateId { get; set; }

        public string TemplateName { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string PayTitle { get; set; }

        public string PayMonth { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public string CreateTime { get; set; }

        /// <summary>
        /// 审核状态0 未审核  2归档
        /// </summary>
        public int Status { get; set; }
    }
}