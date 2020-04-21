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
    public interface IPRData
    {
        DataTable GetDepData(ref string errMsg);
        DataTable GetCategoryData(ref string errMsg);
        DataTable GetObjectiveData(int id, ref string errMsg);
        DataTable GeneratePRNo(int id, ref string errMsg);
        DataTable GetJobData(ref string errMsg);
        DataTable GetAccountData(ref string errMsg);
        DataTable GetDetailData(int Document_Detail_Hid, int tmp, ref string errMsg);
        DataTable GetHeaderData(int Document_id, int tmp, ref string errMsg);
        int InsertHeader(PRHeaderModels Header, ref string errMsg);
        int InserttmpDetail(PRDetailModels detail, ref string errMsg);
        int DeleteTmpDetail(int Hid, ref string errMsg);
        DataTable GetPRData(int DeptID, ref string errMsg);
        DataTable GetPRDataForApprove(int DeptID, ref string errMsg);
        int ApprovePR(int Document_Id, int StaffID, ref string errMsg);
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
        public DataTable GetDetailData(int Document_Detail_Hid,int tmp, ref string errMsg)
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
        public DataTable GetHeaderData(int Document_id,int tmp, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";
            if (tmp == 0)
            { tablename = "DocumentPR_Header_tmp"; }
           
            try
            {
                string strSQL = "\r\n  " +
                      " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate,d.DEPdescT AS Dep,j.JOBdescT As Job ,g.GroupName AS 'Group'" +
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
        public DataTable GetPRData(int DeptID, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";

            try
            {
                string strSQL = "\r\n  " +
                      " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                      ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,d.DEPdescT,j.JOBdescT" +
                      " FROM " + tablename + " p " +
                      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                      " where Document_Delete=0 ";
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "ListPRData";

            return dt;
        }
        public DataTable GetPRDataForApprove(int DeptID, ref string errMsg)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";

            try
            {
                string strSQL = "\r\n  " +
                      " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                      ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,d.DEPdescT,j.JOBdescT" +
                      " FROM " + tablename + " p " +
                      " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                      " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                      " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                      " where Document_Delete=0 AND Document_Status<2 ";
                dt = DBHelper.List(strSQL);
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

                string sql = "Select SUM(Document_Detail_Cog) From DocumentPR_Detail_tmp WHERE Document_Detail_Hid = " + Header.Document_Id;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Transaction = myTran;
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                foreach (DataRow dr in tmp.Rows)
                {
                    amount = Convert.ToDecimal(dr[0] == DBNull.Value? 0: dr[0]);
                }

                string sqlQuery = "INSERT INTO DocumentPR_Header(Document_Group,Document_Category,Document_Objective " +
                                      ",Document_Vnos,Document_Date ,Document_Means,Document_Expect,Document_Cus,Document_Job,Document_Dep,Document_Per" +
                                      ",Document_Doc,Document_Mec,Document_Desc,Document_Nolist,Document_Cog,Document_VatSUM,Document_VatPer" +
                                      " ,Document_NetSUM,Document_Status,Document_Tel,Document_CreateUser,Document_CreateDate,Document_Delete) VALUES " +
                                      " (@Document_Group,@Document_Category,@Document_Objective " +
                                      ",dbo.GeneratePRID(@Document_Group),GETDATE() ,@Document_Means,@Document_Expect,@Document_Cus,@Document_Job,@Document_Dep,@Document_Per" +
                                      ",@Document_Doc,@Document_Mec,@Document_Desc,@Document_Nolist,@Document_Cog,@Document_VatSUM,@Document_VatPer" +
                                      " ,@Document_NetSUM,@Document_Status,@Document_Tel,@Document_CreateUser,GETDATE(),0) SET @Document_Id=SCOPE_IDENTITY()";
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

                cmd.Parameters.AddWithValue("@Document_Cus", "");
                cmd.Parameters.AddWithValue("@Document_Job", Header.Document_Job);
                cmd.Parameters.AddWithValue("@Document_Dep", Header.Document_Dep);
                cmd.Parameters.AddWithValue("@Document_Per", "");
                cmd.Parameters.AddWithValue("@Document_Doc", "");
                cmd.Parameters.AddWithValue("@Document_Mec", "");
                cmd.Parameters.AddWithValue("@Document_Desc", Header.Document_Desc);
                cmd.Parameters.AddWithValue("@Document_Nolist", 0);
                cmd.Parameters.AddWithValue("@Document_Cog", amount);
                cmd.Parameters.AddWithValue("@Document_VatSUM", amount * (decimal)0.07);//()
                cmd.Parameters.AddWithValue("@Document_VatPer", 7);
                cmd.Parameters.AddWithValue("@Document_NetSUM", amount * (decimal)1.07);//
                cmd.Parameters.AddWithValue("@Document_Status", 0);
                cmd.Parameters.AddWithValue("@Document_Tel", Header.Document_Tel == null ? "" : Header.Document_Tel);
                cmd.Parameters.AddWithValue("@Document_CreateUser", Header.Document_CreateUser);
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
                cmd.Parameters.AddWithValue("@Document_Detail_Quan", detail.Document_Detail_Quan);
                cmd.Parameters.AddWithValue("@Document_Detail_UnitPrice", detail.Document_Detail_UnitPrice);
                cmd.Parameters.AddWithValue("@Document_Detail_Cog", detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice);
                
                cmd.Parameters.AddWithValue("@Document_Detail_Vat", (detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice)*(decimal)0.07);
                cmd.Parameters.AddWithValue("@Document_Detail_Sum", (detail.Document_Detail_Quan * detail.Document_Detail_UnitPrice) * (decimal)1.07);

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
        public int ApprovePR(int Document_Id,int StaffID, ref string errMsg)
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

                 sqlQuery = "Update DocumentPR_Header SET " +
                            "Document_EditUser = @Document_EditUser,Document_EditDate=GETDATE(),Document_Status =2 WHERE Document_Id = @Document_Id";
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

        #endregion

    }
}
