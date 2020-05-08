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

namespace APKOnline.Reports
{
    public partial class PrintPreview : System.Web.UI.Page
    {
        int m_id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string id = (Request.QueryString["Parameter"].ToString());
                m_id = Convert.ToInt32(id);
            }
            catch
            { m_id = 1002; }
            if (!IsPostBack)
            {



                LoadReport();
            }
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
        private void LoadReport()
        {
            try
            {
                string sql = "SELECT o.*,r.Document_Vnos AS PR FROM DocumentPO_Header o  LEFT JOIN DocumentPR_Header r ON r.Document_ID = o.Document_PRID WHERE o.Document_ID = " + m_id;
                DataTable Header = new DataTable();
                DataTable List = new DataTable();
                CultureInfo ThaiCulture = new CultureInfo("th-TH");

                SqlConnection conn = sqlConnection();


                SqlCommand com2 = new SqlCommand(sql, conn);

                SqlDataAdapter da = new SqlDataAdapter(com2);
                da.Fill(Header);
                //string depCode = "";
                DS_PO m_ds = new DS_PO();
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
                    int i = 1;
                    foreach (DataRow row in List.Rows)
                    {
                        DataRow drow = m_ds.Tables["List"].NewRow();
                        drow["No"] = i;
                        drow["Dept"] = dr["Document_Dep"].ToString();
                        drow["Desc"] = row["Document_Detail_Acc"].ToString() + "  " + row["Document_Detail_Acc_Desc"].ToString();
                        int qty = Convert.ToInt32(row["Document_Detail_Quan"]);
                        double total_ = Convert.ToDouble(row["Document_Detail_Cog"]);
                        drow["Qty"] = qty.ToString("#,###");
                        drow["Unit"] = "ea";//(total_ / qty).ToString("#,###.00");
                        drow["Amount"] = total_.ToString("#,###.00");
                        drow["Disc"] = "0.00";
                        drow["Total"] = total_.ToString("#,###.00");
                        m_ds.Tables["List"].Rows.Add(drow);

                        i++;
                    }

                    PR_ = dr["PR"].ToString();
                    PO_ = dr["Document_Vnos"].ToString();
                    Date_ = dr["Document_Date"].ToString();
                    RecDate = dr["Document_ReceiveDate"].ToString();
                    credit_ = dr["Document_Credit"].ToString();
                    at_ = dr["Document_Send"].ToString();
                    subtotal_ = Convert.ToDouble(dr["Document_Cog"]).ToString("#,###.00");
                    disc_ = "0.00";
                    _total = Convert.ToDouble(dr["Document_Cog"]).ToString("#,###.00");
                    vat_ = Convert.ToDouble(dr["Document_VatSUM"]).ToString("#,###.00");
                    grandtotal_ = Convert.ToDouble(dr["Document_NetSUM"]).ToString("#,###.00");
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

                sql = "SELECT * FROM Company WHERE ID = 2";
                SqlCommand com3 = new SqlCommand(sql, conn);
                DataTable dt_company = new DataTable();
                da = new SqlDataAdapter(com3);
                da.Fill(dt_company);

                string comAddess = "";
                string comTel = "";
                //string comFax = "";
                string comTax = "";
                string comname = "";
                if (dt_company.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt_company.Rows)
                    {
                        comAddess = dr["Address"].ToString();
                        comTel = "Tel : " + dr["Tel"].ToString() + "  Fax : " + dr["Fax"].ToString();
                        comTax = dr["Tax"].ToString();
                        comname = dr["Company"].ToString();
                    }
                }
                else {
                    comAddess = "501/1 ม.4 ถนน สนามบิน-บ้านกลาง ตำบลควนลัง อำเภอหาดใหญ่ จังหวัดสงขลา 90110";
                    comTel = "Tel : 074-502288-90  Fax : 074-502222 ";
                    comTax = "0905551001999";
                    comname = "บจก.เอ.พี.เค.กรีนเอ็นเนอร์จี";
                }


                //ReportParameter
                ReportParameter PO = new ReportParameter("PO", PO_);
                ReportParameter PR = new ReportParameter("PR", PR_);
                ReportParameter companyname = new ReportParameter("CompanyName", comname);
                ReportParameter companyaddress = new ReportParameter("CompanyAdress", comAddess);
                ReportParameter companytel = new ReportParameter("CompanyTel", comTel);
                ReportParameter companytax = new ReportParameter("CompanyTax", comTax);
                ReportParameter cuscode = new ReportParameter("CusCode", cuscode_);
                ReportParameter cusname = new ReportParameter("CusName", cusname_);
                ReportParameter custel = new ReportParameter("CusTel", custel_);
                ReportParameter cusaddress = new ReportParameter("CusAddress", cusaddress_);
                ReportParameter Date = new ReportParameter("Date", Date_);
                ReportParameter re_date = new ReportParameter("re_Date", RecDate);
                ReportParameter credit = new ReportParameter("credit", credit_);
                ReportParameter at = new ReportParameter("at", at_);
                ReportParameter job = new ReportParameter("job", job_);
                ReportParameter subtotal = new ReportParameter("subtotal", subtotal_);
                ReportParameter disc = new ReportParameter("disc", disc_);
                ReportParameter total = new ReportParameter("total", _total);
                ReportParameter vat = new ReportParameter("vat", vat_);
                ReportParameter grandtotal = new ReportParameter("grandtotal", grandtotal_);
                ReportParameter txttotal = new ReportParameter("txttotal", "(" + ThaiBaht.ThaiBaht1(grandtotal_) + ")");


                //DataRow drow1 = m_ds.Tables["Sum"].NewRow();
                //drow1[0] = "รวมเป็นเงิน";
                //drow1[1] = subtotal_;
                //m_ds.Tables["Sum"].Rows.Add(drow1);
                //drow1 = m_ds.Tables["Sum"].NewRow();
                //drow1[0] = "หักส่วนลด";
                //drow1[1] = disc_;
                //m_ds.Tables["Sum"].Rows.Add(drow1);
                //drow1 = m_ds.Tables["Sum"].NewRow();
                //drow1[0] = "ยอดหักภาษี";
                //drow1[1] = _total;
                //m_ds.Tables["Sum"].Rows.Add(drow1);
                //drow1 = m_ds.Tables["Sum"].NewRow();
                //drow1[0] = "รวมเป็นเงิน";
                //drow1[1] = vat_;
                //m_ds.Tables["Sum"].Rows.Add(drow1);
                //drow1 = m_ds.Tables["Sum"].NewRow();
                //drow1[0] = "รวมเป็นเงิน";
                //drow1[1] = grandtotal_;
                //m_ds.Tables["Sum"].Rows.Add(drow1);



                ReportViewer1.LocalReport.DataSources.Clear(); //clear report
                Microsoft.Reporting.WebForms.ReportDataSource ds
                    = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", m_ds.Tables["List"]);

                ReportViewer1.LocalReport.DataSources.Add(ds);
             

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { PO, PR
                    , companyname, companyaddress, companytel ,companytax
                ,cuscode,cusname,cusaddress,custel
                ,Date,re_date,credit,at,job
                ,subtotal,disc,total,vat,grandtotal,txttotal});

                //GenerateFile("PDF", ReportViewer1.LocalReport, "POExport/"+ PO_ + ".pdf");
                ////ReportViewer1.LocalReport.Refresh();

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
            catch (Exception EX) {  }
        }
        protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { LoadReport(); }
            catch { }
        }
        private void GenerateFile(string fileType, LocalReport report, string savePath)
        {
            // The FileStream class is in the System.IO namespace.
            /* 
             * The savePath include the file name with the proper file extension.
             * If file is a pdf -> filename.pdf
             */
            byte[] bytes = report.Render(fileType);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}