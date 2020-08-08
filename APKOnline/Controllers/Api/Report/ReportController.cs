using APKOnline.DBHelper;
using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace APKOnline.Controllers.Api.Report
{
    public class ReportController : ApiController
    {
        static readonly ReportData Reportrepository = new ReportData();

        [HttpGet]
        [ActionName("ListReportBudget")]
        public HttpResponseMessage GETListReportBudget(int year, int month, int StaffCode, int DEPcode)
         {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtHeaderData = Reportrepository.GetReportBudget(year,  month, StaffCode, DEPcode, ref errMsg);

            ds.Tables.Add(dtHeaderData);

            if (errMsg != "")
            {
                resData.StatusCode = (int)(StatusCodes.Error);
                resData.Messages = errMsg;
            }
            else
            {
                resData.StatusCode = (int)(StatusCodes.Succuss);
                resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
            }

            resData.Results = ds;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("DashBroad")]
        public HttpResponseMessage GETDashBroad()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            try
            {
                ds =  Reportrepository.GetDashBroadData(ref errMsg);

            }
            catch (Exception ex) {
                errMsg = ex.Message;
            }
            if (errMsg != "")
            {
                resData.StatusCode = (int)(StatusCodes.Error);
                resData.Messages = errMsg;
            }
            else
            {
                resData.StatusCode = (int)(StatusCodes.Succuss);
                resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
            }

            resData.Results = ds;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("DashBroadByDepartment")]
        public HttpResponseMessage GETDashBroadByDepartment(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            ds =  Reportrepository.GetDashBroadByDepartment(id, ref errMsg);


            if (errMsg != "")
            {
                resData.StatusCode = (int)(StatusCodes.Error);
                resData.Messages = errMsg;
            }
            else
            {
                resData.StatusCode = (int)(StatusCodes.Succuss);
                resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
            }

            resData.Results = ds;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("DashBroadByDepartment")]
        public HttpResponseMessage GETDashBroadByDepartment(string dep)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            int id = Reportrepository.getdepid(dep);
            ds =  Reportrepository.GetDashBroadByDepartment(id,ref errMsg);


            if (errMsg != "")
            {
                resData.StatusCode = (int)(StatusCodes.Error);
                resData.Messages = errMsg;
            }
            else
            {
                resData.StatusCode = (int)(StatusCodes.Succuss);
                resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
            }

            resData.Results = ds;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
    }
}