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
    public interface IStaffData
    {
        int GetCheckUniqe(string tableName, string columnName, string wheredata, string colFiled, int id);
        int GetCheckUniqeLogin(string tableName, string columnName, string wheredata, string colFiled, int id);
        int GetCheckUniqeAuthorize(string tableName, string columnName, string wheredata, string colFiled, int id);

        string ReplaceString(String text);

        DataTable Login(string username, string password, ref string errMsg);

        DataTable GetStaffData(ref string errMsg);

        DataTable GetStaffDataByID(ref string errMsg, int StaffID);

        DataTable GetPermissionData(ref string errMsg);

        DataTable GetPermissionDataByID(ref string errMsg, int DEPid);

        DataTable GetDepartmentData(ref string errMsg);

        DataTable GetDepartmentDataByID(ref string errMsg,int POSid);
        
        DataTable GetStaffAuthorizeData(ref string errMsg);

        DataTable GetStaffAuthorizeDataByID(ref string errMsg, int Authorizeid);

        Task<bool> SetDepartmentData(Department item);

        Task<bool> SetPositionData(Position item);

        Task<bool> SetStaffData(StaffModels item);

        Task<bool> DeleteDepartment(int id);

        Task<bool> DeleteStaff(int id);

        Task<bool> DeleteStaffAuthorize(int id);

        Task<bool> SetStaffAuthorize(StaffAuthorize item);

        Task<int> GetPRforApprove(int StaffID);
        Task<int> GetPROverDataForApprove(int id);
        Task<int> GetListPreview(int StaffID);
        Task<int> GetListPOForApprove(int id);

        Task<DataTable> GetBudgetByDep(int id);
        bool SetBudget(BudgetByDep item);
    }

    public class StaffData : IStaffData
    {
        #region Login
        public DataTable Login(string username, string password, ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT s.*, isPreview " +
                         "\r\n FROM Staffs s " +
                         "\r\n Left join StaffAuthorize a on a.StaffID = s.StaffID" +
                         "\r\n WHERE s.StaffLogin='" + username + "'" +
                         "\r\n AND s.StaffPassword='" + Base64Encode(password) + "'" +
                         "\r\n AND s.Deleted=0;";
                dt = DBHelper.List(strSQL);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "StaffLogin";
            return dt;
        }
        #endregion

        #region Staff
        public int GetCheckUniqe(string tableName, string columnName, string wheredata,string colFiled, int id)
        {


            try
            {

                string StrSql = @" Select " + columnName + " From " + tableName + "   where (FLG_DEL=0 or FLG_DEL is null) and 0=0  " + wheredata;
                if (id > 0) StrSql += " and " + colFiled + "<>" + id;
                DataTable dt = DBHelper.List(StrSql);
                return dt.Rows.Count;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }

        public int GetCheckUniqeLogin(string tableName, string columnName, string wheredata, string colFiled, int id)
        {


            try
            {

                string StrSql = @" Select " + columnName + " From " + tableName + "   where (Deleted=0 or Deleted is null) and 0=0  " + wheredata;
                if (id > 0) StrSql += " and " + colFiled + "<>" + id;
                DataTable dt = DBHelper.List(StrSql);
                return dt.Rows.Count;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }

        public int GetCheckUniqeAuthorize(string tableName, string columnName, string wheredata, string colFiled, int id)
        {
            try
            {

                string StrSql = @" Select " + columnName + " From " + tableName + "  where  0=0  " + wheredata;
                if (id > 0) StrSql += " and " + colFiled + "<>" + id;
                DataTable dt = DBHelper.List(StrSql);
                return dt.Rows.Count;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }

        public DataTable GetStaffData( ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT a.*,CONCAT(a.StaffFirstName,' ',a.StaffLastName) AS StaffName,b.DEPdescT FROM Staffs a inner join Department b on a.StaffDepartmentID = b.DEPid " +
                         "\r\n WHERE a.Deleted=0 order by a.StaffFirstName;";
                dt = DBHelper.List(strSQL);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["StaffPassword"] = Base64Decode(dr["StaffPassword"].ToString());
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "StaffData";
            return dt;
        }

        public DataTable GetStaffDataByID(ref string errMsg,int StaffID)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT * FROM Staffs " +
                         "\r\n WHERE StaffID=" + StaffID + " and Deleted=0;";
                dt = DBHelper.List(strSQL);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["StaffPassword"] = Base64Decode(dr["StaffPassword"].ToString());
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "StaffData";
            return dt;
        }

        public DataTable GetPermissionData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM PositionPermission ";
                dt = DBHelper.List(strSQL);

              
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "PositionData";
            return dt;
        }

        public DataTable GetPermissionDataByID(ref string errMsg, int POSid)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM PositionPermission where Positionid=" + POSid + "";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "PositionData";
            return dt;
        }

        public DataTable GetDepartmentData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM Department where FLG_DEL=0 or FLG_DEL is null order by DEPcode ";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "DepartmentData";
            return dt;
        }

        public DataTable GetDepartmentDataByID(ref string errMsg,int DEPid)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM Department where DEPid=" + DEPid + " and FLG_DEL =0 or FLG_DEL is null order by DEPcode ";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "DepartmentData";
            return dt;
        }

        public DataTable GetStaffAuthorizeData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT a.*,CONCAT(b.StaffFirstName,' ',b.StaffLastName) AS StaffName FROM StaffAuthorize a inner join Staffs b on a.StaffID=b.StaffID order by StaffCode,DEPid ";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "StaffAuthorizeData";
            return dt;
        }

        public DataTable GetStaffAuthorizeDataByID(ref string errMsg, int Authorizeid)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM StaffAuthorize where Authorizeid=" + Authorizeid + " order by StaffCode,DEPid ";
                dt = DBHelper.List(strSQL);


            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            dt.TableName = "StaffAuthorizeData";
            return dt;
        }
        #endregion


        #region Set Staff
        public async Task<bool> SetStaffData(StaffModels item)
        {
            bool result = false;
            string strSQL = null;
            DataTable dt = new DataTable();

            if (item.StaffID == 0)
            {
                strSQL = "Insert Into Staffs (StaffLogin,StaffPassword,StaffCode,StaffFirstName,StaffLastName,StaffPosition,StaffLevel,StaffDepartmentID,InputDate,UpdateDate) VALUES (@StaffLogin,@StaffPassword,@StaffCode,@StaffFirstName,@StaffLastName,@StaffPosition,@StaffLevel,@StaffDepartmentID,@InputDate,@UpdateDate)";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@StaffLogin", SqlDbType = SqlDbType.NVarChar, Value= item.StaffLogin},
                    new SqlParameter() {ParameterName = "@StaffPassword", SqlDbType = SqlDbType.NVarChar, Value = Base64Encode(item.StaffPassword)},
                    new SqlParameter() {ParameterName = "@StaffCode", SqlDbType = SqlDbType.NVarChar, Value = item.StaffCode.ToUpper()},
                    new SqlParameter() { ParameterName = "@StaffFirstName", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffFirstName)},
                    new SqlParameter() { ParameterName = "@StaffLastName", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffLastName) },
                    new SqlParameter() { ParameterName = "@StaffPosition", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffPosition) },
                    new SqlParameter() { ParameterName = "@StaffLevel", SqlDbType = SqlDbType.TinyInt, Value = item.StaffLevel },
                    new SqlParameter() { ParameterName = "@StaffDepartmentID", SqlDbType = SqlDbType.Int, Value = item.StaffDepartmentID },
                    new SqlParameter() { ParameterName = "@InputDate", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now },
                    new SqlParameter() { ParameterName = "@UpdateDate", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now }
                };
                DBHelper.Execute(strSQL, sp);
            }
            else
            {
                strSQL = "UPDATE Staffs SET StaffLogin=@StaffLogin,StaffPassword=@StaffPassword,StaffCode=@StaffCode,StaffFirstName=@StaffFirstName,StaffLastName=@StaffLastName,StaffPosition=@StaffPosition,StaffLevel=@StaffLevel,StaffDepartmentID=@StaffDepartmentID,UpdateDate=GETDATE() WHERE StaffID=@StaffID";

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@StaffID", SqlDbType = SqlDbType.VarChar, Value= item.StaffID},
                    new SqlParameter() {ParameterName = "@StaffLogin", SqlDbType = SqlDbType.NVarChar, Value= item.StaffLogin},
                    new SqlParameter() {ParameterName = "@StaffPassword", SqlDbType = SqlDbType.NVarChar, Value = Base64Encode(item.StaffPassword)},
                    new SqlParameter() {ParameterName = "@StaffCode", SqlDbType = SqlDbType.NVarChar, Value = item.StaffCode.ToUpper()},
                    new SqlParameter() { ParameterName = "@StaffFirstName", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffFirstName)},
                    new SqlParameter() { ParameterName = "@StaffLastName", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffLastName) },
                    new SqlParameter() { ParameterName = "@StaffPosition", SqlDbType = SqlDbType.NVarChar, Value = DBString(item.StaffPosition)},
                    new SqlParameter() { ParameterName = "@StaffLevel", SqlDbType = SqlDbType.TinyInt, Value = item.StaffLevel},
                    new SqlParameter() { ParameterName = "@StaffDepartmentID", SqlDbType = SqlDbType.Int, Value = item.StaffDepartmentID}
                };
                DBHelper.Execute(strSQL, sp);                
            }
            return true;
        }

        public async Task<bool> SetDepartmentData(Department item)
        {
            bool result = false;
            string strSQL = null;
            DataTable dt = new DataTable();

            if (item.DEPid == 0)
            {
                strSQL = "Insert Into Department (DEPid,DEPCode,DEPdescT,DEPdescE,FLG_DEL,DEPeditDT,DEPGroupID,DEPhide,DEPlock) select isnull(max(DEPid),0) +1 ,@DEPCode,@DEPdescT,@DEPdescE,0 ,GETDATE(),0,0,0 from Department";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@DEPCode", SqlDbType = SqlDbType.VarChar, Value= item.DEPcode},
                    new SqlParameter() {ParameterName = "@DEPdescT", SqlDbType = SqlDbType.NVarChar, Value = item.DEPdescT},
                    new SqlParameter() {ParameterName = "@DEPdescE", SqlDbType = SqlDbType.NVarChar, Value = item.DEPdescE}
                };
                DBHelper.Execute(strSQL, sp);
            }
            else
            {
                strSQL = "UPDATE Department SET DEPCode=@DEPCode,DEPdescT=@DEPdescT,DEPdescE=@DEPdescE,DEPeditDT=GETDATE() WHERE DEPid=@DEPid";

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@DEPid", SqlDbType = SqlDbType.VarChar, Value= item.DEPid},
                    new SqlParameter() {ParameterName = "@DEPCode", SqlDbType = SqlDbType.VarChar, Value= item.DEPcode},
                    new SqlParameter() {ParameterName = "@DEPdescT", SqlDbType = SqlDbType.NVarChar, Value = item.DEPdescT},
                    new SqlParameter() {ParameterName = "@DEPdescE", SqlDbType = SqlDbType.NVarChar, Value = item.DEPdescE}
                };
                DBHelper.Execute(strSQL, sp);
            }
            return true;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            bool result = false;
            string strSQL = null;


            strSQL = "UPDATE Department SET FLG_DEL=1 WHERE DEPid="+id;

            DBHelper.Execute(strSQL);

            return true;
        }

        public async Task<bool> DeleteStaffAuthorize(int id)
        {
            bool result = false;
            string strSQL = null;


            strSQL = "Delete from StaffAuthorize WHERE Authorizeid=" + id;

            DBHelper.Execute(strSQL);

            return true;
        }

        public async Task<bool> DeleteStaff(int id)
        {
            bool result = false;
            string strSQL = null;


            strSQL = "UPDATE Staffs SET Deleted=1 WHERE StaffID=" + id;

            DBHelper.Execute(strSQL);

            return true;
        }

        #endregion
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public String ReplaceString(String text)
        {
            String data = "";
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }
            data = text.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            data = data.Replace("'-'", "");
            data = data.Replace("''", "");
            data = data.Replace("'&'", "");
            data = data.Replace("'*'", "");
            data = data.Replace("' or''-'", "");
            data = data.Replace("' or 'x'='x", "");
            data = data.Replace("' or 'x'='x", "");

            return "'" + data + "'";
        }

        public string DBString(object obj)
        {
            string str;

            str = obj == DBNull.Value ? "" : Convert.ToString(obj);

            return str;
        }

        public string GetNameFromTB(int id, string tb, string xf)
        {
            string strSQL = null;
            DataTable dt = new DataTable();
            string Xcap = "";
            string table = "";
            string tableID = "";

            try
            {
                strSQL = "\r\n SELECT * FROM Department where FLG_DEL=0 or FLG_DEL is null order by DEPcode ";
                dt = DBHelper.List(strSQL);

                switch (tb)
                {
                    case "Department":
                        table = "Department";
                        tableID = "DEPid";
                        break;
                    case "Staffs":
                        table = "Staffs";
                        tableID = "StaffID";
                        break;
                    case "PositionPermission":
                        table = "PositionPermission";
                        tableID = "Positionid";
                        break;
                }

                strSQL = "Select " + xf + " From " + table;
                strSQL += " where " + tableID + "=" + id;
                dt = DBHelper.List(strSQL);

                if (dt.Rows.Count > 0)
                {
                    Xcap = Convert.ToString(dt.Rows[0][xf]);
                }

            }
            catch (Exception e)
            {
                Xcap = "";
            }

            return Xcap;
        }

        #region " ตำแหน่ง "
        public async Task<bool> SetPositionData(Position item)
        {
            bool result = false;
            string strSQL = null;
            DataTable dt = new DataTable();

            if (item.Positionid == 0)
            {
                strSQL = "Insert Into PositionPermission (PositionCode,PositionName,PositionLimit) VALUES (@PositionCode,@PositionName,@PositionLimit)";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@PositionCode", SqlDbType = SqlDbType.NVarChar, Value= item.Positioncode},
                    new SqlParameter() {ParameterName = "@PositionName", SqlDbType = SqlDbType.NVarChar, Value = item.PositionName},
                    new SqlParameter() {ParameterName = "@PositionLimit", SqlDbType = SqlDbType.Decimal, Value = item.PositionLimit}
                };
                DBHelper.Execute(strSQL, sp);
            }
            else
            {
                strSQL = "UPDATE PositionPermission SET PositionCode=@PositionCode,PositionName=@PositionName,PositionLimit=@PositionLimit WHERE Positionid=@Positionid";

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Positionid", SqlDbType = SqlDbType.VarChar, Value= item.Positionid},
                    new SqlParameter() {ParameterName = "@PositionCode", SqlDbType = SqlDbType.VarChar, Value= item.Positioncode},
                    new SqlParameter() {ParameterName = "@PositionName", SqlDbType = SqlDbType.NVarChar, Value = item.PositionName},
                    new SqlParameter() {ParameterName = "@PositionLimit", SqlDbType = SqlDbType.Decimal, Value = item.PositionLimit}
                };
                DBHelper.Execute(strSQL, sp);
            }
            return true;
        }
        #endregion

        #region " บทบาทหน้าที่ "
        public async Task<bool> SetStaffAuthorize(StaffAuthorize item)
        {
            bool result = false;
            string strSQL = null;
            DataTable dt = new DataTable();

            item.StaffCode = GetNameFromTB(item.StaffID, "Staffs", "StaffCode");
            item.DEPdescT = GetNameFromTB(item.DEPid, "Department", "DEPdescT");
            item.PositionCode = GetNameFromTB(item.PositionPermissionId, "PositionPermission", "PositionCode");

            if (item.Authorizeid == 0)
            {
                strSQL = "Insert Into StaffAuthorize (StaffID,StaffCode,DEPid,DEPdescT,PositionPermissionId,PositionCode,PositionLimit,AuthorizeLevel,isPreview) VALUES (@StaffID,@StaffCode,@DEPid,@DEPdescT,@PositionPermissionId,@PositionCode,@PositionLimit,@AuthorizeLevel,@isPreview)";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@StaffID", SqlDbType = SqlDbType.Int, Value= item.StaffID},
                    new SqlParameter() {ParameterName = "@StaffCode", SqlDbType = SqlDbType.NVarChar, Value= item.StaffCode.ToUpper()},
                    new SqlParameter() {ParameterName = "@DEPid", SqlDbType = SqlDbType.Int, Value = item.DEPid},
                    new SqlParameter() {ParameterName = "@DEPdescT", SqlDbType = SqlDbType.NVarChar, Value= item.DEPdescT},
                    new SqlParameter() {ParameterName = "@PositionPermissionId", SqlDbType = SqlDbType.Int, Value = item.PositionPermissionId},
                    new SqlParameter() {ParameterName = "@PositionCode", SqlDbType = SqlDbType.NVarChar, Value= item.PositionCode.ToUpper()},
                    new SqlParameter() {ParameterName = "@PositionLimit", SqlDbType = SqlDbType.Decimal, Value = item.PositionLimit},
                    new SqlParameter() {ParameterName = "@AuthorizeLevel", SqlDbType = SqlDbType.TinyInt, Value = item.AuthorizeLevel},
                    new SqlParameter() {ParameterName = "@isPreview", SqlDbType = SqlDbType.Bit, Value = item.isPreview}
                };
                DBHelper.Execute(strSQL, sp);
            }
            else
            {
                strSQL = "UPDATE StaffAuthorize SET StaffID=@StaffID,StaffCode=@StaffCode,DEPid=@DEPid,DEPdescT=@DEPdescT,PositionPermissionId=@PositionPermissionId,PositionCode=@PositionCode,PositionLimit=@PositionLimit,AuthorizeLevel=@AuthorizeLevel,isPreview=@isPreview WHERE Authorizeid=@Authorizeid";

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Authorizeid", SqlDbType = SqlDbType.VarChar, Value= item.Authorizeid},
                    new SqlParameter() {ParameterName = "@StaffID", SqlDbType = SqlDbType.Int, Value= item.StaffID},
                    new SqlParameter() {ParameterName = "@StaffCode", SqlDbType = SqlDbType.NVarChar, Value= item.StaffCode.ToUpper()},
                    new SqlParameter() {ParameterName = "@DEPid", SqlDbType = SqlDbType.Int, Value = item.DEPid},
                    new SqlParameter() {ParameterName = "@DEPdescT", SqlDbType = SqlDbType.NVarChar, Value= item.DEPdescT},
                    new SqlParameter() {ParameterName = "@PositionPermissionId", SqlDbType = SqlDbType.Int, Value = item.PositionPermissionId},
                    new SqlParameter() {ParameterName = "@PositionCode", SqlDbType = SqlDbType.NVarChar, Value= item.PositionCode.ToUpper()},
                    new SqlParameter() {ParameterName = "@PositionLimit", SqlDbType = SqlDbType.Decimal, Value = item.PositionLimit},
                    new SqlParameter() {ParameterName = "@AuthorizeLevel", SqlDbType = SqlDbType.TinyInt, Value = item.AuthorizeLevel},
                    new SqlParameter() {ParameterName = "@isPreview", SqlDbType = SqlDbType.Bit, Value = item.isPreview}
                };
                DBHelper.Execute(strSQL, sp);
            }
            return true;
        }
        #endregion

        #region StaffNoti
        public async Task<int> GetPRforApprove(int StaffID) {
            int result = 0;

            string Depin = "";
            string tablename = "DocumentPR_Header";
            int staffLevel = 0;

            string sql = " SELECT DEPid,AuthorizeLevel FROM StaffAuthorize WHERE StaffID = " + StaffID;
            DataTable staffauth = DBHelper.List(sql);
            foreach (DataRow drow in staffauth.Rows)
            {

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


                        string strSQL = "\r\n  SELECT distinct * FROM (SELECT aa.*, CASE WHEN  a.Current_Level IS NULL THEN aa.StaffLevel ELSE a.Current_Level END AS Document_Level FROM  (" +
                          "(SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                          ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT " +
                          " FROM " + tablename + " p " +
                          " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                          " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                          " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                          " where Document_Delete=0 AND Document_Status<2 " +
                          " AND Document_Cog <=" + Dep_Budget + " And p.Document_Dep = '" + Depin + "'" +
                          ") UNION ALL (" +
                          " SELECT p.*,convert(nvarchar(MAX), Document_Date, 105) AS DocDate" +
                          ", CONCAT(s.StaffFirstName,' ',StaffLastName)  AS Staff,s.StaffLevel,CAST(d.DEPdescT as NVARCHAR(max)) AS DEPdescT,CAST(j.JOBdescT as NVARCHAR(max)) As JOBdescT" +
                          " FROM " + tablename + " p " +
                          " LEFT JOIN Staffs s on s.StaffID=p.Document_CreateUser " +
                          " LEFT JOIN JOB j on j.JOBcode=p.Document_Job " +
                          " LEFT JOIN Department d on d.DEPid=p.Document_Dep" +
                          " LEFT JOIN ApprovePROverBudget a on a.Approve_Documen_Id=p.Document_Id " +
                          " where Document_Delete=0 AND Document_Status < 2 AND a.Approve_Status = 2" +
                          " AND Document_Cog > " + Dep_Budget + " And p.Document_Dep in ( " + Depin + ")" + ")) aa  " +
                          " left join (SELECT Approve_Documen_Id, MAX(Approve_Current_Level) AS Current_Level" +
                          " FROM ApprovePR GROUP BY Approve_Documen_Id) a on aa.Document_Id = a.Approve_Documen_Id) bb WHERE Document_Level =" + staffLevel;


                        DataTable dt = DBHelper.List(strSQL);
                        result = result + dt.Rows.Count;
                    }
                }

            }

            return result;
        }
        public async Task<int> GetPROverDataForApprove(int id)
        {
            DataTable dt = new DataTable();
            string tablename = "DocumentPR_Header";
            int result = 0;

                string  DeptID = "";
                string sql = " SELECT DEPid,AuthorizeLevel FROM StaffAuthorize WHERE StaffID = " + id;
                DataTable staffauth = DBHelper.List(sql);
                foreach (DataRow drow in staffauth.Rows)
                {

                    DeptID = drow["DEPid"].ToString();

                    sql = "Select * from BudgetOfYearByDepartment WHERE DEPid = " + DeptID;
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
                                  " AND p.Document_Cog >" + Dep_Budget + " And p.Document_Dep= " + DeptID;
                                dt = DBHelper.List(strSQL);

                                result = result + dt.Rows.Count;
                            }
                        }
                        else
                        {
                            //errMsg = "ไม่สารมารถดูข้อมูลการอนุมัติเอกสารได้เนื่องจากไม่มีการตั้งค่างบประมาณของแผนก.";
                        }
                    }
                }
                //else
                //{ errMsg = "คุณไม่มีสิทธิ์ในการอนุมัติเอกสารเกินงบประมาณ"; }



            //dt.TableName = "ListPRData";

            return result;
        }
        public async Task<int> GetListPreview(int StaffID)
        {
            DataTable dt = new DataTable();
            int result = 0; 


                string strSQL = "SELECT l.*,pr.Document_Vnos AS PRNo,pr.Document_Cog AS PRAmount,po.Document_Vnos AS PONo,po.Document_Cog AS POAmount " +
                    ",po.Document_EditDate AS POapprove,po.Document_EditUser AS POapproveBy,po.Document_Id AS POID,pr.Document_Id AS PRID" +
                    " FROM LogPreview l " +
                    " Left Join DocumentPR_Header pr on pr.Document_Id = Document_PRId" +
                    " Left Join DocumentPO_Header po on po.Document_PRID = pr.Document_Id" +
                    " WHERE l.Document_PreviewUser = " + StaffID + "  and po.Document_Status < 2";


                dt = DBHelper.List(strSQL);

            result = dt.Rows.Count;






            return result;
        }
        public async Task<int> GetListPOForApprove(int id)
        {
            DataTable dtPO = new DataTable("PO");
            DataTable dt = new DataTable();
            string tablename = "DocumentPO_Header";
            int staffLevel = 0;
            string Depin = "";
            int irow = 0;
            int result = 0;



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

            result = dt.Rows.Count;

            return result;
        }

        #endregion

        #region Budget Dep
        public async Task<DataTable> GetBudgetByDep(int id)
        {
            DataTable dt = new DataTable();

            string sql = " ";
            DataTable staffauth = DBHelper.List(sql);

            string strSQL = "\r\n   select d.depid,depcode,depdesct,a.*  from department d "+
                            "\r\n left join(SELECT id,DEPid, BudgetYear, DEPmonth1, DEPmonth2, DEPmonth3, DEPmonth4, DEPmonth5, DEPmonth6, DEPmonth7" +
                            "\r\n, DEPmonth8, DEPmonth9, DEPmonth10, DEPmonth11, DEPmonth12, DEPTotalbudget " +
                            "\r\n FROM dbo.BudgetOfYearByDepartment AS b where BudgetYear = "+id+") a on a.depid = d.depid " +
                            "\r\n where d.FLG_DEL = 0";

            dt = DBHelper.List(strSQL);
            dt.TableName = "Budget";
            return dt;
        }

        public bool SetBudget(BudgetByDep item)
        {
            bool result = false;
            string strSQL = "";



            if (item.id == 0)
            {
                strSQL = "Insert Into BudgetOfYearByDepartment (BudgetYear,DEPid,DEPcode,DEPdescT,"+item.ColumnName+")" +
                    " VALUES (@BudgetYear,@DEPid,@DEPcode,@DEPdescT,@Budget)";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@BudgetYear", SqlDbType = SqlDbType.Int, Value= item.Year},
                    new SqlParameter() {ParameterName = "@DEPid", SqlDbType = SqlDbType.Int, Value= item.DEPid},
                    new SqlParameter() {ParameterName = "@DEPcode", SqlDbType = SqlDbType.NVarChar, Value = item.DEPcode},
                    new SqlParameter() {ParameterName = "@DEPdescT", SqlDbType = SqlDbType.NVarChar, Value= item.DEPdescT},
                    new SqlParameter() {ParameterName = "@Budget", SqlDbType = SqlDbType.Decimal, Value = item.Budget},
   
                };
                DBHelper.Execute(strSQL, sp);
            }
            else
            {
                strSQL = "UPDATE BudgetOfYearByDepartment SET " + item.ColumnName + "=@Budget WHERE id=@id";

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@id", SqlDbType = SqlDbType.Int, Value= item.id},
                    new SqlParameter() {ParameterName = "@Budget", SqlDbType = SqlDbType.Decimal, Value= item.Budget},

                };
                DBHelper.Execute(strSQL, sp);
            }
            return result;
        }
        #endregion
    }
}
