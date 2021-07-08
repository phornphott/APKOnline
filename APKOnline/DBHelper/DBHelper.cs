using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace APKOnline.DBHelper
{
    public class DBHelper
    {
        //private static string szDbUser = "sa";
        //private static string szDbPassword = "dawn0200";

        public static SqlConnection SqlConnectionDb()
        {
            string connStrFmt = "Data Source={0}; Initial Catalog={1};User ID={2}; Password={3};";
            connStrFmt = string.Format(connStrFmt, ConfigurationManager.AppSettings["DBServer"]
                , ConfigurationManager.AppSettings["DBName"], ConfigurationManager.AppSettings["DBUser"], ConfigurationManager.AppSettings["DBPassword"]);
            SqlConnection conn = new SqlConnection();
            try
            {

                conn = new SqlConnection(connStrFmt);

                conn.Open();

            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return conn;
        }
        public static SqlConnection sqlConnection()
        {
            string connStrFmt = "Data Source={0}; Initial Catalog={1};User ID={2}; Password={3}";
            connStrFmt = string.Format(connStrFmt, ConfigurationManager.AppSettings["DBServer"], ConfigurationManager.AppSettings["DBName"]
                , ConfigurationManager.AppSettings["DBUser"], ConfigurationManager.AppSettings["DBPassword"]);
            SqlConnection conn = new SqlConnection();
            try
            {
                conn = new SqlConnection(connStrFmt);
                conn.Open();

            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return conn;
        }

        public static SqlConnection SqlConnectionDbMAC5()
        {
            string connStrFmt = "Data Source={0}; Initial Catalog={1};User ID={2}; Password={3};";
            connStrFmt = string.Format(connStrFmt, ConfigurationManager.AppSettings["DBServer"]
                , "M5CM-MA-01", ConfigurationManager.AppSettings["DBUser"], ConfigurationManager.AppSettings["DBPassword"]);
            SqlConnection conn = new SqlConnection();
            try
            {

                conn = new SqlConnection(connStrFmt);

                conn.Open();

            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return conn;
        }

        public static DataTable List(string query)
        {
            var dt = new DataTable();
            SqlConnection conn = DBHelper.SqlConnectionDb();
            var dataAdapter = new SqlDataAdapter(query, conn);
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(dt);
            conn.Close();

            return dt;
        }
        public static int Execute(string query)
        {
            int i = 0;
            SqlConnection conn = DBHelper.SqlConnectionDb();
            var sqlCommand = new SqlCommand(query, conn);
            i = sqlCommand.ExecuteNonQuery();
            conn.Close();

            return (i > 0) ? i : 0;
        }
        public static int Execute(string query, List<SqlParameter> sp)
        {
            int i = 0;
            SqlConnection conn = DBHelper.SqlConnectionDb();
            var sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Parameters.AddRange(sp.ToArray());
           i = sqlCommand.ExecuteNonQuery();
            conn.Close();

            return (i > 0) ? i : 0;
        }
        public static async Task<int> ExecuteStoreProcedure(string query,List<SqlParameter> sp)
        {
            int i = 0;
            SqlConnection conn = DBHelper.SqlConnectionDb();
            var sqlCommand = new SqlCommand(query, conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddRange(sp.ToArray());


            i = (Int32)sqlCommand.ExecuteScalar();//sqlCommand.ExecuteNonQuery();
            conn.Close();

            return (i > 0) ? i : 0;
        }
        /// <summary>
        /// Get max id function for insert data.
        /// </summary>
        /// <param name="columnName">Column name have to get max id</param>
        /// <param name="tableName">Table name have to get max id</param>
        /// <returns>Max id from each table</returns>
        public static int GetMaxID(string columnName, string tableName)
        {
            string strSQL = $"SELECT CASE WHEN Max({columnName}) IS NULL THEN 1 ELSE Max({columnName}) + 1 END AS MaxID FROM {tableName}";

            DataTable dt = List(strSQL);
            int MaxID = Convert.ToInt32(dt.Rows[0]["MaxID"].ToString());

            return MaxID;
        }
        /// <summary>
        /// Function to check column exist in database
        /// </summary>
        /// <param name="tableName">Table name will be check column</param>
        /// <param name="columnName">Column name to check</param>
        /// <returns>Return DataTable and Column name inside</returns>
        public static DataTable ColumnExist(string tableName, string columnName)
        {
            var dt = new DataTable();
            string strSQL = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'{tableName}' AND COLUMN_NAME = N'{columnName}';";

            SqlConnection conn = DBHelper.SqlConnectionDb();
            var dataAdapter = new SqlDataAdapter(strSQL, conn);
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(dt);
            conn.Close();

            return dt;
        }
        /// <summary>
        /// Function to check column exist in database
        /// </summary>
        /// <param name="tableName">Table name to check</param>
        /// <returns>Return DataTable and Table name inside</returns>
        public static DataTable TableExist(string tableName)
        {
            var dt = new DataTable();
            string strSQL = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}'";

            SqlConnection conn = DBHelper.SqlConnectionDb();
            var dataAdapter = new SqlDataAdapter(strSQL, conn);
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(dt);
            conn.Close();

            return dt;
        }

        public static void ConnectionSQL(ref SqlConnection conn)
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.ConnectionString = GetSQLconStr(ConfigurationManager.AppSettings["DBServer"], "M5CM-MA-01", ConfigurationManager.AppSettings["DBUser"], ConfigurationManager.AppSettings["DBPassword"] + ";Connection Timeout=600");
                conn.Open();
            }
            catch
            {
            }
        }

        public static string GetSQLconStr(string servername, string dbname, string username, string password)
        {
            string ret = "";
            ret = "Data Source=" + servername + ";Initial Catalog=" + dbname + ";User Id=" + username;
            if (password.TrimStart(Convert.ToChar(31)).Length > 0)
                ret += ";Password=" + password;
            return ret;
        }
    }
}