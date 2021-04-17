using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APKOnline.Models
{
    public class StaffModels
    {
        public int StaffID { get; set; }
        public string StaffCode { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffPosition { get; set; }    
        public string StaffLogin { get; set; }
        public string StaffPassword { get; set; }
        public int StaffLevel { get; set; }
        public int StaffDepartmentID { get; set; }
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

    public class StaffAuthorize
    {
        public int Authorizeid { get; set; }
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public int DEPid { get; set; }
        public int PositionPermissionId { get; set; }
        public string StaffCode { get; set; }
        public string DEPdescT { get; set; }
        public string PositionCode { get; set; }
        public decimal PositionLimit { get; set; }
        public int AuthorizeLevel { get; set; }
        public bool isPreview { get; set; }
    }

    public class BudgetByDep
    {
        public int id { get; set; }
        public int DEPid { get; set; }
        public string DEPcode { get; set; }
        public string DEPdescT { get; set; }
        public string ColumnName { get; set; }
        public decimal Budget { get; set; }
        public int  Year { get; set; }
    }

}