using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APKOnline.Models
{
    public class StaffModels
    {
        public string StaffLogin { get; set; }
        public string StaffPassword { get; set; }
    }
    public class Department
    {
        public int DEPid { get; set; }
        public string DEPcode { get; set; }
        public string DEPdescT { get; set; }
        public string DEPdescE { get; set; }

    }

    public class Position
    {
        public int Positionid { get; set; }
        public string Positioncode { get; set; }
        public string PositionName { get; set; }
        public decimal PositionLimit { get; set; }

    }

}