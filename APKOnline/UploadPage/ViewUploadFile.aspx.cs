using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APKOnline.UploadPage
{
    public partial class ViewUploadFile : System.Web.UI.Page
    {
        string filepath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                filepath = (Request.QueryString["Parameter"].ToString());


                OpenFile();
            }

        }

        private void OpenFile()
        {
            //string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/" + filepath );
            Response.Write("<script>window.open('/Upload/" + filepath + "');</script>");
        }
    }
}