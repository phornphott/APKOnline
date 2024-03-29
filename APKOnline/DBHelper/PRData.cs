﻿using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APKOnline.DBHelper
{
    public interface IPRData
    {
        DataTable GetDepData(ref string errMsg);
        DataTable GetCategoryData(ref string errMsg);
        DataTable GetObjectiveData(int id, ref string errMsg);
        DataTable GeneratePRNo(int id, ref string errMsg);
        DataTable GetJobData(ref string errMsg);
        DataTable GetAccountData(ref string errMsg);
        DataTable GetAccountData(string depcode, ref string errMsg);
        DataTable GetDetailData(int Document_Detail_Hid, int tmp, ref string errMsg);
        DataTable GetHeaderData(int Document_id, int staffid, ref string errMsg);
        int InsertHeader(PRHeaderModels Header, ref string errMsg);
        int InserttmpDetail(PRDetailModels detail, ref string errMsg);
        int DeleteTmpDetail(int Hid, ref string errMsg);
        DataTable GetPRData(int staffID, ref string errMsg);
        DataTable GetPRDataForApprove(int StaffID, int DeptID, ref string errMsg);
        int ApprovePR(int Document_Id, int StaffID, int DeptID, bool isPreview, ref string errMsg);
        int RejectPR(int Document_Id, int StaffID, int DeptID, bool isPreview, ref string errMsg);

        DataTable GetPROverDataForApprove(int id, int DeptID, ref string errMsg);
        int ApprovePROverBudget(int Document_Id, int StaffID, ref string errMsg);
        bool UpdatePRDetail(PRDetailModels detail, ref string errMsg);
        Task<bool> DeletePRData(int Document_Id);
        int DeletePRDetail(int id, bool tmp, ref string errMsg);
        Task<bool> CheckDeletePRData(int Document_Id);
        DataTable GetListPreview(int StaffID, int DeptID, ref string errMsg);

        bool LogPreview(int logId, ref string errMsg);
        bool UpdatePRDetail(List<PRDetailModels> list, ref string errMsg);
        //bool UpdatePRDetail(List<PRDetailModels> list, ref string errMsg);
        bool CheckAccountBudget(string ACCCode, string depcode, string depid, ref string errMsg);

    }

    public class PRData : IPRData
    {

        #region 
        public DataTable GetGroupDataData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT id AS ID, GroupName AS Name " +
                    " FROM Document_Group ";


                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "DocumentGroup";
            return dt;
        }
        public DataTable GetDepData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT DEPid AS ID, DEPdescT AS Name,DEPcode AS Code " +
                    " FROM Department WHERE FLG_DEL =0 ";


                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "DEP";
            return dt;
        }
        public DataTable GetCategoryData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT Category_Id AS ID, Category_Name AS Name FROM Category group by Category_Id,Category_Name";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Category";
            return dt;
        }
        public DataTable GetObjectiveData(int id, ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT distinct Objective_Id AS ID, Objective_Name AS Name,Objective_Category_Id " +
                    " FROM Objective ";
                if (id > 0)
                {
                    strSQL += " WHERE Objective_Category_Id =  " + id;
                }


                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Objective";
            return dt;
        }
        public DataTable GetJobData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT JOBcode AS Code, JOBdescT AS Name " +
                    " FROM JOB ";


                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "JOB";
            return dt;
        }
        public DataTable GetAccountData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT ACCcode AS Code, ACCdescT AS Name " +
                    " FROM Account ";



                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Account";
            return dt;
        }
        public DataTable GetAccountData(string  depcode, ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT a.ACCcode AS Code, a.ACCdescT AS Name " +
                    " FROM Account  a" +
                    " LEFT JOIN Budget b on a.ACCcode =  b.CodeACC " +
                    " WHERE b.CodeDEP = '" + depcode + "'";



                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Account";
            return dt;
        }

        public DataTable GetDetailData(int Document_Detail_Hid, int tmp, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Detail";
            if (tmp == 0)
            { tablename = "DocumentPR_Detail_tmp"; }
            try
            {
                string strSQL = "\r\n  " +
                      " SELECT * " +
                      " FROM " + tablename + "  where Document_Detail_Delete=0 AND Document_Detail_Hid =" + Document_Detail_Hid;
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Detail";

            return dt;
        }
        public DataTable GetHeaderData(int Document_id, int staffid, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";
            decimal budget = 0;
            decimal DocCog = 0;
            int DEPid = 0;

            try
            {
                //string strSQL = "\r\n  " +
                //     " SELECT * " +
                //     " FROM StaffAuthorize WHERE StaffID = " +staffid;
                //DataTable staffauth = DBHelper.List(strSQL);
                //foreach (DataRow dr in staffauth.Rows)
                //{
                //    budget = Convert.ToDecimal(dr["PositionLimit"]);
                //}

                //",CASE WHEN p.Document_Cog > " + budget + " THEN 'รับทราบ'ELSE 'อนุมัติ' END AS SaveText" +


                string strSQL = "\r\n  " +
                      " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                      " ,CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job ,g.GroupName AS 'Group'" +
                      " ,CAST(Objective_Name as NVARCHAR(max)) AS Objective,CAST(Category_Name as NVARCHAR(max)) AS Category" +
                      " ,'อนุมัติ' AS SaveText,0 AS isPreview,s.StaffDepartmentID" +
                      " FROM " + tablename + " p " +
                      "LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job" +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                      " where Document_Delete=0 AND Document_Id =" + Document_id;
                dt = DBHelper.List(strSQL);


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["Document_ApproveDirect"]) == 1)
                        {
                            DEPid = Convert.ToInt32(dr["StaffDepartmentID"]);
                        }
                        else
                        {
                            DEPid = Convert.ToInt32(dr["Document_Depid"]);
                        }
                        DocCog = Convert.ToDecimal(dr["Document_Cog"]);
                    }

                    strSQL = "\r\n  " +
                     " SELECT * " +
                     " FROM StaffAuthorize WHERE StaffID = " + staffid + " and DEPid = " + DEPid;
                    DataTable staffauth = DBHelper.List(strSQL);
                    foreach (DataRow dr in staffauth.Rows)
                    {
                        budget = Convert.ToDecimal(dr["PositionLimit"]);
                        dt.Rows[0]["isPreview"] = Convert.ToInt32(dr["isPreview"]);
                    }

                    if (DocCog > budget)
                    {
                        dt.Rows[0]["SaveText"] = "รับทราบ";
                        dt.Rows[0]["isPreview"] = 0;
                    }

                }



            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Header";

            return dt;
        }
        public DataTable GetPRData(int staffID, ref string errMsg)
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

                " where Document_Delete=0 ";


                if (staffID > 0 && staffID != 1)
                {
                    strSQL += " AND p.Document_CreateUser=" + staffID;
                }
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }
        public DataTable GetPRDataForApprove(int StaffID, int DeptID, ref string errMsg)
        {
            DataTable dtPR = new DataTable("PR");
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";
            int staffLevel = 0;
            string Depin = "";
            int irow = 0;

            try
            {



                string sql = " SELECT DEPid,AuthorizeLevel FROM StaffAuthorize WHERE StaffID = " + StaffID;
                DataTable staffauth = DBHelper.List(sql);
                foreach (DataRow drow in staffauth.Rows)
                {
                    irow++;
                    //if (Depin.Length == 0)
                    //{
                    //    Depin = dr["DEPid"].ToString();
                    //}
                    //else
                    //{
                    //    Depin = Depin + "," + dr["DEPid"].ToString();
                    //}

                    Depin = drow["DEPid"].ToString();
                    staffLevel = Convert.ToInt32(drow["AuthorizeLevel"]) - 1;

                    int year = DateTime.Now.Year;

                    sql = "Select * from BudgetOfYearByDepartment WHERE  BudgetYear = " + year + " AND DEPid = " + Convert.ToInt32(drow["DEPid"]);
                    DataTable depbudget = DBHelper.List(sql);

                    if (depbudget.Rows.Count > 0)
                    {
                        decimal Dep_Budget = 0;
                        string monthcol = "DEPmonth" + DateTime.Now.Month.ToString();
                        foreach (DataRow dr in depbudget.Rows)
                        {
                            Dep_Budget = Convert.ToDecimal(dr[monthcol]);


                            //string strSQL = "\r\n  SELECT distinct * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevelID ELSE a.Current_Level END AS Document_Level FROM  (" +
                            //  "(SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                            //  ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevelID,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT " +
                            //  " FROM " + tablename + " p " +
                            //  " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                            //  " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                            //  " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                            //  " where Document_Delete=0 AND Document_Status<2 " +
                            //  " AND Document_Cog <=" + Dep_Budget + " And p.Document_Dep= " + DeptID +
                            //  ") UNION ALL (" +
                            //  " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                            //  ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevelID,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT" +
                            //  " FROM " + tablename + " p " +
                            //  " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                            //  " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                            //  " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                            //  " LEFT JOIN ApprovePROverBudget a on a.Approve_Documen_Id=p.Document_Id " +
                            //  " where Document_Delete=0 AND Document_Status < 2 AND a.Approve_Status = 2" +
                            //  " AND Document_Cog > " + Dep_Budget + " And p.Document_Dep= " + DeptID+ ")) aa  " +
                            //  " left join (SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                            //  " FROM ApprovePR GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level ="+ staffLevel;

                            //ตามแผนก
                            string strSQL = "\r\n  SELECT distinct * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevel ELSE a.Current_Level END AS Document_Level FROM  (" +
                              "(SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                              ",CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel," +
                              "CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT " +
                              " FROM " + tablename + " p " +
                              " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                              " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                              " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                              " where Document_Delete=0 AND Document_Status<2 " +
                              " AND Document_Cog <=" + Dep_Budget + " And p.Document_Dep = '" + Depin + "' And (p.Document_ApproveDirect = 0 or p.Document_ApproveDirect is null)" +
                              ") UNION ALL (" +
                              " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                              ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT" +
                              " FROM " + tablename + " p " +
                              " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                              " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                              " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                              " LEFT JOIN ApprovePROverBudget a on a.Approve_Documen_Id=p.Document_Id " +
                              " where Document_Delete=0 AND Document_Status < 2 AND a.Approve_Status = 2" +
                              " AND Document_Cog > " + Dep_Budget + " And p.Document_Dep in ( " + Depin + ")" + " And (p.Document_ApproveDirect = 0 or p.Document_ApproveDirect is null))) aa  " +
                              " left join (SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                              " FROM ApprovePR GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level =" + (staffLevel);


                            dt = DBHelper.List(strSQL);

                            if (irow == 1)
                            {
                                dtPR = new DataTable("ListPRData");
                                dtPR = dt.Clone();
                            }

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drw in dt.Rows)
                                {
                                    dtPR.ImportRow(drw);
                                }
                            }

                            //ตามสายงาน
                            strSQL = "\r\n  SELECT distinct * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevel ELSE a.Current_Level END AS Document_Level FROM  (" +
                              "(SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                              ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT " +
                              " FROM " + tablename + " p " +
                              " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                              " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                              " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                              " where Document_Delete=0 AND Document_Status<2 " +
                              " AND Document_Cog <=" + Dep_Budget + " And s.StaffDepartmentID = '" + Depin + "' And (p.Document_ApproveDirect = 1)" +
                              ") UNION ALL (" +
                              " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                              ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT" +
                              " FROM " + tablename + " p " +
                              " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                              " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                              " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                              " LEFT JOIN ApprovePROverBudget a on a.Approve_Documen_Id=p.Document_Id " +
                              " where Document_Delete=0 AND Document_Status < 2 AND a.Approve_Status = 2" +
                              " AND Document_Cog > " + Dep_Budget + " And s.StaffDepartmentID in ( " + Depin + ")" + " And (p.Document_ApproveDirect = 1))) aa  " +
                              " left join (SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                              " FROM ApprovePR GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level =" + staffLevel;
                            dt = DBHelper.List(strSQL);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drw in dt.Rows)
                                {
                                    dtPR.ImportRow(drw);
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    errMsg = "ไม่สารมารถดูข้อมูลการอนุมัติเอกสารได้เนื่องจากไม่มีการตั้งค่างบประมาณของแผนก.";
                    //}
                }


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dtPR.TableName = "ListPRData";

            return dtPR;
        }
        public DataTable GetListPreview(int StaffID, int DeptID, ref string errMsg)
        {
            DataTable dt = new DataTable();

            try
            {

                string strSQL = "SELECT l.*,pr.Document_Vnos AS PRNo,pr.Document_Cog AS PRAmount,po.Document_Vnos AS PONo,po.Document_Cog AS POAmount " +
                    ",po.Document_EditDate AS POapprove,po.Document_EditUser AS POapproveBy,po.Document_Id AS POID,pr.Document_Id AS PRID" +
                    " FROM LogPreview l " +
                    " Left Join DocumentPR_Header pr on pr.Document_Id = Document_PRId" +
                    " Left Join DocumentPO_Header po on po.Document_PRID = pr.Document_Id" +
                    " WHERE l.Document_PreviewUser = " + StaffID + "  and po.Document_Status < 2";


                dt = DBHelper.List(strSQL);



            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }

        public DataTable GetPROverDataForApprove(int id, int DeptID, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";

            try
            {

                string sql = "Select * from BudgetOfYearByDepartment WHERE DEPid = " + DeptID;
                DataTable depbudget = DBHelper.List(sql);
                sql = "Select * from OverBudgetSetting WHERE StaffID = " + id;
                DataTable BudgetSetting = DBHelper.List(sql);
                if (BudgetSetting.Rows.Count > 0)
                {
                    if (depbudget.Rows.Count > 0)
                    {
                        decimal Dep_Budget = 0;
                        string monthcol = "DEPmonth" + DateTime.Now.Month.ToString();
                        foreach (DataRow dr in depbudget.Rows)
                        {
                            Dep_Budget = Convert.ToDecimal(dr[monthcol]);


                            string strSQL = "\r\n  " +
                              " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                              ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT" +
                              " FROM " + tablename + " p " +
                              " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                              " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                              " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                              " LEFT JOIN ApprovePROverBudget a on a.Approve_Documen_Id=p.Document_Id " +
                              " where Document_Delete=0 AND Document_Status<2 AND (a.Approve_Status < 2 OR a.Approve_Status IS NULL)" +
                              " And p.Document_Dep= " + DeptID + " AND Document_OverBudget = 1"; //AND p.Document_Cog >" + Dep_Budget + "
                            dt = DBHelper.List(strSQL);
                        }
                    }
                    else
                    {
                        errMsg = "ไม่สารมารถดูข้อมูลการอนุมัติเอกสารได้เนื่องจากไม่มีการตั้งค่างบประมาณของแผนก.";
                    }
                }
                else
                { errMsg = "คุณไม่มีสิทธิ์ในการอนุมัติเอกสารเกินงบประมาณ"; }


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }
        public int InsertHeader(PRHeaderModels Header, ref string errMsg)
        {
            int document_id = 0;
            SqlTransaction myTran = null;
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;
            try
            {
                decimal amount = 0;
                //int NoList = 0;
                //decimal Dep_Budget = 0;
                //int year = DateTime.Now.Year;

                //string sql = "Select * from BudgetOfYearByDepartment WHERE  BudgetYear = " + year + " AND DEPid = " + Convert.ToInt32(Header.Document_Dep);
                //DataTable depbudget = DBHelper.List(sql);

                //if (depbudget.Rows.Count > 0)
                //{

                //    string monthcol = "DEPmonth" + DateTime.Now.Month.ToString();
                //    foreach (DataRow dr in depbudget.Rows)
                //    {
                //        Dep_Budget = Convert.ToDecimal(dr[monthcol]);
                //    }
                //}

                string sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail_tmp WHERE Document_Detail_Hid = " + Header.Document_Id;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                foreach (DataRow dr in tmp.Rows)
                {
                    amount += Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                }

                //string fristDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("00") + "-01";
                //int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                //string LastDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).AddDays(1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                //sql = "Select SUM(Document_Cog) From DocumentPR_header  where Document_Date between '" + fristDate + "'and  '" + LastDate + "' and Document_Depid =" + Header.Document_Dep + " and Document_Delete=0 and Document_Status in (2)";
                //da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //tmp = new DataTable();
                //da.Fill(tmp);

                //decimal dep_amount = 0;
                //foreach (DataRow dr in tmp.Rows)
                //{
                //    dep_amount = dep_amount + Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);

                //}

                //dep_amount += amount;
                //dep_amount = Math.Round(dep_amount, 2);
                //bool overbudget = false;
                //if (Dep_Budget < dep_amount)
                //{
                //    errMsg = "ยอดรวมมากกว่างบประมาณอนุมัติของแผนกในเดือนปัจจุบัน";
                //    overbudget = true;
                //}
                //else
                //{
                //จำนวนรายการ
                sql = "Select * From DocumentPR_Detail_tmp WHERE Document_Detail_Hid = " + Header.Document_Id;
                da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                tmp = new DataTable();
                da.Fill(tmp);
                int NoList = tmp.Rows.Count;


                string sqlQuery = "INSERT INTO DocumentPR_Header(Document_Group,Document_ExpenseType,Document_Category,Document_Objective " +
                                      ",Document_Vnos,Document_Date ,Document_Means,Document_Expect,Document_Cus,Document_Job,Document_Depid,Document_Dep,Document_Per" +
                                      ",Document_Doc,Document_Mec,Document_Desc,Document_Nolist,Document_Cog,Document_VatSUM,Document_VatPer" +
                                      " ,Document_NetSUM,Document_Status,Document_Tel,Document_CreateUser,Document_CreateDate,Document_Delete,Document_Term,Document_Project,Document_ApproveDirect) VALUES " +
                                      " (@Document_Group,@Document_ExpenseType,@Document_Category,@Document_Objective " +
                                      ",dbo.GeneratePRID(@Document_Group),GETDATE() ,@Document_Means,@Document_Expect,@Document_Cus,@Document_Job,@Document_Depid,@Document_Dep,@Document_Per" +
                                      ",@Document_Doc,@Document_Mec,@Document_Desc,@Document_Nolist,@Document_Cog,@Document_VatSUM,@Document_VatPer" +
                                      " ,@Document_NetSUM,@Document_Status,@Document_Tel,@Document_CreateUser,GETDATE(),0,@Document_Term,@Document_Project,@Document_ApproveDirect) SET @Document_Id=SCOPE_IDENTITY()";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                shipperIdParam = new SqlParameter("@Document_Id", SqlDbType.Int);
                shipperIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(shipperIdParam);

                cmd.Parameters.AddWithValue("@Document_Group", Header.Document_Group);
                cmd.Parameters.AddWithValue("@Document_ExpenseType", Header.Document_Group);
                cmd.Parameters.AddWithValue("@Document_Category", Header.Document_Category);
                cmd.Parameters.AddWithValue("@Document_Objective", Header.Document_Objective);
                cmd.Parameters.AddWithValue("@Document_Means", Header.Document_Means == null ? "" : Header.Document_Means);
                cmd.Parameters.AddWithValue("@Document_Expect", Header.Document_Expect == null ? "" : Header.Document_Expect);

                cmd.Parameters.AddWithValue("@Document_Cus", "");
                cmd.Parameters.AddWithValue("@Document_Job", Header.Document_Job);
                cmd.Parameters.AddWithValue("@Document_Depid", Header.Document_Dep);
                cmd.Parameters.AddWithValue("@Document_Dep", Header.Document_Dep);
                cmd.Parameters.AddWithValue("@Document_Per", "");
                cmd.Parameters.AddWithValue("@Document_Doc", "");
                cmd.Parameters.AddWithValue("@Document_Mec", "");
                cmd.Parameters.AddWithValue("@Document_Desc", Header.Document_Desc == null ? "" : Header.Document_Desc);
                cmd.Parameters.AddWithValue("@Document_Nolist", NoList);
                cmd.Parameters.AddWithValue("@Document_Cog", amount);
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(amount * (decimal)0.07, 2));//()
                cmd.Parameters.AddWithValue("@Document_VatPer", 7);
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(amount * (decimal)1.07, 2));//
                cmd.Parameters.AddWithValue("@Document_Status", 0);
                cmd.Parameters.AddWithValue("@Document_Tel", Header.Document_Tel == null ? "" : Header.Document_Tel);
                cmd.Parameters.AddWithValue("@Document_CreateUser", Header.Document_CreateUser);
                cmd.Parameters.AddWithValue("@Document_Term", Header.Document_Term);
                cmd.Parameters.AddWithValue("@Document_Project", Header.Document_Project);
                cmd.Parameters.AddWithValue("@Document_ApproveDirect", Header.Document_ApproveDirect);
                //cmd.Parameters.AddWithValue("@Document_OverBudget", 0);
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                document_id = (int)shipperIdParam.Value;


                sql = " INSERT INTO DocumentPR_Detail(Document_Detail_Hid,Document_Detail_Date,Document_Detail_Vnos" +
                        " ,Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                        " ,Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                        " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,Document_Detail_Delete)" +
                        " SELECT " + document_id + ",Document_Detail_Date,Document_Detail_Vnos" +
                        " ,Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                        " ,Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                        " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,0 FROM DocumentPR_Detail_tmp" +
                        "  WHERE Document_Detail_Hid = " + Header.Document_Id;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                sql = " DELETE FROM  DocumentPR_Detail_tmp" +
                       "  WHERE Document_Detail_Hid = " + Header.Document_Id;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                myTran.Commit();

                string sourcePath = System.Web.Hosting.HostingEnvironment.MapPath("~/tmpUpload/" + Header.folderUpload + "/");
                string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/" + document_id.ToString() + "/");
                if (System.IO.Directory.Exists(sourcePath))
                {
                    if (!System.IO.Directory.Exists(targetpath))
                    { System.IO.Directory.CreateDirectory(targetpath); }

                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(targetpath, fileName);
                        System.IO.File.Copy(s, destFile, true);

                    }
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourcePath);
                    foreach (System.IO.FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                //}




            }
            catch (Exception ex)
            {
                myTran.Rollback();
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int InserttmpDetail(PRDetailModels detail, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;

            try
            {

                if (detail.Document_Detail_Id <= 0)
                {


                }





                if (detail.Document_Detail_Id <= 0)
                {
                    sqlQuery = "INSERT INTO DocumentPR_Detail_tmp(Document_Detail_Hid,Document_Detail_Date,Document_Detail_Vnos" +
                        ",Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                        ",Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                        " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,Document_Detail_Delete) VALUES " +
                        " (@Document_Detail_Hid,GETDATE(),@Document_Detail_Vnos,@Document_Detail_Acc,@Document_Detail_Acc_Desc,@Document_Detail_Stk " +
                        ",@Document_Detail_Stk_Desc,@Document_Detail_ListNo,@Document_Detail_Quan,@Document_Detail_Cog,@Document_Detail_Vat,@Document_Detail_Sum" +
                        " ,@Document_Detail_CreateUser,GETDATE(),@Document_Detail_UnitPrice,0)";


                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Detail_Hid", detail.Document_Detail_Hid);
                }
                else
                {
                    sqlQuery = "UPDATE DocumentPR_Detail_tmp SET Document_Detail_Stk=@Document_Detail_Stk" +
                        ",Document_Detail_Acc=@Document_Detail_Acc,Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                        ",Document_Detail_Stk_Desc=@Document_Detail_Stk_Desc,Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                        ",Document_Detail_ListNo=@Document_Detail_ListNo,Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                        ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum WHERE Document_Detail_Id=@Document_Detail_Id";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                }

                cmd.Parameters.AddWithValue("@Document_Detail_Vnos", detail.Document_Detail_Vnos);
                cmd.Parameters.AddWithValue("@Document_Detail_Acc", detail.Document_Detail_Acc);
                cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc == null ? "" : detail.Document_Detail_Acc_Desc);
                cmd.Parameters.AddWithValue("@Document_Detail_Stk", detail.Document_Detail_Stk == null ? "" : detail.Document_Detail_Stk);
                cmd.Parameters.AddWithValue("@Document_Detail_Stk_Desc", detail.Document_Detail_Stk_Desc == null ? "" : detail.Document_Detail_Stk_Desc);
                cmd.Parameters.AddWithValue("@Document_Detail_ListNo", detail.Document_Detail_ListNo);
                cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));

                cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));

                cmd.Parameters.AddWithValue("@Document_Detail_CreateUser", detail.Document_Detail_CreateUser);


                cmd.ExecuteNonQuery();



                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int DeletePRDetail(int id, bool tmp, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            string table = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;
            if (tmp)
            {
                table = "DocumentPR_Detail_tmp";
            }
            else
            {
                table = "DocumentPR_Detail";

            }
            try
            {

                sqlQuery = "DELETE From " + table + " WHERE Document_Detail_Id=@Document_Detail_Id";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Id", id);

                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }


        public int DeleteTmpDetail(int Hid, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;

            try
            {

                sqlQuery = "DELETE From DocumentPR_Detail_tmp WHERE Document_Detail_Hid=@Document_Detail_Id";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Id", Hid);

                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int ApprovePR(int Document_Id, int StaffID, int DeptID, bool isPreview, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;

            decimal budget = 0;
            decimal doc_cog = 0;
            int docLevel = 0;
            int ApproveLevel = 0;
            string depcode = "";
            string msgReturn = "";
            try
            {
                sqlQuery = "SELECT * FROM Department WHERE DEPid  =" + DeptID;
                DataTable DEpData = DBHelper.List(sqlQuery);
                foreach (DataRow row in DEpData.Rows)
                {
                    depcode = row["DEPcode"].ToString();
                }

                sqlQuery = "SELECT * FROM DocumentPR_Detail WHERE Document_Detail_Hid  =" + Document_Id;
                DataTable PRdetialdata = DBHelper.List(sqlQuery);
                foreach (DataRow drow in PRdetialdata.Rows)
                {
                    string refmsg = "";
                    if (!CheckAccountBudget(drow["Document_Detail_Acc"].ToString(), depcode, DeptID.ToString(), ref refmsg)) {
                        msgReturn = drow["Document_Detail_Acc"].ToString() + " ,";
                    }
                }

                if (msgReturn != "") {
                    errMsg = "ไม่สามารถ Approve/รับทราบได้ เนื่องจาก  Account  " + msgReturn.Substring(0, msgReturn.Length - 2) + " อนุมัติเกินกำหนดของงบประมาณเดือนนี้ไปแล้ว";
                }
                else
                {
                    string strSQL = "\r\n  " +
                  " SELECT * " +
                  " FROM StaffAuthorize WHERE StaffID = " + StaffID + " and DEPid = " + DeptID;
                    DataTable staffauth = DBHelper.List(strSQL);
                    foreach (DataRow dr in staffauth.Rows)
                    {
                        budget = Convert.ToDecimal(dr["PositionLimit"]);
                        ApproveLevel = Convert.ToInt32(dr["AuthorizeLevel"]);
                    }

                    //string sql = "Select * from Staffs WHERE StaffID = " + StaffID;
                    //DataTable staff = DBHelper.List(sql);
                    //if (staff.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in staff.Rows)
                    //    {
                    //        ApproveLevel = Convert.ToInt32(dr["StaffLevel"]);
                    //    }
                    //}

                    strSQL = "\r\n  " +
                  " SELECT * " +
                  " FROM DocumentPR_Header h left join Staffs s on s.StaffID=h.Document_CreateUser  WHERE h.Document_Id = " + Document_Id;
                    DataTable docHeader = DBHelper.List(strSQL);
                    foreach (DataRow dr in docHeader.Rows)
                    {
                        doc_cog = Convert.ToDecimal(dr["Document_Cog"]);
                        //docLevel = Convert.ToInt32(dr["StaffLevelID"]);
                        docLevel = Convert.ToInt32(dr["StaffLevel"]);
                    }
                    SqlConnection conn = DBHelper.sqlConnection();
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd = conn.CreateCommand();
                    //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Connection = conn;
                    if (doc_cog > budget)
                    {

                        sqlQuery = "INSERT INTO ApprovePR (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
                         " (@Approve_Documen_Id,@Approve_Create_Level,@Approve_Current_Level,1,0,@Approve_By  )";
                        cmd.CommandText = sqlQuery;
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Approve_Documen_Id", Document_Id);
                        cmd.Parameters.AddWithValue("@Approve_Create_Level", docLevel);
                        cmd.Parameters.AddWithValue("@Approve_Current_Level", ApproveLevel);
                        cmd.Parameters.AddWithValue("@Approve_By", StaffID);
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        sqlQuery = "INSERT INTO ApprovePR (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
                    " (@Approve_Documen_Id,@Approve_Create_Level,@Approve_Current_Level,2,0,@Approve_By  )";
                        cmd.CommandText = sqlQuery;
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Approve_Documen_Id", Document_Id);
                        cmd.Parameters.AddWithValue("@Approve_Create_Level", docLevel);
                        cmd.Parameters.AddWithValue("@Approve_Current_Level", ApproveLevel);
                        cmd.Parameters.AddWithValue("@Approve_By", StaffID);
                        cmd.ExecuteNonQuery();

                        sqlQuery = "Update DocumentPR_Header SET " +
                                    "Document_EditUser = @Document_EditUser,Document_EditDate=GETDATE(),Document_Status =2 WHERE Document_Id = @Document_Id";
                        cmd.CommandText = sqlQuery;
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Document_Id", Document_Id);
                        cmd.Parameters.AddWithValue("@Document_EditUser", StaffID);
                        cmd.ExecuteNonQuery();

                        if (isPreview)
                        {
                            sqlQuery = "Insert into LogPreview (Document_PRId,Document_PreviewUser,logSatus,logCreateDate) Values (" +
                                "@Document_PRId,@Document_PreviewUser,0,GETDATE())";

                            cmd.CommandText = sqlQuery;
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Document_PRId", Document_Id);
                            cmd.Parameters.AddWithValue("@Document_PreviewUser", StaffID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    //document_id = (int)shipperIdParam.Value;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int RejectPR(int Document_Id, int StaffID, int DeptID, bool isPreview, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;

            decimal budget = 0;
            decimal doc_cog = 0;
            int docLevel = 0;
            int ApproveLevel = 0;
            try
            {

                SqlConnection conn = DBHelper.sqlConnection();
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                cmd = conn.CreateCommand();
                //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Connection = conn;

                sqlQuery = "INSERT INTO ApprovePR (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
            " (@Approve_Documen_Id,@Approve_Create_Level,@Approve_Current_Level,9,0,@Approve_By  )";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Approve_Documen_Id", Document_Id);
                cmd.Parameters.AddWithValue("@Approve_Create_Level", docLevel);
                cmd.Parameters.AddWithValue("@Approve_Current_Level", ApproveLevel);
                cmd.Parameters.AddWithValue("@Approve_By", StaffID);
                cmd.ExecuteNonQuery();

                sqlQuery = "Update DocumentPR_Header SET " +
                            "Document_EditUser = @Document_EditUser,Document_EditDate=GETDATE(),Document_Status =9 WHERE Document_Id = @Document_Id";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Id", Document_Id);
                cmd.Parameters.AddWithValue("@Document_EditUser", StaffID);
                cmd.ExecuteNonQuery();



                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public DataTable GeneratePRNo(int id, ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT dbo.GeneratePRID(" + id + ")";

                dt = DBHelper.List(strSQL);
                if (id == 0)
                    dt.Rows[0][0] = "";

            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Document_Vnos";
            return dt;
        }
        public int ApprovePROverBudget(int Document_Id, int StaffID, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;

            try
            {
                sqlQuery = "SELECT * FROM ApprovePROverBudget WHERE Approve_Documen_Id = " + Document_Id;
                DataTable dt = DBHelper.List(sqlQuery);



                if (dt.Rows.Count == 0)
                {
                    sqlQuery = "INSERT INTO ApprovePROverBudget (Approve_Id,Approve_Documen_Id,Approve_Status,Approve_Order,Approve_By) " +
                        " SELECT isnull(max(Approve_Id),0) +1  ,@Approve_Documen_Id,2,0,@Approve_By  FROM ApprovePROverBudget";
                }
                else
                {
                    sqlQuery = "Update ApprovePROverBudget SET " +
                         "Approve_Status = 2,Approve_By=@Approve_By WHERE Approve_Documen_Id = @Approve_Documen_Id";
                }

                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Approve_Documen_Id", Document_Id);
                cmd.Parameters.AddWithValue("@Approve_By", StaffID);
                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public bool UpdatePRDetail(PRDetailModels detail, ref string errMsg)
        {
            bool ret = false;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;

            try
            {
                sqlQuery = "UPDATE DocumentPR_Detail SET " +
                            "Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                             ",Document_Detail_Acc=@Document_Detail_Acc,Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                            ",Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                            ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum WHERE Document_Detail_Id=@Document_Detail_Id";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                cmd.Parameters.AddWithValue("@Document_Detail_Acc", detail.Document_Detail_Acc);
                cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc);
                cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));

                cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));

                cmd.ExecuteNonQuery();

                string sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail WHERE Document_Detail_Hid = " + detail.Document_Detail_Hid;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable tmp = new DataTable();
                da.Fill(tmp);
                decimal amount = 0;
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                    Math.Round(amount, 2);
                }

                sqlQuery = "Update DocumentPR_Header SET Document_Cog=@Document_Cog,Document_VatSUM=@Document_VatSUM,Document_NetSUM=@Document_NetSUM WHERE Document_Id=@Document_Detail_Hid";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Hid", detail.Document_Detail_Hid);

                cmd.Parameters.AddWithValue("@Document_Cog", Math.Round(amount, 2));
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(amount * (decimal)0.07, 2));//()
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(amount * (decimal)1.07, 2));//
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }

            return ret;
        }
        public bool LogPreview(int logId, ref string errMsg)
        {
            bool result = false;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            //myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;

            try
            {

                sqlQuery = "Update LogPreview Set logSatus=1  WHERE logId=@logId";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@logId", logId);

                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return result;
        }
        public async Task<bool> DeletePRData(int id)
        {
            bool result = false;
            string strSQL = null;


            strSQL = "UPDATE DocumentPR_Header SET Document_Delete=1 WHERE Document_Id=" + id;

            DBHelper.Execute(strSQL);

            return true;
        }

        public async Task<bool> CheckDeletePRData(int id)
        {
            string strSQL = null;
            DataTable dt = new DataTable();
            bool ok = false;


            try
            {
                strSQL = "\r\n  " +
                    " SELECT Document_Vnos_PO FROM DocumentPR_Header where Document_Id=" + id;


                dt = DBHelper.List(strSQL);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Document_Vnos_PO"].ToString().Length > 0) ok = true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return ok;
        }

        public bool UpdatePRDetail(List<PRDetailModels> list, ref string errMsg)
        {
            bool ret = false;
            string sqlQuery = "";
            string sql = "";
            SqlCommand cmd = new SqlCommand();
            SqlTransaction myTran = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;
            decimal amount = 0;
            try
            {
                int hid = 0;
                decimal Dep_Budget = 0;
                int dep = 0;
                int year = DateTime.Now.Year;
                string detailid = "";
                foreach (PRDetailModels detail in list)
                {
                    amount += detail.Document_Detail_UnitPrice * detail.Document_Detail_Quan;
                    detailid += "," + detail.Document_Detail_Id;
                    hid = detail.Document_Detail_Hid;
                }

                sql = "Select * from DocumentPR_Header WHERE  Document_Id = " + hid;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                DataTable docheader = new DataTable();
                da.Fill(docheader);
                foreach (DataRow hdrow in docheader.Rows)
                {
                    dep = Convert.ToInt32(hdrow["Document_Depid"]);
                }



                //sql = "Select * from BudgetOfYearByDepartment WHERE  BudgetYear = " + year + " AND DEPid = " + dep;
                //da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //DataTable depbudget = new DataTable();
                //da.Fill(depbudget);
                //if (depbudget.Rows.Count > 0)
                //{

                //    string monthcol = "DEPmonth" + DateTime.Now.Month.ToString();
                //    foreach (DataRow dr in depbudget.Rows)
                //    {
                //        Dep_Budget = Convert.ToDecimal(dr[monthcol]);
                //    }
                //}

                //sql = "Select * from BudgetOfYearByDepartment WHERE  BudgetYear = " + year + " AND DEPid = " + dep;
                //da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //DataTable detaillist = new DataTable();
                //da.Fill(detaillist);



                //sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail WHERE Document_Detail_Hid = " + hid + " AND Document_Detail_Id not in (" + detailid.Substring(1) + ")";
                //da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //DataTable tmp = new DataTable();
                //da.Fill(tmp);
                //foreach (DataRow dr in tmp.Rows)
                //{
                //    amount += Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                //}

                //string fristDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("00") + "-01";
                //int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                //string LastDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).AddDays(1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                //sql = "Select SUM(Document_Cog) From DocumentPR_header  where Document_Date between '" + fristDate + "'and  '" + LastDate + "' and Document_Depid =" + dep +
                //    " and Document_Delete=0 and Document_Status in (2) and Document_Id not in (" + hid + ")";
                //da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //tmp = new DataTable();
                //da.Fill(tmp);

                //decimal dep_amount = 0;
                //foreach (DataRow dr in tmp.Rows)
                //{
                //    dep_amount = Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);

                //}

                //dep_amount += amount;
                //dep_amount = Math.Round(dep_amount, 2);
                //bool overbudget = false;
                //if (Dep_Budget < dep_amount)
                //{
                //    errMsg = "ยอดรวมมากกว่างบประมาณอนุมัติของแผนกในเดือนปัจจุบัน";
                //    overbudget = true;
                //}

                foreach (PRDetailModels detail in list)
                {

                    sqlQuery = "UPDATE DocumentPR_Detail SET " +
                                "Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                                 ",Document_Detail_Acc=@Document_Detail_Acc,Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                                ",Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                                ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum WHERE Document_Detail_Id=@Document_Detail_Id";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                    cmd.Parameters.AddWithValue("@Document_Detail_Acc", detail.Document_Detail_Acc);
                    cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc);
                    cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));

                    cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));
                    cmd.Transaction = myTran;
                    cmd.ExecuteNonQuery();
                }
                sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail WHERE Document_Detail_Hid = " + list[0].Document_Detail_Hid;
                da = new SqlDataAdapter(sql, conn);

                DataTable tmp = new DataTable();
                da.SelectCommand.Transaction = myTran;
                da.Fill(tmp);
                amount = 0;
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                    Math.Round(amount, 2);
                }

                sqlQuery = "Update DocumentPR_Header SET Document_Cog=@Document_Cog,Document_VatSUM=@Document_VatSUM,Document_NetSUM=@Document_NetSUM" +
                    " WHERE Document_Id=@Document_Detail_Hid";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Hid", list[0].Document_Detail_Hid);

                cmd.Parameters.AddWithValue("@Document_Cog", Math.Round(amount, 2));
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(amount * (decimal)0.07, 2));//()
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(amount * (decimal)1.07, 2));//
                cmd.Transaction = myTran;

                cmd.ExecuteNonQuery();

                myTran.Commit();


            }
            catch (Exception ex)
            {
                myTran.Rollback();
                errMsg = ex.Message;
            }

            return ret;
        }



        public bool CheckAccountBudget(string ACCCode,string depcode,string depid, ref string errMsg)
        {
            bool result = false;
            int document_id = 0;
            SqlTransaction myTran = null;
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;
            SqlConnection conn = DBHelper.sqlConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;
            try
            {
                decimal amount = 0;
                //int NoList = 0;
                decimal Dep_Budget = 0;
                int year = DateTime.Now.Year;

                string sql = "Select * from Budget WHERE  Code_GLyear = " + year + " AND CodeACC = '" + ACCCode + "' AND CodeDEP = '" + depcode + "'";
                DataTable depbudget = DBHelper.List(sql);

                if (depbudget.Rows.Count > 0)
                {

                    string monthcol = "SumMonth" + DateTime.Now.Month.ToString();
                    foreach (DataRow dr in depbudget.Rows)
                    {
                        Dep_Budget = Convert.ToDecimal(dr[monthcol]);
                    }
                }

                //string sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail WHERE Document_Detail_Acc = '" + ACCCode + "' ";
                //SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.Transaction = myTran;
                //DataTable tmp = new DataTable();
                //da.Fill(tmp);
                //foreach (DataRow dr in tmp.Rows)
                //{
                //    amount += Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                //}

                string fristDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("00") + "-01";
                int LastdayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                string LastDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, LastdayofMonth).AddDays(1).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                sql = "Select SUM(Document_Cog) From DocumentPR_header  where Document_Date between '" + fristDate + "'and  '" + LastDate + "' and Document_Depid ='" + depid + "' and Document_Delete=0 and Document_Status in (2)";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                DataTable tmp = new DataTable();
                da.Fill(tmp);

                decimal dep_amount = 0;
                foreach (DataRow dr in tmp.Rows)
                {
                    dep_amount = dep_amount + Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);

                }

                //dep_amount += amount;
                //dep_amount = Math.Round(dep_amount, 2);
                //bool overbudget = false;
                //if (Dep_Budget < dep_amount)
                if (Dep_Budget < dep_amount)
                {
                    errMsg = "ยอดรวมมากกว่างบประมาณอนุมัติของแผนกในเดือนปัจจุบัน";
                    result = false;
                }
                else
                {
                    result = true;
                }
                


            }
            catch { }
            return result;
        }
        #endregion

    }
}
