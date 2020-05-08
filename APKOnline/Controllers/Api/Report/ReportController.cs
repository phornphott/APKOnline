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
        public HttpResponseMessage GETListReportBudget(string STARTDATE, string ENDDATE, string MONTHS, int StaffCode, int DEPcode)
         {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtHeaderData = Reportrepository.GetReportBudget(STARTDATE, ENDDATE, MONTHS, StaffCode, DEPcode, ref errMsg);

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
        public async Task<HttpResponseMessage> GETDashBroad()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            ds = await Reportrepository.GetDashBroadData();


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
        public async Task<HttpResponseMessage> GETDashBroadByDepartment(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            ds = await Reportrepository.GetDashBroadByDepartment(id);


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
        public async Task<HttpResponseMessage> GETDashBroadByDepartment(string dep)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            int id = Reportrepository.getdepid(dep);
            ds = await Reportrepository.GetDashBroadByDepartment(id);


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