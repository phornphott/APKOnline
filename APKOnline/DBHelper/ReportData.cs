using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace APKOnline.DBHelper
{
    interface IReportData
    {
        DataTable GetReportBudget(string STARTDATE, string ENDDATE, string MONTHS, string StaffCode, string DEPcode, ref string errMsg);
        Task<DataSet> GetDashBroadData();
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
        public async Task<DataSet> GetDashBroadData()
        {
            string Addition = null;
            DataSet ds = new DataSet();
            string date = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1).ToString("yyyy-MM-dd");
            int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            string todate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).ToString("yyyy-MM-dd");

            date = "2020-04-01";
             todate = "2020-04-30";

            string sql = "";

            sql = " select SUM(Document_NetSUM) AS Amount,Document_Dep  ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,CAST(DEPdescT as NVARCHAR(max)) As DEP " +
                  " from DocumentPO_Header h " +
                  " left join Department d on h.Document_Dep = d.DEPid " +
                  " where Document_Date between  '"+ date + "'  and '"+ todate + "' group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";
            
            DataTable poDepAmount  = DBHelper.List(sql);
            poDepAmount.TableName = "DepAmount";
            ds.Tables.Add(poDepAmount);
            return ds;
        }

    }
}
