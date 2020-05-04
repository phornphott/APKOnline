using APKOnline.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APKOnline
{
    public partial class PrintPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string id = (Request.QueryString["Parameter"].ToString());
            int m_id = Convert.ToInt32(id);

            string sql = "SELECT o.*,r.Document_Vnos AS PR FROM DocumentPO_Header o  LEFT JOIN DocumentPR_Header r ON r.Document_ID = o.Document_PRID WHERE o.Document_ID = " + id;
            DataTable Header = new DataTable();
            DataTable List = new DataTable();
            CultureInfo ThaiCulture = new CultureInfo("th-TH");
            //POList POlist = new POList();
            try
            {

                SqlConnection conn = sqlConnection();
             

                SqlCommand com2 = new SqlCommand(sql, conn);

                SqlDataAdapter da = new SqlDataAdapter(com2);
                da.Fill(Header);
                //string depCode = "";
                fur_DS_Po m_ds = new fur_DS_Po();
                //appPOlist.Tables("POList").newrow();
                string PO_ = "";
                string PR_ = "";
                string Date_ = "";
                string RecDate = "";
                string job_ = "";
                string credit_ = "";
                string at_ = "";
                string subtotal_ = "0.00";
                string disc_ = "0.00";
                string _total = "0.00";
                string vat_ = "0.00";
                string grandtotal_ = "0.00";
                string cuscode_ = "";
                string cusname_ = "";
                string cusaddress_ = "";
                string custel_ = "";
                foreach (DataRow dr in Header.Rows)
                {
                    sql = "SELECT * FROM [DocumentPO_Detail]  WHERE [Document_Detail_Hid] = " + m_id + " Order by Document_Detail_ListNo";
                    com2 = new SqlCommand(sql, conn);

                    da = new SqlDataAdapter(com2);
                    da.Fill(List);
                    foreach (DataRow row in List.Rows)
                    {
                        DataRow drow = m_ds.Tables["List"].NewRow();
                        drow["No"] = row["Document_Detail_ListNo"].ToString();
                        drow["dept"] = dr["Document_Dep"].ToString();
                        drow["desc"] = row["Document_Detail_Acc"].ToString() + "  " + row["Document_Detail_Acc_Desc"].ToString();
                        int qty = Convert.ToInt32(row["Document_Detail_Quan"]);
                        double total_ = Convert.ToDouble(row["Document_Detail_Cog"]);
                        drow["qty"] = row["Document_Detail_Quan"].ToString();
                        drow["unit"] = (total_ / qty).ToString("#,000.00");
                        drow["amount"] = total_.ToString("#,000.00");
                        drow["disc"] = "0.00";
                        drow["total"] = total_.ToString("#,000.00");
                        m_ds.Tables["List"].Rows.Add(drow);
                    }

                    PR_ = dr["PR"].ToString();
                    PO_ = dr["Document_Vnos"].ToString();
                    Date_ = dr["Document_Date"].ToString();
                    RecDate = dr["Document_ReceiveDate"].ToString();
                    credit_ = dr["Document_Credit"].ToString();
                    at_ = dr["Document_Send"].ToString();
                    subtotal_ = dr["Document_Cog"].ToString();
                    disc_ = "0.00";
                    _total = dr["Document_Cog"].ToString();
                    vat_ = dr["Document_VatSUM"].ToString();
                    grandtotal_ = dr["Document_NetSUM"].ToString();
                    job_ = dr["Document_Job"].ToString();

                    sql = "SELECT * FROM CRE WHERE CREcode = '" + dr["Document_Cus"].ToString() + "'";
                    com2 = new SqlCommand(sql, conn);
                    da = new SqlDataAdapter(com2);
                    DataTable tmp = new DataTable();
                    da.Fill(tmp);
                    foreach (DataRow row_ in tmp.Rows)
                    {
                        cuscode_ = row_["CREnameT"].ToString();
                        cusname_ = row_["CREcontactT"].ToString();
                        cusaddress_ = row_["CREadd1AT"].ToString();
                        custel_ = row_["CREtel"].ToString();
                    }

                }
                //ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/fur_PO_report.rdlc");
                //sql = "SELECT * FROM Company WHERE ID = '" + Session["CompanyID"].ToString() + "'";
                //SqlCommand com3 = new SqlCommand(sql, conn);
                //DataTable dt_company = new DataTable();
                //da = new SqlDataAdapter(com3);
                //da.Fill(dt_company);

                //string comAddess = "";
                //string comTel = "";
                ////string comFax = "";
                //string comTax = "";
                ////string com ="";
                //foreach (DataRow dr in dt_company.Rows)
                //{
                //    comAddess = dr["Address"].ToString();
                //    comTel = "Tel : " + dr["Tel"].ToString() + "  Fax : " + dr["Fax"].ToString();
                //    comTax = dr["Tax"].ToString();
                //    //comAddess = dr[""].ToString();
                //    //comAddess = dr[""].ToString();
                //}


                //ReportParameter
                ReportParameter companyname = new ReportParameter("companyname", "");
                ReportParameter companyaddress = new ReportParameter("companyaddress", "");
                ReportParameter companytel = new ReportParameter("companytel", "");
                ReportParameter companytax = new ReportParameter("companytax", "");

                ReportParameter PO = new ReportParameter("PO", PO_);
                ReportParameter PR = new ReportParameter("PR", PR_);

                ReportParameter Date = new ReportParameter("Date", Date_);
                ReportParameter re_date = new ReportParameter("re_date", RecDate);
                ReportParameter credit = new ReportParameter("credit", credit_);
                ReportParameter at = new ReportParameter("at", at_);
                ReportParameter job = new ReportParameter("job", job_);

                ReportParameter cuscode = new ReportParameter("cuscode", cuscode_);
                ReportParameter cusname = new ReportParameter("cusname", cusname_);
                ReportParameter custel = new ReportParameter("custel", custel_);
                ReportParameter cusaddress = new ReportParameter("cusaddress", cusaddress_);

                ReportParameter subtotal = new ReportParameter("subtotal", subtotal_);
                ReportParameter disc = new ReportParameter("disc", disc_);
                ReportParameter total = new ReportParameter("total", _total);
                ReportParameter vat = new ReportParameter("vat", vat_);
                ReportParameter grandtotal = new ReportParameter("grandtotal", grandtotal_);
                ReportParameter txttotal = new ReportParameter("txttotal", "(" + ThaiBaht.ThaiBaht1(grandtotal_) + ")");


                ReportParameter staffreq = new ReportParameter("staffreq", "");
                ReportParameter stafforder = new ReportParameter("stafforder", "");
                ReportParameter staffapp = new ReportParameter("staffapp", "");

                ReportViewer ReportViewer1 = new ReportViewer();


                //ReportViewer1.LocalReport.ReportPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")) + @"\Reports\fur_report_PO.rdlc";
                ReportViewer1.LocalReport.ReportPath = @"D:\Repository\APK Online\APKOnline\Reports\fur_report_PO.rdlc";
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { companyname, companyaddress , companytel
                //, companytax,PO,PR,Date,re_date,credit,at,job,cuscode,cusname,cusaddress,custel,subtotal,disc,total,vat,grandtotal, staffreq, stafforder, staffapp,txttotal });

                ReportViewer1.LocalReport.DataSources.Clear(); //clear report
                Microsoft.Reporting.WebForms.ReportDataSource ds
                    = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", m_ds.Tables["List"]);
                ReportViewer1.LocalReport.DataSources.Add(ds);

                //ReportViewer1.LocalReport.Refresh();

                string fileType = ".pdf";                     
                Warning[] warnings = null;
                string[] streamIds = null;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string PDF_FOLDER_FILE = "../PrintPDF/";
                // string PDF_FILE_NAME = "Document";

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                string PDF_File = "PONo_" + PO_ + fileType;

                //########################### Check Folder ######################################

                string folder = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")) + PDF_FOLDER_FILE;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                //########################### Check Folder ######################################

                int len = 0;
                using (Stream f = File.Open(Server.MapPath(PDF_File), FileMode.Create))
                {
                    if (bytes != null)
                    {
                        len = bytes.Length;
                        f.Write(bytes, 0, bytes.Length);
                    }
                    f.Close();
                }

                Response.Redirect((PDF_File + Convert.ToString("?")) + System.DateTime.Now.ToString());


            }
            catch (Exception ex) { }
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
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                else
                {
                    conn.Close();
                    conn.Open();
                }

            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return conn;
        }
    }
}