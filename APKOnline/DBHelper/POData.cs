using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace APKOnline.DBHelper
{
    public interface IPOData
    {
        DataTable GetDepData(ref string errMsg);
        DataTable GetCategoryData(ref string errMsg);
        DataTable GetObjectiveData(int id, ref string errMsg);
        DataTable GeneratePONo(int id, ref string errMsg);
        DataTable GetJobData(ref string errMsg);
        DataTable GetAccountData(ref string errMsg);
        DataTable GetDetailData(int Document_Detail_Hid, int tmp, ref string errMsg);
        DataTable GetPRHeaderData(int Document_id, int tmp, ref string errMsg);
        int InsertHeader(PRHeaderModels Header, int prid, ref string errMsg);
        int InserttmpDetail(int PRID, int tmpHID, ref string errMsg);
        int UpdatetmpDetail(PRDetailModels detail, ref string errMsg);
        int DeleteTmpDetail(int Hid, ref string errMsg);
        DataTable GetListPO(int id,ref string errMsg);
        DataTable GetPRDataForCreatePO(int DeptID, ref string errMsg);
        int ApprovePO(int Document_Id, int StaffID, int DeptID, ref string errMsg);
        int RejectPO(int Document_Id, int StaffID, ref string errMsg);
        DataTable GetListPOForApprove(int id, ref string errMsg);
        DataTable GetPOHeaderData(int Document_id, int staffid, ref string errMsg);
        DataTable GetDetailData(int Document_Detail_Hid, ref string errMsg);
        DataTable GetCustomer(int id, ref string errMsg);
        DataTable GetSTKData(ref string errMsg);
        bool UpdatePreviewDetail(PRDetailModels detail, ref string errMsg);
        bool UpdatePreviewDetail(List<PRDetailModels> list, ref string errMsg);
    }

    public class POData : IPOData
    {
        static SqlConnection GB_cn = new SqlConnection();
        static SqlDataReader GB_rd = null;
        static SqlCommand GB_cmd = new SqlCommand();
        static string ErrorList = string.Empty;
        static System.IFormatProvider Iformat = new System.Globalization.CultureInfo("en-US");

        static int NofTX = 0;
        static string[] TXtype;
        static string[] TXlinkT;
        static double[] TXquan;
        static decimal[] TXsum;

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
                    " SELECT DEPid AS ID, DEPdescT AS Name " +
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
        public DataTable GetCategoryData( ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    "(SELECT Category_Id AS ID, Category_Name AS Name FROM Category)";
                dt = DBHelper.List(strSQL);

               
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Category";
            return dt;
        }
        public DataTable GetObjectiveData(int id,ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    " SELECT Objective_Id AS ID, Objective_Name AS Name,Objective_Category_Id " +
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
        public DataTable GetJobData( ref string errMsg)
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
        public DataTable GetAccountData( ref string errMsg)
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
        public DataTable GetSTKData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n  " +
                    //" SELECT STKcode AS Code, STKdescT1 AS Name " +
                    "SELECT STKcode AS Code, CONCAT(STKcode, ' : ', STKdescT1) AS Name " +
                    " FROM STK ";



                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "STK";
            return dt;
        }
        public DataTable GetDetailData(int Document_Detail_Hid, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Detail";
           
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
        public DataTable GetDetailData(int Document_Detail_Hid,int tmp, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Detail";
            if (tmp == 0)
            {
                tablename = "DocumentPO_Detail_tmp"; 
            }
            try
            {
                string strSQL = "\r\n  " +
                      " SELECT * " +
                      " FROM " + tablename + "  where Document_Detail_Delete=0 AND Document_PR_Hid =" + Document_Detail_Hid;
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Detail";

            return dt;
        }
        public DataTable GetPRHeaderData(int Document_id,int tmp, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";
            if (tmp == 0)
            { tablename = "DocumentPR_Header_tmp"; }
           
            try
            {
                string strSQL = "\r\n  " +
                      " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,CAST(d.DEPdescT as NVARCHAR(max)) AS Dep" +
                      ",CAST(j.JOBdescT as NVARCHAR(max)) As Job ,g.GroupName AS 'Group'" +
                      " ,CAST(Objective_Name as NVARCHAR(max)) AS Objective,CAST(Category_Name as NVARCHAR(max)) AS Category " +
                      " ,CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff" +
                      " FROM "+ tablename + " p LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job" +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                      " where Document_Delete=0 AND Document_Id =" + Document_id;
                dt = DBHelper.List(strSQL);
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        dr["Document_Date"] = Convert.ToDateTime(dr["Document_Date"]).ToString("dd-MM-yyyy");
                //    }
                //}

            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Header";

            return dt;
        }
        public DataTable GetPOHeaderData(int Document_id, int staffid, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Header";
            decimal budget = 0;
            decimal DocCog = 0;
            int DEPid = 0;

            try
            {
                //string strSQL = "\r\n  " +
                //    " SELECT * " +
                //    " FROM StaffAuthorize WHERE StaffID = " + staffid;
                //DataTable staffauth = DBHelper.List(strSQL);
                //foreach (DataRow dr in staffauth.Rows)
                //{
                //    budget = Convert.ToDecimal(dr["PositionLimit"]);
                //}

                //" ,CASE WHEN p.Document_Cog > " + budget + " THEN 'รับทราบ'ELSE 'อนุมัติ' END AS SaveText " +

                string strSQL = "\r\n  " +
                      " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job,g.GroupName AS 'Group'" +
                      " ,CAST(Objective_Name as NVARCHAR(max)) AS Objective,CAST(Category_Name as NVARCHAR(max)) AS Category " +
                      " ,CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff" +
                      " ,'อนุมัติ' AS SaveText " +
                      " ,CONCAT(CREcode,' : ',CAST(cus.CREnameT as NVARCHAR(max))) AS Customer " +
                      " FROM " + tablename + " p " +
                      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job" +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                      " LEFT JOIN CRE cus on cus.CREcode=p.Document_Cus" +
                      " where Document_Delete=0 AND Document_Id =" + Document_id;
                dt = DBHelper.List(strSQL);
                dt.Columns.Add("CheckPreview", typeof(Boolean));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DEPid = Convert.ToInt32(dr["Document_Dep"]);
                        DocCog = Convert.ToDecimal(dr["Document_Cog"]);

                        dr["CheckPreview"] = VerifyAcceptReview(Convert.ToInt32(dr["Document_PRID"]));

                    }

                    strSQL = "\r\n  " +
                    " SELECT * " +
                    //" FROM StaffAuthorize WHERE StaffID = " + staffid + " and DEPid = " + DEPid;
                    " FROM StaffAuthorize WHERE StaffID = " + staffid;
                    DataTable staffauth = DBHelper.List(strSQL);
                    foreach (DataRow dr in staffauth.Rows)
                    {
                        budget = Convert.ToDecimal(dr["PositionLimit"]);
                    }

                    if (DocCog > budget)
                    {
                        dt.Rows[0]["SaveText"] = "รับทราบ";
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
        public DataTable GetCustomer(int id, ref string errMsg)
        {
            DataTable dt = new DataTable();

            try
            {
                string strSQL = "\r\n  " +
                      " SELECT CREcode AS ID, CONCAT(CREcode,' : ',CREnameT ) AS Name FROM CRE ";
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "Customer";

            return dt;
        }
        public DataTable GetListPO( int id,ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Header";

            try
            {
                string strSQL = "\r\n  " +
                      " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                      ",CASE WHEN p.Document_Status = 0 THEN 'รออนุมัติ' WHEN p.Document_Status = 1 THEN 'รับทราบ' WHEN p.Document_Status = 2 THEN 'อนุมัติ' ELSE 'ไม่อนุมัติ' END AS DocStatus" +
                      ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff " +
                      " ,g.GroupName AS 'Group', Objective_Name AS Objective,Category_Name AS Category" +
                      " FROM " + tablename + " p " +
                      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                      " where Document_Delete=0 and p.Document_CreateUser=" + id;
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }
        public DataTable GetListPOForApprove(int id,ref string errMsg)
        {
            DataTable dtPO = new DataTable("PO");
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Header";
            int staffLevel = 0;
            string Depin = "";
            int irow = 0;

            try
            {

                //string sql = "Select * from Staffs WHERE StaffID = " + id;
                //DataTable staff = DBHelper.List(sql);
                //if (staff.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in staff.Rows)
                //    {
                //        //staffLevel = Convert.ToInt32(dr["StaffLevelID"]) - 1;
                //        staffLevel = Convert.ToInt32(dr["StaffLevel"]) - 1;
                //    }
                //}


                //string strSQL = "\r\n   SELECT * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevelID ELSE a.Current_Level END AS Document_Level FROM " +
                //      " (SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,s.StaffLevelID" +
                //      ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff" +
                //      " ,g.GroupName AS 'Group', Objective_Name AS Objective,Category_Name AS Category" +
                //      " FROM " + tablename + " p " +
                //      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                //      " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                //      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                //      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                //      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                //      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                //      " where Document_Delete=0 AND Document_Status < 2) aa " +
                //      " left join(SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                //          " FROM ApprovePO GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level =" + staffLevel; 

                string sql = " SELECT DEPid,AuthorizeLevel FROM StaffAuthorize WHERE StaffID = " + id;
                DataTable staffauth = DBHelper.List(sql);
                foreach (DataRow drow in staffauth.Rows)
                {
                    irow++;
                    staffLevel = Convert.ToInt32(drow["AuthorizeLevel"]) - 1;
                }


                string strSQL = "\r\n   SELECT * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevel ELSE a.Current_Level END AS Document_Level FROM " +
                    " (SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,s.StaffLevel" +
                    ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff" +
                    " ,g.GroupName AS 'Group', Objective_Name AS Objective,Category_Name AS Category" +
                    " FROM " + tablename + " p " +
                    " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                    " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                    " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                    " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                    " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                    " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                    " where Document_Delete=0 AND Document_Status < 2) aa " +
                    " left join(SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                        " FROM ApprovePO GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level =" + staffLevel;

                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPOData";

            return dt;
        }
        public DataTable GetPRDataForCreatePO(int DeptID, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";

            try
            {
                string strSQL = "\r\n  " +
                      " SELECT p.*,convert(nvarchar(MAX), p.Document_Date, 105) AS DocDate" +
                      ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff" +
                      ",(SELECT Count (*) FROM DocumentPR_Detail WHERE Document_Detail_Hid = p.Document_Id) AS Qty" +
                      " FROM " + tablename + " p LEFT JOIN DocumentPO_Header po on po.Document_PRID = p.Document_Id" +
                      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                      " where p.Document_Delete=0 AND p.Document_Status=2 AND po.Document_PRID IS NULL";
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }
        public int InsertHeader(PRHeaderModels Header,int prid, ref string errMsg)
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

                string sql = "Select SUM(Document_Detail_Cog) From DocumentPO_Detail_tmp WHERE Document_PR_Hid = " + prid;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0]);
                    Math.Round(amount, 2);
                }

                string sqlQuery = "INSERT INTO DocumentPO_Header(Document_Group,Document_Category,Document_Objective " +
                                      ",Document_Vnos,Document_Date ,Document_Means,Document_Expect,Document_Cus,Document_Job,Document_Dep,Document_Per" +
                                      ",Document_Doc,Document_Mec,Document_Desc,Document_Nolist,Document_Cog,Document_VatSUM,Document_VatPer" +
                                      " ,Document_NetSUM,Document_Status,Document_Tel,Document_CreateUser,Document_CreateDate,Document_Delete,Document_PRID,Document_Term,Document_Project) VALUES " +
                                      " (@Document_Group,@Document_Category,@Document_Objective " +
                                      ",dbo.GeneratePOID(),GETDATE() ,@Document_Means,@Document_Expect,@Document_Cus,@Document_Job,@Document_Dep,@Document_Per" +
                                      ",@Document_Doc,@Document_Mec,@Document_Desc,@Document_Nolist,@Document_Cog,@Document_VatSUM,@Document_VatPer" +
                                      " ,@Document_NetSUM,@Document_Status,@Document_Tel,@Document_CreateUser,GETDATE(),0,@Document_PRID,@Document_Term,@Document_Project) SET @Document_Id=SCOPE_IDENTITY()";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                shipperIdParam = new SqlParameter("@Document_Id", SqlDbType.Int);
                shipperIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(shipperIdParam);

                cmd.Parameters.AddWithValue("@Document_Group", Header.Document_Group);
                cmd.Parameters.AddWithValue("@Document_Category", Header.Document_Category);
                cmd.Parameters.AddWithValue("@Document_Objective", Header.Document_Objective);
                cmd.Parameters.AddWithValue("@Document_Means", Header.Document_Means == null ? "" : Header.Document_Means);
                cmd.Parameters.AddWithValue("@Document_Expect", Header.Document_Expect == null ? "" : Header.Document_Expect);

                cmd.Parameters.AddWithValue("@Document_Cus", Header.Document_Cus);
                cmd.Parameters.AddWithValue("@Document_Job", Header.Document_Job);
                cmd.Parameters.AddWithValue("@Document_Dep", Header.Document_Dep);
                cmd.Parameters.AddWithValue("@Document_Per", "");
                cmd.Parameters.AddWithValue("@Document_Doc", "");
                cmd.Parameters.AddWithValue("@Document_Mec", "");
                cmd.Parameters.AddWithValue("@Document_Desc", Header.Document_Desc);
                cmd.Parameters.AddWithValue("@Document_Nolist", 0);
                cmd.Parameters.AddWithValue("@Document_Cog", amount);
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(amount * (decimal)0.07,2));//()
                cmd.Parameters.AddWithValue("@Document_VatPer", 7);
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(amount * (decimal)1.07,2));//
                cmd.Parameters.AddWithValue("@Document_Status", 0);
                cmd.Parameters.AddWithValue("@Document_Tel", Header.Document_Tel == null ? "" : Header.Document_Tel);
                cmd.Parameters.AddWithValue("@Document_PRID", prid);
                cmd.Parameters.AddWithValue("@Document_CreateUser", Header.Document_CreateUser);
                cmd.Parameters.AddWithValue("@Document_Term", Header.Document_Term);
                cmd.Parameters.AddWithValue("@Document_Project", Header.Document_Project == null ? "" : Header.Document_Project);
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                document_id = (int)shipperIdParam.Value;


                sql = " INSERT INTO DocumentPO_Detail(Document_Detail_Hid,Document_Detail_Date,Document_Detail_Vnos" +
                        " ,Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                        " ,Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                        " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,Document_Detail_Delete,Document_PR_Hid,Document_PR_Detail_Id)" +
                        " SELECT " + document_id + ",Document_Detail_Date,Document_Detail_Vnos" +
                        " ,Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                        " ,Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                        " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,0,Document_PR_Hid,Document_PR_Detail_Id FROM DocumentPO_Detail_tmp" +
                        "  WHERE Document_PR_Hid = " + prid;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                sql = " DELETE FROM  DocumentPO_Detail_tmp" +
                       "  WHERE Document_PR_Hid = " + prid;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();


                //sql = " Update DocumentPR_Header Set Document_Status=3 " +
                //      "  WHERE Document_id = " + prid;
                //cmd.CommandText = sql;
                //cmd.CommandTimeout = 30;
                //cmd.CommandType = CommandType.Text;
                //cmd.Parameters.Clear();
                //cmd.Transaction = myTran;
                //cmd.ExecuteNonQuery();

                myTran.Commit();
            }
            catch (Exception ex)
            {
                myTran.Rollback();
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int InserttmpDetail(int PRID,int tmpHID, ref string errMsg)
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
                sqlQuery = "DELETE FROM DocumentPO_Detail_tmp WHERE Document_PR_Hid = " + PRID;
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();

                sqlQuery = "INSERT INTO DocumentPO_Detail_tmp(Document_Detail_Hid,Document_Detail_Date,Document_Detail_Vnos" +
                    ",Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                    ",Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                    " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,Document_Detail_Delete" +
                    " ,Document_PR_Hid,Document_PR_Detail_Id)" +
                    " SELECT " + tmpHID + ",Document_Detail_Date,Document_Detail_Vnos" +
                    ",Document_Detail_Acc,Document_Detail_Acc_Desc,Document_Detail_Stk " +
                    ",Document_Detail_Stk_Desc,Document_Detail_ListNo ,Document_Detail_Quan,Document_Detail_Cog,Document_Detail_Vat,Document_Detail_Sum" +
                    " ,Document_Detail_CreateUser,Document_Detail_CreateDate,Document_Detail_UnitPrice,Document_Detail_Delete" +
                    " ,Document_Detail_Hid,Document_Detail_Id FROM DocumentPR_Detail WHERE Document_Detail_Hid =" + PRID;

                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int UpdatetmpDetail(PRDetailModels detail, ref string errMsg)
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

                sqlQuery = "Select * from DocumentPO_Detail_tmp WHERE  Document_Detail_Id = "+ detail.Document_Detail_Id;
                SqlDataAdapter sa = new SqlDataAdapter(sqlQuery, conn);
                DataTable dt = new DataTable();
                sa.Fill(dt);
                decimal cog = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    cog = Convert.ToDecimal(dr["Document_Detail_Cog"]);
                }



                if (cog >= Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2))
                {



                    sqlQuery = "Update DocumentPO_Detail_tmp Set Document_Detail_Stk=@Document_Detail_Stk,Document_Detail_Stk_Desc=@Document_Detail_Stk_Desc" +
                        ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum,Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                        ",Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                        ",Document_Detail_Acc=@Document_Detail_Acc,Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                        " WHERE Document_Detail_Id=@Document_Detail_Id  ";

                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                    cmd.Parameters.AddWithValue("@Document_Detail_Stk", detail.Document_Detail_Stk == null ? "" : detail.Document_Detail_Stk);
                    cmd.Parameters.AddWithValue("@Document_Detail_Stk_Desc", detail.Document_Detail_Stk_Desc == null ? "" : detail.Document_Detail_Stk_Desc);
                    cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc == null ? "" : detail.Document_Detail_Acc_Desc);
                    cmd.Parameters.AddWithValue("@Document_Detail_Acc", detail.Document_Detail_Acc == null ? "" : detail.Document_Detail_Acc);

                    cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));

                    cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    errMsg = "ราคาสินค้าที่แก้ไข มากกว่าราคาในเอกสารขอซื้อภายในไม่สามารถบันทึกได้ โปรดตรวจสอบ.";
                }
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

                sqlQuery = "DELETE From DocumentPO_Detail_tmp WHERE Document_PR_Hid=@Document_PR_Hid";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_PR_Hid", Hid);

                cmd.ExecuteNonQuery();

                //document_id = (int)shipperIdParam.Value;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;
        }
        public int ApprovePO(int Document_Id,int StaffID, int DeptID, ref string errMsg)
        {
            int document_id = 0;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlParameter shipperIdParam = null;

            decimal budget = 0;
            decimal doc_cog = 0;
            int docLevel = 0;
            int ApproveLevel = 0;
            bool SaveOK = false;


            try
            {
                string strSQL = "\r\n  " +
              " SELECT * " +
              " FROM StaffAuthorize WHERE StaffID = " + StaffID;
                DataTable staffauth = DBHelper.List(strSQL);
                foreach (DataRow dr in staffauth.Rows)
                {
                    budget = Convert.ToDecimal(dr["PositionLimit"]);
                    //ApproveLevel = Convert.ToInt32(dr["PositionPermissionId"]);
                }

                string sql = "Select * from Staffs WHERE StaffID = " + StaffID;
                DataTable staff = DBHelper.List(sql);
                if (staff.Rows.Count > 0)
                {
                    foreach (DataRow dr in staff.Rows)
                    {
                        ApproveLevel = Convert.ToInt32(dr["StaffLevel"]);
                    }
                }

                strSQL = "\r\n  " +
              " SELECT * " +
              " FROM DocumentPO_Header h left join Staffs s on s.StaffID=h.Document_CreateUser  WHERE h.Document_Id = " + Document_Id;
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

                    sqlQuery = "INSERT INTO ApprovePO (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
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
                    sqlQuery = "INSERT INTO ApprovePO (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
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

                    sqlQuery = "Update DocumentPO_Header SET " +
                                "Document_EditUser = @Document_EditUser,Document_EditDate=GETDATE(),Document_Status =2 WHERE Document_Id = @Document_Id";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Id", Document_Id);
                    cmd.Parameters.AddWithValue("@Document_EditUser", StaffID);
                    cmd.ExecuteNonQuery();

                    string DocPO = "";
                    int PRID = 0;
                    strSQL = " SELECT Document_PRID,Document_Vnos FROM DocumentPO_Header WHERE Document_Id = " + Document_Id;
                    docHeader = DBHelper.List(strSQL);
                    foreach (DataRow dr in docHeader.Rows)
                    {
                        PRID = Convert.ToInt32(dr["Document_PRID"]);
                        DocPO = Convert.ToString(dr["Document_Vnos"]);
                    }

                    sqlQuery = "Update DocumentPR_Header SET " +
                                "Document_Vnos_PO = @Document_Vnos_PO,Document_PO=@Document_PO,Document_Status=@Document_Status  WHERE Document_Id = @Document_Id";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Id", PRID);
                    cmd.Parameters.AddWithValue("@Document_Vnos_PO", DocPO);
                    cmd.Parameters.AddWithValue("@Document_PO", 1);
                    cmd.Parameters.AddWithValue("@Document_Status",4);
                    cmd.ExecuteNonQuery();

                    SaveOK = WebServiceSaveVoucherPO(Document_Id, StaffID, DeptID, ref errMsg);

                }
                //document_id = (int)shipperIdParam.Value;                              

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return document_id;

        }
        public int RejectPO(int Document_Id, int StaffID, ref string errMsg)
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

                sqlQuery = "INSERT INTO ApprovePO (Approve_Documen_Id,Approve_Create_Level,Approve_Current_Level,Approve_Status,Approve_Order,Approve_By) VALUES" +
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

                sqlQuery = "Update DocumentPO_Header SET " +
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
        public DataTable GeneratePONo(int id, ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT dbo.GeneratePOID()";
               
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
        public static int DBInt(object obj)
        {
            int i;

            try
            {
                i = obj == DBNull.Value ? 0 : Convert.ToInt32(obj);
            }
            catch
            {
                i = 0;
            }

            return i;
        }
        public static string DBString(object obj)
        {
            string str;

            str = obj == DBNull.Value ? "" : Convert.ToString(obj);

            return str;
        }
        public bool UpdatePreviewDetail(PRDetailModels detail, ref string errMsg)
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
                sqlQuery = "UPDATE DocumentPO_Detail SET " +
                            "Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                             ",Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                            ",Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                            ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum " +
                            ",Document_Detail_PreviewEditUser = @Document_Detail_PreviewEditUser ,Document_Detail_PreviewEditDate = GETDATE()" +
                            " WHERE Document_Detail_Id=@Document_Detail_Id";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc);
                cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));
                                                                                       
                cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));
                cmd.Parameters.AddWithValue("@Document_Detail_PreviewEditUser", detail.Document_Detail_EditUser);
                cmd.ExecuteNonQuery();

                string sql = "Select SUM(Document_Detail_Cog) From DocumentPO_Detail WHERE Document_Detail_Hid = " + detail.Document_Detail_Hid;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable tmp = new DataTable();
                da.Fill(tmp);
                decimal amount = 0;
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                    Math.Round(amount, 2);
                }

                sqlQuery = "Update DocumentPO_Header SET Document_Cog=@Document_Cog,Document_VatSUM=@Document_VatSUM,Document_NetSUM=@Document_NetSUM " +
                            ",Document_PreviewEditUser = @Document_PreviewEditUser ,Document_PreviewEditDate = GETDATE()" +
                    "WHERE Document_Id=@Document_Detail_Hid";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Hid", detail.Document_Detail_Hid);

                cmd.Parameters.AddWithValue("@Document_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice * (decimal)0.07, 2));//()
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice * (decimal)1.07, 2));//
                cmd.Parameters.AddWithValue("@Document_PreviewEditUser", detail.Document_Detail_EditUser);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }

            return ret;
        }
        public bool UpdatePreviewDetail(List<PRDetailModels> list, ref string errMsg)
        {
            bool ret = false;
            string sqlQuery = "";
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = DBHelper.sqlConnection();
            SqlTransaction  myTran = null;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = conn.CreateCommand();
            myTran = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Connection = conn;
            int hid = 0;
            try
            {

                foreach (PRDetailModels detail in list)
                {
                    hid = detail.Document_Detail_Hid;
                     sqlQuery = "UPDATE DocumentPO_Detail SET " +
                                    "Document_Detail_UnitPrice=@Document_Detail_UnitPrice" +
                                     ",Document_Detail_Acc_Desc=@Document_Detail_Acc_Desc" +
                                    ",Document_Detail_Quan=@Document_Detail_Quan,Document_Detail_Cog=@Document_Detail_Cog" +
                                    ",Document_Detail_Vat=@Document_Detail_Vat,Document_Detail_Sum=@Document_Detail_Sum " +
                                    ",Document_Detail_PreviewEditUser = @Document_Detail_PreviewEditUser ,Document_Detail_PreviewEditDate = GETDATE()" +
                                    " WHERE Document_Detail_Id=@Document_Detail_Id";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Document_Detail_Id", detail.Document_Detail_Id);
                    cmd.Parameters.AddWithValue("@Document_Detail_Acc_Desc", detail.Document_Detail_Acc_Desc);
                    cmd.Parameters.AddWithValue("@Document_Detail_Quan", Math.Round(detail.Document_Detail_Quan, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", Math.Round(detail.Document_Detail_UnitPrice, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Cog", Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2));

                    cmd.Parameters.AddWithValue("@Document_Detail_Vat", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)0.07, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_Sum", Math.Round((detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07, 2));
                    cmd.Parameters.AddWithValue("@Document_Detail_PreviewEditUser", detail.Document_Detail_EditUser);
                    cmd.Transaction = myTran;
                    cmd.ExecuteNonQuery();

                }
                string sql = "Select SUM(Document_Detail_Cog) From DocumentPO_Detail WHERE Document_Detail_Hid = " + hid;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable tmp = new DataTable();
                da.SelectCommand.Transaction = myTran;
                da.Fill(tmp);
                decimal amount = 0;
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0] == DBNull.Value ? 0 : dr[0]);
                    Math.Round(amount, 2);
                }

                sqlQuery = "Update DocumentPO_Header SET Document_Cog=@Document_Cog,Document_VatSUM=@Document_VatSUM,Document_NetSUM=@Document_NetSUM " +
                            ",Document_PreviewEditUser = @Document_PreviewEditUser ,Document_PreviewEditDate = GETDATE(),Document_PreviewNote=@Document_PreviewNote" +
                    " WHERE Document_Id=@Document_Detail_Hid";
                cmd.CommandText = sqlQuery;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_Detail_Hid", hid);

                cmd.Parameters.AddWithValue("@Document_Cog", amount);
                cmd.Parameters.AddWithValue("@Document_VatSUM", Math.Round(amount * (decimal)0.07, 2));//()
                cmd.Parameters.AddWithValue("@Document_NetSUM", Math.Round(amount * (decimal)1.07, 2));//
                cmd.Parameters.AddWithValue("@Document_PreviewEditUser", list[0].Document_Detail_EditUser);
                cmd.Parameters.AddWithValue("@Document_PreviewNote", "Edit By Preview "+ list[0].Document_PreviewNote +" : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
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

        private bool VerifyAcceptReview(int PRid)
        {
            bool ret = false;
            string strSQL = null;
            DataTable dt = new DataTable();

            strSQL = "\r\n  " +
                " select * from LogPreview " +
                " Where Document_PRid = " + PRid;


            dt = DBHelper.List(strSQL);


            foreach (DataRow dr in dt.Rows)
            {
                if (dr["logSatus"].ToString().Trim() == "1") {
                    ret = true;
                }
            }

            return ret;
        }

        public void SavePOtoDB(int Document_Id)
        {

        }

        public static bool WebServiceSaveVoucherPO(int Document_Id, int StaffID, int DeptID, ref string errMsg)
        {
            DataSet DataS;
            DataTable _DT;
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionBase"].ConnectionString);
            bool Sok = false;
            int[] PRid = new int[0];

            try
            {
                //conn.Open();

                string sqldata = "Select * FROM DocumentPO_Header WHERE Document_Id = " + Document_Id;

                DataTable tmpdata = DBHelper.List(sqldata);
                DateTime Zdate = DateTime.MaxValue;

                //double total = 0;
                DataS = new DataSet();
                DataS.Tables.Clear();

                foreach (DataRow dr in tmpdata.Rows)
                {
                    #region " MIH "
                    _DT = new DataTable("MIH");
                    _DT.Columns.Clear();
                    _DT.Columns.Add("MIHday", typeof(int));
                    _DT.Columns.Add("MIHmonth", typeof(int));
                    _DT.Columns.Add("MIHyear", typeof(int));
                    _DT.Columns.Add("MIHtype", typeof(string));
                    _DT.Columns.Add("MIHvnos", typeof(string));
                    _DT.Columns.Add("MIHcus", typeof(string));
                    _DT.Columns.Add("MIHjob", typeof(string));
                    _DT.Columns.Add("MIHdep", typeof(string));
                    _DT.Columns.Add("MIHper", typeof(string));
                    _DT.Columns.Add("MIHdoc", typeof(string));
                    _DT.Columns.Add("MIHmec", typeof(string));
                    _DT.Columns.Add("MIHvatNO", typeof(string));
                    _DT.Columns.Add("MIHdesc", typeof(string));
                    _DT.Columns.Add("MIHmemo", typeof(string));
                    _DT.Columns.Add("MIHnotes", typeof(string));
                    _DT.Columns.Add("MIHref1", typeof(string));
                    _DT.Columns.Add("MIHref2", typeof(string));
                    _DT.Columns.Add("MIHref3", typeof(string));
                    _DT.Columns.Add("MIHdiscHT1", typeof(string));
                    _DT.Columns.Add("MIHdiscHT2", typeof(string));
                    _DT.Columns.Add("MIHdelvSite", typeof(string));
                    _DT.Columns.Add("MIHdelvDate", typeof(DateTime));
                    _DT.Columns.Add("MIHstanding", typeof(int));
                    _DT.Columns.Add("MIHlinkGL", typeof(int));
                    _DT.Columns.Add("MIHnoUPLINK", typeof(int));
                    _DT.Columns.Add("MIHnolist", typeof(int));
                    _DT.Columns.Add("MIHcurC", typeof(int));
                    _DT.Columns.Add("MIHvatInList", typeof(int));
                    _DT.Columns.Add("MIHlock", typeof(int));
                    _DT.Columns.Add("MIHprintN", typeof(int));
                    _DT.Columns.Add("MIHcheque", typeof(int));
                    _DT.Columns.Add("MIHcancel", typeof(int));
                    _DT.Columns.Add("MIHstatus", typeof(int));
                    _DT.Columns.Add("MIHclearUM", typeof(int));
                    _DT.Columns.Add("MIHctermNO", typeof(int));
                    _DT.Columns.Add("MIHcog", typeof(decimal));
                    _DT.Columns.Add("MIHvatSUM", typeof(decimal));
                    _DT.Columns.Add("MIHnetSUM", typeof(decimal));
                    _DT.Columns.Add("MIHdiscLST", typeof(decimal));
                    _DT.Columns.Add("MIHdiscHF1", typeof(decimal));
                    _DT.Columns.Add("MIHdiscHF2", typeof(decimal));
                    _DT.Columns.Add("MIHexchg", typeof(decimal));
                    _DT.Columns.Add("MIHextraSUM", typeof(decimal));
                    _DT.Columns.Add("MIHextraVAT", typeof(decimal));
                    _DT.Columns.Add("MIHextraCUT", typeof(int));
                    _DT.Columns.Add("MIHextraMEM", typeof(string));
                    _DT.Columns.Add("MIHrecNO", typeof(string));
                    _DT.Columns.Add("MIHrecDATE", typeof(DateTime));
                    _DT.Columns.Add("MIHduePO", typeof(DateTime));
                    
                    _DT.Columns.Add("MIHsaveTAG", typeof(string));
                    _DT.Columns.Add("MIHkeyUser", typeof(string));
                    _DT.Columns.Add("MIHdocRunNo", typeof(string));
                    _DT.Columns.Add("MIHisCF", typeof(int));
                    _DT.Columns.Add("MIHisSP", typeof(int));
                    _DT.Columns.Add("MIHnumSI", typeof(int));
                    _DT.Columns.Add("MIHnumSC", typeof(decimal));
                    _DT.Columns.Add("MIHvatPC", typeof(decimal));
                    _DT.Columns.Add("MIHincluded", typeof(int));
                    _DT.Columns.Add("MIHtaxvnos", typeof(string));
                    _DT.Columns.Add("MIHtaxsum", typeof(decimal));
                    _DT.Columns.Add("MIHload", typeof(int));
                    _DT.Columns.Add("MIHbookOrderNum", typeof(string));

                    Zdate = ZDnulls(Date_CvDMY(Convert.ToDateTime(dr["Document_Date"]).Day, Convert.ToDateTime(dr["Document_Date"]).Month, Convert.ToDateTime(dr["Document_Date"]).Year, false));
                    _DT.Rows.Add();
                    _DT.Rows[0]["MIHday"] = Convert.ToDateTime(dr["Document_Date"]).Day;
                    _DT.Rows[0]["MIHmonth"] = Convert.ToDateTime(dr["Document_Date"]).Month;
                    _DT.Rows[0]["MIHyear"] = Convert.ToDateTime(dr["Document_Date"]).Year;
                    _DT.Rows[0]["MIHtype"] = "PP";
                    _DT.Rows[0]["MIHvnos"] = Convert.ToString(dr["Document_Vnos"]);
                    _DT.Rows[0]["MIHcus"] = Convert.ToString(dr["Document_Cus"]);
                    _DT.Rows[0]["MIHjob"] = "";
                    _DT.Rows[0]["MIHdep"] = GetNameFromTBname(DeptID, "Department", "DEPcode");
                    _DT.Rows[0]["MIHper"] = "";
                    _DT.Rows[0]["MIHdoc"] = "";
                    _DT.Rows[0]["MIHmec"] = "";
                    _DT.Rows[0]["MIHvatNO"] = "";
                    _DT.Rows[0]["MIHdesc"] = Convert.ToString(dr["Document_Desc"]);
                    _DT.Rows[0]["MIHmemo"] = "";
                    _DT.Rows[0]["MIHnotes"] = "";
                    _DT.Rows[0]["MIHref1"] = "";
                    _DT.Rows[0]["MIHref2"] = "";
                    _DT.Rows[0]["MIHref3"] = "";
                    _DT.Rows[0]["MIHdiscHT1"] = "";
                    _DT.Rows[0]["MIHdiscHT2"] = "";
                    _DT.Rows[0]["MIHdelvSite"] = "";
                    _DT.Rows[0]["MIHdelvDate"] = Zdate;
                    _DT.Rows[0]["MIHstanding"] = 0;
                    _DT.Rows[0]["MIHlinkGL"] = 0;
                    _DT.Rows[0]["MIHnoUPLINK"] = 0;
                    _DT.Rows[0]["MIHnolist"] = Convert.ToInt32(dr["Document_Nolist"]);
                    _DT.Rows[0]["MIHcurC"] = 0;
                    _DT.Rows[0]["MIHvatInList"] = 0;
                    _DT.Rows[0]["MIHlock"] = 0;
                    _DT.Rows[0]["MIHprintN"] = 0;
                    _DT.Rows[0]["MIHcheque"] = 0;
                    _DT.Rows[0]["MIHcancel"] = 0;
                    _DT.Rows[0]["MIHstatus"] = 0;
                    _DT.Rows[0]["MIHclearUM"] = 0;
                    _DT.Rows[0]["MIHctermNO"] = 1;
                    _DT.Rows[0]["MIHcog"] = Convert.ToDecimal(dr["Document_Cog"]);
                    _DT.Rows[0]["MIHvatSUM"] = Convert.ToDecimal(dr["Document_VatSUM"]);
                    _DT.Rows[0]["MIHnetSUM"] = Convert.ToDecimal(dr["Document_NetSUM"]);
                    _DT.Rows[0]["MIHdiscLST"] = 0;
                    _DT.Rows[0]["MIHdiscHF1"] = 0;
                    _DT.Rows[0]["MIHdiscHF2"] = 0;
                    _DT.Rows[0]["MIHexchg"] = 1;
                    _DT.Rows[0]["MIHextraSUM"] = 0;
                    _DT.Rows[0]["MIHextraVAT"] = 0;
                    _DT.Rows[0]["MIHextraCUT"] = 0;
                    _DT.Rows[0]["MIHextraMEM"] = "";
                    _DT.Rows[0]["MIHrecNO"] = "";
                    _DT.Rows[0]["MIHrecDATE"] = DateTime.MaxValue;
                    _DT.Rows[0]["MIHduePO"] = DateTime.MaxValue;
                    
                    _DT.Rows[0]["MIHsaveTAG"] = "";
                    _DT.Rows[0]["MIHkeyUser"] = "ADMIN";
                    _DT.Rows[0]["MIHdocRunNo"] = "";
                    _DT.Rows[0]["MIHisCF"] = 0;
                    _DT.Rows[0]["MIHisSP"] = 0;
                    _DT.Rows[0]["MIHnumSI"] = 0;
                    _DT.Rows[0]["MIHnumSC"] = 0;
                    _DT.Rows[0]["MIHvatPC"] = Convert.ToInt32(dr["Document_VatPer"]);
                    _DT.Rows[0]["MIHincluded"] = 0;
                    _DT.Rows[0]["MIHtaxvnos"] = "";
                    _DT.Rows[0]["MIHtaxsum"] = 0;
                    _DT.Rows[0]["MIHload"] = 0;
                    _DT.Rows[0]["MIHbookOrderNum"] = "";
                    //PRid = DBInt(dr["Document_VatPer"]));
                    DataS.Tables.Add(_DT);
                    #endregion

                    #region " MIE "
                    _DT = new DataTable("MIE");
                    _DT.Columns.Clear();
                    _DT.Columns.Add("MIEday", typeof(int));
                    _DT.Columns.Add("MIEmonth", typeof(int));
                    _DT.Columns.Add("MIEyear", typeof(int));
                    _DT.Columns.Add("MIEtype", typeof(string));
                    _DT.Columns.Add("MIEvnos", typeof(string));
                    _DT.Columns.Add("MIEcus", typeof(string));
                    _DT.Columns.Add("MIErecDT3", typeof(decimal));
                    _DT.Columns.Add("MIErecDT53", typeof(decimal));
                    _DT.Columns.Add("MIErecDRND", typeof(decimal));
                    _DT.Columns.Add("MIErecDEXG", typeof(decimal));
                    _DT.Columns.Add("MIErecDCSH", typeof(decimal));
                    _DT.Columns.Add("MIErecDOTH", typeof(decimal));
                    _DT.Columns.Add("MIErecPLUS", typeof(decimal));
                    _DT.Columns.Add("MIErecCASH", typeof(decimal));
                    _DT.Columns.Add("MIErecCQRD", typeof(decimal));
                    _DT.Columns.Add("MIEctermCX", typeof(string));
                    _DT.Columns.Add("MIEctermPX", typeof(string));
                    _DT.Columns.Add("MIEctermAX", typeof(string));
                    _DT.Columns.Add("MIEctermDX", typeof(string));
                    _DT.Columns.Add("MIEtag", typeof(int));
                    _DT.Columns.Add("MIEbaseDT3", typeof(decimal));
                    _DT.Columns.Add("MIEbaseDT53", typeof(decimal));
                    _DT.Columns.Add("MIEdescDT3", typeof(string));
                    _DT.Columns.Add("MIEdescDT53", typeof(string));
                    _DT.Columns.Add("MIEconDT3", typeof(int));
                    _DT.Columns.Add("MIEconDT53", typeof(int));
                    _DT.Columns.Add("MIErecFC", typeof(string));
                    _DT.Columns.Add("MIErecDT32", typeof(decimal));
                    _DT.Columns.Add("MIErecDT532", typeof(decimal));
                    _DT.Columns.Add("MIEbaseDT32", typeof(decimal));
                    _DT.Columns.Add("MIEbaseDT532", typeof(decimal));
                    _DT.Columns.Add("MIEdescDT32", typeof(string));
                    _DT.Columns.Add("MIEdescDT532", typeof(string));
                    _DT.Columns.Add("MIEconDT32", typeof(int));
                    _DT.Columns.Add("MIEconDT532", typeof(int));

                    _DT.Rows.Add();
                    _DT.Rows[0]["MIEday"] = Convert.ToDateTime(dr["Document_Date"]).Day;
                    _DT.Rows[0]["MIEmonth"] = Convert.ToDateTime(dr["Document_Date"]).Month;
                    _DT.Rows[0]["MIEyear"] = Convert.ToDateTime(dr["Document_Date"]).Year;
                    _DT.Rows[0]["MIEtype"] = "PP";
                    _DT.Rows[0]["MIEvnos"] = Convert.ToString(dr["Document_Vnos"]);
                    _DT.Rows[0]["MIEcus"] = Convert.ToString(dr["Document_Cus"]);
                    _DT.Rows[0]["MIErecDT3"] = 0;
                    _DT.Rows[0]["MIErecDT53"] = 0;
                    _DT.Rows[0]["MIErecDRND"] = 0;
                    _DT.Rows[0]["MIErecDEXG"] = 0;
                    _DT.Rows[0]["MIErecDCSH"] = 0;
                    _DT.Rows[0]["MIErecDOTH"] = 0;
                    _DT.Rows[0]["MIErecPLUS"] = 0;
                    _DT.Rows[0]["MIErecCASH"] = 0;
                    _DT.Rows[0]["MIErecCQRD"] = 0;
                    _DT.Rows[0]["MIEctermCX"] = "30|";
                    _DT.Rows[0]["MIEctermPX"] = "100|";
                    _DT.Rows[0]["MIEctermAX"] = Convert.ToDecimal(dr["Document_NetSUM"]).ToString("0.00") + "|";
                    _DT.Rows[0]["MIEctermDX"] = Date_CvDMY(Convert.ToDateTime(dr["Document_Date"]).AddDays(29).Day, Convert.ToDateTime(dr["Document_Date"]).AddDays(29).Month, Convert.ToDateTime(dr["Document_Date"]).AddDays(29).Year, false) + "|";
                    _DT.Rows[0]["MIEtag"] = 0;
                    _DT.Rows[0]["MIEbaseDT3"] = 0;
                    _DT.Rows[0]["MIEbaseDT53"] = 0;
                    _DT.Rows[0]["MIEdescDT3"] = "";
                    _DT.Rows[0]["MIEdescDT53"] = "";
                    _DT.Rows[0]["MIEconDT3"] = 0;
                    _DT.Rows[0]["MIEconDT53"] = 0;
                    _DT.Rows[0]["MIErecFC"] = "";
                    _DT.Rows[0]["MIErecDT32"] = 0;
                    _DT.Rows[0]["MIErecDT532"] = 0;
                    _DT.Rows[0]["MIEbaseDT32"] = 0;
                    _DT.Rows[0]["MIEbaseDT532"] = 0;
                    _DT.Rows[0]["MIEdescDT32"] = "";
                    _DT.Rows[0]["MIEdescDT532"] = "";
                    _DT.Rows[0]["MIEconDT32"] = 0;
                    _DT.Rows[0]["MIEconDT532"] = 0;
                    DataS.Tables.Add(_DT);
                    #endregion

                }

                #region " MIL "
                _DT = new DataTable("MIL");
                _DT.Columns.Clear();
                _DT.Columns.Add("MILday", typeof(int));
                _DT.Columns.Add("MILmonth", typeof(int));
                _DT.Columns.Add("MILyear", typeof(int));
                _DT.Columns.Add("MILtype", typeof(string));
                _DT.Columns.Add("MILvnos", typeof(string));
                _DT.Columns.Add("MILcus", typeof(string));
                _DT.Columns.Add("MILstk", typeof(string));
                _DT.Columns.Add("MILjob", typeof(string));
                _DT.Columns.Add("MILdep", typeof(string));
                _DT.Columns.Add("MILper", typeof(string));
                _DT.Columns.Add("MILdoc", typeof(string));
                _DT.Columns.Add("MILmec", typeof(string));
                _DT.Columns.Add("MILsto", typeof(string));
                _DT.Columns.Add("MILstoMT", typeof(string));
                _DT.Columns.Add("MILlistNO", typeof(int));
                _DT.Columns.Add("MILquan", typeof(double));
                _DT.Columns.Add("MILquanP2", typeof(double));
                _DT.Columns.Add("MILadisc", typeof(decimal));
                _DT.Columns.Add("MILcog", typeof(decimal));
                _DT.Columns.Add("MILconv", typeof(int));
                _DT.Columns.Add("MILdiscA", typeof(decimal));
                _DT.Columns.Add("MILdiscT", typeof(string));
                _DT.Columns.Add("MILvat", typeof(decimal));
                _DT.Columns.Add("MILsum", typeof(decimal));
                _DT.Columns.Add("MILcut", typeof(int));
                _DT.Columns.Add("MILstype", typeof(int));
                _DT.Columns.Add("MILuname", typeof(string));
                _DT.Columns.Add("MILdesc", typeof(string));
                _DT.Columns.Add("MILvcol1", typeof(string));
                _DT.Columns.Add("MILvcol2", typeof(string));
                _DT.Columns.Add("MILvcol3", typeof(string));
                _DT.Columns.Add("MILvcol4", typeof(string));
                _DT.Columns.Add("MILcurC", typeof(int));
                _DT.Columns.Add("MILexchg", typeof(double));
                _DT.Columns.Add("MILacost", typeof(decimal));
                _DT.Columns.Add("MILpqm", typeof(int));
                _DT.Columns.Add("MILpmFrom", typeof(int));
                _DT.Columns.Add("MILpmTo", typeof(int));
                _DT.Columns.Add("MILtag", typeof(int));
                _DT.Columns.Add("MILlinkVCno", typeof(string));
                _DT.Columns.Add("MILlinkVCtype", typeof(string));
                _DT.Columns.Add("MILlinkVCdeposit", typeof(string));
                _DT.Columns.Add("MILlinkVCid", typeof(int));
                _DT.Columns.Add("MILclearUM", typeof(int));
                _DT.Columns.Add("MILnotes", typeof(string));
                _DT.Columns.Add("MILnotKL", typeof(int));
                
                _DT.Columns.Add("MILdelvDT", typeof(DateTime));
                _DT.Columns.Add("MILnumCSP", typeof(decimal));
                _DT.Columns.Add("MILnumISP", typeof(int));
                _DT.Columns.Add("MILcost", typeof(decimal));
                _DT.Columns.Add("MILucost", typeof(decimal));
                _DT.Columns.Add("MILuprice", typeof(decimal));
                _DT.Columns.Add("MILuseQuanP", typeof(int));
               

                #endregion


                string sqllist = "SELECT * FROM DocumentPO_Detail WHERE Document_Detail_Hid = " + Document_Id + " order by Document_Detail_ListNo";

                DataTable tmplist = DBHelper.List(sqllist);

                int i = -1;
                foreach (DataRow drow in tmplist.Rows)
                {
                    i++;
                    Array.Resize(ref PRid, i + 1);
                    _DT.Rows.Add();
                    _DT.Rows[i]["MILday"] = Convert.ToDateTime(drow["Document_Detail_Date"]).Day;
                    _DT.Rows[i]["MILmonth"] = Convert.ToDateTime(drow["Document_Detail_Date"]).Month;
                    _DT.Rows[i]["MILyear"] = Convert.ToDateTime(drow["Document_Detail_Date"]).Year;
                    _DT.Rows[i]["MILtype"] = "PP";
                    _DT.Rows[i]["MILvnos"] = Convert.ToString(drow["Document_Detail_Vnos"]);
                    _DT.Rows[i]["MILcus"] = Convert.ToString(Convert.ToChar(133)) + Convert.ToString(Convert.ToChar(133));
                    _DT.Rows[i]["MILstk"] = Convert.ToString(drow["Document_Detail_Stk"]);
                    _DT.Rows[i]["MILjob"] = "";
                    _DT.Rows[i]["MILdep"] = GetNameFromTBname(DeptID, "Department", "DEPcode");
                    _DT.Rows[i]["MILper"] = "";
                    _DT.Rows[i]["MILdoc"] = "";
                    _DT.Rows[i]["MILmec"] = "";
                    _DT.Rows[i]["MILsto"] = "MAIN";
                    _DT.Rows[i]["MILstoMT"] = "";
                    _DT.Rows[i]["MILlistNO"] = i + 1; //Convert.ToInt32(drow["Document_Detail_ListNo"]);
                    _DT.Rows[i]["MILquan"] = Convert.ToDouble(drow["Document_Detail_Quan"]);
                    _DT.Rows[i]["MILquanP2"] = 0;
                    _DT.Rows[i]["MILadisc"] = 0;
                    _DT.Rows[i]["MILcog"] = Convert.ToDecimal(drow["Document_Detail_Cog"]);
                    _DT.Rows[i]["MILconv"] = 1;
                    _DT.Rows[i]["MILdiscA"] = 0;
                    _DT.Rows[i]["MILdiscT"] = "";
                    _DT.Rows[i]["MILvat"] = 0;
                    _DT.Rows[i]["MILsum"] = Convert.ToDecimal(drow["Document_Detail_Sum"]);
                    _DT.Rows[i]["MILcut"] = -1;
                    _DT.Rows[i]["MILstype"] = 1;
                    _DT.Rows[i]["MILuname"] = "pcs";
                    _DT.Rows[i]["MILdesc"] = Convert.ToString(drow["Document_Detail_Stk_Desc"]);
                    _DT.Rows[i]["MILvcol1"] = "";
                    _DT.Rows[i]["MILvcol2"] = "";
                    _DT.Rows[i]["MILvcol3"] = "";
                    _DT.Rows[i]["MILvcol4"] = "";
                    _DT.Rows[i]["MILcurC"] = 0;
                    _DT.Rows[i]["MILexchg"] = 1;
                    _DT.Rows[i]["MILacost"] = 0;
                    _DT.Rows[i]["MILpqm"] = 0;
                    _DT.Rows[i]["MILpmFrom"] = 0;
                    _DT.Rows[i]["MILpmTo"] = 0;
                    _DT.Rows[i]["MILtag"] = 0;
                    _DT.Rows[i]["MILlinkVCno"] = "";
                    _DT.Rows[i]["MILlinkVCtype"] = "";
                    _DT.Rows[i]["MILlinkVCdeposit"] = "";
                    _DT.Rows[i]["MILlinkVCid"] = DBInt(GetRandom("I"));
                    _DT.Rows[i]["MILclearUM"] = 0;
                    _DT.Rows[i]["MILnotes"] = "";
                    _DT.Rows[i]["MILnotKL"] = 0;
                    _DT.Rows[0]["MILdelvDT"] = Zdate;
                    _DT.Rows[i]["MILnumCSP"] = 0;
                    _DT.Rows[i]["MILnumISP"] = 0;
                    _DT.Rows[i]["MILcost"] = 0;
                    _DT.Rows[i]["MILucost"] = 0;
                    _DT.Rows[i]["MILuprice"] = 0;
                    _DT.Rows[i]["MILuseQuanP"] = 0;
                    PRid[i] = DBInt(drow["Document_PR_Hid"]);
                }

                DataS.Tables.Add(_DT);


                _DT = new DataTable("MIR");
                DataS.Tables.Add(_DT);


                _DT = new DataTable("MIS");
                DataS.Tables.Add(_DT);

                //GridView1.DataSource = tmplist;
                //GridView1.DataBind();
                //conn.Close();

                //m_APKService = new ServiceReference1.APKClient();
                //if (Endpoint != "")
                //{
                //    m_APKService = new ServiceReference1.APKClient();
                //    m_APKService.Endpoint.Address = new EndpointAddress(Endpoint);
                //}
                //else
                //{ m_APKService = new ServiceReference1.APKClient(); }

                ///*bindingConfiguration= "basicHttp"*/
                //BasicHttpBinding httpBinding = new BasicHttpBinding();
                //httpBinding.MaxReceivedMessageSize = 2147483647;
                //httpBinding.MaxBufferSize = 2147483647;
                //m_APKService.Endpoint.Binding = httpBinding;
                //m_APKService.Open();

                //m_product = m_APKService.GetCodeSTK("127.0.0.1", "M5CM-AB-01", "sa", "dct@123", 0, "");
                string VoucherNo = "";
                string ErrorFromWCF = "";

                Sok = Save_Voucher_MI(DataS, ref VoucherNo, ref ErrorFromWCF, 0, 0, 0);

                errMsg = ErrorFromWCF;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return Sok;
        }

        public static bool Save_Voucher_MI(DataSet Voucher, ref string VoucherNumber, ref string ErrMessage, int Mode = 0, int PrevLno = 0, int PrevSno = 0)
        {
            #region Variable
            DataSet MI = new DataSet();
            DataTable dt = null;
            bool OK = true;
            DataRow drMIE = Voucher.Tables["MIE"].Rows[0];
            DataRow drMIH = Voucher.Tables["MIH"].Rows[0];
            DataTable dtMIH = Voucher.Tables["MIH"];
            DataTable dtMIE = Voucher.Tables["MIE"];
            DataTable dtMIL = Voucher.Tables["MIL"];
            DataTable dtMIK = Voucher.Tables["MIK"];
            DataTable dtMIR = Voucher.Tables["MIR"];
            DataTable dtMIS = Voucher.Tables["MIS"];

            //แก้ไข
            int QctermNo = 0;
            string ctermC = string.Empty;
            string ctermD = string.Empty;
            string ctermP = string.Empty;
            string ctermA = string.Empty;
            int[] QctermC;
            string[] QctermD;
            double[] QctermP;
            decimal[] QctermA;
            //
            int td = 0;
            double vd = 0;
            int i = 0;
            string vdate = string.Empty;
            decimal vp = 0;
            string DateT = string.Empty;
            bool Fte = false;
            decimal vsumnet = Convert.ToDecimal(ZNnull(drMIH["MIHnetSUM"], 0));
            string job = string.Empty;
            string dep = string.Empty;
            string per = string.Empty;
            string doc = string.Empty;
            string mec = string.Empty;
            string stk = string.Empty;
            string sto = string.Empty;
            double vx = 0;

            #endregion

            try
            {
                #region "--- MIE ---"

                QctermNo = 0;
                QctermC = new int[0];
                QctermD = new string[0];
                QctermP = new double[0];
                QctermA = new decimal[0];

                Array.Clear(QctermC, 0, QctermC.Length);
                Array.Clear(QctermD, 0, QctermD.Length);
                Array.Clear(QctermP, 0, QctermP.Length);
                Array.Clear(QctermA, 0, QctermA.Length);


                QctermNo = Convert.ToInt32(ZNnull(drMIH["MIHctermNO"], 0));
                if (QctermNo > 0)
                {
                    QctermC = returnarray(QctermC, QctermNo);
                    QctermD = returnarray(QctermD, QctermNo);
                    QctermP = returnarray(QctermP, QctermNo);
                    QctermA = returnarray(QctermA, QctermNo);

                    ctermC = ZSnull(drMIE["MIEctermCX"]).ToString();
                    ctermD = ZSnull(drMIE["MIEctermDX"]).ToString();
                    ctermP = ZSnull(drMIE["MIEctermPX"]).ToString();
                    ctermA = ZSnull(drMIE["MIEctermAX"]).ToString();

                    MIE_UnparseStr(QctermNo, ctermC, ctermD, ctermP, ctermA, ref QctermC, ref QctermD, ref QctermP, ref QctermA);

                }
                else
                {
                    QctermNo = 1;
                    QctermC = returnarray(QctermC, QctermNo);
                    QctermD = returnarray(QctermD, QctermNo);
                    QctermP = returnarray(QctermP, QctermNo);
                    QctermA = returnarray(QctermA, QctermNo);

                    DateT = Date_SQLDMY(Convert.ToInt32(ZNnull(drMIH["MIHday"], 0)), Convert.ToInt32(ZNnull(drMIH["MIHmonth"], 0)), Convert.ToInt32(ZNnull(drMIH["MIHyear"], 0)));
                    string CusType = "";
                    switch (Voucher.Tables["MIH"].Rows[0]["MIHtype"].ToString())
                    {
                        case "QS":
                        case "PS":
                        case "DS":
                        case "IS":
                        case "AS":
                        case "BS":
                        case "CS":
                            CusType = "DEB";
                            break;
                        case "QP":
                        case "PP":
                        case "DP":
                        case "IP":
                        case "AP":
                        case "BP":
                        case "CP":
                            CusType = "CRE";
                            break;
                        case "SS":
                        case "TS":
                        case "JX":
                        case "MX":
                            CusType = "DEB";
                            break;
                        case "SP":
                        case "TP":
                        case "PX":
                            CusType = "CRE";
                            break;
                    }
                    //CheckCreditLine(CusType, Convert.ToString(ZSnull(drMIE["MIEcus"])), ref td, ref vd, ref vdate, ref vp, DateT, Fte, vsumnet, true);

                    QctermC[0] = td;
                    QctermD[0] = vdate.ToString().Substring(0, 10);
                    QctermP[0] = vd;
                    QctermA[0] = vp;
                }

                //ctermC = (int)int.Parse(drMIE["MIEctermCX"].ToString().Replace("|",""));
                //string ctermD = (string)drMIE["MIEctermDX"].ToString().Replace("|","");
                //decimal ctermP = (decimal)decimal.Parse(drMIE["MIEctermPX"].ToString().Replace("|",""));
                //decimal ctermA = (decimal)decimal.Parse(drMIE["MIEctermAX"].ToString().Replace("|",""));

                //td = 0;
                //vd = ctermP;
                //DateT = Date_SQLDMY(Convert.ToInt32(ZNnull(drMIH["MIHday"])), Convert.ToInt32(ZNnull(drMIH["MIHmonth"],0)), Convert.ToInt32(ZNnull(drMIH["MIHyear"],0)));
                //vp = ctermA;


                //ctermC = td; 
                //ctermP = vd;
                //ctermD = vdate.ToString().Substring(0, 10); 
                //ctermA = vp;
                ctermC = string.Empty;
                ctermD = string.Empty;
                ctermP = string.Empty;
                ctermA = string.Empty;
                MIE_ParseStr(QctermNo, QctermC, QctermD, QctermP, QctermA, ref ctermC, ref ctermD, ref ctermP, ref ctermA);

                dt = new DataTable("MIE");
                drMIE["MIEctermCX"] = ctermC;
                drMIE["MIEctermPX"] = ctermP;
                drMIE["MIEctermAX"] = ctermA;
                drMIE["MIEctermDX"] = ctermD;
                drMIE["MIErecDT3"] = 0;
                drMIE["MIErecDT53"] = 0;
                drMIE["MIErecDRND"] = 0;
                drMIE["MIErecDEXG"] = 0;
                drMIE["MIErecDCSH"] = 0;
                drMIE["MIErecDOTH"] = 0;
                drMIE["MIErecPLUS"] = 0;
                drMIE["MIErecCASH"] = 0;
                drMIE["MIErecCQRD"] = 0;
                drMIE["MIEdescDT53"] = 0;
                drMIE["MIEbaseDT53"] = 0;
                drMIE["MIEconDT3"] = 0;
                drMIE["MIEconDT53"] = 0;
                drMIE["MIEconDT53"] = 0;
                drMIE["MIErecDT532"] = 0;
                drMIE["MIEdescDT532"] = "";
                drMIE["MIEbaseDT32"] = 0;
                drMIE["MIEbaseDT532"] = 0;
                drMIE["MIEconDT32"] = 1;
                drMIE["MIEconDT32"] = 1;

                dt = new DataTable("MIE");
                dt = dtMIE.Clone();
                dt.ImportRow(drMIE);
                MI.Tables.Add(dt);//Save DataSet MIE

                #endregion

                #region "--- MIL ---"

                for (i = 0; i < dtMIL.Rows.Count; i++)
                {
                    dtMIL.Rows[i]["MILstk"] = dtMIL.Rows[i]["MILstk"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILstk"].ToString();
                    if (dtMIL.Rows[i]["MILstk"].ToString().Length > 25) dtMIL.Rows[i]["MILstk"] = dtMIL.Rows[i]["MILstk"].ToString().Substring(0, 25);

                    dtMIL.Rows[i]["MILjob"] = dtMIL.Rows[i]["MILjob"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILjob"].ToString();
                    if (dtMIL.Rows[i]["MILdep"].ToString().Length > 15) dtMIL.Rows[i]["MILdep"] = dtMIL.Rows[i]["MILdep"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILdep"] = dtMIL.Rows[i]["MILdep"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILdep"].ToString();
                    if (dtMIL.Rows[i]["MILdep"].ToString().Length > 25) dtMIL.Rows[i]["MILdep"] = dtMIL.Rows[i]["MILdep"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILper"] = dtMIL.Rows[i]["MILper"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILper"].ToString();
                    if (dtMIL.Rows[i]["MILper"].ToString().Length > 15) dtMIL.Rows[i]["MILper"] = dtMIL.Rows[i]["MILper"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILdoc"] = dtMIL.Rows[i]["MILdoc"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILdoc"].ToString();
                    if (dtMIL.Rows[i]["MILdoc"].ToString().Length > 15) dtMIL.Rows[i]["MILdoc"] = dtMIL.Rows[i]["MILdoc"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILmec"] = dtMIL.Rows[i]["MILmec"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILmec"].ToString();
                    if (dtMIL.Rows[i]["MILmec"].ToString().Length > 15) dtMIL.Rows[i]["MILmec"] = dtMIL.Rows[i]["MILmec"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILsto"] = dtMIL.Rows[i]["MILsto"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILsto"].ToString();
                    if (dtMIL.Rows[i]["MILsto"].ToString().Length > 15) dtMIL.Rows[i]["MILsto"] = dtMIL.Rows[i]["MILsto"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILstoMT"] = dtMIL.Rows[i]["MILstoMT"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILstoMT"].ToString();
                    if (dtMIL.Rows[i]["MILstoMT"].ToString().Length > 15) dtMIL.Rows[i]["MILstoMT"] = dtMIL.Rows[i]["MILstoMT"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILlinkVCno"] = dtMIL.Rows[i]["MILlinkVCno"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILlinkVCno"].ToString();
                    if (dtMIL.Rows[i]["MILlinkVCno"].ToString().Length > 15) dtMIL.Rows[i]["MILlinkVCno"] = dtMIL.Rows[i]["MILlinkVCno"].ToString().Substring(0, 15);

                    dtMIL.Rows[i]["MILlinkVCtype"] = dtMIL.Rows[i]["MILlinkVCtype"] == DBNull.Value ? "" : dtMIL.Rows[i]["MILlinkVCtype"].ToString();
                    if (dtMIL.Rows[i]["MILlinkVCtype"].ToString().Length > 15) dtMIL.Rows[i]["MILlinkVCtype"] = dtMIL.Rows[i]["MILlinkVCtype"].ToString().Substring(0, 25);

                    if (i > 0)
                    {
                        if (!job.Equals(dtMIL.Rows[i]["MILjob"].ToString())) job = "";
                        if (!dep.Equals(dtMIL.Rows[i]["MILdep"].ToString())) dep = "";
                        if (!per.Equals(dtMIL.Rows[i]["MILper"].ToString())) per = "";
                        if (!doc.Equals(dtMIL.Rows[i]["MILdoc"].ToString())) doc = "";
                        if (!mec.Equals(dtMIL.Rows[i]["MILmec"].ToString())) mec = "";
                    }
                    job = ZSnull(dtMIL.Rows[i]["MILjob"]).ToString();
                    dep = ZSnull(dtMIL.Rows[i]["MILdep"]).ToString();
                    per = ZSnull(dtMIL.Rows[i]["MILper"]).ToString();
                    doc = ZSnull(dtMIL.Rows[i]["MILdoc"]).ToString();
                    mec = ZSnull(dtMIL.Rows[i]["MILmec"]).ToString();
                    stk = ZSnull(dtMIL.Rows[i]["MILstk"]).ToString();
                    sto = ZSnull(dtMIL.Rows[i]["MILsto"]).ToString();

                    #region "STK"
                    if (stk.Length > 0)
                    {
                        if (GB_TBfindOK("STK", stk))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["STKlock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสสินค้า " + stk + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                            dtMIL.Rows[i]["MILuname"] = ZSnull(GB_rd["STKuname1"]);
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสสินค้า " + stk + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                    #region "JOB"
                    if (job.Length > 0)
                    {
                        if (GB_TBfindOK("JOB", job))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["JOBlock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสงาน " + job + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสงาน " + job + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;

                        }
                    }
                    #endregion
                    #region "DEP"
                    if (dep.Length > 0)
                    {
                        if (GB_TBfindOK("DEP", dep))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["DEPlock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสแผนก " + dep + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสแผนก " + dep + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                    #region "PER"
                    if (per.Length > 0)
                    {
                        if (GB_TBfindOK("PER", per))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["PERlock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสพนักงาน " + per + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสพนักงาน " + per + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                    #region "DOC"
                    if (doc.Length > 0)
                    {
                        if (GB_TBfindOK("DOC", doc))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["DOClock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสเอกสาร " + per + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสเอกสาร " + per + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                    #region "MEC"
                    if (mec.Length > 0)
                    {
                        if (GB_TBfindOK("MEC", mec))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["MEClock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสส่วนขยาย " + per + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสส่วนขยาย " + per + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                    #region "STO"
                    if (sto.Length > 0)
                    {
                        if (GB_TBfindOK("STO", sto))
                        {
                            if (Convert.ToInt32(ZNnull(GB_rd["STOlock"], 0)) == -1)
                            {
                                ErrorList += Environment.NewLine + "MIL --- รหัสโสตร์ " + per + " ถูกล็อค รายการที่ " + (i + 1).ToString();
                                OK = false;
                            }
                        }
                        else
                        {
                            ErrorList += Environment.NewLine + "MIL --- รหัสโสตร์ " + sto + " ไม่มีกำหนด รายการที่ " + (i + 1).ToString();
                            OK = false;
                        }
                    }
                    #endregion
                }
                dt = new DataTable("MIL");
                dt = dtMIL.Copy();
                MI.Tables.Add(dt); //Save DataSet MIL

                #endregion

                #region "--- MIH ---"

                dt = new DataTable("MIH");
                drMIH["MIHtype"] = drMIH["MIHtype"] == DBNull.Value ? "" : drMIH["MIHtype"];
                if (drMIH["MIHtype"].ToString().Length > 2) drMIH["MIHtype"] = drMIH["MIHtype"].ToString().Substring(0, 2);

                drMIH["MIHvnos"] = drMIH["MIHvnos"] == DBNull.Value ? "" : drMIH["MIHvnos"];
                if (drMIH["MIHvnos"].ToString().Length > 15) drMIH["MIHvnos"] = drMIH["MIHvnos"].ToString().Substring(0, 15);

                drMIH["MIHcus"] = drMIH["MIHcus"] == DBNull.Value ? "" : drMIH["MIHcus"];
                if (drMIH["MIHcus"].ToString().Length > 15) drMIH["MIHcus"] = drMIH["MIHcus"].ToString().Substring(0, 15);

                drMIH["MIHjob"] = job;
                drMIH["MIHdep"] = dep;
                drMIH["MIHper"] = per;
                drMIH["MIHdoc"] = doc;
                drMIH["MIHmec"] = mec;


                drMIH["MIHvatNO"] = drMIH["MIHvatNO"] == DBNull.Value ? "" : drMIH["MIHvatNO"];
                if (drMIH["MIHvatNO"].ToString().Length > 15) drMIH["MIHvatNO"] = drMIH["MIHvatNO"].ToString().Substring(0, 15);

                drMIH["MIHrecNO"] = drMIH["MIHrecNO"] == DBNull.Value ? "" : drMIH["MIHrecNO"];
                if (drMIH["MIHrecNO"].ToString().Length > 15) drMIH["MIHrecNO"] = drMIH["MIHrecNO"].ToString().Substring(0, 15);


                if (Convert.ToInt32(ZNnull(drMIH["MIHexchg"], 0)) == 0) drMIH["MIHexchg"] = 1;

                drMIH["MIHdesc"] = drMIH["MIHdesc"].ToString().Trim();

                if (drMIH["MIHtype"].ToString() == "PS" || drMIH["MIHtype"].ToString() == "PP")
                {
                    if (Convert.ToDecimal(ZNnull(drMIH["MIHextraSUM"], 0)) == 0 || Convert.ToDecimal(ZNnull(drMIH["MIHextraVAT"], 0)) == 0) drMIH["MIHvatNO"] = "";
                }

                dt = new DataTable("MIH");
                dt = dtMIH.Clone();
                dt.ImportRow(drMIH);
                MI.Tables.Add(dt);//Save DataSet MIH

                //if (!IsDate(drMIH["MIHdelvDate"]))
                //{
                //  ErrorList += Environment.NewLine + "ไม่ได้กำหนดวันที่ส่งของ";
                //  OK = false;
                //}
                if (drMIH["MIHcus"].ToString().Length == 0)
                {
                    ErrorList += Environment.NewLine + "ไม่ได้กำหนดรหัสเจ้าหนี้";
                    OK = false;
                }

                if (drMIH["MIHtype"].ToString() == "PP")
                {
                    if (GB_TBfindOK("CRE", drMIH["MIHcus"].ToString()))
                    {
                        //if (Convert.ToInt32(ZNnull(GB_rd["DEBlock"], 0)) == -1)
                        //{
                        //  ErrorList += Environment.NewLine + "รหัสลูกหนี้ถูกล็อค";
                        //  OK = false;
                        //}
                    }
                    else
                    {
                        ErrorList += Environment.NewLine + "รหัสเจ้าหนี้ไม่มีกำหนด";
                        OK = false;
                    }
                }

                //if (VC_MIexactExists(drMIH["MIHtype"].ToString(),drMIH["MIHvnos"].ToString(),"",drMIH["MIHcus"].ToString(),Convert.ToInt32(ZNnull(drMIH["MIHday"],0)),Convert.ToInt32(ZNnull(drMIH["MIHmonth"],0)),Convert.ToInt32(ZNnull(drMIH["MIHyear"],0)),1))
                //{
                //  ErrorList += Environment.NewLine + "มีเลขที่ใบสำคัญนี้แล้ว";
                //  OK = false;
                //}

                vx = Convert.ToDouble(ZNnull(drMIH["MIHnetSUM"], 0));
                if (Math.Round(vx, 2) != (Math.Round(Convert.ToDouble(ZNnull(drMIH["MIHnetSUM"], 0)), 2)))
                {
                    ErrorList += Environment.NewLine + "MIH --- ผมรวมท้ายใบสำคัญผิดพลาด";
                    OK = false;
                }


                //if ((Convert.ToDecimal(ZNnull(drMIH["MIHextraSUM"], 0)) + Convert.ToDecimal(ZNnull(drMIH["MIHextraVAT"], 0))) > Convert.ToDecimal(ZNnull(drMIH["MIHnetSUM"], 0)))
                //{
                //  ErrorList += Environment.NewLine + "ยอดเงินมัดจำมากกว่ายอดสุทธิ";
                //  OK = false;
                //}

                //if (Convert.ToDecimal(ZNnull(drMIH["MIHextraVAT"], 0)) > Convert.ToDecimal(ZNnull(drMIH["MIHvatSUM"], 0)))
                //{
                //  ErrorList += Environment.NewLine + "ยอดภาษีเงินมัดจำมากกว่ายอดภาษีเงินสุทธิ";
                //  OK = false;
                //}

                if (Convert.ToDecimal(ZNnull(drMIH["MIHcurC"], 0)) > 0 && Convert.ToDecimal(ZNnull(drMIE["MIErecCQRD"], 0)) > 0)
                {
                    ErrorList += Environment.NewLine + "ไม่อนุญาตให้ใช้ระบบเช็คกับสกุลเงินต่างประเทศ";
                    OK = false;
                }

                #endregion

                if (Voucher.Tables["MIK"] == null)
                {
                    //MI.Tables.Add(Voucher.Tables["MIK"]);
                    dt = new DataTable();
                    dt = GetVoucherData("MIK", 1, "XXXXXX", Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHtype"]), "XXX", 1, 1, 2050);
                    dt.TableName = "MIK";
                    MI.Tables.Add(dt);
                }

                dt = new DataTable("MIS");
                dt = dtMIS.Copy();
                MI.Tables.Add(dt); //Save DataSet MIL

                switch (Voucher.Tables["MIH"].Rows[0]["MIHtype"].ToString())
                {
                    case "VS":
                    case "VP":
                    case "RS":
                    case "RP":
                        dt = new DataTable("MIR");
                        dt = dtMIR.Copy();
                        MI.Tables.Add(dt); //Save DataSet MIR
                        break;
                }

                //dt.ImportRow(drMIH);
                //MI.Tables.Add(dt);//Save DataSet MIH
                //MI.Tables.Add(Voucher.Tables["MIK"]);
                Voucher.Clear();
                Voucher = MI.Copy();
                if (OK)
                {
                    //if (Voucher.Tables["MIK"].Rows.Count > 0)
                    //{
                    //  MI.Tables.Add(Voucher.Tables["MIK"]);
                    //}
                    int Td = 0;
                    int Tm = 0;
                    int Ty = 0;
                    string Tno = "";
                    string Ttype = "";
                    string Tcus = "";
                    string Tsys = "";

                    switch (Voucher.Tables["MIH"].Rows[0]["MIHtype"].ToString())
                    {
                        case "VS":
                        case "VP":
                        case "RS":
                        case "RP":
                            //Td = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHday"]);
                            //Tm = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHmonth"]);
                            //Ty = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHyear"]);
                            //Tno = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHvnos"]);
                            //Ttype = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHtype"]);
                            //Tcus = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHcus"]);
                            //Tsys = "";

                            //switch (Ttype)
                            //{
                            //    case "VS":
                            //    case "RS":
                            //        Tsys = "AR";
                            //        break;
                            //    case "VP":
                            //    case "RP":
                            //        Tsys = "AP";
                            //        break;
                            //}
                            //SaveVoucherReceiveToDatabase(GB_dbcfg, Voucher, Mode, Td, Tm, Ty, Tno, Ttype, Tcus, Tsys, ref ErrorList, PrevLno);
                            break;
                        default:
                            Td = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHday"]);
                            Tm = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHmonth"]);
                            Ty = Convert.ToInt32(Voucher.Tables["MIH"].Rows[0]["MIHyear"]);
                            Tno = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHvnos"]);
                            Ttype = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHtype"]);
                            Tcus = Convert.ToString(Voucher.Tables["MIH"].Rows[0]["MIHcus"]);
                            Tsys = "";

                            switch (Ttype)
                            {
                                case "QS":
                                case "PS":
                                case "DS":
                                case "IS":
                                case "AS":
                                case "BS":
                                case "CS":
                                    Tsys = "AR";
                                    break;
                                case "QP":
                                case "PP":
                                case "DP":
                                case "IP":
                                case "AP":
                                case "BP":
                                case "CP":
                                    Tsys = "AP";
                                    break;
                                case "SS":
                                case "TS":
                                case "JX":
                                case "MX":
                                    Tsys = "IC";
                                    break;
                                case "SP":
                                case "TP":
                                case "PX":
                                    Tsys = "IC";
                                    break;
                            }
                            SaveVoucherToDatabase(Voucher, Mode, Td, Tm, Ty, Tno, Ttype, Tcus, Tsys, ref ErrorList, PrevLno, PrevSno);
                            break;
                    }
                }
                else
                {
                    OK = false;
                }
            }
            catch (Exception e)
            {
                OK = false;
                //throw new FaultException(e.Message);
            }
            finally
            {
                //OK = true;
                //if (OK)
                //{
                //  //if (Voucher.Tables["MIK"].Rows.Count > 0)
                //  //{
                //  //  MI.Tables.Add(Voucher.Tables["MIK"]);
                //  //}
                //  Voucher.Clear();
                //  Voucher = MI.Copy();

                //  Save_Voucher_MI_Final(GB_dbcfg, Voucher, ref VoucherNumber, ref ErrorList);
                //}
                ErrMessage = ErrorList;
            }

            return OK;
        }

        private static void SaveVoucherToDatabase(DataSet DSvoucher, int Mode, int VDay, int VMonth, int VYear, string VNo, string VType, string VCus, string VSys, ref string ErrVoucher, int PrevNo, int PrevSNO)
        {
            SqlConnection conn = new SqlConnection();
            SqlConnection cn = new SqlConnection();
            SqlCommand comm = new SqlCommand();
            SqlCommand cmd = new SqlCommand();

            SqlConnection Vdb = new SqlConnection();

            SqlDataReader rd = null;
            SqlTransaction tran = null;
            DataTable dt = new DataTable();
            DataTable dtMIH = new DataTable();
            DataTable dtMIE = new DataTable();
            DataTable dtMIK = new DataTable();
            DataTable dtMIL = new DataTable();
            DataTable dtMIR = new DataTable();
            DataTable dtMIS = new DataTable();

            #region " string "
            int Gday, Gmonth, Gyear;
            string Gvnos = "",Gcus = "",Gtype = "";
            string Fname = "", Xs = "";
            string Xstr;
            string SQL;
            string Xsp;
            string LKvnos, LKtype, LKcus, LKstk;
            #endregion

            #region " int "
            int i = 0, j = 0, k = 0, Fs = 0;
            int VnoList = 0, SnoSerial = 0;
            int Scut = 0, Sclude = 0;
            int NoV;
            int Uclear = 0;
            int ClrSTK = 0;
            int ClrSum = 0;
            int ClrAll = 0;
            int ClrCLR = 0;
            int LKid;
            int Isnsv = 0;
            #endregion

            #region " double "
            double Zqr = 0;
            double Zq1 = 0;
            double Zq2 = 0;
            double Squan = 0;
            #endregion

            #region " decimal "
            decimal Tx3 = 0;
            decimal Zsr = 0;
            decimal Zs1 = 0;
            decimal Zs2 = 0;
            decimal Zbc = 0;
            decimal Ssum;
            #endregion

            #region " datetime "
            DateTime Zdate, TKCdate, LKdate;
            #endregion

            #region " bool "
            bool Sok;
            #endregion

            if (DSvoucher == null) return;

            try
            {
                //ConnectionSQL(ref conn, dbcfg);
                 DBHelper.ConnectionSQL(ref conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                dtMIH = DSvoucher.Tables["MIH"];
                dtMIE = DSvoucher.Tables["MIE"];
                dtMIK = DSvoucher.Tables["MIK"];
                dtMIL = DSvoucher.Tables["MIL"];
                dtMIR = DSvoucher.Tables["MIR"];
                dtMIS = DSvoucher.Tables["MIS"];

                //Zdate = System.Convert.ToDateTime(Date_CvDMY(VDay, VMonth, VYear, false), Iformat);
                Zdate = System.Convert.ToDateTime(Date_CvDMY(VDay, VMonth, VYear, false));
                Xsp = VType.Substring(VType.Length - 1, 1);

                //Add record to the database
                #region "  Section 1  "

                #region "  MIH  "
                if (dtMIH == null) return;

                Gvnos = Convert.ToString(dtMIH.Rows[0]["MIHvnos"]);
                Gday = VDay;
                Gmonth = VMonth;
                Gyear = VYear;
                Gcus = VCus;
                Gtype = VType;

                while (RunVC_ExistsMIvnos("PP", Gvnos, Convert.ToInt32(dtMIH.Rows[0]["MIHmonth"]), Convert.ToInt32(dtMIH.Rows[0]["MIHyear"])))
                {
                    Gvnos = SequenceStr(Gvnos);
                }

                Sclude = Convert.ToInt32(dtMIH.Rows[0]["MIHincluded"]);
                VnoList = dtMIH.Rows.Count;

                if (Mode == 0 || Mode == 2)
                {
                    SQL = "INSERT INTO MIH WITH (UPDLOCK) (MIHday, MIHmonth, MIHyear, MIHtype, MIHvnos, MIHcus, MIHjob, MIHdep, MIHper, MIHdoc, MIHmec, MIHvatNO, MIHdesc,"
                          + " MIHmemo, MIHnotes, MIHref1, MIHref2, MIHref3, MIHdiscHT1, MIHdiscHT2, MIHdelvSite, MIHdelvDate, MIHstanding, MIHlinkGL, MIHnoUPLINK, MIHnolist, "
                          + " MIHcurC, MIHvatInList, MIHlock, MIHprintN, MIHcheque, MIHcancel, MIHstatus, MIHclearUM, MIHctermNO, MIHcog, MIHvatSUM, MIHnetSUM, MIHdiscLST,"
                          + " MIHdiscHF1, MIHdiscHF2, MIHexchg, MIHextraSUM, MIHextraVAT, MIHextraCUT, MIHextraMEM, MIHrecNO, MIHrecDATE, MIHsaveTAG, MIHkeyUser, MIHkeyDate,  MIHdocRunNo,"
                          + " MIHisCF, MIHisSP, MIHnumSI, MIHnumSC, MIHvatPC, MIHincluded, MIHtaxvnos, MIHtaxsum, MIHload, MIHpaidDate,  MIHbookOrderNum)"
                          + " VALUES(@MIHday, @MIHmonth, @MIHyear, @MIHtype, @MIHvnos, @MIHcus, @MIHjob, @MIHdep, @MIHper, @MIHdoc, @MIHmec, @MIHvatNO, @MIHdesc,"
                          + " @MIHmemo, @MIHnotes, @MIHref1, @MIHref2, @MIHref3, @MIHdiscHT1, @MIHdiscHT2, @MIHdelvSite, @MIHdelvDate,  @MIHstanding, @MIHlinkGL, @MIHnoUPLINK, @MIHnolist,"
                          + " @MIHcurC, @MIHvatInList, @MIHlock, @MIHprintN, @MIHcheque, @MIHcancel, @MIHstatus, @MIHclearUM, @MIHctermNO, @MIHcog, @MIHvatSUM, @MIHnetSUM, @MIHdiscLST,"
                          + " @MIHdiscHF1, @MIHdiscHF2, @MIHexchg, @MIHextraSUM, @MIHextraVAT, @MIHextraCUT, @MIHextraMEM, @MIHrecNO, @MIHrecDATE, @MIHsaveTAG, @MIHkeyUser, @MIHkeyDate, @MIHdocRunNo,"
                          + " @MIHisCF, @MIHisSP, @MIHnumSI, @MIHnumSC, @MIHvatPC, @MIHincluded, @MIHtaxvnos, @MIHtaxsum, @MIHload, @MIHpaidDate, @MIHbookOrderNum) ";
                }
                else
                {
                    SQL = "UPDATE MIH WITH (UPDLOCK) set MIHjob=@MIHjob,MIHdep=@MIHdep, MIHper=@MIHper, MIHdoc=@MIHdoc, MIHmec=@MIHmec, MIHvatNO=@MIHvatNO, MIHdesc=@MIHdesc,"
                          + " MIHmemo=@MIHmemo, MIHnotes=@MIHnotes, MIHref1=@MIHref1, MIHref2=@MIHref2, MIHref3=@MIHref3, MIHdiscHT1=@MIHdiscHT1, MIHdiscHT2=@MIHdiscHT2, MIHdelvSite=@MIHdelvSite,"
                          + " MIHdelvDate=@MIHdelvDate, MIHstanding=@MIHstanding, MIHlinkGL=@MIHlinkGL, MIHnoUPLINK=@MIHnoUPLINK, MIHnolist=@MIHnolist,"
                          + " MIHcurC=@MIHcurC, MIHvatInList=@MIHvatInList, MIHlock=@MIHlock, MIHprintN=@MIHprintN, MIHcheque=@MIHcheque, MIHcancel=@MIHcancel, MIHstatus=@MIHstatus,"
                          + " MIHclearUM=@MIHclearUM, MIHctermNO=@MIHctermNO, MIHcog=@MIHcog, MIHvatSUM=@MIHvatSUM, MIHnetSUM=@MIHnetSUM, MIHdiscLST=@MIHdiscLST,"
                          + " MIHdiscHF1=@MIHdiscHF1, MIHdiscHF2=@MIHdiscHF2, MIHexchg=@MIHexchg, MIHextraSUM=@MIHextraSUM, MIHextraVAT=@MIHextraVAT, MIHextraCUT=@MIHextraCUT,"
                          + " MIHextraMEM=@MIHextraMEM, MIHrecNO=@MIHrecNO, MIHrecDATE=@MIHrecDATE, MIHsaveTAG=@MIHsaveTAG, MIHkeyUser=@MIHkeyUser, MIHkeyDate=@MIHkeyDate,  MIHdocRunNo=@MIHdocRunNo,"
                          + " MIHisCF=@MIHisCF, MIHisSP=@MIHisSP, MIHnumSI=@MIHnumSI, MIHnumSC=@MIHnumSC, MIHvatPC=@MIHvatPC, MIHincluded=@MIHincluded, MIHtaxvnos=@MIHtaxvnos,"
                          + " MIHtaxsum=@MIHtaxsum, MIHload=@MIHload, MIHpaidDate=@MIHpaidDate,  MIHbookOrderNum=@MIHbookOrderNum"
                          + " Where MIHday=@MIHday and MIHmonth=@MIHmonth and MIHyear=@MIHyear and MIHtype=@MIHtype and MIHvnos=@MIHvnos and MIHcus=@MIHcus";
                }
                comm = new SqlCommand(SQL, conn);
                comm.CommandText = SQL;
                comm.CommandTimeout = 30;
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@MIHday", SqlDbType.SmallInt).Value = Gday;
                comm.Parameters.Add("@MIHmonth", SqlDbType.SmallInt).Value = Gmonth;
                comm.Parameters.Add("@MIHyear", SqlDbType.SmallInt).Value = Gyear;
                comm.Parameters.Add("@MIHtype", SqlDbType.NVarChar, 2).Value = Gtype;
                comm.Parameters.Add("@MIHvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                comm.Parameters.Add("@MIHcus", SqlDbType.NVarChar, 15).Value = Gcus;
                comm.Parameters.Add("@MIHjob", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHjob"]);
                comm.Parameters.Add("@MIHdep", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHdep"]);
                comm.Parameters.Add("@MIHper", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHper"]);
                comm.Parameters.Add("@MIHdoc", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHdoc"]);
                comm.Parameters.Add("@MIHmec", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHmec"]);
                comm.Parameters.Add("@MIHvatNO", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHvatNO"]);
                if ((ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MaxValue))
                {
                    comm.Parameters.Add("@MIHrecDATE", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    comm.Parameters.Add("@MIHrecDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIH.Rows[0]["MIHrecDATE"], Iformat);
                }
                comm.Parameters.Add("@MIHdesc", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdesc"]);
                comm.Parameters.Add("@MIHmemo", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHmemo"]);
                comm.Parameters.Add("@MIHnotes", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHnotes"]);
                comm.Parameters.Add("@MIHref1", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHref1"]);
                comm.Parameters.Add("@MIHref2", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHref2"]);
                comm.Parameters.Add("@MIHref3", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHref3"]);
                comm.Parameters.Add("@MIHdiscHT1", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdiscHT1"]);
                comm.Parameters.Add("@MIHdiscHT2", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdiscHT2"]);
                comm.Parameters.Add("@MIHdelvSite", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdelvSite"]);
                if ((ZDnulls(dtMIH.Rows[0]["MIHdelvDate"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHdelvDate"]) == DateTime.MaxValue))
                {
                    comm.Parameters.Add("@MIHdelvDate", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    comm.Parameters.Add("@MIHdelvDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIH.Rows[0]["MIHdelvDate"], Iformat);
                }
                comm.Parameters.Add("@MIHstanding", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHstanding"]);
                comm.Parameters.Add("@MIHlinkGL", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHlinkGL"]);
                comm.Parameters.Add("@MIHnoUPLINK", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHnoUPLINK"]);
                VnoList = dtMIL.Rows.Count;
                //comm.Parameters.Add("@MIHnolist", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHnolist"]); 
                comm.Parameters.Add("@MIHnolist", SqlDbType.SmallInt).Value = VnoList;
                comm.Parameters.Add("@MIHcurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                comm.Parameters.Add("@MIHvatInList", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHvatInList"]);
                comm.Parameters.Add("@MIHlock", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHlock"]);
                comm.Parameters.Add("@MIHprintN", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHprintN"]);
                comm.Parameters.Add("@MIHcheque", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcheque"]);
                comm.Parameters.Add("@MIHcancel", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcancel"]);
                comm.Parameters.Add("@MIHstatus", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHstatus"]);
                comm.Parameters.Add("@MIHclearUM", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHclearUM"]);
                comm.Parameters.Add("@MIHctermNO", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHctermNO"]);
                comm.Parameters.Add("@MIHcog", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHcog"]);
                comm.Parameters.Add("@MIHvatSUM", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHvatSUM"]);
                comm.Parameters.Add("@MIHnetSUM", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                comm.Parameters.Add("@MIHdiscLST", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscLST"]);
                comm.Parameters.Add("@MIHdiscHF1", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscHF1"]);
                comm.Parameters.Add("@MIHdiscHF2", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscHF2"]);
                comm.Parameters.Add("@MIHexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIH.Rows[0]["MIHexchg"]);
                comm.Parameters.Add("@MIHextraSUM", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHextraSUM"]);
                comm.Parameters.Add("@MIHextraVAT", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHextraVAT"]);
                comm.Parameters.Add("@MIHextraCUT", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHextraCUT"]);
                comm.Parameters.Add("@MIHextraMEM", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHextraMEM"]);
                comm.Parameters.Add("@MIHrecNO", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHrecNO"]);
                comm.Parameters.Add("@MIHsaveTAG", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHsaveTAG"]);
                comm.Parameters.Add("@MIHkeyUser", SqlDbType.NVarChar, 10).Value = Convert.ToString(dtMIH.Rows[0]["MIHkeyUser"]);
                //if ((ZDnulls(dtMIH.Rows[0]["MIHkeyDate"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHkeyDate"]) == DateTime.MaxValue))
                //{
                //    comm.Parameters.Add("@MIHkeyDate", SqlDbType.DateTime).Value = DBNull.Value;
                //}
                //else
                //{
                //    comm.Parameters.Add("@MIHkeyDate", SqlDbType.DateTime).Value = Convert.ToDateTime(ZDnulls(dtMIH.Rows[0]["MIHkeyDate"]), Iformat);
                //}
                comm.Parameters.Add("@MIHkeyDate", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@MIHdocRunNo", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdocRunNo"]);

                if (Convert.ToString(dtMIE.Rows[0]["MIEctermCX"]).Length == 0)
                {
                    comm.Parameters.Add("@MIHduePO", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    if ((ZDnulls(dtMIH.Rows[0]["MIHduePO"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHduePO"]) == DateTime.MaxValue))
                    {
                        comm.Parameters.Add("@MIHduePO", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        comm.Parameters.Add("@MIHduePO", SqlDbType.DateTime).Value = ZDnulls(dtMIH.Rows[0]["MIHduePO"]);
                    }
                }
                comm.Parameters.Add("@MIHisCF", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHisCF"]);
                comm.Parameters.Add("@MIHisSP", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHisSP"]);
                comm.Parameters.Add("@MIHnumSI", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHnumSI"]);
                comm.Parameters.Add("@MIHnumSC", SqlDbType.Money).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHnumSC"]);
                Tx3 = 0;
                if (Convert.ToInt32(dtMIH.Rows[0]["MIHvatPC"]) == 0)
                {
                    Tx3 = 0;
                    Tx3 = Convert.ToDecimal(dtMIH.Rows[0]["MIHcog"]) - Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscLST"]) - Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscHF1"]) - Convert.ToDecimal(dtMIH.Rows[0]["MIHdiscHF2"]);
                    if (Tx3 > 0) dtMIH.Rows[0]["MIHvatPC"] = Math.Round((Convert.ToDecimal(dtMIH.Rows[0]["MIHvatSUM"]) * 100) / Tx3,2);
                }
                comm.Parameters.Add("@MIHvatPC", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHvatPC"]);
                comm.Parameters.Add("@MIHincluded", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHincluded"]);
                comm.Parameters.Add("@MIHtaxvnos", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHtaxvnos"]);
                comm.Parameters.Add("@MIHtaxsum", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHtaxsum"]);
                comm.Parameters.Add("@MIHload", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHload"]);
                comm.Parameters.Add("@MIHpaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now, Iformat);
                comm.Parameters.Add("@MIHbookOrderNum", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHbookOrderNum"]);

                comm.Transaction = tran;
                comm.ExecuteNonQuery();
                #endregion

                #region "  MIE  "
                if (dtMIE == null) return;

                if (Mode == 0 || Mode == 2)
                {
                    SQL = "INSERT INTO MIE WITH (UPDLOCK) (MIEday, MIEmonth, MIEyear, MIEtype, MIEvnos, MIEcus, MIErecDT3, MIErecDT32, MIErecDT53, MIErecDT532,"
                          + " MIErecDRND, MIErecDEXG, MIErecDCSH, MIErecDOTH, MIErecPLUS, MIErecCASH, MIErecCQRD, MIEctermCX, MIEctermPX, MIEctermAX, MIEctermDX,"
                          + " MIEtag, MIEbaseDT3, MIEbaseDT32, MIEbaseDT53, MIEbaseDT532, MIEdescDT3, MIEdescDT32, MIEdescDT53, MIEdescDT532, MIEconDT3, MIEconDT32, MIEconDT53, MIEconDT532, MIErecFC)"
                          + " VALUES(@MIEday, @MIEmonth, @MIEyear, @MIEtype, @MIEvnos, @MIEcus, @MIErecDT3, @MIErecDT32, @MIErecDT53, @MIErecDT532,"
                          + " @MIErecDRND, @MIErecDEXG, @MIErecDCSH, @MIErecDOTH, @MIErecPLUS, @MIErecCASH, @MIErecCQRD, @MIEctermCX, @MIEctermPX, @MIEctermAX, @MIEctermDX,"
                          + " @MIEtag, @MIEbaseDT3, @MIEbaseDT32, @MIEbaseDT53, @MIEbaseDT532, @MIEdescDT3, @MIEdescDT32, @MIEdescDT53, @MIEdescDT532, @MIEconDT3, @MIEconDT32, @MIEconDT53, @MIEconDT532, @MIErecFC)";
                }
                else
                {
                    SQL = "UPDATE MIE WITH (UPDLOCK) set MIErecDT3=@MIErecDT3, MIErecDT32=@MIErecDT32, MIErecDT53=@MIErecDT53, MIErecDT532=@MIErecDT532,"
                          + " MIErecDRND=@MIErecDRND, MIErecDEXG=@MIErecDEXG, MIErecDCSH=@MIErecDCSH, MIErecDOTH=@MIErecDOTH, MIErecPLUS=@MIErecPLUS, MIErecCASH=@MIErecCASH,"
                          + " MIErecCQRD=@MIErecCQRD, MIEctermCX=@MIEctermCX, MIEctermPX=@MIEctermPX, MIEctermAX=@MIEctermAX, MIEctermDX=@MIEctermDX,"
                          + " MIEtag=@MIEtag, MIEbaseDT3=@MIEbaseDT3, MIEbaseDT32=@MIEbaseDT32, MIEbaseDT53=@MIEbaseDT53, MIEbaseDT532=@MIEbaseDT532, MIEdescDT3=@MIEdescDT3, "
                          + " MIEdescDT32=@MIEdescDT32, MIEdescDT53=@MIEdescDT53, MIEdescDT532=@MIEdescDT532, MIEconDT3=@MIEconDT3, MIEconDT32=@MIEconDT32, MIEconDT53=@MIEconDT53, MIEconDT532=@MIEconDT532, MIErecFC=@MIErecFC"
                          + " Where MIEday=@MIEday and MIEmonth=@MIEmonth and MIEyear=@MIEyear and MIEtype=@MIEtype and MIEvnos=@MIEvnos and MIEcus=@MIEcus";
                }
                comm = new SqlCommand(SQL, conn);
                comm.CommandText = SQL;
                comm.CommandTimeout = 30;
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@MIEday", SqlDbType.SmallInt).Value = Gday;
                comm.Parameters.Add("@MIEmonth", SqlDbType.SmallInt).Value = Gmonth;
                comm.Parameters.Add("@MIEyear", SqlDbType.SmallInt).Value = Gyear;
                comm.Parameters.Add("@MIEtype", SqlDbType.NVarChar, 2).Value = Gtype;
                comm.Parameters.Add("@MIEvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                comm.Parameters.Add("@MIEcus", SqlDbType.NVarChar, 15).Value = Gcus;
                comm.Parameters.Add("@MIErecDT3", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDT3"]);
                comm.Parameters.Add("@MIErecDT32", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDT32"]);
                comm.Parameters.Add("@MIErecDT53", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDT53"]);
                comm.Parameters.Add("@MIErecDT532", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDT532"]);
                comm.Parameters.Add("@MIErecDRND", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDRND"]);
                comm.Parameters.Add("@MIErecDEXG", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDEXG"]);
                comm.Parameters.Add("@MIErecDCSH", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDCSH"]);
                comm.Parameters.Add("@MIErecDOTH", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecDOTH"]);
                comm.Parameters.Add("@MIErecPLUS", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecPLUS"]);
                comm.Parameters.Add("@MIErecCASH", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecCASH"]);
                comm.Parameters.Add("@MIErecCQRD", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIErecCQRD"]);
                comm.Parameters.Add("@MIEctermCX", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEctermCX"]);
                comm.Parameters.Add("@MIEctermPX", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEctermPX"]);
                comm.Parameters.Add("@MIEctermAX", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEctermAX"]);
                comm.Parameters.Add("@MIEctermDX", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEctermDX"]);
                comm.Parameters.Add("@MIEtag", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIE.Rows[0]["MIEtag"]);
                comm.Parameters.Add("@MIEbaseDT3", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIEbaseDT3"]);
                comm.Parameters.Add("@MIEbaseDT32", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIEbaseDT32"]);
                comm.Parameters.Add("@MIEbaseDT53", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIEbaseDT53"]);
                comm.Parameters.Add("@MIEbaseDT532", SqlDbType.Money).Value = Convert.ToDecimal(dtMIE.Rows[0]["MIEbaseDT532"]);
                comm.Parameters.Add("@MIEdescDT3", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEdescDT3"]);
                comm.Parameters.Add("@MIEdescDT32", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEdescDT32"]);
                comm.Parameters.Add("@MIEdescDT53", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEdescDT53"]);
                comm.Parameters.Add("@MIEdescDT532", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIEdescDT532"]);
                comm.Parameters.Add("@MIEconDT3", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIE.Rows[0]["MIEconDT3"]);
                comm.Parameters.Add("@MIEconDT32", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIE.Rows[0]["MIEconDT32"]);
                comm.Parameters.Add("@MIEconDT53", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIE.Rows[0]["MIEconDT53"]);
                comm.Parameters.Add("@MIEconDT532", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIE.Rows[0]["MIEconDT532"]);
                comm.Parameters.Add("@MIErecFC", SqlDbType.NText).Value = Convert.ToString(dtMIE.Rows[0]["MIErecFC"]);
                comm.Transaction = tran;
                comm.ExecuteNonQuery();
                #endregion

                #region "  MIK  "
                if (Convert.ToString(dtMIH.Rows[0]["MIHcus"]) == "WALKIN")
                {
                    if (Mode == 0 || Mode == 2)
                    {
                        SQL = "INSERT INTO MIK WITH (UPDLOCK) (MIKday, MIKmonth, MIKyear, MIKtype, MIKvnos, MIKcname, MIKcaddr, MIKctel, MIKtag)"
                              + " VALUES(@MIKday, MIKmonth, MIKyear, MIKtype, MIKvnos, MIKcname, MIKcaddr, MIKctel, MIKtag)";
                    }
                    else
                    {
                        SQL = "UPDATE MIK WITH (UPDLOCK) set MIKcname=@MIKcname, MIKcaddr=@MIKcaddr, MIKctel=@MIKctel, MIKtag=@MIKtag"
                              + " Where MIKday=@MIKday and MIKday=@MIKday and MIKmonth=@MIKmonth and MIKyear=@MIKyear and MIKvnos=@MIKvnos and MIKtype=@MIKtype";
                    }

                    comm = new SqlCommand(SQL, conn);
                    comm.CommandText = SQL;
                    comm.CommandTimeout = 30;
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@MIKday", SqlDbType.SmallInt).Value = Gday;
                    comm.Parameters.Add("@MIKmonth", SqlDbType.SmallInt).Value = Gmonth;
                    comm.Parameters.Add("@MIKyear", SqlDbType.SmallInt).Value = Gyear;
                    comm.Parameters.Add("@MIKtype", SqlDbType.NVarChar, 2).Value = Gtype;
                    comm.Parameters.Add("@MIKvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                    comm.Parameters.Add("@MIKcname", SqlDbType.NText).Value = Convert.ToString(dtMIK.Rows[0]["MIKcname"]);
                    comm.Parameters.Add("@MIKcaddr", SqlDbType.NText).Value = Convert.ToString(dtMIK.Rows[0]["MIKcaddr"]);
                    comm.Parameters.Add("@MIKctel", SqlDbType.NText).Value = Convert.ToString(dtMIK.Rows[0]["MIKctel"]);
                    comm.Parameters.Add("@MIKtag", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIK.Rows[0]["Str_MIK.MIKtag"]);
                    comm.Transaction = tran;
                    comm.ExecuteNonQuery();
                }
                #endregion

                #region "  MIL  "
                if (dtMIL == null) return;
                VnoList = dtMIL.Rows.Count;
                if (VnoList > 0)
                {
                    for (i = 0; i < VnoList; i++)
                    {

                        int sMode = Mode;

                        if (Mode == 1)
                        {
                            if (i >= PrevNo)
                            {
                                sMode = 0;
                            }
                        }

                        if (sMode == 0 || sMode == 2)
                        {
                            SQL = "INSERT INTO MIL WITH (UPDLOCK) (MILday, MILmonth, MILyear, MILtype, MILvnos, MILcus, MILstk, MILjob, MILdep, MILper, MILdoc, MILmec, MILsto, MILstoMT, MILlistNo,"
                                  + " MILquan, MILquanP2, MILadisc, MILcog, MILconv, MILdiscA, MILdiscT, MILvat, MILsum, MILcut, MILstype, MILuname, MILdesc, MILvCol1, MILvCol2,"
                                  + " MILvCol3, MILvCol4, MILcurC, MILexchg, MILacost, MILpqm, MILpmFrom, MILpmTo, MILtag, MILlinkVCno,MILlinkVCdate,MILlinkVCtype, MILlinkVCdeposit, MILlinkVCid, MILclearUM, MILnotes,"
                                  + " MILnotKL, MILdelvDT, MILnumCSP, MILnumISP, MILcost, MILucost, MILuprice, MILuseQuanP)"
                                  + " VALUES(@MILday, @MILmonth, @MILyear, @MILtype, @MILvnos, @MILcus, @MILstk, @MILjob, @MILdep, @MILper, @MILdoc, @MILmec, @MILsto, @MILstoMT, @MILlistNo,"
                                  + " @MILquan, @MILquanP2, @MILadisc, @MILcog, @MILconv, @MILdiscA, @MILdiscT, @MILvat, @MILsum, @MILcut, @MILstype, @MILuname, @MILdesc, @MILvCol1, @MILvCol2,"
                                  + " @MILvCol3, @MILvCol4, @MILcurC, @MILexchg, @MILacost, @MILpqm, @MILpmFrom, @MILpmTo, @MILtag, @MILlinkVCno,@MILlinkVCdate, @MILlinkVCtype, @MILlinkVCdeposit, @MILlinkVCid, @MILclearUM, @MILnotes,"
                                  + " @MILnotKL, @MILdelvDT,  @MILnumCSP, @MILnumISP, @MILcost, @MILucost, @MILuprice, @MILuseQuanP)";
                        }
                        else
                        {
                            SQL = "UPDATE MIL WITH (UPDLOCK) Set MILstk=@MILstk, MILjob=@MILjob, MILdep=@MILdep, MILper=@MILper, MILdoc=@MILdoc, MILmec=@MILmec, MILsto=@MILsto, MILstoMT=@MILstoMT, MILlistNo=@MILlistNo,"
                                  + " MILquan=@MILquan, MILquanP2=@MILquanP2, MILadisc=@MILadisc, MILcog=@MILcog, MILconv=@MILconv, MILdiscA=@MILdiscA, MILdiscT=@MILdiscT, MILvat=@MILvat, MILsum=@MILsum, MILcut=@MILcut,"
                                  + " MILstype=@MILstype, MILuname=@MILuname, MILdesc=@MILdesc, MILvCol1=@MILvCol1, MILvCol2=@MILvCol2, MILvCol3=@MILvCol3, MILvCol4=@MILvCol4, MILcurC=@MILcurC, MILexchg=@MILexchg, MILacost=@MILacost,"
                                  + " MILpqm=@MILpqm, MILpmFrom=@MILpmFrom, MILpmTo=@MILpmTo, MILtag=@MILtag, MILlinkVCno=@MILlinkVCno,  MILlinkVCdate=@MILlinkVCdate, MILlinkVCtype=@MILlinkVCtype, MILlinkVCdeposit=@MILlinkVCdeposit, MILlinkVCid=@MILlinkVCid,"
                                  + " MILclearUM=@MILclearUM, MILnotes=@MILnotes, MILnotKL=@MILnotKL, MILdelvDT=@MILdelvDT, MILnumCSP=@MILnumCSP, MILnumISP=@MILnumISP, MILcost=@MILcost, MILucost=@MILucost, MILuprice=@MILuprice, MILuseQuanP=@MILuseQuanP"
                                  + " Where MILday=@MILday and MILmonth=@MILmonth and MILyear=@MILyear and MILtype=@MILtype and MILvnos=@MILvnos and MILcus=@MILcus and MILlistNo=@MILlistNo";
                        }
                        comm = new SqlCommand(SQL, conn);
                        comm.CommandText = SQL;
                        comm.CommandTimeout = 30;
                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@MILday", SqlDbType.SmallInt).Value = Gday;
                        comm.Parameters.Add("@MILmonth", SqlDbType.SmallInt).Value = Gmonth;
                        comm.Parameters.Add("@MILyear", SqlDbType.SmallInt).Value = Gyear;
                        comm.Parameters.Add("@MILtype", SqlDbType.NVarChar, 2).Value = Gtype;
                        comm.Parameters.Add("@MILvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                        comm.Parameters.Add("@MILcus", SqlDbType.NVarChar, 15).Value = Gcus;
                        comm.Parameters.Add("@MILstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                        comm.Parameters.Add("@MILjob", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILjob"]);
                        comm.Parameters.Add("@MILdep", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILdep"]);
                        comm.Parameters.Add("@MILper", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILper"]);
                        comm.Parameters.Add("@MILdoc", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILdoc"]);
                        comm.Parameters.Add("@MILmec", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILmec"]);
                        comm.Parameters.Add("@MILsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                        comm.Parameters.Add("@MILstoMT", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILstoMT"]);
                        comm.Parameters.Add("@MILlistNo", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlistNo"]);
                        comm.Parameters.Add("@MILquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                        comm.Parameters.Add("@MILquanP2", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]);
                        comm.Parameters.Add("@MILadisc", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]);
                        comm.Parameters.Add("@MILcog", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]);
                        comm.Parameters.Add("@MILconv", SqlDbType.Real).Value = Convert.ToDouble(dtMIL.Rows[i]["MILconv"]);
                        comm.Parameters.Add("@MILdiscA", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]);
                        comm.Parameters.Add("@MILdiscT", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdiscT"]);
                        comm.Parameters.Add("@MILvat", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILvat"]);
                        comm.Parameters.Add("@MILsum", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILsum"]);
                        comm.Parameters.Add("@MILcut", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILcut"]);
                        comm.Parameters.Add("@MILstype", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILstype"]);
                        comm.Parameters.Add("@MILuname", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILuname"]);
                        comm.Parameters.Add("@MILdesc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdesc"]);
                        comm.Parameters.Add("@MILvCol1", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILvCol1"]);
                        comm.Parameters.Add("@MILvCol2", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILvCol2"]);
                        comm.Parameters.Add("@MILvCol3", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILvCol3"]);
                        comm.Parameters.Add("@MILvCol4", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILvCol4"]);
                        comm.Parameters.Add("@MILcurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILcurC"]);
                        comm.Parameters.Add("@MILexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILexchg"]);
                        comm.Parameters.Add("@MILacost", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILacost"]);
                        comm.Parameters.Add("@MILpqm", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILpqm"]);
                        comm.Parameters.Add("@MILpmFrom", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILpmFrom"]);
                        comm.Parameters.Add("@MILpmTo", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILpmTo"]);
                        comm.Parameters.Add("@MILtag", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILtag"]);
                        comm.Parameters.Add("@MILlinkVCno", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]);
                        //if ((ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]) == DateTime.MinValue) || (ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]) == DateTime.MaxValue))
                        //{
                        //    comm.Parameters.Add("@MILlinkVCdate", SqlDbType.DateTime).Value = DBNull.Value;
                        //}
                        //else
                        //{
                        //    comm.Parameters.Add("@MILlinkVCdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIL.Rows[i]["MILlinkVCdate"], Iformat);
                        //}
                        comm.Parameters.Add("@MILlinkVCdate", SqlDbType.DateTime).Value = DBNull.Value;
                        comm.Parameters.Add("@MILlinkVCtype", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                        comm.Parameters.Add("@MILlinkVCdeposit", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCdeposit"]);
                        comm.Parameters.Add("@MILlinkVCid", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                        comm.Parameters.Add("@MILclearUM", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILclearUM"]);
                        comm.Parameters.Add("@MILnotes", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILnotes"]);
                        comm.Parameters.Add("@MILnotKL", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILnotKL"]);
                        if ((ZDnulls(dtMIH.Rows[0]["MIHdelvDate"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHdelvDate"]) == DateTime.MaxValue))
                        {
                            comm.Parameters.Add("@MILdelvDT", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            comm.Parameters.Add("@MILdelvDT", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIH.Rows[0]["MIHdelvDate"], Iformat);
                        }
                        comm.Parameters.Add("@MILnumCSP", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILnumCSP"]);
                        comm.Parameters.Add("@MILnumISP", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILnumISP"]);
                        comm.Parameters.Add("@MILcost", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILcost"]);
                        comm.Parameters.Add("@MILucost", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILucost"]);
                        comm.Parameters.Add("@MILuprice", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILuprice"]);
                        comm.Parameters.Add("@MILuseQuanP", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILuseQuanP"]);
                        comm.Transaction = tran;
                        comm.ExecuteNonQuery();
                    }
                }
                #endregion

                #region "  MIS  "
                //if (dtMIS == null) return;

                SnoSerial = dtMIS.Rows.Count;

                if (SnoSerial > 0)
                {
                    for (i = 0; i < SnoSerial; i++)
                    {
                        int sMode = Mode;

                        if (Mode == 1)
                        {
                            if (i >= PrevSNO)
                            {
                                sMode = 0;
                            }
                        }

                        if (sMode == 0 || sMode == 2)
                        {
                            SQL = "INSERT INTO MIS WITH (UPDLOCK) (MISday,MISmonth,MISyear,MIStype,MISvnos,MIScus,MISstk,MISserialM,MISserialS,MISnotes,"
                                + "MISquan,MISsnsv,MISdep,MISjob,MISsto,MISper,MISmec,MIScostSU,MIScurC,MISexchg,MISio,MISline,MISstoMT,MISmfgDATE,MISexpDATE,MIStag)"
                                + " VALUES(@MISday,@MISmonth,@MISyear,@MIStype,@MISvnos,@MIScus,@MISstk,@MISserialM,@MISserialS,@MISnotes,"
                                + "@MISquan,@MISsnsv,@MISdep,@MISjob,@MISsto,@MISper,@MISmec,@MIScostSU,@MIScurC,@MISexchg,@MISio,@MISline,@MISstoMT,@MISmfgDATE,@MISexpDATE,@MIStag)";
                        }
                        else
                        {
                            SQL = "UPDATE MIS WITH (UPDLOCK) Set MISstk=@MISstk,MISserialM=@MISserialM,MISserialS=@MISserialS,MISnotes=@MISnotes,"
                                + "MISquan=@MISquan,MISsnsv=@MISsnsv,MISdep=@MISdep,MISjob=@MISjob,MISsto=@MISsto,MISper=@MISper,MISmec=@MISmec,"
                                + "MIScostSU=@MIScostSU,MIScurC=@MIScurC,MISexchg=@MISexchg,MISio=@MISio,MISline=@MISline,MISstoMT=@MISstoMT,MISmfgDATE=@MISmfgDATE,MISexpDATE=@MISexpDATE,MIStag=@MIStag"
                                + " where MISday=@MISday and MISmonth=@MISmonth and MISyear=@MISyear and MIStype=@MIStype and MISvnos=@MISvnos and MIScus=@MIScus and MISstk=@MISstk and MISserialM=@MISserialM and MISline=@MISline";
                        }

                        comm = new SqlCommand(SQL, conn);
                        comm.CommandText = SQL;
                        comm.CommandTimeout = 30;
                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@MISday", SqlDbType.SmallInt).Value = Gday;
                        comm.Parameters.Add("@MISmonth", SqlDbType.SmallInt).Value = Gmonth;
                        comm.Parameters.Add("@MISyear", SqlDbType.SmallInt).Value = Gyear;
                        comm.Parameters.Add("@MIStype", SqlDbType.NVarChar, 2).Value = Gtype;
                        comm.Parameters.Add("@MISvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                        comm.Parameters.Add("@MIScus", SqlDbType.NVarChar, 15).Value = Gcus;
                        comm.Parameters.Add("@MISstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIS.Rows[i]["MISstk"]);
                        comm.Parameters.Add("@MISserialM", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialM"]);
                        comm.Parameters.Add("@MISserialS", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialS"]);
                        comm.Parameters.Add("@MISnotes", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISnotes"]);
                        comm.Parameters.Add("@MISquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISquan"]);
                        comm.Parameters.Add("@MISsnsv", SqlDbType.SmallInt).Value = Convert.ToDouble(dtMIS.Rows[i]["MISsnsv"]);
                        comm.Parameters.Add("@MISdep", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISdep"]);
                        comm.Parameters.Add("@MISjob", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISjob"]);
                        comm.Parameters.Add("@MISper", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISper"]);
                        comm.Parameters.Add("@MISmec", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISmec"]);
                        comm.Parameters.Add("@MIScostSU", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MIScostSU"]);
                        comm.Parameters.Add("@MIScurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIS.Rows[i]["MIScurC"]);
                        comm.Parameters.Add("@MISexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISexchg"]);
                        comm.Parameters.Add("@MISio", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIS.Rows[i]["MISio"]);
                        comm.Parameters.Add("@MISline", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIS.Rows[i]["MISline"]);
                        comm.Parameters.Add("@MISstoMT", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIS.Rows[i]["MISstoMT"]);
                        if ((ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MaxValue))
                        {
                            comm.Parameters.Add("@MISmfgDATE", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            comm.Parameters.Add("@MISmfgDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISmfgDATE"], Iformat);
                        }
                        if ((ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MaxValue))
                        {
                            comm.Parameters.Add("@MISexpDATE", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            comm.Parameters.Add("@MISexpDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISexpDATE"], Iformat);
                        }
                        comm.Parameters.Add("@MIStag", SqlDbType.SmallInt).Value = 0;
                        comm.Transaction = tran;
                        comm.ExecuteNonQuery();
                    }
                }
                #endregion

                #region "  VMP  "
                //Add VMP for Conut Voucher
                switch (VType)
                {
                    case "QP":
                        Xs = "VMAPquo";
                        break;
                    case "QS":
                        Xs = "VMARquo";
                        break;
                    case "PP":
                        Xs = "VMAPpo";
                        break;
                    case "PS":
                        Xs = "VMARpo";
                        break;
                    case "DP":
                        Xs = "VMAPdel";
                        break;
                    case "DS":
                        Xs = "VMARdel";
                        break;
                    case "IP":
                        Xs = "VMAPinv";
                        break;
                    case "IS":
                        Xs = "VMARinv";
                        break;
                    case "AP":
                        Xs = "VMAPdrn";
                        break;
                    case "AS":
                        Xs = "VMARdrn";
                        break;
                    case "BP":
                        Xs = "VMAPcrn";
                        break;
                    case "BS":
                        Xs = "VMARcrn";
                        break;
                    case "CP":
                        Xs = "VMAPcash";
                        break;
                    case "CS":
                        Xs = "VMARcash";
                        break;
                    case "SS":
                        Xs = "VMICgout";
                        break;
                    case "SP":
                        Xs = "VMICgin";
                        break;
                    case "TS":
                        Xs = "VMICgrf";
                        break;
                    case "TP":
                        Xs = "VMICgrt";
                        break;
                    case "JX":
                        Xs = "VMICjour";
                        break;
                    case "MX":
                        Xs = "VMICmstr";
                        break;
                    case "PX":
                        Xs = "VMICpr";
                        break;
                }

                int Iv = 0;
                SQL = "Select Count(VMmonth) From VMP Where VMmonth=@VMmonth And VMyear=@VMyear";
                //ConnectionSQL(ref cn, dbcfg);
                cn =  DBHelper.SqlConnectionDbMAC5();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand(SQL, cn);
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                //cmd.Transaction = tran;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@VMmonth", SqlDbType.Int).Value = Gmonth;
                cmd.Parameters.Add("@VMyear", SqlDbType.Int).Value = Gyear;
                Iv = System.Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
                if (Iv == 0)
                {
                    SQL = "INSERT INTO VMP WITH (UPDLOCK) (VMmonth, VMyear, " + Xs + ", VM" + VSys + "total)"
                      + " VALUES(@VMmonth, @VMyear, @" + Xs + ", @VM" + VSys + "total)";

                    comm = new SqlCommand(SQL, conn);
                    comm.CommandText = SQL;
                    comm.CommandTimeout = 30;
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@VMmonth", SqlDbType.Int).Value = Gmonth;
                    comm.Parameters.Add("@VMyear", SqlDbType.Int).Value = Gyear;
                    comm.Parameters.Add("@" + Xs + "", SqlDbType.Int).Value = 1;
                    comm.Parameters.Add("@VM" + VSys + "total", SqlDbType.Int).Value = 1;
                    comm.Transaction = tran;
                    comm.ExecuteNonQuery();
                }
                else
                {
                    if (Mode == 0)
                    {
                        SQL = "UPDATE VMP WITH(UPDLOCK) set ";
                        SQL += " " + Xs + " = " + Xs + " + @" + Xs + "";
                        SQL += " ,VM" + VSys + "total = VM" + VSys + "total + @VM" + VSys + "total";
                        SQL += " WHERE VMmonth = @VMmonth and VMyear = @VMyear";

                        comm = new SqlCommand(SQL, conn);
                        comm.CommandText = SQL;
                        comm.CommandTimeout = 30;
                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@VMmonth", SqlDbType.Int).Value = Gmonth;
                        comm.Parameters.Add("@VMyear", SqlDbType.Int).Value = Gyear;
                        comm.Parameters.Add("@" + Xs + "", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@VM" + VSys + "total", SqlDbType.Int).Value = 1;
                        comm.Transaction = tran;
                        comm.ExecuteNonQuery();
                    }
                }
                #endregion

                #endregion

                #region "  Section 2  "
                #region "  PX  "
                if (VType == "PX")
                {
                    if (VnoList > 0)
                    {


                        for (i = 0; i < VnoList; i++)
                        {
                            int sMode = Mode;

                            if (Mode == 1)
                            {
                                if (i >= PrevNo)
                                {
                                    sMode = 0;
                                }
                            }

                            if (sMode == 0 || sMode == 2)
                            {
                                SQL = "INSERT INTO CPX WITH (UPDLOCK) (CPXstkID, CPXlinkID, CPXvnosID, CPXdateID, CPXjob, CPXdep, CPXper, CPXdoc, CPXmec, CPXsto, CPXstkREQ, CPXstkCUT, CPXclearSTK, CPXclearUSER, CPXclearALL, CPXstatus)"
                                    + " VALUES(@CPXstkID, @CPXlinkID, @CPXvnosID, @CPXdateID, @CPXjob, @CPXdep, @CPXper, @CPXdoc, @CPXmec, @CPXsto, @CPXstkREQ, @CPXstkCUT, @CPXclearSTK, @CPXclearUSER, @CPXclearALL, @CPXstatus)";
                            }
                            else
                            {
                                SQL = "UPDATE CPX WITH (UPDLOCK) Set CPXjob=@CPXjob, CPXdep=@CPXdep, CPXper=@CPXper, CPXdoc=@CPXdoc, CPXmec=@CPXmec, CPXsto=@CPXsto, CPXstkREQ=@CPXstkREQ, CPXstkCUT=@CPXstkCUT, CPXclearSTK=@CPXclearSTK, CPXclearUSER=@CPXclearUSER, CPXclearALL=@CPXclearALL, CPXstatus=@CPXstatus"
                                    + " where CPXstkID=@CPXstkID and  CPXlinkID=@CPXlinkID and  CPXvnosID=@CPXvnosID and  CPXdateID=@CPXdateID";
                            }

                            //find down-link items
                            ClrSTK = 0;
                            ClrSum = 0;
                            ClrAll = 0;
                            //Str_MIL.MILyear = Str_MIL.MILyear + 543;

                            comm = new SqlCommand(SQL, conn);
                            comm.CommandText = SQL;
                            comm.CommandTimeout = 30;
                            comm.CommandType = CommandType.Text;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@CPXstkID", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            comm.Parameters.Add("@CPXlinkID", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                            comm.Parameters.Add("@CPXvnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                            comm.Parameters.Add("@CPXdateID", SqlDbType.DateTime).Value = Zdate;
                            comm.Parameters.Add("@CPXjob", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILjob"]);
                            comm.Parameters.Add("@CPXdep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdep"]);
                            comm.Parameters.Add("@CPXper", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILper"]);
                            comm.Parameters.Add("@CPXdoc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdoc"]);
                            comm.Parameters.Add("@CPXmec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILmec"]);
                            comm.Parameters.Add("@CPXsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                            comm.Parameters.Add("@CPXstkREQ", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                            comm.Parameters.Add("@CPXstatus", SqlDbType.SmallInt).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILacost"]);
                            ClrSTK = 0;
                            if (Convert.ToInt32(dtMIL.Rows[i]["MILcut"]) == 0) ClrSTK = -1;
                            ClrAll = 0;
                            if (Convert.ToInt32(dtMIL.Rows[i]["MILclearUM"]) != 0) ClrAll = -1;

                            Zqr = Squan;
                            Zq1 = 0;
                            Zq2 = 0;
                            Zsr = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILvat"]);
                            Zs1 = 0;
                            Zs2 = 0;

                            NofTX = 0;
                            TXtype = new string[0];
                            TXlinkT = new string[0];
                            TXquan = new double[0];
                            TXsum = new decimal[0];

                            Array.Clear(TXtype, 0, TXtype.Length);
                            Array.Clear(TXlinkT, 0, TXlinkT.Length);
                            Array.Clear(TXquan, 0, TXquan.Length);
                            Array.Clear(TXsum, 0, TXsum.Length);

                            //ConnectionSQL(ref cn, dbcfg);
                            //If GP_AgetTABstkOK(U_No, U_Type, U_Cus, ZSnull(Tls.Fields("MILstk")), ZNnull(Tls.Fields("MILlinkVCid"), 0), Zdate) Then

                            if (cn.State == ConnectionState.Open)
                            {
                                cn.Close();
                            }

                            Xstr = "Select TABquan,TABsum,TABtypeID,TABlinkType From TAB Where TABlinkVcus=@TABlinkVcus And TABlinkVid=@TABlinkVid And TABstk=@TABstk "
                            + " AND (TABtypeID IN('AS','BS','TS','AP','BP','TP') or ((TABlinkVno=@TABlinkVno) And (TABlinkVtype=@TABlinkVtype) And TABlinkVdate=@TABlinkVdate))";

                            //ConnectionSQL(ref cn, dbcfg);
                            cn =  DBHelper.SqlConnectionDbMAC5();
                            if (cn.State == ConnectionState.Closed)
                                cn.Open();
                            cmd = new SqlCommand(Xstr, cn);
                            cmd.CommandText = Xstr;
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.Text;
                            //cmd.Transaction = tran;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@TABlinkVcus", SqlDbType.NVarChar, 15).Value = Gcus;
                            cmd.Parameters.Add("@TABlinkVid", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                            cmd.Parameters.Add("@TABstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            cmd.Parameters.Add("@TABlinkVno", SqlDbType.NVarChar, 15).Value = Gvnos;
                            cmd.Parameters.Add("@TABlinkVtype", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIL.Rows[i]["MILtype"]);
                            cmd.Parameters.Add("@TABlinkVdate", SqlDbType.DateTime).Value = Zdate;
                            rd = cmd.ExecuteReader();
                            if (rd.HasRows)
                            {
                                while (rd.Read())
                                {
                                    NofTX++;
                                    TXtype = returnarray(TXtype, NofTX);
                                    TXlinkT = returnarray(TXlinkT, NofTX);
                                    TXquan = returnarray(TXquan, NofTX);
                                    TXsum = returnarray(TXsum, NofTX);

                                    TXquan[NofTX] = Convert.ToDouble(rd["TABquan"]);
                                    //TXquanP2(NofTX) = Xtab.Fields("TABquanP2");
                                    TXsum[NofTX] = Convert.ToDecimal(rd["TABsum"]);
                                    TXtype[NofTX] = Convert.ToString(rd["TABtypeID"]);
                                    TXlinkT[NofTX] = Convert.ToString(rd["TABlinkType"]);

                                }
                                if (NofTX > 0)
                                {
                                    for (k = 1; k <= NofTX; k++)
                                    {
                                        if (TXtype[k] == "PP") Zq1 = Zq1 + TXquan[k];
                                    }
                                }
                                Zq1 = Math.Round(Zq1, 5);
                                if (Convert.ToDouble(dtMIL.Rows[i]["MILquan"]) == Zq1)
                                {
                                    ClrSTK = -1;
                                    ClrAll = -1;
                                }
                            }
                            rd.Close();
                            cmd.Dispose();

                            comm.Parameters.Add("@CPXstkCUT", SqlDbType.Float).Value = Zq1;
                            comm.Parameters.Add("@CPXclearUSER", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILclearUM"]);
                            comm.Parameters.Add("@CPXclearSTK", SqlDbType.SmallInt).Value = ClrSTK;
                            comm.Parameters.Add("@CPXclearALL", SqlDbType.SmallInt).Value = ClrAll;
                            comm.Transaction = tran;
                            comm.ExecuteNonQuery();
                        }
                    }
                }

                if ((VType == "PP") && (Convert.ToInt32(dtMIH.Rows[0]["MIHnoUPLINK"]) > 0))
                {
                    for (i = 0; i < VnoList; i++)
                    {
                        Scut = Convert.ToInt32(dtMIL.Rows[i]["MILcut"]);
                        if ((Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]).Length > 0) && (Scut != 0))
                        {
                            LKid = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                            LKvnos = Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]);
                            LKdate = ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]);
                            LKtype = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                            LKcus = Convert.ToString(dtMIL.Rows[i]["MILcus"]);
                            LKstk = Convert.ToString(dtMIL.Rows[i]["MILstk"]);

                            SQL = "select * from CPX where CPXstkID='" + LKstk + "' And CPXvnosID='" + LKvnos + "' And CPXlinkID=" + Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]) + "";
                            //ConnectionSQL(ref cn, dbcfg);
                            cn =  DBHelper.SqlConnectionDbMAC5();
                            if (cn.State == ConnectionState.Closed)
                                cn.Open();
                            cmd = new SqlCommand(SQL, cn);
                            cmd.CommandText = SQL;
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.Text;
                            rd = cmd.ExecuteReader();
                            if (rd.HasRows)
                            {
                                rd.Read();
                                Uclear = Convert.ToInt32(rd["CPXclearUSER"]);
                                Zqr = Convert.ToDouble(rd["CPXstkREQ"]);
                                Zq1 = Convert.ToDouble(rd["CPXstkCUT"]);
                                Zq1 = Math.Round(Zq1 + Convert.ToDouble(dtMIL.Rows[i]["MILquan"]), 5);
                                ClrSTK = 0;
                                if (Zqr == Zq1) ClrSTK = -1;
                            }
                            if (!rd.IsClosed) rd.Close();
                            cmd.Dispose();

                            SQL = "UPDATE CPX WITH (UPDLOCK) Set CPXstkCUT=@CPXstkCUT, CPXclearSTK=@CPXclearSTK, CPXclearALL=@CPXclearALL"
                                  + " where CPXstkID=@CPXstkID and  CPXlinkID=@CPXlinkID and  CPXvnosID=@CPXvnosID and  CPXdateID=@CPXdateID";


                            comm = new SqlCommand(SQL, conn);
                            comm.CommandText = SQL;
                            comm.CommandTimeout = 30;
                            comm.CommandType = CommandType.Text;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@CPXstkID", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            comm.Parameters.Add("@CPXlinkID", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                            comm.Parameters.Add("@CPXvnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                            comm.Parameters.Add("@CPXdateID", SqlDbType.DateTime).Value = Zdate;
                            comm.Parameters.Add("@CPXstkCUT", SqlDbType.Float).Value = Zq1;
                            comm.Parameters.Add("@CPXclearSTK", SqlDbType.SmallInt).Value = ClrSTK;
                            comm.Parameters.Add("@CPXclearALL", SqlDbType.SmallInt).Value = ClrAll;
                            comm.Transaction = tran;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                #region "  CPP,CPS  "
                switch (VType)
                {
                    case "PS":
                    case "PP":
                        for (i = 0; i < VnoList; i++)
                        {
                            Squan = Math.Round(Convert.ToDouble(dtMIL.Rows[i]["MILquan"]), 5);
                            Scut = Convert.ToInt32(dtMIL.Rows[i]["MILcut"]);
                            if (Squan != 0)
                            {
                                int sMode = Mode;

                                if (Mode == 1)
                                {
                                    if (i >= PrevNo)
                                    {
                                        sMode = 0;
                                    }
                                }
                                if (sMode == 0 || sMode == 2)
                                {
                                    SQL = "INSERT INTO CP" + Xsp + " WITH (UPDLOCK) (CP" + Xsp + "stkID, CP" + Xsp + "linkID, CP" + Xsp + "vnosID, CP" + Xsp + "dateID, CP" + Xsp + "cusID, CP" + Xsp + "cut, CP" + Xsp + "job, CP" + Xsp + "dep, CP" + Xsp + "per, CP" + Xsp + "doc, CP" + Xsp + "mec, CP" + Xsp + "sto,"
                                        + " CP" + Xsp + "stkREQ, CP" + Xsp + "stkCUT1, CP" + Xsp + "stkCUT2, CP" + Xsp + "sumREQ, CP" + Xsp + "sumCUT1,CP" + Xsp + "sumCUT2, CP" + Xsp + "clearSTK, CP" + Xsp + "clearSUM, CP" + Xsp + "clearUSER, CP" + Xsp + "clearALL, CP" + Xsp + "curC, CP" + Xsp + "exchg, CP" + Xsp + "delvDate, CP" + Xsp + "listNo)"
                                        + " VALUES(@CP" + Xsp + "stkID, @CP" + Xsp + "linkID, @CP" + Xsp + "vnosID, @CP" + Xsp + "dateID, @CP" + Xsp + "cusID, @CP" + Xsp + "cut, @CP" + Xsp + "job, @CP" + Xsp + "dep, @CP" + Xsp + "per, @CP" + Xsp + "doc, @CP" + Xsp + "mec, @CP" + Xsp + "sto,"
                                        + " @CP" + Xsp + "stkREQ, @CP" + Xsp + "stkCUT1, @CP" + Xsp + "stkCUT2, @CP" + Xsp + "sumREQ, @CP" + Xsp + "sumCUT1, @CP" + Xsp + "sumCUT2, @CP" + Xsp + "clearSTK, @CP" + Xsp + "clearSUM, @CP" + Xsp + "clearUSER, @CP" + Xsp + "clearALL, @CP" + Xsp + "curC, @CP" + Xsp + "exchg, @CP" + Xsp + "delvDate, @CP" + Xsp + "listNo)";
                                }
                                else
                                {
                                    SQL = "Update CP" + Xsp + " WITH (UPDLOCK) set CP" + Xsp + "cut=@CP" + Xsp + "cut, CP" + Xsp + "job=@CP" + Xsp + "job, CP" + Xsp + "dep=@CP" + Xsp + "dep, CP" + Xsp + "per=@CP" + Xsp + "per, CP" + Xsp + "doc=@CP" + Xsp + "doc, CP" + Xsp + "mec=@CP" + Xsp + "mec, CP" + Xsp + "sto=@CP" + Xsp + "sto,"
                                        + " CP" + Xsp + "stkREQ=@CP" + Xsp + "stkREQ, CP" + Xsp + "stkCUT1=@CP" + Xsp + "stkCUT1, CP" + Xsp + "stkCUT2=@CP" + Xsp + "stkCUT2, CP" + Xsp + "sumREQ=@CP" + Xsp + "sumREQ, CP" + Xsp + "sumCUT1=@CP" + Xsp + "sumCUT1, CP" + Xsp + "sumCUT2=@CP" + Xsp + "sumCUT2,"
                                        + " CP" + Xsp + "clearSTK=@CP" + Xsp + "clearSTK, CP" + Xsp + "clearSUM=@CP" + Xsp + "clearSUM, CP" + Xsp + "clearUSER=@CP" + Xsp + "clearUSER, CP" + Xsp + "clearALL=@CP" + Xsp + "clearALL, CP" + Xsp + "curC=@CP" + Xsp + "curC, CP" + Xsp + "exchg=@CP" + Xsp + "exchg, CP" + Xsp + "delvDate=@CP" + Xsp + "delvDate, CP" + Xsp + "listNo=@CP" + Xsp + "listNo"
                                        + " where CP" + Xsp + "stkID=@CP" + Xsp + "stkID and CP" + Xsp + "linkID=@CP" + Xsp + "linkID and CP" + Xsp + "vnosID=@CP" + Xsp + "vnosID and CP" + Xsp + "dateID=@CP" + Xsp + "dateID and CP" + Xsp + "cusID=@CP" + Xsp + "cusID";
                                }

                                ClrSTK = 0;
                                ClrSum = 0;
                                ClrAll = 0;

                                comm = new SqlCommand(SQL, conn);
                                comm.CommandText = SQL;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@CP" + Xsp + "stkID", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                comm.Parameters.Add("@CP" + Xsp + "linkID", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                                comm.Parameters.Add("@CP" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                comm.Parameters.Add("@CP" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                comm.Parameters.Add("@CP" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILcus"]);
                                comm.Parameters.Add("@CP" + Xsp + "cut", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILcut"]);
                                comm.Parameters.Add("@CP" + Xsp + "job", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILjob"]);
                                comm.Parameters.Add("@CP" + Xsp + "dep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdep"]);
                                comm.Parameters.Add("@CP" + Xsp + "per", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILper"]);
                                comm.Parameters.Add("@CP" + Xsp + "doc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdoc"]);
                                comm.Parameters.Add("@CP" + Xsp + "mec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILmec"]);
                                comm.Parameters.Add("@CP" + Xsp + "sto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                                comm.Parameters.Add("@CP" + Xsp + "stkREQ", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);

                                Zqr = Squan;
                                Zq1 = 0;
                                Zq2 = 0;
                                Zsr = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILvat"]);
                                Zs1 = 0;
                                Zs2 = 0;

                                NofTX = 0;
                                TXtype = new string[0];
                                TXlinkT = new string[0];
                                TXquan = new double[0];
                                TXsum = new decimal[0];

                                Array.Clear(TXtype, 0, TXtype.Length);
                                Array.Clear(TXlinkT, 0, TXlinkT.Length);
                                Array.Clear(TXquan, 0, TXquan.Length);
                                Array.Clear(TXsum, 0, TXsum.Length);

                                if (cn.State == ConnectionState.Open)
                                {
                                    cn.Close();
                                }

                                Xstr = "Select TABquan,TABsum,TABtypeID,TABlinkType From TAB Where TABlinkVcus=@TABlinkVcus And TABlinkVid=@TABlinkVid And TABstk=@TABstk "
                                + " AND (TABtypeID IN('AS','BS','TS','AP','BP','TP') or ((TABlinkVno=@TABlinkVno) And (TABlinkVtype=@TABlinkVtype) And TABlinkVdate=@TABlinkVdate))";
                                //ConnectionSQL(ref cn, dbcfg);
                                cn =  DBHelper.SqlConnectionDbMAC5();
                                if (cn.State == ConnectionState.Closed)
                                    cn.Open();
                                cmd = new SqlCommand(Xstr, cn);
                                cmd.CommandText = Xstr;
                                cmd.CommandTimeout = 30;
                                cmd.CommandType = CommandType.Text;
                                //cmd.Transaction = tran;
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("@TABlinkVcus", SqlDbType.NVarChar, 15).Value = Gcus;
                                cmd.Parameters.Add("@TABlinkVid", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                                cmd.Parameters.Add("@TABstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                cmd.Parameters.Add("@TABlinkVno", SqlDbType.NVarChar, 15).Value = Gvnos;
                                cmd.Parameters.Add("@TABlinkVtype", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIL.Rows[i]["MILtype"]);
                                cmd.Parameters.Add("@TABlinkVdate", SqlDbType.DateTime).Value = Zdate;
                                rd = cmd.ExecuteReader();
                                if (rd.HasRows)
                                {
                                    while (rd.Read())
                                    {
                                        NofTX++;
                                        TXtype = returnarray(TXtype, NofTX);
                                        TXlinkT = returnarray(TXlinkT, NofTX);
                                        TXquan = returnarray(TXquan, NofTX);
                                        TXsum = returnarray(TXsum, NofTX);

                                        TXquan[NofTX - 1] = Convert.ToDouble(rd["TABquan"]);
                                        //TXquanP2(NofTX) = Xtab.Fields("TABquanP2");
                                        TXsum[NofTX - 1] = Convert.ToDecimal(rd["TABsum"]);
                                        TXtype[NofTX - 1] = Convert.ToString(rd["TABtypeID"]);
                                        TXlinkT[NofTX - 1] = Convert.ToString(rd["TABlinkType"]);

                                    }
                                    if (NofTX > 0)
                                    {
                                        for (k = 0; k < NofTX; k++)
                                        {
                                            switch (TXtype[k])
                                            {
                                                case "IS":
                                                case "IP":
                                                case "DS":
                                                case "DP":
                                                case "SS":
                                                case "SP":
                                                case "CS":
                                                case "CP":
                                                    Zq1 = (Zq1 + TXquan[k]);
                                                    Zs1 = (Zs1 + TXsum[k]);
                                                    break;
                                                case "AS":
                                                case "AP":
                                                    if (TXlinkT[k] != "2")
                                                    {
                                                        Zq2 = (Zq2 + TXquan[k]);
                                                        Zs2 = (Zs2 + TXsum[k]);
                                                    }
                                                    break;
                                                case "BS":
                                                case "TS":
                                                case "BP":
                                                case "TP":
                                                    if (TXlinkT[k] != "2")
                                                    {
                                                        Zq2 = (Zq2 - TXquan[k]);
                                                        Zs2 = (Zs2 - TXsum[k]);
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                rd.Close();
                                cmd.Dispose();

                                comm.Parameters.Add("@CP" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILcurC"]);
                                comm.Parameters.Add("@CP" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILexchg"]);
                                if ((ZDnulls(dtMIL.Rows[i]["MILdelvDT"]) == DateTime.MinValue) || (ZDnulls(dtMIL.Rows[i]["MILdelvDT"]) == DateTime.MaxValue))
                                {
                                    comm.Parameters.Add("@CP" + Xsp + "delvDate", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    comm.Parameters.Add("@CP" + Xsp + "delvDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIL.Rows[i]["MILdelvDT"], Iformat);
                                }
                                comm.Parameters.Add("@CP" + Xsp + "listNo", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlistNo"]);
                                comm.Parameters.Add("@CP" + Xsp + "stkCUT1", SqlDbType.Float).Value = Zq1;
                                comm.Parameters.Add("@CP" + Xsp + "stkCUT2", SqlDbType.Float).Value = Zq2;
                                comm.Parameters.Add("@CP" + Xsp + "sumCUT1", SqlDbType.Float).Value = Zs1;
                                comm.Parameters.Add("@CP" + Xsp + "sumCUT2", SqlDbType.Float).Value = Zs2;
                                comm.Parameters.Add("@CP" + Xsp + "sumREQ", SqlDbType.Float).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]);

                                if (Zqr == (Zq1 + Zq2)) ClrSTK = -1;
                                if (Zsr == (Zs1 + Zs2)) ClrSum = -1;
                                comm.Parameters.Add("@CP" + Xsp + "clearSTK", SqlDbType.SmallInt).Value = ClrSTK;
                                comm.Parameters.Add("@CP" + Xsp + "clearSUM", SqlDbType.SmallInt).Value = ClrSum;
                                comm.Parameters.Add("@CP" + Xsp + "clearUSER", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILclearUM"]);

                                if (Convert.ToInt32(dtMIL.Rows[i]["MILclearUM"]) != 0 || (ClrSTK != 0 && ClrSum != 0))
                                {
                                    ClrAll = -1;
                                }
                                comm.Parameters.Add("@CP" + Xsp + "clearALL", SqlDbType.SmallInt).Value = ClrAll;
                                comm.Transaction = tran;
                                comm.ExecuteNonQuery();
                            }
                        }
                        break;
                    case "IS":
                    case "IP":
                    case "DS":
                    case "DP":
                    case "SS":
                    case "SP":
                    case "CS":
                    case "CP":
                    case "AS":
                    case "AP":
                    case "BS":
                    case "BP":
                    case "TS":
                    case "TP":
                        Sok = true;
                        switch (VType)
                        {
                            case "BS":
                            case "BP":
                            case "AS":
                            case "AP":
                                if (Sclude == -1) Sok = false;
                                break;
                            default:
                                Sok = true;
                                break;
                        }
                        if (Sclude == -1)
                        {
                            for (i = 0; i < VnoList; i++)
                            {
                                Scut = Convert.ToInt32(dtMIL.Rows[i]["MILcut"]);
                                Squan = Math.Round(Convert.ToDouble(dtMIL.Rows[i]["MILquan"]), 5);
                                if (Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]).Length > 0)
                                {
                                    LKid = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                                    LKvnos = Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]);
                                    LKdate = ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]);
                                    LKtype = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                                    LKcus = Convert.ToString(dtMIL.Rows[i]["MILcus"]);
                                    LKstk = Convert.ToString(dtMIL.Rows[i]["MILstk"]);

                                    switch (VType)
                                    {
                                        case "AS":
                                        case "AP":
                                        case "BS":
                                        case "BP":
                                        case "TS":
                                        case "TP":
                                            SQL = "select * from CP" + Xsp + " where CP" + Xsp + "stkID='" + LKstk + "' And CP" + Xsp + "cusID='" + LKcus + "' And CP" + Xsp + "linkID=" + Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]) + "";
                                            break;
                                        default:
                                            SQL = "select * from CP" + Xsp + " where CP" + Xsp + "stkID='" + LKstk + "' And CP" + Xsp + "cusID='" + LKcus + "' And CP" + Xsp + "vnosID='" + LKvnos + "' And CP" + Xsp + "linkID=" + Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]) + "";
                                            break;
                                    }

                                    //ConnectionSQL(ref cn, dbcfg);
                                    cn =  DBHelper.SqlConnectionDbMAC5();
                                    if (cn.State == ConnectionState.Closed)
                                        cn.Open();
                                    cmd = new SqlCommand(SQL, cn);
                                    cmd.CommandText = SQL;
                                    cmd.CommandTimeout = 30;
                                    cmd.CommandType = CommandType.Text;
                                    rd = cmd.ExecuteReader();
                                    if (rd.HasRows)
                                    {
                                        rd.Read();

                                        Uclear = Convert.ToInt32(rd["CP" + Xsp + "clearUSER"]);
                                        Zqr = Convert.ToDouble(rd["CP" + Xsp + "stkREQ"]);
                                        Zq1 = Convert.ToDouble(rd["CP" + Xsp + "stkCUT1"]);
                                        Zq2 = Convert.ToDouble(rd["CP" + Xsp + "stkCUT2"]);
                                        Zsr = Convert.ToDecimal(rd["CP" + Xsp + "sumREQ"]);
                                        Zs1 = Convert.ToDecimal(rd["CP" + Xsp + "sumCUT1"]);
                                        Zs2 = Convert.ToDecimal(rd["CP" + Xsp + "sumCUT2"]);
                                        Ssum = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]);
                                        switch (VType)
                                        {
                                            case "IS":
                                            case "IP":
                                            case "DS":
                                            case "DP":
                                            case "SS":
                                            case "SP":
                                            case "CS":
                                            case "CP":
                                                Zq1 = Math.Round((Zq1 + Squan), 5);
                                                Zs1 = Math.Round((Zs1 + Ssum), 2);
                                                break;
                                            case "AS":
                                            case "AP":
                                                if (Scut != 2)
                                                {
                                                    Zq2 = Math.Round((Zq2 + Squan), 5);
                                                    Zs2 = Math.Round((Zs2 + Ssum), 2);
                                                }
                                                break;
                                            case "BS":
                                            case "BP":
                                            case "TS":
                                            case "TP":
                                                if (Scut != 2)
                                                {
                                                    Zq2 = Math.Round((Zq2 - Squan), 5);
                                                    Zs2 = Math.Round((Zs2 - Ssum), 2);
                                                }
                                                break;
                                        }
                                        ClrSTK = 0;
                                        ClrSum = 0;
                                        ClrAll = 0;
                                        if (Zqr == (Zq1 + Zq2)) ClrSTK = -1;
                                        if (Zsr == (Zs1 + Zs2)) ClrSum = -1;
                                        //if (Scut != 2)
                                        if ((Uclear == -1) || ((ClrSTK == -1) && (ClrSum == -1))) ClrAll = -1;

                                        SQL = "UPDATE CP" + Xsp + " WITH (UPDLOCK) Set CP" + Xsp + "sumCUT1=@CP" + Xsp + "sumCUT1, CP" + Xsp + "sumCUT2=@CP" + Xsp + "sumCUT2, CP" + Xsp + "clearSTK=@CP" + Xsp + "clearSTK,";
                                        SQL += "CP" + Xsp + "clearSUM=@CP" + Xsp + "clearSUM and CP" + Xsp + "clearALL=@CP" + Xsp + "clearALL";
                                        if (Scut != 2)
                                        {
                                            SQL += " and CP" + Xsp + "stkCUT1=@CP" + Xsp + "stkCUT1 and CP" + Xsp + "stkCUT2=@CP" + Xsp + "stkCUT2";
                                        }
                                        SQL += " where CP" + Xsp + "stkID=@CP" + Xsp + "stkID and CP" + Xsp + "linkID=@CP" + Xsp + "linkID and CP" + Xsp + "vnosID=@CP" + Xsp + "vnosID and CP" + Xsp + "dateID=@CP" + Xsp + "dateID and CP" + Xsp + "cusID=@CP" + Xsp + "cusID";

                                        comm = new SqlCommand(SQL, conn);
                                        comm.CommandText = SQL;
                                        comm.CommandTimeout = 30;
                                        comm.CommandType = CommandType.Text;
                                        comm.Parameters.Clear();
                                        comm.Parameters.Add("@CP" + Xsp + "stkID", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                        comm.Parameters.Add("@CP" + Xsp + "linkID", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                                        comm.Parameters.Add("@CP" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                        comm.Parameters.Add("@CP" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                        comm.Parameters.Add("@CP" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Gcus;
                                        comm.Parameters.Add("@CP" + Xsp + "sumCUT1", SqlDbType.Float).Value = Zs1;
                                        comm.Parameters.Add("@CP" + Xsp + "sumCUT2", SqlDbType.Float).Value = Zs2;
                                        comm.Parameters.Add("@CP" + Xsp + "clearSTK", SqlDbType.SmallInt).Value = ClrSTK;
                                        comm.Parameters.Add("@CP" + Xsp + "clearSUM", SqlDbType.SmallInt).Value = ClrSum;
                                        comm.Parameters.Add("@CP" + Xsp + "clearALL", SqlDbType.SmallInt).Value = ClrAll;
                                        if (Scut != 2)
                                        {
                                            comm.Parameters.Add("@CP" + Xsp + "stkCUT1", SqlDbType.Float).Value = Zq1;
                                            comm.Parameters.Add("@CP" + Xsp + "stkCUT2", SqlDbType.Float).Value = Zq2;
                                        }
                                        comm.Transaction = tran;
                                        comm.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        break;
                }
                #endregion

                #endregion

                #region "  Section 3  "

                #region "  CFP,CFS  "
                switch (VType)
                {
                    case "IS":
                    case "IP":
                    case "PS":
                    case "PP":
                    case "AS":
                    case "BS":
                    case "AP":
                    case "BP":
                        switch (VType)
                        {
                            case "IS":
                            case "IP":
                                if (Mode == 0 || Mode == 2)
                                {
                                    SQL = "INSERT INTO CF" + Xsp + " WITH (UPDLOCK) (CF" + Xsp + "vnosID, CF" + Xsp + "dateID, CF" + Xsp + "cusID, CF" + Xsp + "typeID, CF" + Xsp + "job, CF" + Xsp + "dep, CF" + Xsp + "per, CF" + Xsp + "doc, CF" + Xsp + "mec,"
                                          + " CF" + Xsp + "sumREQ, CF" + Xsp + "sumCUT1, CF" + Xsp + "sumCUT2, CF" + Xsp + "clearSUM, CF" + Xsp + "clearUSER, CF" + Xsp + "clearALL, CF" + Xsp + "curC, CF" + Xsp + "exchg, CF" + Xsp + "billCUT, CF" + Xsp + "billCLR)"
                                          + " VALUES(@CF" + Xsp + "vnosID, @CF" + Xsp + "dateID, @CF" + Xsp + "cusID, @CF" + Xsp + "typeID, @CF" + Xsp + "job, @CF" + Xsp + "dep, @CF" + Xsp + "per, @CF" + Xsp + "doc, @CF" + Xsp + "mec,"
                                          + " @CF" + Xsp + "sumREQ, @CF" + Xsp + "sumCUT1, @CF" + Xsp + "sumCUT2, @CF" + Xsp + "clearSUM, @CF" + Xsp + "clearUSER, @CF" + Xsp + "clearALL, @CF" + Xsp + "curC, @CF" + Xsp + "exchg, @CF" + Xsp + "billCUT, @CF" + Xsp + "billCLR)";
                                }
                                else
                                {
                                    SQL = "Update CF" + Xsp + " WITH (UPDLOCK) set CF" + Xsp + "vnosID=@CF" + Xsp + "vnosID, CF" + Xsp + "dateID=@CF" + Xsp + "dateID, CF" + Xsp + "cusID=@CF" + Xsp + "cusID, CF" + Xsp + "typeID=@CF" + Xsp + "typeID,"
                                          + " CF" + Xsp + "job=@CF" + Xsp + "job, CF" + Xsp + "dep=@CF" + Xsp + "dep, CF" + Xsp + "per=@CF" + Xsp + "per, CF" + Xsp + "doc=@CF" + Xsp + "doc, CF" + Xsp + "mec=@CF" + Xsp + "mec,"
                                          + " CF" + Xsp + "sumREQ=@CF" + Xsp + "sumREQ, CF" + Xsp + "sumCUT1=@CF" + Xsp + "sumCUT1, CF" + Xsp + "sumCUT2=@CF" + Xsp + "sumCUT2, CF" + Xsp + "clearSUM=@CF" + Xsp + "clearSUM,"
                                          + " CF" + Xsp + "clearUSER=@CF" + Xsp + "clearUSER, CF" + Xsp + "clearALL=@CF" + Xsp + "clearALL, CF" + Xsp + "curC=@CF" + Xsp + "curC, CF" + Xsp + "exchg=@CF" + Xsp + "exchg, CF" + Xsp + "billCUT=@CF" + Xsp + "billCUT, CF" + Xsp + "billCLR=@CF" + Xsp + "billCLR"
                                          + " where CF" + Xsp + "vnosID=@CF" + Xsp + "vnosID and CF" + Xsp + "dateID=@CF" + Xsp + "dateID and CF" + Xsp + "cusID=@CF" + Xsp + "cusID and CF" + Xsp + "typeID=@CF" + Xsp + "typeID";
                                }

                                comm = new SqlCommand(SQL, conn);
                                comm.CommandText = SQL;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@CF" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                comm.Parameters.Add("@CF" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                comm.Parameters.Add("@CF" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Gcus;
                                comm.Parameters.Add("@CF" + Xsp + "typeID", SqlDbType.NVarChar, 2).Value = Gtype;
                                comm.Parameters.Add("@CF" + Xsp + "job", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHjob"]);
                                comm.Parameters.Add("@CF" + Xsp + "dep", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdep"]);
                                comm.Parameters.Add("@CF" + Xsp + "per", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHper"]);
                                comm.Parameters.Add("@CF" + Xsp + "doc", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdoc"]);
                                comm.Parameters.Add("@CF" + Xsp + "mec", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHmec"]);
                                comm.Parameters.Add("@CF" + Xsp + "sumREQ", SqlDbType.Float).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);

                                //find down-link items
                                ClrSum = 0;
                                ClrCLR = 0;
                                ClrAll = 0;

                                Zsr = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                                Zs1 = 0;
                                Zs2 = 0;
                                Zbc = 0;

                                #region "  TabvcOK  "
                                if (GetTABvcOK(VNo, VType, VCus, Zdate))
                                {
                                    for (k = 1; k <= NofTX; k++)
                                    {
                                        switch (TXtype[k])
                                        {
                                            case "RS":
                                            case "RP":
                                                Zs1 = Math.Round((Zs1 + TXsum[k]), 2);
                                                break;
                                            case "AS":
                                            case "AP":
                                                Zs2 = Math.Round((Zs2 + TXsum[k]), 2);
                                                break;
                                            case "BS":
                                            case "BP":
                                            case "TS":
                                            case "TP":
                                                Zs2 = Math.Round((Zs2 - TXsum[k]), 2);
                                                break;
                                            case "VS":
                                            case "VP":
                                                //If GB_IsBilling Then Zbc = RoundUP2C(Zbc + TXsum(k))
                                                break;
                                        }
                                    }
                                    //If GB_IsBilling Then DSfn.Fields("CF" + Xsp + "billCUT") = Zbc
                                    ClrSum = 0;
                                    if (Zsr == (Zs1 - Zs2)) ClrSum = -1;
                                    if (Zsr == (Zbc - Zs2)) ClrCLR = -1;
                                    Zsr = Zs1;
                                }
                                #endregion

                                if (Convert.ToInt32(dtMIH.Rows[i]["MIHclearUM"]) != 0 || (ClrSum != 0))
                                {
                                    ClrAll = -1;
                                }

                                comm.Parameters.Add("@CF" + Xsp + "sumCUT1", SqlDbType.Float).Value = Zsr;
                                comm.Parameters.Add("@CF" + Xsp + "sumCUT2", SqlDbType.Float).Value = Zs2;
                                comm.Parameters.Add("@CF" + Xsp + "clearSUM", SqlDbType.SmallInt).Value = ClrSum;
                                comm.Parameters.Add("@CF" + Xsp + "clearUSER", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHclearUM"]);
                                comm.Parameters.Add("@CF" + Xsp + "clearALL", SqlDbType.SmallInt).Value = ClrAll;
                                comm.Parameters.Add("@CF" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                                comm.Parameters.Add("@CF" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHexchg"]);
                                comm.Parameters.Add("@CF" + Xsp + "billCUT", SqlDbType.Float).Value = 0;
                                comm.Parameters.Add("@CF" + Xsp + "billCLR", SqlDbType.SmallInt).Value = 0;
                                comm.Transaction = tran;
                                comm.ExecuteNonQuery();
                                break;
                            case "PS":
                            case "PP":
                                if ((Convert.ToDecimal(dtMIH.Rows[0]["MIHextraSUM"]) + Convert.ToDecimal(dtMIH.Rows[0]["MIHextraVAT"])) > 0)
                                {
                                    if (Convert.ToString(dtMIH.Rows[0]["MIHrecNO"]).Length > 0)
                                    {
                                        ClrSum = -1;
                                    }
                                    else
                                    {
                                        ClrSum = 0;
                                    }

                                    if (Mode == 0 || Mode == 2)
                                    {
                                        SQL = "INSERT INTO CF" + Xsp + " WITH (UPDLOCK) (CF" + Xsp + "vnosID, CF" + Xsp + "dateID, CF" + Xsp + "cusID, CF" + Xsp + "typeID, CF" + Xsp + "job, CF" + Xsp + "dep, CF" + Xsp + "per, CF" + Xsp + "doc, CF" + Xsp + "mec,"
                                              + " CF" + Xsp + "sumREQ, CF" + Xsp + "sumCUT1, CF" + Xsp + "sumCUT2, CF" + Xsp + "clearSUM, CF" + Xsp + "clearUSER, CF" + Xsp + "clearALL, CF" + Xsp + "curC, CF" + Xsp + "exchg, CF" + Xsp + "billCUT, CF" + Xsp + "billCLR)"
                                              + " VALUES(@CF" + Xsp + "vnosID, @CF" + Xsp + "dateID, @CF" + Xsp + "cusID, @CF" + Xsp + "typeID, @CF" + Xsp + "job, @CF" + Xsp + "dep, @CF" + Xsp + "per, @CF" + Xsp + "doc, @CF" + Xsp + "mec,"
                                              + " @CF" + Xsp + "sumREQ, @CF" + Xsp + "sumCUT1, @CF" + Xsp + "sumCUT2, @CF" + Xsp + "clearSUM, @CF" + Xsp + "clearUSER, @CF" + Xsp + "clearALL, @CF" + Xsp + "curC, @CF" + Xsp + "exchg, @CF" + Xsp + "billCUT, @CF" + Xsp + "billCLR)";
                                    }
                                    else
                                    {
                                        SQL = "Update CF" + Xsp + " WITH (UPDLOCK) set CF" + Xsp + "vnosID=@CF" + Xsp + "vnosID, CF" + Xsp + "dateID=@CF" + Xsp + "dateID, CF" + Xsp + "cusID=@CF" + Xsp + "cusID, CF" + Xsp + "typeID=@CF" + Xsp + "typeID,"
                                              + " CF" + Xsp + "job=@CF" + Xsp + "job, CF" + Xsp + "dep=@CF" + Xsp + "dep, CF" + Xsp + "per=@CF" + Xsp + "per, CF" + Xsp + "doc=@CF" + Xsp + "doc, CF" + Xsp + "mec=@CF" + Xsp + "mec,"
                                              + " CF" + Xsp + "sumREQ=@CF" + Xsp + "sumREQ, CF" + Xsp + "sumCUT1=@CF" + Xsp + "sumCUT1, CF" + Xsp + "sumCUT2=@CF" + Xsp + "sumCUT2, CF" + Xsp + "clearSUM=@CF" + Xsp + "clearSUM,"
                                              + " CF" + Xsp + "clearUSER=@CF" + Xsp + "clearUSER, CF" + Xsp + "clearALL=@CF" + Xsp + "clearALL, CF" + Xsp + "curC=@CF" + Xsp + "curC, CF" + Xsp + "exchg=@CF" + Xsp + "exchg, CF" + Xsp + "billCUT=@CF" + Xsp + "billCUT, CF" + Xsp + "billCLR=@CF" + Xsp + "billCLR"
                                              + " where CF" + Xsp + "vnosID=@CF" + Xsp + "vnosID and CF" + Xsp + "dateID=@CF" + Xsp + "dateID and CF" + Xsp + "cusID=@CF" + Xsp + "cusID and CF" + Xsp + "typeID=@CF" + Xsp + "typeID";
                                    }

                                    comm = new SqlCommand(SQL, conn);
                                    comm.CommandText = SQL;
                                    comm.CommandTimeout = 30;
                                    comm.CommandType = CommandType.Text;
                                    comm.Parameters.Clear();
                                    comm.Parameters.Add("@CF" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                    comm.Parameters.Add("@CF" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                    comm.Parameters.Add("@CF" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Gcus;
                                    comm.Parameters.Add("@CF" + Xsp + "typeID", SqlDbType.NVarChar, 2).Value = Gtype;
                                    comm.Parameters.Add("@CF" + Xsp + "job", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHjob"]);
                                    comm.Parameters.Add("@CF" + Xsp + "dep", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdep"]);
                                    comm.Parameters.Add("@CF" + Xsp + "per", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHper"]);
                                    comm.Parameters.Add("@CF" + Xsp + "doc", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdoc"]);
                                    comm.Parameters.Add("@CF" + Xsp + "mec", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHmec"]);

                                    Zsr = Convert.ToDecimal(dtMIH.Rows[0]["MIHextraSUM"]) + Convert.ToDecimal(dtMIH.Rows[0]["MIHextraVAT"]);
                                    comm.Parameters.Add("@CF" + Xsp + "sumREQ", SqlDbType.Float).Value = Zsr;
                                    if (ClrSum == 0)
                                    {
                                        Zsr = 0;
                                    }
                                    comm.Parameters.Add("@CF" + Xsp + "sumCUT1", SqlDbType.Float).Value = Zsr;
                                    comm.Parameters.Add("@CF" + Xsp + "sumCUT2", SqlDbType.Float).Value = 0;
                                    comm.Parameters.Add("@CF" + Xsp + "clearSUM", SqlDbType.SmallInt).Value = ClrSum;
                                    comm.Parameters.Add("@CF" + Xsp + "clearUSER", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHclearUM"]);
                                    comm.Parameters.Add("@CF" + Xsp + "clearALL", SqlDbType.SmallInt).Value = ClrSum;
                                    comm.Parameters.Add("@CF" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                                    comm.Parameters.Add("@CF" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHexchg"]);
                                    comm.Parameters.Add("@CF" + Xsp + "billCUT", SqlDbType.Float).Value = 0;
                                    comm.Parameters.Add("@CF" + Xsp + "billCLR", SqlDbType.SmallInt).Value = 0;
                                    comm.Transaction = tran;
                                    comm.ExecuteNonQuery();
                                }
                                break;
                            case "AS":
                            case "BS":
                            case "AP":
                            case "BP":
                                Sok = true;
                                if (Sclude == -1) Sok = false;
                                //if (Sok)
                                //{
                                //  decimal[] CNsum = new decimal[0];
                                //  decimal[] CNvat = new decimal[0];
                                //  int CNno=0;
                                //  Zbc = 0; Zsr = 0;
                                //  for (i =0 ;i < VnoList;i++)
                                //  {
                                //    CNno++;
                                //    CNsum = returnarray(CNsum, CNno);
                                //    CNvat = returnarray(CNsum, CNno);
                                //    CNsum[CNno] = Math.Round(Convert.ToDecimal(dtMIL.Rows[i]["MILsum"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]),2);
                                //    Zbc = Math.Round((Zbc + CNsum[CNno]),2);
                                //  }
                                //  if ((Zbc > 0) && (Convert.ToInt32(dt.Rows[0]["MIHvatInList"]) == 0))
                                //  {
                                //    for (i=0;i < VnoList;i++)
                                //    {
                                //      CNvat[i+1] = Math.Round((CNsum[i+1] * Convert.ToDecimal(dtMIH.Rows[0]["MIHvatSUM"]) / Zbc),2);
                                //      Zsr = Math.Round((Zsr + CNvat[i+1]),2);
                                //    }
                                //    if (Zsr != Convert.ToDecimal(dtMIH.Rows[0]["MIHvatSUM"])) CNvat[CNno] = Math.Round((CNvat[CNno] + (Convert.ToDecimal(dt.Rows[0]["MIHvatSUM"]) - Zsr)),2);
                                //    for (i=0;i < VnoList ;i++)
                                //    {
                                //      CNsum[i+1] = Math.Round((CNsum[i+1] + CNvat[i+1]),2);
                                //    }
                                //  }
                                //  for (i=0;i < VnoList;i++)
                                //  {
                                //    LKid = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                                //    LKvnos = Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]);
                                //    LKdate = ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]);
                                //    LKtype = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                                //    //Check on CFS,CFP
                                //    if ((LKtype == "V" + Xsp) || (LKtype == "I" + Xsp))
                                //    {
                                //      SQL = "Update CF" + Xsp  + " WITH (UPDLOCK) set CF" + Xsp  + "vnosID=@CF" + Xsp  + "vnosID, CF" + Xsp  + "dateID=@CF" + Xsp  + "dateID, CF" + Xsp  + "cusID=@CF" + Xsp  + "cusID, CF" + Xsp  + "typeID=@CF" + Xsp  + "typeID,"
                                //        + " CF" + Xsp  + "job=@CF" + Xsp  + "job, CF" + Xsp  + "dep=@CF" + Xsp  + "dep, CF" + Xsp  + "per=@CF" + Xsp  + "per, CF" + Xsp  + "doc=@CF" + Xsp  + "doc, CF" + Xsp  + "mec=@CF" + Xsp  + "mec,"
                                //        + " CF" + Xsp  + "sumREQ=@CF" + Xsp  + "sumREQ, CF" + Xsp  + "sumCUT1=@CF" + Xsp  + "sumCUT1, CF" + Xsp  + "sumCUT2=@CF" + Xsp  + "sumCUT2, CF" + Xsp  + "clearSUM=@CF" + Xsp  + "clearSUM,"
                                //        + " CF" + Xsp  + "clearUSER=@CF" + Xsp  + "clearUSER, CF" + Xsp  + "clearALL=@CF" + Xsp  + "clearALL, CF" + Xsp  + "curC=@CF" + Xsp  + "curC, CF" + Xsp  + "exchg=@CF" + Xsp  + "exchg, CF" + Xsp  + "billCUT=@CF" + Xsp  + "billCUT, CF" + Xsp  + "billCLR=@CF" + Xsp  + "billCLR"
                                //        + " where CF" + Xsp  + "vnosID=@CF" + Xsp  + "vnosID and CF" + Xsp  + "dateID=@CF" + Xsp  + "dateID and CF" + Xsp  + "cusID=@CF" + Xsp  + "cusID and CF" + Xsp  + "typeID=@CF" + Xsp  + "typeID";

                                //      If (GB_ConString = 2 And GB_Trans And ComDefault.DC_TransactionOK) Then
                                //        'DSfn.Open "Select * From CF" + Xsp + " Where " & SelCTR, GB_TransDB, adOpenKeyset, adLockOptimistic, adCmdText
                                //        DSfn.Open GetSQLquery(1, "CF" & Xsp) & " Where " & SelCTR, GB_TransDB, adOpenKeyset, adLockOptimistic, adCmdText
                                //      Else
                                //        'DSfn.Open "Select * From CF" + Xsp + " Where " & SelCTR, DBfin, adOpenKeyset, adLockOptimistic, adCmdText
                                //        DSfn.Open GetSQLquery(1, "CF" & Xsp) & " Where " & SelCTR, DBfin, adOpenKeyset, adLockOptimistic, adCmdText
                                //      End If
                                //      If DSfn.RecordCount > 0 Then
                                //        Uclear = ZNnull(DSfn.Fields("CF" + Xsp + "clearUSER"), 0)
                                //        Zsr = ZNnull(DSfn.Fields("CF" + Xsp + "sumREQ"), 0)
                                //        Zs1 = ZNnull(DSfn.Fields("CF" + Xsp + "sumCUT1"), 0)
                                //        Zs2 = ZNnull(DSfn.Fields("CF" + Xsp + "sumCUT2"), 0)
                                //        If U_Type = "A" & Xsp Then Zs2 = RoundUP2C(Zs2 + CNsum(i)) Else Zs2 = RoundUP2C(Zs2 - CNsum(i))
                                //        ClrSum = False
                                //        If (Zsr = (Zs1 - Zs2)) Then ClrSum = True
                                //        DSfn.Fields("CF" + Xsp + "sumCUT1") = Zs1
                                //        DSfn.Fields("CF" + Xsp + "sumCUT2") = Zs2
                                //        DSfn.Fields("CF" + Xsp + "clearSUM") = ClrSum
                                //        DSfn.Fields("CF" + Xsp + "clearALL") = False
                                //        If Uclear Or ClrSum Then DSfn.Fields("CF" + Xsp + "clearALL") = True

                                //        DSfn.Update
                                //      End If
                                //      SetNothingA DSfn
                                //    }
                                //    //Check on CDS,CDP
                                //    if (LKtype == "D" + Xsp)
                                //    {
                                //      SelCTR = "CD" + Xsp + "vnosID='" & LKvnos & "' And CD" + Xsp + "cusID='" & U_Cus & "' And " & GetSQLdateStr("CD" + Xsp + "dateID", "=", LKdate, GB_ConString)

                                //      If (GB_ConString = 2 And GB_Trans And ComDefault.DC_TransactionOK) Then
                                //        'DSdo.Open "Select * From CD" + Xsp + " Where " & SelCTR, GB_TransDB, adOpenKeyset, adLockOptimistic, adCmdText
                                //        DSdo.Open GetSQLquery(1, "CD" & Xsp) & " Where " & SelCTR, GB_TransDB, adOpenKeyset, adLockOptimistic, adCmdText
                                //      Else
                                //        'DSdo.Open "Select * From CD" + Xsp + " Where " & SelCTR, DBstk, adOpenKeyset, adLockOptimistic, adCmdText
                                //        DSdo.Open GetSQLquery(1, "CD" & Xsp) & " Where " & SelCTR, DBstk, adOpenKeyset, adLockOptimistic, adCmdText
                                //      End If
                                //      If DSdo.RecordCount > 0 Then
                                //        Uclear = ZNnull(DSdo.Fields("CD" + Xsp + "clearUSER"), 0)
                                //        Zsr = ZNnull(DSdo.Fields("CD" + Xsp + "sumREQ"), 0)
                                //        Zs1 = ZNnull(DSdo.Fields("CD" + Xsp + "sumCUT1"), 0)
                                //        Zs2 = ZNnull(DSdo.Fields("CD" + Xsp + "sumCUT2"), 0)
                                //        If U_Type = "A" & Xsp Then Zs2 = RoundUP2C(Zs2 + CNsum(i)) Else Zs2 = RoundUP2C(Zs2 - CNsum(i))
                                //        ClrSum = False
                                //        If (Zsr = (Zs1 - Zs2)) Then ClrSum = True
                                //        DSdo.Fields("CD" + Xsp + "sumCUT1") = Zs1
                                //        DSdo.Fields("CD" + Xsp + "sumCUT2") = Zs2
                                //        DSdo.Fields("CD" + Xsp + "clearSUM") = ClrSum
                                //        DSdo.Fields("CD" + Xsp + "clearALL") = False
                                //        If Uclear Or ClrSum Then DSdo.Fields("CD" + Xsp + "clearALL") = True
                                //        DSdo.Update
                                //      End If
                                //    }
                                //  }
                                //}
                                break;
                        }
                        break;
                }
                #endregion

                switch (VType)
                {
                    case "CS":
                    case "CP":
                    case "PS":
                    case "PP":
                        switch (VType)
                        {
                            case "CS":
                            case "CP":
                                if (Mode == 0 || Mode == 2)
                                {
                                    SQL = "INSERT INTO CR" + Xsp + " WITH (UPDLOCK) (CR" + Xsp + "vnosID, CR" + Xsp + "dateID, CR" + Xsp + "cusID, CR" + Xsp + "typeID, CR" + Xsp + "total, CR" + Xsp + "vat,"
                                    + " CR" + Xsp + "cog, CR" + Xsp + "exchg, CR" + Xsp + "recNo, CR" + Xsp + "recDate, CR" + Xsp + "curC)"
                                    + " VALUES(@CR" + Xsp + "vnosID, @CR" + Xsp + "dateID, @CR" + Xsp + "cusID, @CR" + Xsp + "typeID, @CR" + Xsp + "total, @CR" + Xsp + "vat,"
                                    + " @CR" + Xsp + "cog, @CR" + Xsp + "exchg, @CR" + Xsp + "recNo, @CR" + Xsp + "recDate, @CR" + Xsp + "curC)";
                                }
                                else
                                {
                                    SQL = "Update CR" + Xsp + " WITH (UPDLOCK) set CR" + Xsp + "vnosID=@CR" + Xsp + "vnosID, CR" + Xsp + "dateID=@CR" + Xsp + "dateID, CR" + Xsp + "cusID=@CR" + Xsp + "cusID, CR" + Xsp + "typeID=@CR" + Xsp + "typeID,"
                                    + " CR" + Xsp + "total=@CR" + Xsp + "total, CR" + Xsp + "vat=@CR" + Xsp + "vat, CR" + Xsp + "cog=@CR" + Xsp + "cog, CR" + Xsp + "exchg=@CR" + Xsp + "exchg, CR" + Xsp + "recNo=@CR" + Xsp + "recNo, CR" + Xsp + "recDate=@CR" + Xsp + "recDate, CR" + Xsp + "curC=@CR" + Xsp + "curC "
                                    + " where CR" + Xsp + "vnosID=@CR" + Xsp + "vnosID and CR" + Xsp + "dateID=@CR" + Xsp + "dateID and CR" + Xsp + "cusID=@CR" + Xsp + "cusID and CR" + Xsp + "typeID=@CR" + Xsp + "typeID ";
                                }

                                comm = new SqlCommand(SQL, conn);
                                comm.CommandText = SQL;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@CR" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                comm.Parameters.Add("@CR" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                comm.Parameters.Add("@CR" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Gcus;
                                comm.Parameters.Add("@CR" + Xsp + "typeID", SqlDbType.NVarChar, 2).Value = Gtype;
                                comm.Parameters.Add("@CR" + Xsp + "total", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                                comm.Parameters.Add("@CR" + Xsp + "vat", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHvatSUM"]);
                                comm.Parameters.Add("@CR" + Xsp + "cog", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHcog"]);
                                comm.Parameters.Add("@CR" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIH.Rows[0]["MIHexchg"]);
                                comm.Parameters.Add("@CR" + Xsp + "recNo", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHrecNO"]);
                                if ((ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MaxValue))
                                {
                                    comm.Parameters.Add("@CR" + Xsp + "recDate", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    comm.Parameters.Add("@CR" + Xsp + "recDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIH.Rows[0]["MIHrecDATE"], Iformat);
                                }
                                comm.Parameters.Add("@CR" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                                comm.Transaction = tran;
                                comm.ExecuteNonQuery();

                                break;
                            case "PS":
                            case "PP":
                                if (Convert.ToDecimal(dtMIH.Rows[0]["MIHextraSUM"]) > 0)
                                {
                                    if (Mode == 0 || Mode == 2)
                                    {
                                        SQL = "INSERT INTO CR" + Xsp + " WITH (UPDLOCK) (CR" + Xsp + "vnosID, CR" + Xsp + "dateID, CR" + Xsp + "cusID, CR" + Xsp + "typeID, CR" + Xsp + "total, CR" + Xsp + "vat,"
                                        + " CR" + Xsp + "cog, CR" + Xsp + "exchg, CR" + Xsp + "recNo, CR" + Xsp + "curC)"
                                        + " VALUES(@CR" + Xsp + "vnosID, @CR" + Xsp + "dateID, @CR" + Xsp + "cusID, @CR" + Xsp + "typeID, @CR" + Xsp + "total, @CR" + Xsp + "vat,"
                                        + " @CR" + Xsp + "cog, @CR" + Xsp + "exchg, @CR" + Xsp + "recNo, @CR" + Xsp + "curC)";
                                    }
                                    else
                                    {
                                        SQL = "Update CR" + Xsp + " WITH (UPDLOCK) set CR" + Xsp + "vnosID=@CR" + Xsp + "vnosID, CR" + Xsp + "dateID=@CR" + Xsp + "dateID, CR" + Xsp + "cusID=@CR" + Xsp + "cusID, CR" + Xsp + "typeID=@CR" + Xsp + "typeID,"
                                        + " CR" + Xsp + "total=@CR" + Xsp + "total, CR" + Xsp + "vat=@CR" + Xsp + "vat, CR" + Xsp + "cog=@CR" + Xsp + "cog, CR" + Xsp + "exchg=@CR" + Xsp + "exchg, CR" + Xsp + "recNo=@CR" + Xsp + "recNo, CR" + Xsp + "curC=@CR" + Xsp + "curC "
                                        + " where CR" + Xsp + "vnosID=@CR" + Xsp + "vnosID and CR" + Xsp + "dateID=@CR" + Xsp + "dateID and CR" + Xsp + "cusID=@CR" + Xsp + "cusID and CR" + Xsp + "typeID=@CR" + Xsp + "typeID ";
                                    }

                                    comm = new SqlCommand(SQL, conn);
                                    comm.CommandText = SQL;
                                    comm.CommandTimeout = 30;
                                    comm.CommandType = CommandType.Text;
                                    comm.Parameters.Clear();
                                    comm.Parameters.Add("@CR" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                                    comm.Parameters.Add("@CR" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                                    comm.Parameters.Add("@CR" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHcus"]);
                                    comm.Parameters.Add("@CR" + Xsp + "typeID", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIH.Rows[0]["MIHtype"]);
                                    comm.Parameters.Add("@CR" + Xsp + "total", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHextraSUM"]);
                                    comm.Parameters.Add("@CR" + Xsp + "vat", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHextraVAT"]);
                                    comm.Parameters.Add("@CR" + Xsp + "cog", SqlDbType.Money).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                                    comm.Parameters.Add("@CR" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIH.Rows[0]["MIHexchg"]);
                                    comm.Parameters.Add("@CR" + Xsp + "recNo", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHrecNO"]);
                                    if ((ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIH.Rows[0]["MIHrecDATE"]) == DateTime.MaxValue))
                                    {
                                        comm.Parameters.Add("@CR" + Xsp + "recDate", SqlDbType.DateTime).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        comm.Parameters.Add("@CR" + Xsp + "recDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIH.Rows[0]["MIHrecDATE"], Iformat);
                                    }
                                    comm.Parameters.Add("@CR" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                                    comm.Transaction = tran;
                                    comm.ExecuteNonQuery();
                                }
                                break;
                        }
                        break;
                }


                switch (VType)
                {
                    case "DS":
                    case "DP":
                        if (Mode == 0 || Mode == 2)
                        {
                            SQL = "INSERT INTO CD" + Xsp + " WITH (UPDLOCK) (CD" + Xsp + "vnosID, CD" + Xsp + "dateID, CD" + Xsp + "cusID,"
                            + " CD" + Xsp + "job, CD" + Xsp + "dep, CD" + Xsp + "per, CD" + Xsp + "doc, CD" + Xsp + "mec,"
                            + " CD" + Xsp + "sumREQ,CD" + Xsp + "sumCUT1, CD" + Xsp + "sumCUT2, CD" + Xsp + "clearSUM, CD" + Xsp + "clearUSER, CD" + Xsp + "clearALL, CD" + Xsp + "curC, CD" + Xsp + "exchg)"
                            + " VALUES(@CD" + Xsp + "vnosID, @CD" + Xsp + "dateID, @CD" + Xsp + "cusID,"
                            + " @CD" + Xsp + "job, @CD" + Xsp + "dep, @CD" + Xsp + "per, @CD" + Xsp + "doc, @CD" + Xsp + "mec,"
                            + " @CD" + Xsp + "sumREQ, @CD" + Xsp + "sumCUT1, @CD" + Xsp + "sumCUT2, @CD" + Xsp + "clearSUM, @CD" + Xsp + "clearUSER, @CD" + Xsp + "clearALL, @CD" + Xsp + "curC, @CD" + Xsp + "exchg)";
                        }
                        else
                        {
                            SQL = "Update CD" + Xsp + " WITH (UPDLOCK) set "
                            + " CD" + Xsp + "job=@CD" + Xsp + "job, CD" + Xsp + "dep=@CD" + Xsp + "dep, CD" + Xsp + "per=@CD" + Xsp + "per, CD" + Xsp + "doc=@CD" + Xsp + "doc, CD" + Xsp + "mec=@CD" + Xsp + "mec,"
                            + " CD" + Xsp + "sumREQ=@CD" + Xsp + "sumREQ, CD" + Xsp + "sumCUT1=@CD" + Xsp + "sumCUT1, CD" + Xsp + "sumCUT2=@CD" + Xsp + "sumCUT2, CD" + Xsp + "clearSUM=@CD" + Xsp + "clearSUM,"
                            + " CD" + Xsp + "clearUSER=@CD" + Xsp + "clearUSER, CD" + Xsp + "clearALL=@CD" + Xsp + "clearALL, CD" + Xsp + "curC=@CD" + Xsp + "curC, CD" + Xsp + "exchg=@CD" + Xsp + "exchg"
                            + " where CD" + Xsp + "vnosID=@CD" + Xsp + "vnosID and CD" + Xsp + "dateID=@CD" + Xsp + "dateID and CD" + Xsp + "cusID=@CD" + Xsp + "cusID";
                        }

                        comm = new SqlCommand(SQL, conn);
                        comm.CommandText = SQL;
                        comm.CommandTimeout = 30;
                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@CD" + Xsp + "vnosID", SqlDbType.NVarChar, 15).Value = Gvnos;
                        comm.Parameters.Add("@CD" + Xsp + "dateID", SqlDbType.DateTime).Value = Zdate;
                        comm.Parameters.Add("@CD" + Xsp + "cusID", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHcus"]);
                        comm.Parameters.Add("@CD" + Xsp + "job", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHjob"]);
                        comm.Parameters.Add("@CD" + Xsp + "dep", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdep"]);
                        comm.Parameters.Add("@CD" + Xsp + "per", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHper"]);
                        comm.Parameters.Add("@CD" + Xsp + "doc", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHdoc"]);
                        comm.Parameters.Add("@CD" + Xsp + "mec", SqlDbType.NText).Value = Convert.ToString(dtMIH.Rows[0]["MIHmec"]);
                        comm.Parameters.Add("@CD" + Xsp + "sumREQ", SqlDbType.Float).Value = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);

                        //find down-link items
                        ClrSum = 0;
                        ClrCLR = 0;
                        ClrAll = 0;

                        Zsr = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                        Zs1 = 0;
                        Zs2 = 0;
                        Zbc = 0;

                        #region "  TabvcOK  "
                        if (GetTABvcOK(VNo, VType, VCus, Zdate))
                        {
                            Zsr = Convert.ToDecimal(dtMIH.Rows[0]["MIHnetSUM"]);
                            Zs1 = 0;
                            Zs2 = 0;
                            for (k = 1; k <= NofTX; k++)
                            {
                                Zs1 = Math.Round((Zs1 + TXsum[k]), 2);
                            }
                            if (Zsr == (Zs1 - Zs2)) ClrSum = -1;
                        }
                        #endregion

                        if (Convert.ToInt32(dtMIH.Rows[0]["MIHclearUM"]) != 0 || (ClrSum != 0))
                        {
                            ClrAll = -1;
                        }

                        comm.Parameters.Add("@CD" + Xsp + "sumCUT1", SqlDbType.Float).Value = Zs1;
                        comm.Parameters.Add("@CD" + Xsp + "sumCUT2", SqlDbType.Float).Value = Zs2;
                        comm.Parameters.Add("@CD" + Xsp + "clearSUM", SqlDbType.SmallInt).Value = ClrSum;
                        comm.Parameters.Add("@CD" + Xsp + "clearUSER", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHclearUM"]);
                        comm.Parameters.Add("@CD" + Xsp + "clearALL", SqlDbType.SmallInt).Value = ClrAll;
                        comm.Parameters.Add("@CD" + Xsp + "curC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]);
                        comm.Parameters.Add("@CD" + Xsp + "exchg", SqlDbType.Float).Value = Convert.ToInt32(dtMIH.Rows[0]["MIHexchg"]);
                        comm.Transaction = tran;
                        comm.ExecuteNonQuery();
                        break;
                }

                switch (VType)
                {
                    case "AS":
                    case "BS":
                    case "AP":
                    case "BP":
                        if (Sclude == -1)
                        {

                            //DSfn.AddNew
                            //DSfn.Fields("CDCvnosID") = U_No
                            //DSfn.Fields("CDCdateID") = Zdate
                            //DSfn.Fields("CDCcusID") = U_Cus
                            //DSfn.Fields("CDCtypeID") = U_Type
                            //DSfn.Fields("CDCjob") = ZSnull(Ths.Fields("MIHjob"))
                            //DSfn.Fields("CDCdep") = ZSnull(Ths.Fields("MIHdep"))
                            //DSfn.Fields("CDCper") = ZSnull(Ths.Fields("MIHper"))
                            //DSfn.Fields("CDCdoc") = ZSnull(Ths.Fields("MIHdoc"))
                            //DSfn.Fields("CDCmec") = ZSnull(Ths.Fields("MIHmec"))
                            //'DSfn.Fields("CDCsto") = ZSnull(Ths.Fields("MIHsto"))
                            //DSfn.Fields("CDCnolist") = ZNnull(Ths.Fields("MIHnolist"), 0)
                            //DSfn.Fields("CDCload") = ZNnull(Ths.Fields("MIHload"), 0)
                            //DSfn.Fields("CDCrefType") = ""
                            //If ZNnull(Ths.Fields("MIHload"), 0) > 0 Then
                            //  For i = 1 To QnoList
                            //    If i = 1 Then Tls.MoveFirst Else Tls.MoveNext
                            //    If QLen(ZSnull(Tls.Fields("MILlinkVCno"))) > 0 Then
                            //      DSfn.Fields("CDCrefType") = ZSnull(Tls.Fields("MILlinkVCtype"))
                            //      Exit For
                            //    End If
                            //  Next i
                            //Else
                            //  DSfn.Fields("CDCrefType") = GB_CDCrefType
                            //End If
                            //'DSfn.Fields("CDCcog") = ZNnull(Ths.Fields("MIHcog"), 0)
                            //DSfn.Fields("CDCcog") = ZNnull(Ths.Fields("MIHnetSUM"), 0) - ZNnull(Ths.Fields("MIHvatSUM"), 0)
                            //DSfn.Fields("CDCvatSUM") = ZNnull(Ths.Fields("MIHvatSUM"), 0)
                            //DSfn.Fields("CDCnetSUM") = ZNnull(Ths.Fields("MIHnetSUM"), 0)
                            //DSfn.Fields("CDCdiscLST") = ZNnull(Ths.Fields("MIHdiscLST"), 0)
                            //If Left$(U_Type, 1) = "B" Then
                            //  'DSfn.Fields("CDCcog") = -Abs(ZNnull(Ths.Fields("MIHcog"), 0))
                            //  DSfn.Fields("CDCcog") = -Abs(ZNnull(Ths.Fields("MIHnetSUM"), 0) - ZNnull(Ths.Fields("MIHvatSUM"), 0))
                            //  DSfn.Fields("CDCvatSUM") = -Abs(ZNnull(Ths.Fields("MIHvatSUM"), 0))
                            //  DSfn.Fields("CDCnetSUM") = -Abs(ZNnull(Ths.Fields("MIHnetSUM"), 0))
                            //  DSfn.Fields("CDCdiscLST") = -Abs(ZNnull(Ths.Fields("MIHdiscLST"), 0))
                            //End If
                            //DSfn.Fields("CDCcurC") = ZNnull(Ths.Fields("MIHcurC"), 0)
                            //DSfn.Fields("CDCexchg") = ZNnull(Ths.Fields("MIHexchg"), 0)
                            //DSfn.Fields("CDCflag") = False
                            //DSfn.Fields("CDClinkVno1") = GB_CDClinkVno1
                            //DSfn.Fields("CDClinkVdate1") = ZDnullS(GB_CDClinkVdate1)
                            //DSfn.Fields("CDClinkVcus1") = GB_CDClinkVcus1
                            //DSfn.Fields("CDClinkVtype1") = GB_CDClinkVtype1
                            //DSfn.Fields("CDClinkVno2") = GB_CDClinkVno2
                            //DSfn.Fields("CDClinkVdate2") = ZDnullS(GB_CDClinkVdate2)
                            //DSfn.Fields("CDClinkVcus2") = GB_CDClinkVcus2
                            //DSfn.Fields("CDClinkVtype2") = GB_CDClinkVtype2
                            //If (QLen(GB_CDClinkVtype1) > 0) Or (QLen(GB_CDClinkVtype2) > 0) Then
                            //  DSfn.Fields("CDCflag") = True
                            //End If
                            //DSfn.Fields("CDCcancel") = ZNnull(Ths.Fields("MIHcancel"), 0)
                            //DSfn.Fields("CDClinkVid") = GetRandom("I")
                            //DSfn.Update
                            //DSfn.Close: SetNothing DSfn
                        }
                        break;
                }


                #endregion

                #region "  Section 4  "

                #region "  AS,AP,BS,BP  "
                switch (VType)
                {
                    case "AS":
                    case "BS":
                    case "AP":
                    case "BP":
                        //G_Msg = "GPMI-ACT3-04"   '........................MSG
                        //        For i = 1 To QnoList
                        //          If i = 1 Then Tls.MoveFirst Else Tls.MoveNext
                        //          'only if cut stock=2 and not service type  ...Zstx(i) is only 1-3 from ZMI_AddVoucherDataOK
                        //          Zsx = ZNnull(Tls.Fields("MILstype"), 0)
                        //          If (ZNnull(Tls.Fields("MILcut"), 0) = 2) And ((Zsx = 1) Or (Zsx = 2) Or (Zsx = 4) Or (Zsx = 5)) Then
                        //            LKvnos = ZSnull(Tls.Fields("MILlinkVCno"))
                        //            LKtype = ZSnull(Tls.Fields("MILlinkVCtype"))
                        //            'Search for CST with linkage
                        //            SelCTR = "CSTvnos='" & LKvnos & "' And CSTtype='" & LKtype & "' And "
                        //            SelCTR = SelCTR & " CSTcus='" & U_Cus & "' And CSTlinkVCid=" & ZNnull(Tls.Fields("MILlinkVCid"), 0)
                        //            SelCTR = SelCTR & " And CSTstk='" & ZSnull(Tls.Fields("MILstk")) & "'"
                        //            'omit date, VCid can just do
                        //            If (GB_ConString = 2 And GB_Trans And ComDefault.DC_TransactionOK) Then
                        //              DScst.Open GetSQLquery(2, "CST") & " Where " & SelCTR, GB_TransDB, adOpenKeyset, adLockOptimistic, adCmdText
                        //            Else
                        //              DScst.Open GetSQLquery(2, "CST") & " Where " & SelCTR, DBcst, adOpenKeyset, adLockOptimistic, adCmdText
                        //            End If
                        //            If DScst.RecordCount > 0 Then
                        //              Zs1 = ZNnull(Tls.Fields("MILcog"), 0) + ZNnull(Tls.Fields("MILacost"), 0) - ZNnull(Tls.Fields("MILdiscA"), 0) - ZNnull(Tls.Fields("MILadisc"), 0) '- ZNnull(Tls.Fields("MILvat"), 0))
                        //              If ZNnull(Ths.Fields("MIHcurC"), 0) > 0 Then Zs1 = Zs1 * ZNnull(Ths.Fields("MIHexchg"), 0)
                        //              Zs1 = RoundUP2C(Zs1)
                        //              Select Case U_Type
                        //              Case "AS", "AP": DScst.Fields("CSTcogSP") = ZNnull(DScst.Fields("CSTcogSP"), 0) + Zs1
                        //              Case "BS", "BP": DScst.Fields("CSTcogSP") = ZNnull(DScst.Fields("CSTcogSP"), 0) - Zs1
                        //              End Select
                        //              DScst.Update
                        //            End If
                        //            SetNothingA DScst
                        //          End If
                        //        Next i
                        break;
                }

                #endregion

                #region "  CS  "
                for (i = 0; i < VnoList; i++)
                {
                    int Pok = 0;
                    int Zsx = 0;
                    int IOvalue = 0;
                    int ioZW = 0;

                    Zsx = Convert.ToInt32(dtMIL.Rows[i]["MILstype"]);
                    //Zdate = System.Convert.ToDateTime(Date_CvDMY(Str_MIL.MILday, Str_MIL.MILmonth, Str_MIL.MILyear, false), Iformat);
                    Pok = 0;
                    //Stock cut and not service type, or Stock cut by amount only on Lot_ID
                    if ((Convert.ToInt32(dtMIL.Rows[i]["MILcut"]) == -1) && (Zsx == 1 || Zsx == 2 || Zsx == 4 || Zsx == 5)) Pok = 1;
                    if ((Convert.ToInt32(dtMIL.Rows[i]["MILcut"]) == 2) && (Zsx == 4)) Pok = 2;
                    if (Pok > 0)
                    {
                        Ssum = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) + Convert.ToDecimal(dtMIL.Rows[i]["MILacost"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]);
                        if (Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]) > 0) Ssum = Ssum * Convert.ToDecimal(dtMIH.Rows[0]["MIHexchg"]);

                        int sMode = Mode;

                        if (Mode == 1)
                        {
                            if (i >= PrevNo)
                            {
                                sMode = 0;
                            }
                        }

                        if (sMode == 0 || sMode == 2)
                        {
                            SQL = "INSERT INTO CST WITH (UPDLOCK) (CSTdate, CSTtype, CSTvnos, CSTcus, CSTstk, CSTjob, CSTdep, CSTper, CSTdoc, CSTmec, CSTsto,"
                              + " CSTquan, CSTquanP2, CSTcogSP, CSTexpDate, CSTlinkVC, CSTlinkVCid, CSTio, CSTioZW, CSTlist, CSTcostA, CSTcostF, CSTcostL)"
                              + " VALUES(@CSTdate, @CSTtype, @CSTvnos, @CSTcus, @CSTstk, @CSTjob, @CSTdep, @CSTper, @CSTdoc, @CSTmec, @CSTsto,"
                              + " @CSTquan, @CSTquanP2, @CSTcogSP, @CSTexpDate, @CSTlinkVC, @CSTlinkVCid, @CSTio, @CSTioZW, @CSTlist, @CSTcostA, @CSTcostF, @CSTcostL)";
                        }
                        else
                        {
                            SQL = "Update CST WITH (UPDLOCK) set "
                            + " CSTjob=@CSTjob, CSTdep=@CSTdep, CSTper=@CSTper, CSTdoc=@CSTdoc, CSTmec=@CSTmec,"
                            + " CSTquan=@CSTquan, CSTquanP2=@CSTquanP2, CSTcogSP=@CSTcogSP, CSTexpDate=@CSTexpDate, CSTlinkVC=@CSTlinkVC, CSTlinkVCid=@CSTlinkVCid,"
                            + " CSTioZW=@CSTioZW, CSTlist=@CSTlist, CSTcostA=@CSTcostA, CSTcostF=@CSTcostF, CSTcostL=@CSTcostL"
                            + " where CSTdate=@CSTdate and CSTtype= @CSTtype and CSTvnos=@CSTvnos and CSTcus=@CSTcus and CSTstk=@CSTstk and CSTsto=@CSTsto and CSTio=@CSTio and CSTlist=@CSTlist";
                        }

                        comm = new SqlCommand(SQL, conn);
                        comm.CommandText = SQL;
                        comm.CommandTimeout = 30;
                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@CSTdate", SqlDbType.DateTime).Value = Zdate;
                        comm.Parameters.Add("@CSTtype", SqlDbType.NVarChar, 2).Value = Gtype;
                        comm.Parameters.Add("@CSTvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                        comm.Parameters.Add("@CSTcus", SqlDbType.NVarChar, 15).Value = Gcus;
                        comm.Parameters.Add("@CSTstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                        comm.Parameters.Add("@CSTjob", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILjob"]);
                        comm.Parameters.Add("@CSTdep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdep"]);
                        comm.Parameters.Add("@CSTper", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILper"]);
                        comm.Parameters.Add("@CSTdoc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdoc"]);
                        comm.Parameters.Add("@CSTmec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILmec"]);
                        comm.Parameters.Add("@CSTsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);

                        comm.Parameters.Add("@CSTquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                        comm.Parameters.Add("@CSTquanP2", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]);
                        Ssum = Math.Round(Ssum,2);
                        comm.Parameters.Add("@CSTcogSP", SqlDbType.Money).Value = Ssum;
                        //if ((ZDnulls(dtMIL.Rows[i]["MILexpDate"]) == DateTime.MinValue) || (ZDnulls(dtMIL.Rows[i]["MILexpDate"]) == DateTime.MaxValue))
                        //{
                        //    comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = DBNull.Value;
                        //}
                        //else
                        //{
                        //    comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIL.Rows[i]["MILexpDate"], Iformat);
                        //}
                        comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = DBNull.Value;
                        comm.Parameters.Add("@CSTlist", SqlDbType.SmallInt).Value = i + 1;
                        comm.Parameters.Add("@CSTlinkVC", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                        comm.Parameters.Add("@CSTlinkVCid", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                        if (Pok == 2)
                        {
                            IOvalue = 3;
                        }
                        ioZW = IOvalue;
                        if (ioZW == 2)
                        {
                            ioZW = 5;
                        }
                        comm.Parameters.Add("@CSTio", SqlDbType.SmallInt).Value = IOvalue;
                        comm.Parameters.Add("@CSTioZW", SqlDbType.SmallInt).Value = ioZW;
                        comm.Parameters.Add("@CSTcostA", SqlDbType.Money).Value = 0;
                        comm.Parameters.Add("@CSTcostF", SqlDbType.Money).Value = 0;
                        comm.Parameters.Add("@CSTcostL", SqlDbType.Money).Value = 0;
                        comm.Transaction = tran;
                        comm.ExecuteNonQuery();

                        if (Pok == 1)
                        {
                            j = 0;
                            int mode;

                            if (IOvalue == 1) j = 1;
                            if (IOvalue == 2) j = -1;
                            if (IOvalue == 0) j = 0;

                            Xstr = "Select Count(SBLquan) From SBL WITH(NOLOCK) Where SBLstk=@SBLstk And SBLsto=@SBLsto";
                            if (cn.State == ConnectionState.Open)
                            {
                                cn.Close();
                            }
                            //ConnectionSQL(ref cn, dbcfg);
                            comm = new SqlCommand(Xstr, conn);
                            comm.CommandText = Xstr;
                            comm.CommandTimeout = 30;
                            comm.CommandType = CommandType.Text;
                            comm.Transaction = tran;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@SBLstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            comm.Parameters.Add("@SBLsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                            mode = System.Convert.ToInt32(comm.ExecuteScalar());
                            //comm.Dispose();
                            if (mode == 0)
                            {
                                SQL = "INSERT INTO SBL WITH (UPDLOCK) (SBLstk, SBLsto, SBLquan, SBLquanP2)"
                                  + " VALUES(@SBLstk, @SBLsto, @SBLquan, @SBLquanP2)";

                            }
                            else
                            {
                                SQL = "UPDATE SBL WITH(UPDLOCK) set";
                                SQL += " SBLquan = SBLquan + @SBLquan";
                                SQL += " ,SBLquanP2 = SBLquanP2 + @SBLquanP2";
                                SQL += " WHERE SBLstk = @SBLstk and SBLsto = @SBLsto";
                            }

                            comm = new SqlCommand(SQL, conn);
                            comm.CommandText = SQL;
                            comm.CommandTimeout = 30;
                            comm.CommandType = CommandType.Text;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@SBLstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            comm.Parameters.Add("@SBLsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                            comm.Parameters.Add("@SBLquan", SqlDbType.Float).Value = Math.Abs(Convert.ToDouble(dtMIL.Rows[i]["MILquan"])) * j;
                            comm.Parameters.Add("@SBLquanP2", SqlDbType.Float).Value = Math.Abs(Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"])) * j;
                            comm.Transaction = tran;
                            comm.ExecuteNonQuery();

                            #region "  MX  "
                            if (VType == "MX")
                            {
                                sMode = Mode;

                                if (Mode == 1)
                                {
                                    if (i >= PrevNo)
                                    {
                                        sMode = 0;
                                    }
                                }

                                if (sMode == 0 || sMode == 2)
                                {
                                    SQL = "INSERT INTO CST WITH (UPDLOCK) (CSTdate, CSTtype, CSTvnos, CSTcus, CSTstk, CSTjob, CSTdep, CSTper, CSTdoc, CSTmec, CSTsto,"
                                      + " CSTquan, CSTquanP2, CSTcogSP, CSTexpDate, CSTlinkVC, CSTlinkVCid, CSTio, CSTioZW, CSTlist)"
                                      + " VALUES(@CSTdate, @CSTtype, @CSTvnos, @CSTcus, @CSTstk, @CSTjob, @CSTdep, @CSTper, @CSTdoc, @CSTmec, @CSTsto,"
                                      + " @CSTquan, @CSTquanP2, @CSTcogSP, @CSTexpDate, @CSTlinkVC, @CSTlinkVCid, @CSTio, @CSTioZW, @CSTlist)";
                                }
                                else
                                {
                                    SQL = "Update CST WITH (UPDLOCK) set "
                                    + " CSTjob=@CSTjob, CSTdep=@CSTdep, CSTper=@CSTper, CSTdoc=@CSTdoc, CSTmec=@CSTmec,"
                                    + " CSTquan=@CSTquan, CSTquanP2=@CSTquanP2, CSTcogSP=@CSTcogSP, CSTexpDate=@CSTexpDate, CSTlinkVC=@CSTlinkVC, CSTlinkVCid=@CSTlinkVCid,"
                                    + " CSTioZW=@CSTioZW, CSTlist=@CSTlist"
                                    + " where CSTdate=@CSTdate and CSTtype= @CSTtype and CSTvnos=@CSTvnos and CSTcus=@CSTcus and CSTstk=@CSTstk and CSTsto=@CSTsto and CSTio=@CSTio and CSTlist=@CSTlist";
                                }

                                comm = new SqlCommand(SQL, conn);
                                comm.CommandText = SQL;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@CSTdate", SqlDbType.DateTime).Value = Zdate;
                                comm.Parameters.Add("@CSTtype", SqlDbType.NVarChar, 2).Value = Gtype;
                                comm.Parameters.Add("@CSTvnos", SqlDbType.NVarChar, 15).Value = Gvnos;
                                comm.Parameters.Add("@CSTcus", SqlDbType.NVarChar, 15).Value = Gcus;
                                comm.Parameters.Add("@CSTstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                comm.Parameters.Add("@CSTjob", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILjob"]);
                                comm.Parameters.Add("@CSTdep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdep"]);
                                comm.Parameters.Add("@CSTper", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILper"]);
                                comm.Parameters.Add("@CSTdoc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILdoc"]);
                                comm.Parameters.Add("@CSTmec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILmec"]);
                                comm.Parameters.Add("@CSTsto", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[i]["MILstoMT"]);
                                comm.Parameters.Add("@CSTquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                                comm.Parameters.Add("@CSTquanP2", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]);
                                Ssum = Math.Round(Ssum,2);
                                comm.Parameters.Add("@CSTcogSP", SqlDbType.Money).Value = Ssum;
                                //if ((ZDnulls(dtMIL.Rows[i]["MILexpDate"]) == DateTime.MinValue) || (ZDnulls(dtMIL.Rows[i]["MILexpDate"]) == DateTime.MaxValue))
                                //{
                                //    comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = DBNull.Value;
                                //}
                                //else
                                //{
                                //    comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIL.Rows[i]["MILexpDate"], Iformat);
                                //}
                                comm.Parameters.Add("@CSTexpDate", SqlDbType.DateTime).Value = DBNull.Value;
                                comm.Parameters.Add("@CSTlist", SqlDbType.SmallInt).Value = i + 1;
                                comm.Parameters.Add("@CSTlinkVC", SqlDbType.NVarChar, 2).Value = "";
                                comm.Parameters.Add("@CSTlinkVCid", SqlDbType.SmallInt).Value = 0;
                                comm.Parameters.Add("@CSTio", SqlDbType.SmallInt).Value = 1;
                                comm.Parameters.Add("@CSTioZW", SqlDbType.SmallInt).Value = 1;
                                comm.Transaction = tran;
                                comm.ExecuteNonQuery();

                                Xstr = "Select Count(SBLquan) From SBL WITH(NOLOCK) Where SBLstk=@SBLstk And SBLsto=@SBLsto";
                                if (cn.State == ConnectionState.Open)
                                {
                                    cn.Close();
                                }
                                //ConnectionSQL(ref cn, dbcfg);
                                comm = new SqlCommand(Xstr, conn);
                                comm.CommandText = Xstr;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Transaction = tran;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@SBLstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                comm.Parameters.Add("@SBLsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILstoMT"]);
                                mode = System.Convert.ToInt32(comm.ExecuteScalar());
                                //comm.Dispose();
                                if (mode == 0)
                                {
                                    SQL = "INSERT INTO SBL WITH (UPDLOCK) (SBLstk, SBLsto, SBLquan, SBLquanP2)"
                                      + " VALUES(@SBLstk, @SBLsto, @SBLquan, @SBLquanP2)";

                                }
                                else
                                {
                                    SQL = "UPDATE SBL WITH(UPDLOCK) set";
                                    SQL += " SBLquan = SBLquan + @SBLquan";
                                    SQL += " ,SBLquanP2 = SBLquanP2 + @SBLquanP2";
                                    SQL += " WHERE SBLstk = @SBLstk and SBLsto = @SBLsto";
                                }

                                comm = new SqlCommand(SQL, conn);
                                comm.CommandText = SQL;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@SBLstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                comm.Parameters.Add("@SBLsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILstoMT"]);
                                comm.Parameters.Add("@SBLquan", SqlDbType.Float).Value = Math.Abs(Convert.ToDouble(dtMIL.Rows[i]["MILquan"]));
                                comm.Parameters.Add("@SBLquanP2", SqlDbType.Float).Value = Math.Abs(Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]));
                                comm.Transaction = tran;
                                comm.ExecuteNonQuery();

                            }
                            #endregion

                        }


                        if ((Convert.ToInt32(dtMIL.Rows[i]["MILcut"]) == 2) || (Pok > 0))
                        {
                            //Add to CSX table
                            if (Convert.ToInt32(dtMIL.Rows[i]["MILstype"]) != 3)
                            {
                                string Xsto = string.Empty;
                                Xsto = Convert.ToString(dtMIL.Rows[i]["MILsto"]);
                                int iC = 0;
                                Xstr = "Select Count(CSXstk) From CSX WITH(NOLOCK) Where CSXstk=@CSXstk And CSXsto=@CSXsto";
                                if (Convert.ToString(dtMIL.Rows[i]["MILsto"]).Length == 0) Xsto = "-";
                                if (cn.State == ConnectionState.Open)
                                {
                                    cn.Close();
                                }
                                //ConnectionSQL(ref cn, dbcfg);
                                comm = new SqlCommand(Xstr, conn);
                                comm.CommandText = Xstr;
                                comm.CommandTimeout = 30;
                                comm.CommandType = CommandType.Text;
                                comm.Transaction = tran;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@CSXstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                comm.Parameters.Add("@CSXsto", SqlDbType.NVarChar, 15).Value = Xsto;
                                iC = System.Convert.ToInt32(comm.ExecuteScalar());
                                //cmd.Dispose();
                                if (iC == 0)
                                {
                                    SQL = "INSERT INTO CSX WITH (UPDLOCK) (CSXstk, CSXsto)"
                                    + " VALUES(@CSXstk, @CSXsto)";

                                    comm = new SqlCommand(SQL, conn);
                                    comm.CommandText = SQL;
                                    comm.CommandTimeout = 30;
                                    comm.CommandType = CommandType.Text;
                                    comm.Parameters.Clear();
                                    comm.Parameters.Add("@CSXstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                                    comm.Parameters.Add("@CSXsto", SqlDbType.NVarChar, 15).Value = Xsto;
                                    comm.Transaction = tran;
                                    comm.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                }
                #endregion

                #region "  SRN  "
                SnoSerial = dtMIS.Rows.Count;
                if (SnoSerial > 0)
                {
                    for (i = 0; i < SnoSerial; i++)
                    {
                        j = Convert.ToInt32(dtMIS.Rows[i]["MISline"]);
                        if (j > 0)
                        {
                            int l = -1;
                            for (k = 0; k < VnoList; k++)
                            {
                                if ((k + 1) == Convert.ToInt32(dtMIL.Rows[k]["MILlistNo"]))
                                {
                                    l = k;
                                    break;
                                }
                            }

                            if (l >= 0)
                            {
                                if (Convert.ToInt32(dtMIL.Rows[l]["MILcut"]) == -1)
                                {
                                    Isnsv = Convert.ToInt32(dtMIS.Rows[i]["MISsnsv"]);

                                    int sMode = Mode;
                                    if (Mode == 1)
                                    {
                                        if (i >= PrevSNO)
                                        {
                                            sMode = 0;
                                        }
                                    }
                                    if (sMode == 0 || sMode == 2)
                                    {
                                        SQL = "INSERT INTO SRN WITH (UPDLOCK) (SRNdate, SRNtype, SRNvnos, SRNcus, SRNstk, SRNdep, SRNjob, SRNsto, SRNper, SRNmec, SRNserialM, SRNserialS, "
                                            + " SRNcost, SRNcurC, SRNexchg, SRNnotes, SRNio, SRNdoc, SRNquan, SRNcostSU, SRNAutoNO, SRNmfgDATE, SRNexpDATE)"
                                            + " VALUES(@SRNdate, @SRNtype, @SRNvnos, @SRNcus, @SRNstk, @SRNdep, @SRNjob, @SRNsto, @SRNper, @SRNmec, @SRNserialM, @SRNserialS,"
                                            + " @SRNcost, @SRNcurC, @SRNexchg, @SRNnotes, @SRNio, @SRNdoc, @SRNquan, @SRNcostSU, @SRNAutoNO, @SRNmfgDATE, @SRNexpDATE)";
                                    }
                                    else
                                    {
                                        SQL = "Update SRN WITH (UPDLOCK) (SRNdate=@SRNdate, SRNtype=@SRNtype, SRNvnos=@SRNvnos, SRNcus=@SRNcus, SRNstk=@SRNstk, SRNdep=@SRNdep, SRNjob=@SRNjob,"
                                            + " SRNsto=@SRNsto, SRNper=@SRNper, SRNmec=@SRNmec, SRNserialM=@SRNserialM, SRNserialS=@SRNserialS,SRNcost=@SRNcost,SRNcurC=@SRNcurC,SRNexchg=@SRNexchg, "
                                            + " SRNnotes=@SRNnotes, SRNio=@SRNio, SRNdoc=@SRNdoc, SRNquan=@SRNquan, SRNcostSU=@SRNcostSU, SRNAutoNO=@SRNAutoNO, SRNmfgDATE=@SRNmfgDATE, SRNexpDATE=@SRNexpDATE"
                                            + " where SRNdate=@SRNdate and SRNtype=@SRNtype and SRNvnos=@SRNvnos and SRNcus=@SRNcus and SRNserialM=@SRNserialM";
                                    }

                                    comm = new SqlCommand(SQL, conn);
                                    comm.CommandText = SQL;
                                    comm.CommandTimeout = 30;
                                    comm.CommandType = CommandType.Text;
                                    comm.Parameters.Clear();
                                    comm.Parameters.Add("@SRNdate", SqlDbType.DateTime).Value = Zdate;
                                    comm.Parameters.Add("@SRNtype", SqlDbType.NVarChar, 2).Value = VType;
                                    comm.Parameters.Add("@SRNvnos", SqlDbType.NVarChar, 15).Value = VNo;
                                    comm.Parameters.Add("@SRNcus", SqlDbType.NVarChar, 15).Value = VCus;
                                    comm.Parameters.Add("@SRNstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIS.Rows[i]["MISstk"]);
                                    comm.Parameters.Add("@SRNserialM", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialM"]);
                                    comm.Parameters.Add("@SRNserialS", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialS"]);
                                    comm.Parameters.Add("@SRNnotes", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISnotes"]);
                                    comm.Parameters.Add("@SRNquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISquan"]);
                                    comm.Parameters.Add("@SRNdep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILdep"]);
                                    comm.Parameters.Add("@SRNjob", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILjob"]);
                                    comm.Parameters.Add("@SRNsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[l]["MILsto"]);
                                    comm.Parameters.Add("@SRNper", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILper"]);
                                    comm.Parameters.Add("@SRNmec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILmec"]);
                                    comm.Parameters.Add("@SRNdoc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILdoc"]);
                                    comm.Parameters.Add("@SRNcostSU", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MIScostSU"]);
                                    comm.Parameters.Add("@SRNcurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIS.Rows[i]["MIScurC"]);
                                    comm.Parameters.Add("@SRNexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISexchg"]);

                                    k = 0;
                                    switch (VType)
                                    {
                                        case "DP":
                                        case "IP":
                                        case "AP":
                                        case "BS":
                                        case "CP":
                                        case "SP":
                                        case "TS":
                                            k = 1;
                                            break;
                                        case "DS":
                                        case "IS":
                                        case "AS":
                                        case "BP":
                                        case "CS":
                                        case "SS":
                                        case "TP":
                                            k = 2;
                                            break;
                                        case "JX":
                                            k = 1;
                                            if (Convert.ToDouble(dtMIL.Rows[l]["MILquan"]) < 0) k = 2;
                                            break;
                                        case "MX":
                                            k = 2;
                                            break;
                                    }
                                    if (Convert.ToInt32(dtMIS.Rows[i]["MIScost"]) == 2) k = 0;
                                    comm.Parameters.Add("@SRNio", SqlDbType.SmallInt).Value = k;
                                    if ((ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MaxValue))
                                    {
                                        comm.Parameters.Add("@SRNmfgDATE", SqlDbType.DateTime).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        comm.Parameters.Add("@SRNmfgDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISmfgDATE"], Iformat);
                                    }
                                    if ((ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MaxValue))
                                    {
                                        comm.Parameters.Add("@SRNexpDATE", SqlDbType.DateTime).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        comm.Parameters.Add("@SRNexpDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISexpDATE"], Iformat);
                                    }
                                    comm.Transaction = tran;
                                    comm.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }


                if (VType == "MX")
                {
                    SnoSerial = dtMIS.Rows.Count;
                    if (SnoSerial > 0)
                    {
                        for (i = 0; i < SnoSerial; i++)
                        {
                            j = Convert.ToInt32(dtMIS.Rows[i]["MISline"]);
                            if (j > 0)
                            {
                                int l = -1;
                                for (k = 0; k < VnoList; k++)
                                {
                                    if ((k + 1) == Convert.ToInt32(dtMIL.Rows[k]["MILlistNo"]))
                                    {
                                        l = k;
                                        break;
                                    }
                                }

                                if (l >= 0)
                                {
                                    if (Convert.ToInt32(dtMIL.Rows[l]["MILcut"]) == -1)
                                    {
                                        Isnsv = Convert.ToInt32(dtMIS.Rows[i]["MISsnsv"]);

                                        int sMode = Mode;
                                        if (Mode == 1)
                                        {
                                            if (i >= PrevSNO)
                                            {
                                                sMode = 0;
                                            }
                                        }
                                        if (sMode == 0 || sMode == 2)
                                        {
                                            SQL = "INSERT INTO SRN WITH (UPDLOCK) (SRNdate, SRNtype, SRNvnos, SRNcus, SRNstk, SRNdep, SRNjob, SRNsto, SRNper, SRNmec, SRNserialM, SRNserialS, "
                                                + " SRNcost, SRNcurC, SRNexchg, SRNnotes, SRNio, SRNdoc, SRNquan, SRNcostSU, SRNAutoNO, SRNmfgDATE, SRNexpDATE)"
                                                + " VALUES(@SRNdate, @SRNtype, @SRNvnos, @SRNcus, @SRNstk, @SRNdep, @SRNjob, @SRNsto, @SRNper, @SRNmec, @SRNserialM, @SRNserialS,"
                                                + " @SRNcost, @SRNcurC, @SRNexchg, @SRNnotes, @SRNio, @SRNdoc, @SRNquan, @SRNcostSU, @SRNAutoNO, @SRNmfgDATE, @SRNexpDATE)";
                                        }
                                        else
                                        {
                                            SQL = "Update SRN WITH (UPDLOCK) (SRNdate=@SRNdate, SRNtype=@SRNtype, SRNvnos=@SRNvnos, SRNcus=@SRNcus, SRNstk=@SRNstk, SRNdep=@SRNdep, SRNjob=@SRNjob,"
                                                + " SRNsto=@SRNsto, SRNper=@SRNper, SRNmec=@SRNmec, SRNserialM=@SRNserialM, SRNserialS=@SRNserialS,SRNcost=@SRNcost,SRNcurC=@SRNcurC,SRNexchg=@SRNexchg, "
                                                + " SRNnotes=@SRNnotes, SRNio=@SRNio, SRNdoc=@SRNdoc, SRNquan=@SRNquan, SRNcostSU=@SRNcostSU, SRNAutoNO=@SRNAutoNO, SRNmfgDATE=@SRNmfgDATE, SRNexpDATE=@SRNexpDATE"
                                                + " where SRNdate=@SRNdate and SRNtype=@SRNtype and SRNvnos=@SRNvnos and SRNcus=@SRNcus and SRNserialM=@SRNserialM";
                                        }

                                        comm = new SqlCommand(SQL, conn);
                                        comm.CommandText = SQL;
                                        comm.CommandTimeout = 30;
                                        comm.CommandType = CommandType.Text;
                                        comm.Parameters.Clear();
                                        comm.Parameters.Add("@SRNdate", SqlDbType.DateTime).Value = Zdate;
                                        comm.Parameters.Add("@SRNtype", SqlDbType.NVarChar, 2).Value = VType;
                                        comm.Parameters.Add("@SRNvnos", SqlDbType.NVarChar, 15).Value = VNo;
                                        comm.Parameters.Add("@SRNcus", SqlDbType.NVarChar, 15).Value = VCus;
                                        comm.Parameters.Add("@SRNstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIS.Rows[i]["MISstk"]);
                                        comm.Parameters.Add("@SRNserialM", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialM"]);
                                        comm.Parameters.Add("@SRNserialS", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialS"]);
                                        comm.Parameters.Add("@SRNnotes", SqlDbType.NText).Value = Convert.ToString(dtMIS.Rows[i]["MISnotes"]);
                                        comm.Parameters.Add("@SRNquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISquan"]);
                                        comm.Parameters.Add("@SRNdep", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILdep"]);
                                        comm.Parameters.Add("@SRNjob", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILjob"]);
                                        comm.Parameters.Add("@SRNsto", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[l]["MILstoMT"]);
                                        comm.Parameters.Add("@SRNper", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILper"]);
                                        comm.Parameters.Add("@SRNmec", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILmec"]);
                                        comm.Parameters.Add("@SRNdoc", SqlDbType.NText).Value = Convert.ToString(dtMIL.Rows[l]["MILdoc"]);
                                        comm.Parameters.Add("@SRNcostSU", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MIScostSU"]);
                                        comm.Parameters.Add("@SRNcurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIS.Rows[i]["MIScurC"]);
                                        comm.Parameters.Add("@SRNexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIS.Rows[i]["MISexchg"]);
                                        comm.Parameters.Add("@SRNio", SqlDbType.SmallInt).Value = 1;
                                        if ((ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISmfgDATE"]) == DateTime.MaxValue))
                                        {
                                            comm.Parameters.Add("@SRNmfgDATE", SqlDbType.DateTime).Value = DBNull.Value;
                                        }
                                        else
                                        {
                                            comm.Parameters.Add("@SRNmfgDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISmfgDATE"], Iformat);
                                        }
                                        if ((ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MinValue) || (ZDnulls(dtMIS.Rows[i]["MISexpDATE"]) == DateTime.MaxValue))
                                        {
                                            comm.Parameters.Add("@SRNexpDATE", SqlDbType.DateTime).Value = DBNull.Value;
                                        }
                                        else
                                        {
                                            comm.Parameters.Add("@SRNexpDATE", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIS.Rows[i]["MISexpDATE"], Iformat);
                                        }
                                        comm.Transaction = tran;
                                        comm.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region "  LOT  "
                SnoSerial = dtMIS.Rows.Count;
                if (SnoSerial > 0)
                {
                    for (i = 0; i < SnoSerial; i++)
                    {
                        if (Convert.ToInt32(dtMIS.Rows[i]["MISsnsv"]) == 4)
                        {
                            j = Convert.ToInt32(dtMIS.Rows[i]["MISline"]);
                            if (j > 0)
                            {
                                int l = -1;
                                for (k = 0; k < VnoList; k++)
                                {
                                    if ((k + 1) == Convert.ToInt32(dtMIL.Rows[k]["MILlistNo"]))
                                    {
                                        l = k;
                                        break;
                                    }
                                }
                                if (l >= 0)
                                {
                                    k = 0;
                                    switch (VType)
                                    {
                                        case "DP":
                                        case "IP":
                                        case "AP":
                                        case "BS":
                                        case "CP":
                                        case "SP":
                                        case "TS":
                                            k = 1;
                                            break;
                                        case "DS":
                                        case "IS":
                                        case "AS":
                                        case "BP":
                                        case "CS":
                                        case "SS":
                                        case "TP":
                                            k = 2;
                                            break;
                                        case "JX":
                                            k = 1;
                                            if (Convert.ToDouble(dtMIL.Rows[l]["MILquan"]) < 0) k = 2;
                                            break;
                                        case "MX":
                                            k = 3;
                                            break;
                                    }
                                    if (Convert.ToDouble(dtMIL.Rows[l]["MIcut"]) == 2) k = 0;

                                    double LOTquan = 0, LOTcostSU = 0, LOTqin = 0, LOTqout = 0;
                                    decimal LOTcostIN = 0;

                                    Zqr = 0;

                                    Xstr = "Select * From LOT Where LOTstk=@LOTstk And LOTid=@LOTid";
                                    //ConnectionSQL(ref cn, dbcfg);
                                    cn =  DBHelper.SqlConnectionDbMAC5();
                                    if (cn.State == ConnectionState.Closed)
                                        cn.Open();
                                    cmd = new SqlCommand(Xstr, cn);
                                    cmd.CommandText = Xstr;
                                    cmd.CommandTimeout = 30;
                                    cmd.CommandType = CommandType.Text;
                                    //cmd.Transaction = tran;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@LOTstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIS.Rows[i]["MISstk"]);
                                    cmd.Parameters.Add("@LOTid", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialM"]);
                                    rd = cmd.ExecuteReader();
                                    if (rd.HasRows)
                                    {
                                        while (rd.Read())
                                        {
                                            switch (k)
                                            {
                                                case 1: //Stock IN
                                                    LOTqin = Math.Round(Convert.ToDouble(rd["LOTqin"]) + Math.Abs(Convert.ToDouble(dtMIS.Rows[i]["MISquan"])), 5);
                                                    Zqr = Convert.ToDouble(dtMIS.Rows[i]["MIScostSU"]);
                                                    if (Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]) > 0) Zqr = Zqr * Convert.ToInt32(dtMIH.Rows[0]["MIHexchg"]);
                                                    LOTcostIN = Math.Round(Convert.ToDecimal(rd["LOTcostIN"]) + Convert.ToDecimal(Zqr * Math.Abs(Convert.ToDouble(dtMIS.Rows[i]["MISquan"]))), 4);
                                                    break;
                                                case 2: //Stock OUT
                                                    LOTqout = Math.Round(Convert.ToDouble(rd["LOTqout"]) + Math.Abs(Convert.ToDouble(dtMIS.Rows[i]["MISquan"])), 5);
                                                    break;
                                                case 0: //Add/Subtract only amount not considering quantity
                                                    Zqr = Convert.ToDouble(dtMIS.Rows[i]["MIScostSU"]);
                                                    if (Convert.ToInt32(dtMIH.Rows[0]["MIHcurC"]) > 0) Zqr = Zqr * Convert.ToInt32(dtMIH.Rows[0]["MIHexchg"]);
                                                    if (VType == "AP") LOTcostIN = Math.Round(Convert.ToDecimal(rd["LOTcostIN"]) + Convert.ToDecimal(Zqr * Math.Abs(Convert.ToDouble(dtMIS.Rows[i]["MISquan"]))), 4);
                                                    if (VType == "BP") LOTcostIN = Math.Round(Convert.ToDecimal(rd["LOTcostIN"]) - Convert.ToDecimal(Zqr * Math.Abs(Convert.ToDouble(dtMIS.Rows[i]["MISquan"]))), 4);
                                                    break;
                                                case 3: //Store movement
                                                    break;
                                            }
                                            if (k != 3)
                                            {
                                                LOTquan = Math.Round((LOTqin - LOTqout), 5);
                                                LOTcostSU = 0; //GBX_LotIDcost(ZNnull(DSsrn.Fields("LOTqin"), 0), ZNnull(DSsrn.Fields("LOTcostIN"), 0))
                                            }

                                        }
                                    }
                                    rd.Close();
                                    cmd.Dispose();


                                    int sMode = Mode;
                                    if (Mode == 1)
                                    {
                                        if (i >= PrevSNO)
                                        {
                                            sMode = 0;
                                        }
                                    }
                                    if (sMode == 0 || sMode == 2)
                                    {
                                        SQL = "INSERT INTO LOT WITH (UPDLOCK) (LOTstk, LOTid, LOTquan, LOTcostSU, LOTqin, LOTqout, LOTcostIN)"
                                            + " VALUES(@LOTstk, @LOTid, @LOTquan, @LOTcostSU, @LOTqin, @LOTqout, @LOTcostIN)";
                                    }
                                    else
                                    {
                                        SQL = "Update LOT WITH (UPDLOCK) (LOTstk=@LOTstk, LOTid=@LOTid, LOTquan=@LOTquan, LOTcostSU=@LOTcostSU, LOTqin=@LOTqin, LOTqout=@LOTqout, LOTcostIN=@LOTcostIN"
                                            + " where LOTstk=@LOTstk and LOTid=@LOTid";
                                    }

                                    comm = new SqlCommand(SQL, conn);
                                    comm.CommandText = SQL;
                                    comm.CommandTimeout = 30;
                                    comm.CommandType = CommandType.Text;
                                    comm.Parameters.Clear();
                                    comm.Parameters.Add("@LOTstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIS.Rows[i]["MISstk"]);
                                    comm.Parameters.Add("@LOTid", SqlDbType.NVarChar, 30).Value = Convert.ToString(dtMIS.Rows[i]["MISserialM"]);
                                    comm.Parameters.Add("@LOTquan", SqlDbType.Float).Value = LOTquan;
                                    comm.Parameters.Add("@LOTcostSU", SqlDbType.Float).Value = LOTcostSU;
                                    comm.Parameters.Add("@LOTqin", SqlDbType.Float).Value = LOTqin;
                                    comm.Parameters.Add("@LOTqout", SqlDbType.Float).Value = LOTqout;
                                    comm.Parameters.Add("@LOTcostIN", SqlDbType.Money).Value = LOTcostIN;
                                    comm.Transaction = tran;
                                    comm.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                #endregion

                #region "  TAB  "

                if (Convert.ToInt32(dtMIH.Rows[0]["MIHnoUPLINK"]) > 0)
                {
                    for (i = 0; i < VnoList; i++)
                    {
                        if (Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]).Length > 0)
                        {
                            int sMode = Mode;
                            if (Mode == 1)
                            {
                                if (i >= PrevNo)
                                {
                                    sMode = 0;
                                }
                            }

                            if (sMode == 0 || sMode == 2)
                            {
                                SQL = "INSERT INTO TAB WITH (UPDLOCK) (TABvnosID, TABdateID, TABcusID, TABtypeID, TABlinkType, TABlinkVno, TABlinkVdate, TABlinkVcus, TABlinkVtype, TABlinkVid, TABstk, TABquan, TABquanP2, TABsum, TABcurC, TABexchg, TABlistNo)"
                                    + " VALUES (@TABvnosID, @TABdateID, @TABcusID, @TABtypeID, @TABlinkType, @TABlinkVno, @TABlinkVdate, @TABlinkVcus, @TABlinkVtype, @TABlinkVid, @TABstk, @TABquan, @TABquanP2, @TABsum, @TABcurC, @TABexchg, @TABlistNo)";
                            }
                            else
                            {
                                SQL = "Update TAB WITH (UPDLOCK) set TABvnosID=@TABvnosID, TABdateID=@TABdateID, TABcusID=@TABcusID, TABtypeID=@TABtypeID, TABlinkType=@TABlinkType, TABlinkVno=@TABlinkVno, TABlinkVdate=@TABlinkVdate, TABlinkVcus=@TABlinkVcus,"
                                    + " TABlinkVtype=@TABlinkVtype, TABlinkVid=@TABlinkVid, TABstk=@TABstk, TABquan=@TABquan, TABquanP2=@TABquanP2, TABsum=@TABsum, TABcurC=@TABcurC, TABexchg=@TABexchg, TABlistNo=@TABlistNo"
                                    + " where TABvnosID=@TABvnosID and TABdateID=@TABdateID and TABcusID=@TABcusID and TABtypeID=@TABtypeID";
                            }

                            comm = new SqlCommand(SQL, conn);
                            comm.CommandText = SQL;
                            comm.CommandTimeout = 30;
                            comm.CommandType = CommandType.Text;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@TABvnosID", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHvnos"]);
                            comm.Parameters.Add("@TABdateID", SqlDbType.DateTime).Value = Zdate;
                            comm.Parameters.Add("@TABcusID", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIH.Rows[0]["MIHcus"]);
                            comm.Parameters.Add("@TABtypeID", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIH.Rows[0]["MIHtype"]);

                            comm.Parameters.Add("@TABlinkVno", SqlDbType.NVarChar, 15).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCno"]);
                            if ((ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]) == DateTime.MinValue) || (ZDnulls(dtMIL.Rows[i]["MILlinkVCdate"]) == DateTime.MaxValue))
                            {
                                comm.Parameters.Add("@TABlinkVdate", SqlDbType.DateTime).Value = DBNull.Value;
                            }
                            else
                            {
                                comm.Parameters.Add("@TABlinkVdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtMIL.Rows[i]["MILlinkVCdate"], Iformat);
                            }
                            comm.Parameters.Add("@TABlinkVcus", SqlDbType.NVarChar, 15).Value = VCus;
                            comm.Parameters.Add("@TABlinkVtype", SqlDbType.NVarChar, 2).Value = Convert.ToString(dtMIL.Rows[i]["MILlinkVCtype"]);
                            comm.Parameters.Add("@TABlinkVid", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlinkVCid"]);
                            comm.Parameters.Add("@TABstk", SqlDbType.NVarChar, 25).Value = Convert.ToString(dtMIL.Rows[i]["MILstk"]);
                            comm.Parameters.Add("@TABsum", SqlDbType.Money).Value = Convert.ToDecimal(dtMIL.Rows[i]["MILcog"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILdiscA"]) - Convert.ToDecimal(dtMIL.Rows[i]["MILadisc"]);
                            comm.Parameters.Add("@TABcurC", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILcurC"]);
                            comm.Parameters.Add("@TABexchg", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILexchg"]);
                            comm.Parameters.Add("@TABlistNo", SqlDbType.SmallInt).Value = Convert.ToInt32(dtMIL.Rows[i]["MILlistNo"]);

                            switch (VType)
                            {
                                case "AS":
                                case "AP":
                                case "BS":
                                case "BP":
                                    if (Convert.ToInt32(dtMIL.Rows[i]["MILcut"]) == 2)
                                    {
                                        comm.Parameters.Add("@TABquan", SqlDbType.Float).Value = 0;
                                        comm.Parameters.Add("@TABquanP2", SqlDbType.Float).Value = 0;
                                        comm.Parameters.Add("@TABlinkType", SqlDbType.NVarChar, 2).Value = "2";
                                    }
                                    else
                                    {
                                        comm.Parameters.Add("@TABquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                                        comm.Parameters.Add("@TABquanP2", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]);
                                        comm.Parameters.Add("@TABlinkType", SqlDbType.NVarChar, 2).Value = "";
                                    }
                                    break;
                                default:
                                    comm.Parameters.Add("@TABquan", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquan"]);
                                    comm.Parameters.Add("@TABquanP2", SqlDbType.Float).Value = Convert.ToDouble(dtMIL.Rows[i]["MILquanP2"]);
                                    comm.Parameters.Add("@TABlinkType", SqlDbType.NVarChar, 2).Value = "";
                                    break;
                            }
                            comm.Transaction = tran;
                            comm.ExecuteNonQuery();
                        }
                    }
                }


                #endregion

                #endregion

                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                ErrVoucher = e.Message.ToString();
            }
        }
        public static string GetNameFromTBname(int id, string tb, string xf)
        {
            DataTable dt = new DataTable(tb);
            SqlConnection cn = new SqlConnection();
            SqlDataAdapter da = null;
            string sql = string.Empty;
            string Xcap = "";
            string table = "";
            string tableCode = "";

            //cls_Global_DB.ConnectDatabase(ref cn);

            try
            {
                switch (tb)
                {
                    case "Department":
                        table = "Department";
                        tableCode = "DEPid";
                        break;                   

                }

                sql = "Select " + xf + " From " + table;
                sql += " where " + tableCode + "= " + id + "";

                dt = DBHelper.List(sql);
                if (dt.Rows.Count > 0)
                {
                    Xcap = Convert.ToString(dt.Rows[0][xf]);
                }
            }
            catch
            {
                Xcap = "";
            }
            finally
            {
            }
            return Xcap;
        }

        private static bool GetTABvcOK(string Xno, string Xtype, string Xcus, DateTime Xdate)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd = null;
            DataTable dt = null;
            SqlDataAdapter da = null;
            string Xstr = "";
            int i, j, k, Nrec = 0;
            decimal Zbc, Zsr;
            bool vcOK = false;
            decimal[] CNsum = new decimal[0];
            decimal[] CNvat = new decimal[0];

            NofTX = 0;
            TXtype = new string[0];
            TXlinkT = new string[0];
            TXquan = new double[0];
            TXsum = new decimal[0];

            Array.Clear(TXtype, 0, TXtype.Length);
            Array.Clear(TXlinkT, 0, TXlinkT.Length);
            Array.Clear(TXquan, 0, TXquan.Length);
            Array.Clear(TXsum, 0, TXsum.Length);

            Xstr = "Select TABquan,TABsum,TABtypeID,TABlinkType From TAB Where TABlinkVcus=@TABlinkVcus "
            + " AND TABlinkVno=@TABlinkVno And TABlinkVtype=@TABlinkVtype And TABlinkVdate=@TABlinkVdate";
            //ConnectionSQL(ref cn, dbcfg);
            cn = DBHelper.SqlConnectionDbMAC5();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            cmd = new SqlCommand(Xstr, cn);
            cmd.CommandText = Xstr;
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.Text;
            //cmd.Transaction = tran;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@TABlinkVcus", SqlDbType.NVarChar, 15).Value = Xcus;
            cmd.Parameters.Add("@TABlinkVno", SqlDbType.NVarChar, 15).Value = Xno;
            cmd.Parameters.Add("@TABlinkVtype", SqlDbType.NVarChar, 2).Value = Xtype;
            cmd.Parameters.Add("@TABlinkVdate", SqlDbType.DateTime).Value = Xdate;
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    NofTX++;
                    TXtype = returnarray(TXtype, NofTX);
                    TXlinkT = returnarray(TXlinkT, NofTX);
                    TXquan = returnarray(TXquan, NofTX);
                    TXsum = returnarray(TXsum, NofTX);

                    TXquan[NofTX] = Convert.ToDouble(rd["TABquan"]);
                    //TXquanP2(NofTX) = Xtab.Fields("TABquanP2");
                    TXsum[NofTX] = Convert.ToDecimal(rd["TABsum"]);
                    TXtype[NofTX] = Convert.ToString(rd["TABtypeID"]);
                    TXlinkT[NofTX] = Convert.ToString(rd["TABlinkType"]);

                }
            }
            rd.Close();
            cmd.Dispose();

            //  If NofTX > 0 Then 'sort arrays of TX
            //    For i = 1 To NofTX - 1
            //      For j = 1 To NofTX - i
            //        If TXtype(j) > TXtype(j + 1) Then
            //          GP_SwopTX j
            //        Else
            //          If TXtype(j) = TXtype(j + 1) Then
            //            If TXvno(j) > TXvno(j + 1) Then
            //              GP_SwopTX j
            //            Else
            //              If TXvno(j) = TXvno(j + 1) Then
            //                If TXdate(j) > TXdate(j + 1) Then GP_SwopTX j
            //              End If
            //            End If
            //          End If
            //        End If
            //      Next j
            //    Next i
            //ReDeleteTX: 'now delete similar list
            //    If NofTX > 1 Then
            //      For i = 1 To NofTX - 1
            //        If (TXtype(i) = TXtype(i + 1)) And (TXvno(i) = TXvno(i + 1)) And (TXdate(i) = TXdate(i + 1)) Then
            //          For j = i To NofTX - 1
            //            TXsum(j) = TXsum(j + 1)
            //            TXtype(j) = TXtype(j + 1)
            //            TXdate(j) = TXdate(j + 1)
            //            TXvno(j) = TXvno(j + 1)
            //          Next j
            //          NofTX = NofTX - 1
            //          ReDim Preserve TXsum(NofTX), TXtype(NofTX), TXdate(NofTX), TXvno(NofTX)
            //          GoTo ReDeleteTX
            //        End If
            //      Next i
            //    End If

            if (NofTX > 0)
            {

                for (i = 1; i <= NofTX; i++)
                {
                    if (TXtype[i] == "AP" || TXtype[i] == "AS" || TXtype[i] == "BP" || TXtype[i] == "BS")
                    {
                        Xstr = "SELECT MIH.MIHvatInList, MIH.MIHvatSUM, MIL.MILsum, MIL.MILadisc, MIL.MILlinkVCno, MIL.MILlinkVCdate, MIL.MILlinkVCtype"
                            + " FROM  dbo.MIH INNER JOIN MIL ON MIH.MIHday = MIL.MILday AND MIH.MIHmonth = MIL.MILmonth AND MIH.MIHyear = MIL.MILyear"
                            + " AND MIH.MIHtype = MIL.MILtype AND MIH.MIHvnos = MIL.MILvnos AND MIH.MIHcus = MIL.MILcus"
                            + " Where MIH.MIHtype=@MIHtype And MIH.MIHcus=@MIHcus And MIH.MIHday=@MIHday And MIH.MIHmonth=@MIHmonth And MIH.MIHyear=@MIHyear"
                            + " And MIH.MIHvnos=@MIHvnos";

                        CNsum = new decimal[0];
                        CNvat = new decimal[0];
                        Nrec = 0;
                        Zbc = 0; Zsr = 0;

                        //ConnectionSQL(ref cn, dbcfg);
                        cn = DBHelper.SqlConnectionDbMAC5();
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        da = new SqlDataAdapter(Xstr, cn);
                        da.SelectCommand.Parameters.Clear();
                        da.SelectCommand.Parameters.Add("@MIHcus", SqlDbType.NVarChar, 15).Value = Xcus;
                        da.SelectCommand.Parameters.Add("@MIHvnos", SqlDbType.NVarChar, 15).Value = Xno;
                        da.SelectCommand.Parameters.Add("@MIHtype", SqlDbType.NVarChar, 2).Value = Xtype;
                        da.SelectCommand.Parameters.Add("@MIHday", SqlDbType.SmallInt).Value = Xdate.Day;
                        da.SelectCommand.Parameters.Add("@MIHmonth", SqlDbType.SmallInt).Value = Xdate.Month;
                        da.SelectCommand.Parameters.Add("@MIHyear", SqlDbType.SmallInt).Value = Xdate.Year;
                        dt = new DataTable("voucher");
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            for (j = 0; j < dt.Rows.Count; j++)
                            {
                                Nrec++;
                                CNsum = returnarray(CNsum, Nrec);
                                CNvat = returnarray(CNsum, Nrec);

                                CNsum[Nrec] = Math.Round(Convert.ToDecimal(dt.Rows[j]["MILsum"]) - Convert.ToDecimal(dt.Rows[j]["MILadisc"]), 2);
                                Zbc = Math.Round((Zbc + CNsum[Nrec]), 2);


                            }
                            if ((Zbc > 0) && (DBInt(dt.Rows[0]["MIHvatInList"]) == 0))
                            {
                                for (k = 1; k <= Nrec; k++)
                                {
                                    CNvat[k] = Math.Round((CNsum[k] * Convert.ToDecimal(dt.Rows[0]["MIHvatSUM"]) / Zbc), 2);
                                    Zsr = Math.Round((Zsr + CNvat[k]), 2);
                                }
                                if (Zsr != Convert.ToDecimal(dt.Rows[0]["MIHvatSUM"])) CNvat[Nrec] = Math.Round((CNvat[Nrec] + (Convert.ToDecimal(dt.Rows[0]["MIHvatSUM"]) - Zsr)), 2);
                                for (k = 1; k <= Nrec; k++)
                                {
                                    CNsum[k] = Math.Round((CNsum[k] + CNvat[k]), 2);
                                }
                            }
                            TXsum[i] = 0;
                            for (j = 0; j < dt.Rows.Count; j++)
                            {
                                if ((DBString(dt.Rows[j]["MILlinkVCno"]) == Xno) && (DBString(dt.Rows[j]["MILlinkVCtype"]) == Xtype) && (ZDnulls(dt.Rows[j]["MILlinkVCdate"]) == Xdate))
                                {
                                    TXsum[i] = Math.Round((TXsum[i] + CNsum[j]), 2);
                                }
                            }
                        }
                    }

                }
                vcOK = true;
            }

            return vcOK;
        }

        public static object Date_CvDMY(int dd, int mm, int yy, bool Tdisp)
        {
            //Get And Check And Assign to Public Function the Input Date String
            //Data must be integer of Date,Month,Year
            int Yx;

            Yx = yy;
            if (Tdisp)
            {
                Yx = Yx - 543;
            }

            if (Date_ValidIDMY(dd, mm, Yx))
            {
                return  DateAndTime.DateSerial(Yx, mm, dd).ToShortDateString();
            }
            else
            {
                return DBNull.Value;
            }
        }

        public static bool Date_ValidIDMY(int Dx, int Mx, int Yx)
        {
            bool functionReturnValue = false;
            //Xdate is in English Year ONLY
            if ((Mx < 1) | (Mx > 12))
                goto VDinvalidIDMY;
            if ((Dx < 1) | (Dx > DayssinMonth(Mx, Yx)))
                goto VDinvalidIDMY;
            functionReturnValue = true;
            return functionReturnValue;
            VDinvalidIDMY:
            functionReturnValue = false;
            return functionReturnValue;
        }

        private static string Date_SQLDMY(int _day, int _month, int _year)
        {
            string Datestr = string.Empty;
            string day = string.Empty;
            string month = string.Empty;
            string year = string.Empty;

            if (_day < 10) { day = "0" + _day.ToString(); }
            else { day = _day.ToString(); }

            if (_month < 10) { month = "0" + _month.ToString(); }
            else { month = _month.ToString(); }

            year = _year.ToString();

            Datestr = day + "/" + month + "/" + year;
            return Datestr;
        }

        public static int DayssinMonth(int Mx, int Yx)
        {
            //Day ss is to avoid conflict with CScalendar Property
            //Xdate is in English Year ONLY, yx must be in full Year
            int[] MDarray = new int[13];
            if (Mx == 0 || Yx == 0)
            {
                return 0;
            }
            else
            {
                MDarray[1] = 31;
                MDarray[2] = 28;
                MDarray[3] = 31;
                MDarray[4] = 30;
                MDarray[5] = 31;
                MDarray[6] = 30;
                MDarray[7] = 31;
                MDarray[8] = 31;
                MDarray[9] = 30;
                MDarray[10] = 31;
                MDarray[11] = 30;
                MDarray[12] = 31;
                if (Yx % 4 == 0) MDarray[2] = 29;
                return MDarray[Mx];
            }
        }

        public static object Date_CvS10(string Xdate, bool Tdisp)
        {
            //Length of String must be VALID 10
            string Sx, Xd;
            int id, im, iy;
            Xd = Xdate;
            if (!Date_ValidS10TE(Xd, Tdisp))
            {
                return DBNull.Value;  //DateSerial(1990, 1, 1)
            }
            Xd = Date_TE10E10(Xd, Tdisp);
            Sx = Xd.Substring(0, 2); //Left(Xd, 2);
            id = Convert.ToInt16(Sx);
            Sx = Xd.Substring(3, 2);
            im = Convert.ToInt16(Sx);
            Sx = Right(Xd, 4);
            iy = Convert.ToInt16(Sx);
            return DateAndTime.DateSerial(iy, im, id);
        }

        public static string Date_Show10(object Xvar, bool Tdisp)
        {
            //'Display variant date in Thai & Eng XX/XX/XXXX Format$
            int yy, mm, Dd;//', Xr$
            string ret = "";

            if (IsDate(Xvar))
            {
                yy = (Convert.ToDateTime(Xvar)).Year;
                mm = (Convert.ToDateTime(Xvar)).Month;
                Dd = (Convert.ToDateTime(Xvar)).Day;
                if (Tdisp)
                    yy = yy + 543;
                ret = string.Format("{0:00}", Dd) + "/" + string.Format("{0:00}", mm) + "/" + string.Format("{0:0000}", yy);
                //ret = AssignStr(Dd, 2, 0) + "/" + AssignStr(mm, 2, 0) + "/" + AssignStr(yy, 4, 0);
            }
            else
            {
                ret = "";
            }
            return ret;
        }

        public static bool Date_ValidS10TE(string Xdate, bool TdisX)
        {
            //Verify that Xdate is a valid date in Thai/Eng
            string Xs;
            int Xv;

            if (TdisX)  //if Thai Display then Covert Year first
            {
                if (Xdate.Length != 10) return false;
                Xs = Right(Xdate, 4);
                Xv = Convert.ToInt16(Xs);
                if ((Xv < 2450) || (Xv > 2600)) return false;
                Xv = Xv - 543;
                Xs = Left(Xdate, 6) + Convert.ToString(Xv);
                return Date_ValidS10(Xs);
                //Xdate = Xs only check for validity, not changing status
            }
            else
            {
                return Date_ValidS10(Xdate);
            }

        }

        public static bool Date_ValidS10(string Xdate)
        {
            //Xdate is in English Year ONLY : Length of String must be 10
            string Sx;
            int id, im, iy; //', FullYear%

            //Xdate = Xdate;
            if (Xdate.Length != 10) return false;


            if ((Xdate.Substring(2, 1) != "/") || (Xdate.Substring(5, 1) != "/")) return false;
            Sx = Xdate.Substring(0, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Xdate.Substring(1, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Xdate.Substring(3, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Xdate.Substring(4, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Xdate.Substring(6, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Xdate.Substring(7, 1);
            if (InStr("0123456789", Sx) == 0) return false;
            Sx = Left(Xdate, 2);
            id = Convert.ToInt16(Sx);
            if ((id < 1) || (id > 31)) return false;
            Sx = Xdate.Substring(3, 2);
            im = Convert.ToInt16(Sx);
            if ((im < 1) || (im > 12)) return false;
            Sx = Right(Xdate, 4);
            iy = Convert.ToInt16(Sx);
            if ((iy < 1900) || (iy > 2050)) return false;
            if (id > DayssinMonth(im, iy)) return false;
            return true;
        }

        public static string Date_TE10E10(string Xdate, bool Tdisp)
        {
            //Input is Valid S10 in Thai/Eng
            //Return is English Only Format$
            int iy;

            if (!Date_ValidS10TE(Xdate, Tdisp))
                return "00/00/0000";
            if (Tdisp)
            {
                iy = Convert.ToInt16(Right(Xdate, 4)) - 543;
                return Left(Xdate, 6) + Convert.ToInt16(iy);
            }
            else
            {
                return Xdate;
            }
        }

        public static bool IsNumeric(object anyString)
        {

            if (Information.IsNumeric(anyString))
                return true;
            else
                return false;





            //if (anyString == null)
            //  anyString = "";
            //if (anyString.ToString().Length > 0)
            //{
            //  int dummyint;
            //  try
            //  {
            //    dummyint = int.Parse(anyString);
            //  }
            //  catch
            //  {
            //    return false;
            //  }
            //  return true;
            //}
            //else
            //  return false;
        }

        public static bool IsDate(object obj)
        {
            string strDate = "";
            bool ret = false;
            try
            {
                strDate = obj.ToString();
                DateTime dt;
                DateTime.TryParse(strDate, out dt);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                {
                    ret = true;
                }
            }
            catch
            {

            }
            return ret;
        }

        public static object GetRandom(string a)
        {
            object functionReturnValue = null;
            //get random figure for output as Single/Long/Integer
            VBMath.Randomize();
            functionReturnValue = "";
            switch (a)
            {
                case "S":
                case "L":
                    functionReturnValue = Conversion.Int((VBMath.Rnd() * 10000) + (VBMath.Rnd() * 100000) + (VBMath.Rnd() * 1000000) + (VBMath.Rnd() * 10000000));
                    break;
                case "I":
                    functionReturnValue = Conversion.Int((VBMath.Rnd() * 2000) + (VBMath.Rnd() * 10000) + (VBMath.Rnd() * 20000));
                    break;
            }
            return functionReturnValue;
        }

        private static bool GB_TBfindOK(string _table, string _var)
        {
            string str = string.Empty;
            bool OK = false;

            //ConnectionSQL(ref GB_cn, GB_dbcfg);
            GB_cn = DBHelper.SqlConnectionDbMAC5();
            if (GB_cn.State == ConnectionState.Closed)
                GB_cn.Open();

            try
            {
                str = "SELECT * FROM " + _table + " WHERE " + _table + "code = @code";
                GB_cmd = new SqlCommand(str, GB_cn);
                GB_cmd.Parameters.Add("@code", SqlDbType.NVarChar).Value = _var;
                GB_rd = GB_cmd.ExecuteReader();
                while (GB_rd.Read())
                {
                    OK = true;
                    break;
                }
            }
            catch
            {
                OK = false;
            }
            finally
            {
                //GB_rd.Close();

            }
            return OK;
        }

        public static DataTable GetVoucherData(string SelectTable, int mode, string Vno = "", string Vtype = "", string Vcus = "", int Vd = 0, int Vm = 0, int Vy = 0, string Squery = "")
        {
            SqlConnection cn = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string Xt = "";


            try
            {
                //ConnectionSQL(ref cn, dbcfg);

                cn = DBHelper.SqlConnectionDbMAC5();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                switch (mode)
                {
                    case 0:
                        switch (SelectTable)
                        {
                            case "MIH":
                                sql = "Select * From MIH Where MIHtype=@Htype order by MIHyear,MIHmonth,MIHday";
                                if (Squery.Length > 0)
                                {
                                    sql = Squery;
                                }
                                dt = new DataTable("MIH");
                                break;
                            case "MIE":
                                sql = "Select * From MIE Where MIEtype=@Htype";
                                break;
                            case "MIK":
                                sql = "Select * From MIK Where MIKtype=@Htype";
                                break;
                            case "MIL":
                                sql = "Select * From MIL Where MILtype=@Htype";
                                break;
                            case "MIR":
                                sql = "Select * From MIR Where MIRtype=@Htype";
                                break;
                        }
                        break;
                    case 1:
                        switch (SelectTable)
                        {
                            case "MIH":
                                sql = "Select * From MIH Where MIHday=@MIHday and MIHmonth=@MIHmonth and MIHyear=@MIHyear and MIHvnos=@MIHvnos and MIHtype=@MIHtype and MIHcus=@MIHcus";
                                if (Squery.Length > 0)
                                {
                                    sql += " and " + Squery;
                                }
                                Xt = "MIH";
                                dt = new DataTable("MIH");
                                break;
                            case "MIE":
                                sql = "Select * From MIE Where MIEday=@MIEday and MIEmonth=@MIEmonth and MIEyear=@MIEyear and MIEvnos=@MIEvnos and MIEtype=@MIEtype and MIEcus=@MIEcus";
                                Xt = "MIE";
                                dt = new DataTable("MIE");
                                break;
                            case "MIK":
                                sql = "Select * From MIK Where MIKday=@MIKday and MIKmonth=@MIKmonth and MIKyear=@MIKyear and MIKvnos=@MIKvnos and MIKtype=@MIKtype";
                                Xt = "MIK";
                                dt = new DataTable("MIK");
                                break;
                            case "MIL":
                                sql = "Select * From MIL Where MILday=@MILday and MILmonth=@MILmonth and MILyear=@MILyear and MILvnos=@MILvnos and MILtype=@MILtype and MILcus=@MILcus"
                                  + " order by MILlistNo";
                                Xt = "MIL";
                                dt = new DataTable("MIL");
                                break;
                            case "MIR":
                                sql = "Select * From MIR Where MIRday=@MIRday and MIRmonth=@MIRmonth and MIRyear=@MIRyear and MIRvnos=@MIRvnos and MIRtype=@MIRtype and MIRcus=@MIRcus"
                                  + " order by MIRlistNo";
                                Xt = "MIR";
                                dt = new DataTable("MIR");
                                break;
                            case "MIS":
                                sql = "Select * From MIS Where MISday=@MISday and MISmonth=@MISmonth and MISyear=@MISyear and MISvnos=@MISvnos and MIStype=@MIStype and MIScus=@MIScus"
                                  + " order by MISline";
                                Xt = "MIS";
                                dt = new DataTable("MIS");
                                break;
                        }
                        break;
                }

                da = new SqlDataAdapter(sql, cn);

                switch (mode)
                {
                    case 0:
                        if (Squery.Length == 0)
                        {
                            da.SelectCommand.Parameters.Add("@Htype", SqlDbType.NVarChar, 2).Value = Vtype;
                        }
                        break;
                    case 1:
                        switch (SelectTable)
                        {
                            case "MIH":
                            case "MIE":
                            case "MIL":
                            case "MIR":
                            case "MIS":
                                da.SelectCommand.Parameters.Add("@" + Xt + "day", SqlDbType.SmallInt).Value = Vd;
                                da.SelectCommand.Parameters.Add("@" + Xt + "month", SqlDbType.SmallInt).Value = Vm;
                                da.SelectCommand.Parameters.Add("@" + Xt + "year", SqlDbType.SmallInt).Value = Vy;
                                da.SelectCommand.Parameters.Add("@" + Xt + "vnos", SqlDbType.NVarChar, 15).Value = Vno;
                                da.SelectCommand.Parameters.Add("@" + Xt + "type", SqlDbType.NVarChar, 2).Value = Vtype;
                                da.SelectCommand.Parameters.Add("@" + Xt + "cus", SqlDbType.NVarChar, 15).Value = Vcus;
                                break;
                            case "MIK":
                                da.SelectCommand.Parameters.Add("@" + Xt + "day", SqlDbType.SmallInt).Value = Vd;
                                da.SelectCommand.Parameters.Add("@" + Xt + "month", SqlDbType.SmallInt).Value = Vm;
                                da.SelectCommand.Parameters.Add("@" + Xt + "year", SqlDbType.SmallInt).Value = Vy;
                                da.SelectCommand.Parameters.Add("@" + Xt + "vnos", SqlDbType.NVarChar, 15).Value = Vno;
                                da.SelectCommand.Parameters.Add("@" + Xt + "type", SqlDbType.NVarChar, 2).Value = Vtype;
                                break;
                        }
                        break;
                }

                da.Fill(dt);
            }
            catch
            {
            }
            finally
            {

            }

            return dt;
        }

        private static void MIE_UnparseStr(int XnoTerm, string XTermC, string XTermD, string XTermP, string XTermA, ref int[] ctermC, ref string[] ctermD, ref double[] ctermP, ref decimal[] ctermA)
        {
            bool OK = false;
            string Xs = string.Empty;
            string Xm = string.Empty;
            DateTime Xd;
            int j = 0;
            int k = 0;
            int i = 0;

            if (XnoTerm == 0)
            {

            }
            else
            {
                for (k = 1; k <= 4; k++)
                {
                    switch (k)
                    {
                        case 1:
                            Xs = XTermC;
                            break;
                        case 2:
                            Xs = XTermD;
                            break;
                        case 3:
                            Xs = XTermP;
                            break;
                        case 4:
                            Xs = XTermA;
                            break;
                    }
                    j = 0;

                    while (Xs.Length > 0 && (j < XnoTerm))
                    {
                        i = Xs.IndexOf("|", 0);
                        if (i >= 0)
                        {
                            Xm = Xs.Substring(0, i);
                            OK = false;
                        }
                        else
                        {
                            Xm = Xs;
                            OK = true;
                        }

                        j++;

                        switch (k)
                        {
                            case 1:
                            case 3:
                            case 4:
                                if (Xm.Length == 0) Xm = "0";
                                switch (k)
                                {
                                    case 1:
                                        ctermC[j - 1] = 0;
                                        if (IsNumeric(Xm)) ctermC[j - 1] = System.Convert.ToInt32(ZSnull(Xm).ToString());
                                        break;
                                    case 3:
                                        ctermP[j - 1] = 0;
                                        if (IsNumeric(Xm)) ctermP[j - 1] = System.Convert.ToDouble(ZSnull(Xm).ToString());
                                        break;
                                    case 4:
                                        ctermA[j - 1] = 0;
                                        if (IsNumeric(Xm)) ctermA[j - 1] = System.Convert.ToDecimal(ZSnull(Xm).ToString());
                                        break;
                                }
                                break;
                            case 2:
                                ctermD[j - 1] = "";
                                if (Xm.Length == 10)
                                {
                                    Xd = System.Convert.ToDateTime(Date_CvS10(Xm, false), Iformat);
                                    if (IsDate(Xd))
                                    {
                                        ctermD[j - 1] = System.Convert.ToString(Date_Show10(Xd, false));
                                    }
                                }
                                break;
                        }
                        if (OK)
                        {
                            Xs = "";
                        }
                        else
                        {
                            Xs = Xs.Substring(i + 1, Xs.Length - (i + 1));
                        }
                    }
                }
            }
        }

        private static void MIE_ParseStr(int XTermNo, int[] XTermC, string[] XTermD, double[] XTermP, decimal[] XTermA, ref string ctermC, ref string ctermD, ref string ctermP, ref string ctermA)
        {
            int i = 0;
            //DateTime Xdate;
            string Xs = string.Empty;
            string GBCT_XtermA = string.Empty;
            string GBCT_XtermD = string.Empty;
            string GBCT_XtermC = string.Empty;
            string GBCT_XtermP = string.Empty;

            if (XTermNo == 0)
            {
            }

            for (i = 0; i <= XTermNo - 1; i++)
            {
                if (!IsNumeric(XTermC[i])) XTermC[i] = 0;
                GBCT_XtermC = GBCT_XtermC + System.Convert.ToString(ZNnull(XTermC[i], 0)) + "|";

                if (IsDate(XTermD[i]))
                {
                    Xs = XTermD[i];
                }
                else
                {
                    Xs = "";
                }
                GBCT_XtermD = GBCT_XtermD + Xs + "|";
                //Xdate = Date_CvS10(XTermD(i), Xte)
                //if ISdate(Xdate) Then Xs = Date_Show10(Xdate, False) Else Xs = ""
                if (!IsNumeric(XTermP[i])) XTermP[i] = 0;
                GBCT_XtermP = GBCT_XtermP + System.Convert.ToString(ZNnull(XTermP[i], 0)) + "|";
                if (!IsNumeric(XTermA[i])) XTermA[i] = 0;
                GBCT_XtermA = GBCT_XtermA + System.Convert.ToString(ZNnull(XTermA[i], 0)) + "|";
            }

            ctermC = GBCT_XtermC;
            ctermD = GBCT_XtermD;
            ctermP = GBCT_XtermP;
            ctermA = GBCT_XtermA;
        }

        private string RunVC_Read(int Xm, int Xy, string Xtype, string Xno)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd = null;
            string Xs = "";
            string Xstr;
            int Xyear;
            string Xmonth = "";

            //clslib.ConnectionSQL(ref cn, Sqlconn);
            cn = DBHelper.SqlConnectionDbMAC5();
            if (cn.State == ConnectionState.Closed)
                cn.Open();

            //Xstr = "Select VRNvnos From VRN Where VRNno=@VRNno and VRNgrp=@VRNgrp";
            //2012-01-17 
            //Xstr = "Select top 1 MIHvnos From MIH Where MIHtype=@MIHtype And MIHmonth=@MIHmonth And MIHyear=@MIHyear";
            Xstr = "Select top 1 MIHvnos From MIH Where MIHtype=@MIHtype ";
            if (Xno.Length > 0)
            {
                Xstr += " and MIHvnos Like '" + Xno + "%'";
            }
            Xstr += " order by MIHvnos desc,MIHyear desc,MIHmonth desc,MIHday desc";
            cmd = new SqlCommand(Xstr, cn);
            cmd.CommandText = Xstr;
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.Text;
            //cmd.Transaction = tran;
            cmd.Parameters.Clear();
            //cmd.Parameters.Add("@MIHmonth", SqlDbType.SmallInt).Value = Xm;
            //cmd.Parameters.Add("@MIHyear", SqlDbType.SmallInt).Value = Xy;
            cmd.Parameters.Add("@MIHtype", SqlDbType.NVarChar, 2).Value = Xtype;
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    Xs = ZSnull(rd["MIHvnos"]).ToString();
                }
            }
            else
            {
                Xyear = DateTime.Now.Year;
                if (Xyear < 2500) Xyear = Xyear + 543;
                Xmonth = DateTime.Now.Month.ToString("00");
                switch (Xtype)
                {
                    case "PX":
                        Xs = "PR" + DBString(System.Convert.ToString(Xyear).Substring(2, 2)) + Xmonth + "-001";
                        break;
                    case "PP":
                        Xs = "PO" + DBString(System.Convert.ToString(Xyear).Substring(2, 2)) + Xmonth + "-001";
                        break;
                }
            }

            while (RunVC_ExistsMIvnos(Xtype, Xs, Xm, Xy))
            {
                Xs = SequenceStr(Xs);
            }
            return Xs;
        }

        private static bool RunVC_ExistsMIvnos(string Xtype, string Xno, int Xmonth, int Xyear)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd = null;
            bool OK = false;
            string SQL = string.Empty;

            //SQL="Select Top 1 MIHday From MIH Where MIHtype=@MIHtype and MIHvnos=@MIHvnos And MIHmonth=@MIHmonth And MIHyear=@MIHyear";
            SQL = "Select Top 1 MIHday From MIH Where MIHtype=@MIHtype and MIHvnos=@MIHvnos";
            try
            {
                //clslib.ConnectionSQL(ref cn, Sqlconn);
                cn = DBHelper.SqlConnectionDbMAC5();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand(SQL, cn);
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                //cmd.Transaction = tran;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@MIHvnos", SqlDbType.NVarChar, 15).Value = Xno;
                //cmd.Parameters.Add("@MIHmonth", SqlDbType.SmallInt).Value = Xmonth;
                //cmd.Parameters.Add("@MIHyear", SqlDbType.SmallInt).Value = Xyear;
                cmd.Parameters.Add("@MIHtype", SqlDbType.NVarChar, 2).Value = Xtype;
                rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    OK = true;
                }
                else
                {
                    OK = false;
                }
                if (!rd.IsClosed) rd.Close();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception e)
            {

                OK = false;
                //throw new FaultException(e.Message);
            }

            return OK;
        }

        public static string SequenceStr(string value)
        {
            string Xstr = value;
            object[] array = new object[1];
            Array.Clear(array, 0, array.Length);
            Xstr = Chr(255) + value;
            int NoS = Xstr.Length;
            Array.Resize(ref array, NoS);
            for (int i = 1; i < NoS; i++)
            {
                array[i] = Mid(Xstr, i, 1);
            }
            int j = 1;
            for (int i = NoS - 1; i > 1; i--)
            {

                int ch = Strings.Asc(Convert.ToString(array[i]));
                ch += j;
                switch (ch)
                {
                    case 91:
                        array[i] = Chr(65);
                        j = 1;
                        break;
                    case 207:
                        array[i] = Chr(161);
                        j = 1;
                        break;
                    case 58:
                        array[i] = Chr(48);
                        j = 1;
                        break;
                    case 256:
                        i = 1;
                        break;
                    default:
                        array[i] = Chr(ch);
                        i = 1;
                        break;

                }
                //break;
            }

            Xstr = "";
            for (int i = 1; i < NoS; i++)
            {
                Xstr += Convert.ToString(array[i]);
            }
            Xstr = XYZremoveFLCNL(Xstr, Chr(255));

            return Xstr;
        }

        public static string Chr(int Ascii)
        {
            return Convert.ToString(Convert.ToChar(Ascii));
        }

        public static object ZNnull(object Vx, object Amt)
        {
            //Check for null And assign NUMBER value
            //ZNnull is a variant
            object Ret;
            string dummystring;
            if (Vx == DBNull.Value)
                dummystring = "";
            else
                dummystring = Vx.ToString();
            try
            {
                if (!IsNumeric(dummystring))
                    Ret = Amt;
                else
                    Ret = Vx;
            }
            catch
            {
                Ret = "";
            }
            return Ret;
        }

        public static string ZSnull(object Vx)
        {
            string functionReturnValue = null;
            //Check for null And assign STRING
            try
            {
                if (Vx == null || Vx.Equals(DBNull.Value))
                    Vx = "";
                functionReturnValue = (Vx + "");
            }
            catch
            {
                return "";
            }
            return functionReturnValue;
        }

        public static DateTime ZDnulls(object Vx)
        {
            //Check for null And assign DATE
            DateTime Ret;
            Ret = IsDate(Vx) ? Convert.ToDateTime(Vx) : Ret = DateTime.MinValue;
            return Ret;
        }

        public static string[] returnarray(string[] old, int length)
        {
            string[] newarr = new string[length];
            Array.Copy(old, newarr, old.Length);
            return newarr;
        }

        public static int[] returnarray(int[] old, int length)
        {
            int[] newarr = new int[length];
            Array.Copy(old, newarr, old.Length);
            return newarr;
        }

        public static double[] returnarray(double[] old, int length)
        {
            double[] newarr = new double[length];
            Array.Copy(old, newarr, old.Length);
            return newarr;
        }

        public static decimal[] returnarray(decimal[] old, int length)
        {
            decimal[] newarr = new decimal[length];
            Array.Copy(old, newarr, old.Length);
            return newarr;
        }

        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            if (param.Length > 0)
                return param.Substring(0, length);
            else
                return ""; //return the result of the operation


        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            if (param.Length > 0)
                return param.Substring(param.Length - length, length);
            else
                return "";//return the result of the operation


        }

        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable

            string result = "";

            if (param.Length <= length)
                length = param.Length;
            result = param.Substring(startIndex, length);


            //return the result of the operation
            return result;
        }

        public static string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = param.Substring(startIndex);
            //return the result of the operation
            return result;
        }

        public static int InStr(string str1, string str2)
        {
            string StrTemp;
            int index = 0;
            if (str1.Length > 0)
            {
                while (str1.Length > 0)
                {
                    StrTemp = str1.Substring(0, 1);
                    str1 = str1.Remove(0, 1);
                    index += 1;
                    if (StrTemp.IndexOf(str2) > -1) return index;
                }
            }
            return 0;
        }

        public static string XYZremoveFLCNL(string Xs, string Sep)
        {
            //delete first/last enter-string And Sep. characters
            string Xstr = null;
            //, i%, j%

            Xstr = (Xs);
            while ((Left(Xstr, 1) == Chr(10)) | (Left(Xstr, 1) == Chr(13)))
            {
                Xstr = (Right(Xstr, Xstr.Length - 1));
            }
            while ((Right(Xstr, 1) == Chr(10)) | (Right(Xstr, 1) == Chr(13)))
            {
                Xstr = (Left(Xstr, Xstr.Length - 1));
            }
            if (Sep.Length > 0)
            {
                while (Left(Xstr, 1) == Sep)
                {
                    Xstr = (Right(Xstr, Xstr.Length - 1));
                }
                while (Right(Xstr, 1) == Sep)
                {
                    Xstr = (Left(Xstr, Xstr.Length - 1));
                }
            }
            return (Xstr);
        }
        #endregion

    }
}
