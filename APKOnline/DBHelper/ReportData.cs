using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace APKOnline.DBHelper
{
    interface IReportData
    {
        DataTable GetReportBudget(string STARTDATE, string ENDDATE, string MONTHS, int StaffCode, int DEPcode, ref string errMsg);
        DataSet GetDashBroadData(ref string errMsg);
        DataSet GetDashBroadByDepartment(int id, ref string errMsg);
        int getdepid(string dep);
    }

    public class ReportData: IReportData
    {
        public DataTable GetReportBudget(string STARTDATE, string ENDDATE, string MONTHS, int StaffCode, int DEPcode, ref string errMsg)
        {
            string Addition = null;
            DataTable dt = new DataTable();

            try
            {
                //if (StaffCode != null && StaffCode != "undefined")
                if (StaffCode != 0)
                {
                    //Addition += " AND s.StaffCode=" + StaffCode;
                    Addition += " AND p.Document_CreateUser=" + StaffCode;
                }
                //if (DEPcode != null && StaffCode != "undefined")
                if (DEPcode != 0)
                {
                    Addition += " AND p.Document_Dep=" + DEPcode;
                }

                string strSQL = "\r\n SELECT distinct p.*, b.DEPmonth" + MONTHS + " as SumMonth, s.StaffCode, s.StaffFirstName, s.StaffLastName" +
                    " , d.DEPcode, CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job, Objective_Name AS Objective, Category_Name AS Category FROM DocumentPR_Header p " +
                      "\r\n LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep " +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category " +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective " +
                      " LEFT JOIN BudgetOfYearByDepartment b on d.DEPcode=b.DEPcode" +
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
        public  DataSet GetDashBroadData(ref string  errMsg)
        {
            string Addition = null;
            DataSet ds = new DataSet();
            string date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            string todate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).ToString("yyyy-MM-dd", new CultureInfo("en-US"));

            //date = "2020-04-01";
            //todate = "2020-04-30";

            string sql = "";

            //sql = " select SUM(Document_NetSUM) AS Amount,Document_Dep  ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,REPLACE(REPLACE(trim(CAST(DEPdescT as NVARCHAR(max))), CHAR(13), ''), CHAR(10), '') As DEP " +
            //      " from DocumentPO_Header h " +
            //      " left join Department d on h.Document_Dep = d.DEPid " +
            //      " where Document_Date between  '" + date + "'  and '" + todate + "' group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";
            try
            {
                sql = " select SUM(Document_NetSUM) AS Amount,Document_Dep  ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,REPLACE(REPLACE(CAST(DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                      " from DocumentPO_Header h " +
                      " left join Department d on h.Document_Dep = d.DEPid " +
                      " where Document_Date between  '" + date + "'  and '" + todate + "' group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";
                errMsg = sql;
DataTable poDepAmount = DBHelper.List(sql);
                poDepAmount.TableName = "DepAmount";
                ds.Tables.Add(poDepAmount);
            }

            catch(Exception ex) { errMsg = ex.Message; }
            return ds;
        }
        public  DataSet GetDashBroadByDepartment(int id, ref string errMsg)
        {
            string Addition = null;
            DataSet ds = new DataSet();
            int startdate = 1;
            string month = DateTime.Today.Month.ToString("00");
            //month = "04";
            int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, Convert.ToInt32(month));

            string date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            string todate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).ToString("yyyy-MM-dd", new CultureInfo("en-US"));

            //date = "2020-04-01";
            //todate = "2020-04-30";



            string sql = "";
            try
            {
                for (int i = startdate; i <= LastdayofMonth; i++)
                {

                    string selectdate = "'" + (DateTime.Today.Year>2500? (DateTime.Today.Year-543).ToString() : DateTime.Today.Year.ToString()) + "-" + month + "-" + i.ToString("00") + "'";
                    string selecttodate = "'" + (DateTime.Today.Year > 2500 ? (DateTime.Today.Year - 543).ToString() : DateTime.Today.Year.ToString()) + "-" + month + "-" + (i + 1).ToString("00") + "'";
                    if (i == LastdayofMonth)
                    {
                        selecttodate = "'" + (DateTime.Today.Year > 2500 ? (DateTime.Today.Year - 543).ToString() : DateTime.Today.Year.ToString()) + "-" + (DateTime.Today.Month + 1).ToString("00") + "-" + "01'";
                    }
                    else if (i == 1)
                    {
                        sql += " SELECT * FROM (";
                    }
                    sql += "\r\n (select " + i + " as date,ISNULL(SUM(Document_NetSUM),0) AS Amount from DocumentPO_Header h  " +
                           " where Document_Dep = " + id + " and Document_Date BETWEEN " + selectdate + " AND " + selecttodate + ")";
                    if (i < LastdayofMonth)
                        sql += " UNION ";
                    else if (i == LastdayofMonth)
                        sql += " ) aa Order By date";
                }



                DataTable poDepAmount = DBHelper.List(sql);
                poDepAmount.TableName = "DepAmount";
                ds.Tables.Add(poDepAmount);


                sql = "select po.Document_Vnos,po.Document_NetSUM AS POAmount,pr.Document_NetSUM AS PRAmount" +
                    " from DocumentPO_Header po " +
                    " left join DocumentPR_Header pr on pr.Document_Id = po.Document_PRID " +
                    " where po.Document_Status = 2 AND po.Document_Date BETWEEN '" + date + "' AND '" + todate + "' ";
                DataTable podata = new DataTable();
                podata = DBHelper.List(sql);
                podata.TableName = "PRPOData";
                ds.Tables.Add(podata);

                sql = "Select DEPdescT as Name from Department Where DEPid = " + id;
                DataTable dt = DBHelper.List(sql);
                dt.TableName = "Department";
                ds.Tables.Add(dt);
            }
            catch(Exception ex) { errMsg = ex.Message; }
            return ds;
        }

        public int getdepid(string dep)
        {
            int depid = 0;
            string sql = "Select *  from Department Where REPLACE(REPLACE((CAST(DEPdescT as NVARCHAR(max))), CHAR(13), ''), CHAR(10), '') = '" + dep + "'";
            DataTable dt = DBHelper.List(sql);
            dt.TableName = "Department";
            foreach (DataRow dr in dt.Rows)
            {
                depid = Convert.ToInt32(dr["DEPid"]);
            }

            return depid;
        }
    }
}
