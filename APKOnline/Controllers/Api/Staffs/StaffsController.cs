﻿using APKOnline.DBHelper;
using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Internal;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace APKOnline.Controllers
{
    public class StaffsController : ApiController
    {
        static readonly IStaffData repository = new StaffData();

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage PostLogin(StaffModels item)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.Login(item.StaffLogin, item.StaffPassword, ref errMsg);

            ds.Tables.Add(dt);

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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("StaffRoleData")]
        public HttpResponseMessage GETStaffRoleData()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPermissionData(ref errMsg);

            //dt = repository.GetDepartmentData(ref errMsg);

            ds.Tables.Add(dt);

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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("StaffData")]
        public HttpResponseMessage GETStaffData()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetStaffData(ref errMsg);

            ds.Tables.Add(dt);

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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("DepartmentRoleData")]
        public HttpResponseMessage GETDepartmentData()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetDepartmentData(ref errMsg);

            ds.Tables.Add(dt);

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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("PermissionRoleData")]
        public HttpResponseMessage GETPermissionRoleData()
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetStaffAuthorizeData(ref errMsg);

            ds.Tables.Add(dt);

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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }

        [HttpPost]
        [ActionName("SetDepartmentData")]
        public async Task<HttpResponseMessage> SetDepartment(Department item)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ret = await repository.SetDepartmentData(item);

                if (ret)
                {
                    response.StatusCode = (int)StatusCodes.Succuss;
                    response.Messages = "";
                }
                else
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "";

                }
            }
            catch (Exception e)
            {
                response.StatusCode = (int)StatusCodes.Error;
                response.Messages = e.Message;
            }


            //response.Results = ds;
            //response.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpPost]
        [ActionName("DeleteDepartment")]
        public async Task<HttpResponseMessage> DeleteDepartment(int id)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {                                 
                ret = await repository.DeleteDepartment(id);

                if (ret)
                {
                    response.StatusCode = (int)StatusCodes.Succuss;
                    response.Messages = "";
                }
                else
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "";

                }
            }
            catch (Exception e)
            {
                response.StatusCode = (int)StatusCodes.Error;
                response.Messages = e.Message;
            }


            //response.Results = ds;
            //response.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
