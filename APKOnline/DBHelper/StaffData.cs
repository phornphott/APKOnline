using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APKOnline.DBHelper
{
    public interface IStaffData
    {
        DataTable Login(string username, string password, ref string errMsg);
        DataTable GetStaffData(ref string errMsg);
        DataTable GetPermissionData(ref string errMsg);

        DataTable GetDepartmentData(ref string errMsg);

        DataTable GetStaffAuthorizeData(ref string errMsg);
        
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

        public DataTable GetDepartmentData(ref string errMsg)
        {
            string strSQL = null;
            DataTable dt = new DataTable();

            try
            {
                //strSQL = "\r\n SELECT * FROM PermissionGroup ";
                strSQL = "\r\n SELECT * FROM Department order by DEPcode ";
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
    }
}
