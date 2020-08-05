using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        DataTable GetListPOForApprove(int id, ref string errMsg);
        DataTable GetPOHeaderData(int Document_id, int staffid, ref string errMsg);
        DataTable GetDetailData(int Document_Detail_Hid, ref string errMsg);
        DataTable GetCustomer(int id, ref string errMsg);
        DataTable GetSTKData(ref string errMsg);
    }

    public class POData : IPOData
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
                    " SELECT STKcode AS Code, STKdescT1 AS Name " +
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
            { tablename = "DocumentPO_Detail_tmp"; }
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
                      " SELECT distinct p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,CAST(d.DEPdescT as NVARCHAR(max)) AS Dep,CAST(j.JOBdescT as NVARCHAR(max)) As Job ,g.GroupName AS 'Group'" +
                      " , Objective_Name AS Objective,Category_Name AS Category" +
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
                      " , Objective_Name AS Objective,Category_Name AS Category " +
                      " ,'อนุมัติ' AS SaveText " +
                      " ,CONCAT(CREcode,' : ',CAST(cus.CREnameT as NVARCHAR(max))) AS Customer " +
                      " FROM " + tablename + " p LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode = p.Document_Job" +
                      " LEFT JOIN Department d on d.DEPid = p.Document_Dep" +
                      " LEFT JOIN Category c on c.Category_Id = p.Document_Category" +
                      " LEFT JOIN Objective o on o.Objective_Id = p.Document_Objective" +
                      " LEFT JOIN Document_Group g on g.id=p.Document_Group" +
                      " LEFT JOIN CRE cus on cus.CREcode=p.Document_Cus" +
                      " where Document_Delete=0 AND Document_Id =" + Document_id;
                dt = DBHelper.List(strSQL);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DEPid = DBInt(dr["Document_Dep"]);
                        DocCog = Convert.ToDecimal(dr["Document_Cog"]);
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
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Header";
            int staffLevel = 0;
            try
            {

                string sql = "Select * from Staffs WHERE StaffID = " + id;
                DataTable staff = DBHelper.List(sql);
                if (staff.Rows.Count > 0)
                {
                    foreach (DataRow dr in staff.Rows)
                    {
                        //staffLevel = Convert.ToInt32(dr["StaffLevelID"]) - 1;
                        staffLevel = Convert.ToInt32(dr["StaffLevel"]) - 1;
                    }
                }
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

            dt.TableName = "ListPRData";

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



                if (cog > Math.Round(detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice, 2))
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
                        PRID = DBInt(dr["Document_PRID"]);
                        DocPO = DBString(dr["Document_Vnos"]);
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

                }
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
        public int DBInt(object obj)
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
        public string DBString(object obj)
        {
            string str;

            str = obj == DBNull.Value ? "" : Convert.ToString(obj);

            return str;
        }
        #endregion

    }
}
