using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Models
{
    /// <summary>
    /// 岗位信息
    /// </summary>
    public class PositionInfo
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public string Desc { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal BaseSalary { get; set; }

        /// <summary>
        /// 辅助工资
        /// </summary>
        public decimal AssistSalary { get; set; }

        /// <summary>
        /// 慰问费
        /// </summary>
        public decimal ConsolationFee { get; set; }

        /// <summary>
        /// 社保/年
        /// </summary>
        public decimal Insurance { get; set; }

        /// <summary>
        /// 公积金/年
        /// </summary>
        public decimal AccumulationFund { get; set; }
    }
}