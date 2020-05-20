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

        string ReplaceString(String text);

        DataTable Login(string username, string password, ref string errMsg);

        DataTable GetStaffData(ref string errMsg);

        DataTable GetStaffDataByID(ref string errMsg, int StaffID);

        DataTable GetPermissionData(ref string errMsg);

        DataTable GetPermissionDataByID(ref string errMsg, int DEPid);

        DataTable GetDepartmentData(ref string errMsg);

        DataTable GetDepartmentDataByID(ref string errMsg,int POSid);
        
        DataTable GetStaffAuthorizeData(ref string errMsg);

        Task<bool> SetDepartmentData(Department item);

        Task<bool> SetPositionData(Position item);

        Task<bool> DeleteDepartment(int id);
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
                strSQL = "\r\n SELECT * FROM Staffs " +
                         "\r\n WHERE StaffLogin='" + username + "'" +
                         "\r\n AND StaffPassword='" + Base64Encode(password) + "'" +
                         "\r\n AND Deleted=0;";
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

        public DataTable GetStaffData( ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                strSQL = "\r\n SELECT * FROM Staffs " +
                         "\r\n WHERE Deleted=0;";
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
                strSQL = "\r\n SELECT * FROM StaffAuthorize order by StaffCode,DEPid ";
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

        #region Position
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
    }
}
