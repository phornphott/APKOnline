using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        DataTable GetReportBudget(int year,int month, int StaffCode, int DEPcode, ref string errMsg);
        DataSet GetDashBroadData(int id, ref string errMsg);
        DataSet GetDashBroadByDepartment(int id, ref string errMsg);
        int getdepid(string dep);
        DataTable GetDEP();
        DataTable GetPRStatus();
        DataTable GetPRReport(DateTime startdate, DateTime finishdate, int dep, int status, ref string errMsg);
    }

    public class ReportData: IReportData
    {
        public DataTable GetReportBudget(int year, int month, int StaffCode, int DEPcode, ref string errMsg)
        {
            string Addition = null;
            DataTable dt = new DataTable();

            try
            {
                string date = new DateTime(year, month, 1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                int LastdayofMonth = DateTime.DaysInMonth(year, month);
                string todate = new DateTime(year, month, LastdayofMonth).ToString("yyyy-MM-dd", new CultureInfo("en-US"));


                //if (StaffCode != null && StaffCode != "undefined")
                if (StaffCode != 0)
                {
                    //Addition += " AND s.StaffCode=" + StaffCode;
                    if (StaffCode != 1) Addition += " AND p.Document_CreateUser=" + StaffCode;
                }
                //if (DEPcode != null && StaffCode != "undefined")
                if ((DEPcode != 0) && (DEPcode != 99))
                {
                    Addition += " AND p.Document_Dep=" + DEPcode;
                }

                string strSQL = "\r\n SELECT distinct p.*, b.DEPmonth" + month + " as SumMonth, s.StaffCode, s.StaffFirstName, s.StaffLastName" +
                    " , d.DEPcode, CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job, Objective_Name AS Objective, Category_Name AS Category " +
                    " FROM DocumentPR_Header p " +
                      "\r\n LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep " +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category " +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective " +
                      " LEFT JOIN BudgetOfYearByDepartment b on d.DEPcode=b.DEPcode" +
                      " where Document_Delete=0 AND p.Document_Date BETWEEN '" + date + "' AND '" + todate + "'" + Addition + ";";
                dt = DBHelper.List(strSQL);
                errMsg = strSQL;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ReportBudget";
            return dt;
        }
        public  DataSet GetDashBroadData(int id, ref string  errMsg)
        {
            string Addition = null;
            DataSet ds = new DataSet();
            string date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            string todate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).AddDays(1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));

            int Imonth = DateTime.Today.Month;
            int Iyear = DateTime.Today.Year;
            string Monthname = "DEPmonth" + Imonth.ToString();
            //date = "2021-01-01";
            //todate = "2022-01-01";

            string sql = "";
            int depid = 0;

            sql = "Select * from staffs where StaffID = " + id;
            DataTable dt = new DataTable();
             dt = DBHelper.List(sql);
            foreach (DataRow dr in dt.Rows)
            {
                depid = Convert.ToInt32(dr["StaffDepartmentID"]);
            }
            try
            {
                sql = " select SUM(Document_Cog) AS Amount,Document_Dep ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,'งบประมาณที่อนุมัติ : ' + REPLACE(REPLACE(CAST(DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                      ",'งบประมาณที่อนุมัติ' AS LabelDesc ,0 as status  from DocumentPR_Header h " +
                      " left join Department d on h.Document_Dep = d.DEPid " +
                      " where Document_Date between  '" + date + "'  and '" + todate + "' and Document_Status in (2)";
                if (depid != 99)
                {
                    sql += " and (h.Document_Dep = " + depid +" )";
                }
                sql +=   " group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";

                sql += " UNION ";

                sql += " select SUM(Document_Cog) AS Amount,Document_Dep ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,'งบประมาณที่รออนุมัติ : ' + REPLACE(REPLACE(CAST(DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                      " ,'งบประมาณที่รออนุมัติ' AS LabelDesc ,1 as status from DocumentPR_Header h " +
                      " left join Department d on h.Document_Dep = d.DEPid " +
                      " where Document_Date between  '" + date + "'  and '" + todate + "' and Document_Status in (0,1)";
                if (depid != 99)
                {
                    sql += " and (h.Document_Dep = " + depid + " )";
                }
                sql += " group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";

                sql += " UNION ";

                sql += " select SUM(Document_Cog) AS Amount,Document_Dep ,CAST(DEPcode as NVARCHAR(max)) As DEPCode,'งบประมาณที่ไม่อนุมัติ : ' + REPLACE(REPLACE(CAST(DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                      ",'งบประมาณที่ไม่อนุมัติ' AS LabelDesc ,2 as status from DocumentPR_Header h " +
                      " left join Department d on h.Document_Dep = d.DEPid " +
                      " where Document_Date between  '" + date + "'  and '" + todate + "' and Document_Status in (9) ";
                if (depid != 99)
                {
                    sql += " and (h.Document_Dep = " + depid + " )";
                }
                sql += " group by Document_Dep,CAST(DEPcode as NVARCHAR(max)),CAST(DEPdescT as NVARCHAR(max))";



                errMsg = sql;
                DataTable poDepAmount = DBHelper.List(sql);
                poDepAmount.TableName = "DepAmount";

                //Sum of Month
                if (id != 1)
                {
                    sql = " select h." + Monthname + " AS Amount,h.DEPid as Document_Dep ,CAST(d.DEPcode as NVARCHAR(max)) As DEPCode,'งบประมาณคงเหลือ : ' + REPLACE(REPLACE(CAST(d.DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                    ",'งบประมาณคงเหลือ' AS LabelDesc ,3 as status from BudgetOfYearByDepartment h " +
                    " left join Department d on h.DEPid = d.DEPid " +
                    " where BudgetYear = " + Iyear + "";
                    if (depid != 999)
                    {
                        sql += " and h.DEPid = " + depid + "";
                    }

                    sql += " UNION ";

                    sql += " select h." + Monthname + " AS Amount,h.DEPid as Document_Dep ,CAST(d.DEPcode as NVARCHAR(max)) As DEPCode,'งบประมาณทั้งหมด : ' + REPLACE(REPLACE(CAST(d.DEPdescT as NVARCHAR(max)), CHAR(13), ''), CHAR(10), '') As DEP " +
                    ",'งบประมาณทั้งหมด' AS LabelDesc ,4 as status from BudgetOfYearByDepartment h " +
                    " left join Department d on h.DEPid = d.DEPid " +
                    " where BudgetYear = " + Iyear + "";
                    if (depid != 999)
                    {
                        sql += " and h.DEPid = " + depid + "";
                    }

                    DataTable AllDepAmount = DBHelper.List(sql);
                    if (AllDepAmount.Rows.Count > 0)
                    {   foreach (DataRow dr in AllDepAmount.Rows)
                        {
                            if(poDepAmount.Rows.Count > 0 && dr["status"].ToString() == "3")
                            {
                                decimal useBudget = 0;
                                foreach (DataRow drow in poDepAmount.Rows)
                                {
                                    if (drow["status"].ToString() == "0")
                                    {
                                        useBudget += Convert.ToDecimal(drow["Amount"]);
                                    }  
                                }

                                dr["Amount"] = Convert.ToDecimal(dr["Amount"]) - useBudget;
                            }
                            poDepAmount.ImportRow(dr);

                        }
                    }
                }
                


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

        public DataTable GetDEP()
        {
            string sql = "Select DEPid AS ID,DEPdescT AS Name from Department";
            DataTable dt = DBHelper.List(sql);
            dt.TableName = "Department";
            return dt;
        }
        public DataTable GetPRStatus()
        {
            string sql = "Select  0 AS ID,'รออนุมัติ' AS Name  ";
            DataTable dt = DBHelper.List(sql);


            DataRow drow = dt.NewRow();
            drow[0] = 1;
            drow[1] = "รับทราบ";
            dt.Rows.Add(drow);
            drow = dt.NewRow();
            drow[0] = 2;
            drow[1] = "อนุมัติ";
            dt.Rows.Add(drow);
            drow = dt.NewRow();
            drow[0] = 3;
            drow[1] = "สร้างเอกสารสั่งซื้อ";
            dt.Rows.Add(drow);
            drow = dt.NewRow();
            drow[0] = 4;
            drow[1] = "อนุมัติเอกสารสั่งซื้อ";
            dt.Rows.Add(drow);
            drow = dt.NewRow();
            drow[0] = 9;
            drow[1] = "ไม่อนุมัติ";
            dt.Rows.Add(drow);

            dt.TableName = "Status";
            return dt;
        }
        public DataTable GetPRReport(DateTime startdate, DateTime finishdate, int dep, int status, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";

            try
            {
                string strSQL = "";

                strSQL = "\r\n  " +
                " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT " +
                ",CASE WHEN p.Document_Status = 0 THEN 'รออนุมัติ' WHEN p.Document_Status = 1 THEN 'รับทราบ' WHEN p.Document_Status = 2 THEN 'อนุมัติ' " +
                " WHEN p.Document_Status = 3 THEN 'สร้างเอกสารสั่งซื้อ' WHEN p.Document_Status = 4 THEN 'อนุมัติเอกสารสั่งซื้อ' ELSE 'ไม่อนุมัติ' END AS DocStatus" +
                " FROM " + tablename + " p " +
                " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +

                //" where Document_Date BETWEEN '"+startdate + "' AND  DATEADD(day,1,'" + finishdate + "')";
                " where Document_Date BETWEEN @startdate AND @finishdate";


                if (dep > 0 )
                {
                    strSQL += " AND p.Document_Dep=" + dep;
                }
                if (status >= 0)
                {
                    strSQL += " AND p.Document_Status=" + status;
                }
                SqlConnection conn = DBHelper.SqlConnectionDb();
                var dataAdapter = new SqlDataAdapter(strSQL, conn);
                dataAdapter.SelectCommand.Parameters.Clear();
                dataAdapter.SelectCommand.Parameters.Add("@startdate", SqlDbType.DateTime).Value = startdate;
                dataAdapter.SelectCommand.Parameters.Add("@finishdate", SqlDbType.DateTime).Value = finishdate.AddDays(1);              
                dataAdapter.Fill(dt);            
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "PRReport";

            return dt;
        }
    }
}
