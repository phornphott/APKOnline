using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APKOnline
{
    public partial class popupUploadfile : System.Web.UI.Page
    {
        string tmppath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            tmppath = (Request.QueryString["Parameter"].ToString());
        }
        protected void uploadFile_Click(object sender, EventArgs e)
        {
            if (UploadImages.HasFiles)
            {
                string flder = Server.MapPath("~/tmpUpload/" + tmppath + "/");
                if (Directory.Exists(flder))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(flder);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    listofuploadedfiles.Text = "";
                }
                Directory.CreateDirectory(flder);


                foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
                {
                    string pathfile = System.IO.Path.Combine(Server.MapPath("~/tmpUpload/" + tmppath + "/"), uploadedFile.FileName);
                    uploadedFile.SaveAs(pathfile);
                    listofuploadedfiles.Text += String.Format("{0}<br />", uploadedFile.FileName);
                }
            }
        }
    }
}