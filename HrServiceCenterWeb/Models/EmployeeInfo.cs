using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrServiceCenterWeb.Models
{
    public class EmployeeInfo
    {
        public int PersonId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PositionId { get; set; }
        public string PersonName { get; set; }
        public string CardId { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public int Degree { get; set; }
        public string Polity { get; set; }
        public string Nation { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ContactsPerson { get; set; }
        public string ContactsPhone { get; set; }
        public string Specialty { get; set; }
        public string School { get; set; }
        public int Creator { get; set; }
        public string CreateTime { get; set; }
        public int State { get; set; }
    }
}