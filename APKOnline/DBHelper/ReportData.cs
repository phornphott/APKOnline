using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APKOnline.DBHelper
{
    interface IReportData
    {
        DataTable GetReportBudget(string STARTDATE, string ENDDATE, string MONTHS, string StaffCode, string DEPcode, ref string errMsg);
    }

    public class ReportData: IReportData
    {
        public DataTable GetReportBudget(string STARTDATE, string ENDDATE, string MONTHS, string StaffCode, string DEPcode, ref string errMsg)
        {
            string Addition = null;
            DataTable dt = new DataTable();

            try
            {
                if (StaffCode != null && StaffCode != "undefined")
                {
                    Addition += " AND s.StaffCode=" + StaffCode;
                }
                if (DEPcode != null && StaffCode != "undefined")
                {
                    Addition += " AND d.DEPcode=" + DEPcode;
                }

                string strSQL = "\r\n SELECT distinct p.*, b.SumMonth" + MONTHS + " as SumMonth, s.StaffCode, s.StaffFirstName, s.StaffLastName" +
                    " , d.DEPcode, CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job, Objective_Name AS Objective, Category_Name AS Category FROM DocumentPR_Header p " +
                      "\r\n LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep " +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category " +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective " +
                      " LEFT JOIN Budget b on d.DEPcode=b.CodeDEP" +
                      " where Document_Delete=0 AND p.Document_Date BETWEEN '" + STARTDATE + "' AND '" + ENDDATE + "'" + Addition + ";";
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ReportBudget";
            return dt;
        }
    }
}
