using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APKOnline.Models
{
    public class PRHeaderModels
    {
        public int Document_Id { get; set; }
        public int Document_Group { get; set; }
        public int Document_Category { get; set; }
        public int Document_Objective { get; set; }
        public string Document_Vnos { get; set; }
        public DateTime Document_Date { get; set; }
        public string Document_Means { get; set; }
        public string Document_Expect { get; set; }
        public string Document_Cus { get; set; }
        public string Document_Job { get; set; }
        public int Document_Depid { get; set; }
        public string Document_Dep { get; set; }
        public string Document_Per { get; set; }
        public string Document_Doc { get; set; }
        public string Document_Mec { get; set; }
        public string Document_Desc { get; set; }
        public string Document_Nolist { get; set; }
        public decimal Document_Cog { get; set; }
        public decimal Document_VatSUM { get; set; }
        public int Document_VatPer { get; set; }
        public decimal Document_NetSUM { get; set; }
        public int Document_Status { get; set; }
        public int Document_CreateUser { get; set; }
        public DateTime Document_CreateDate { get; set; }
        public int Document_EditUser { get; set; }
        public DateTime Document_EditDate { get; set; }
        public int Document_Delete { get; set; }
        public int Document_DeleteUser { get; set; }
        public DateTime Document_DeleteDate { get; set; }
        public string Document_Tel { get; set; }
        public string folderUpload { get; set; }
        public int Document_Term { get; set; }
        public string Document_Project { get; set; }
    }

    public class PRDetailModels
    {
        public int Document_Detail_Id { get; set; }
        public int Document_Detail_Hid { get; set; }

        public string Document_Detail_Vnos { get; set; }
        public DateTime Document_Detail_Date { get; set; }
        public string Document_Detail_Acc { get; set; }
        public string Document_Detail_Acc_Desc { get; set; }
        public string Document_Detail_Stk { get; set; }
        public string Document_Detail_Stk_Desc { get; set; }
        public int Document_Detail_ListNo { get; set; }
        public int Document_Detail_Quan { get; set; }
        public decimal Document_Detail_Cog { get; set; }
        public decimal Document_Detail_Vat { get; set; }
        public int Document_Detail_Sum { get; set; }
        public int Document_Detail_CreateUser { get; set; }
        public string Document_Detail_CreateDate { get; set; }
        public int Document_Detail_EditUser { get; set; }
        public string Document_Detail_EditDate { get; set; }
        public string Document_Detail_Delete { get; set; }
        public int Document_Detail_DeleteUser { get; set; }
        public string Document_Detail_DeleteDate { get; set; }
        public decimal Document_Detail_UnitPrice { get; set; }
    }
}