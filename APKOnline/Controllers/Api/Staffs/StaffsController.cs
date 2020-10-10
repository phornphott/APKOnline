using APKOnline.DBHelper;
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
            if (dt.Rows.Count <= 0 && errMsg == "")
            {
                errMsg = "รหัสพนักงาน/รหัสผ่าน ไม่ถูกต้อง." + Environment.NewLine + "โปรดตรวจสอบ รหัสพนักงาน/รหัสผ่าน ก่อน login เข้าระบบอีกครั้ง";
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
            resData.Records = ds.Tables[0].Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }

        #region " ตำแหน่ง "
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
        [ActionName("StaffRoleDataByID")]
        public HttpResponseMessage GETStaffRoleDataByID(int POSid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPermissionDataByID(ref errMsg, POSid);

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
        [HttpPost]
        [ActionName("SetPositionData")]
        public async Task<HttpResponseMessage> SetPosition(Position item)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                int iCount = repository.GetCheckUniqe(
                 " PositionPermission ",
                 " Positioncode ",
                 " and Positioncode=" + repository.ReplaceString(item.Positioncode), "Positionid", item.Positionid);

                bool ExistOK = false;

                if (iCount > 0) ExistOK = true;

                if (ExistOK)
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "รหัสตำแหน่งซ้ำ";
                    //throw new Exception(" รหัสแผนก ซ้ำ !!!");
                }
                else
                {
                    ret = await repository.SetPositionData(item);

                    if (ret)
                    {
                        response.StatusCode = (int)StatusCodes.Succuss;
                        response.Messages = "บันทึกข้อมูลเรียบร้อยแล้ว";
                    }
                    else
                    {
                        response.StatusCode = (int)StatusCodes.Error;
                        response.Messages = "";

                    }
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
        #endregion

        #region " พนักงาน "
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
        [ActionName("StaffDataByID")]
        public HttpResponseMessage GETStaffDataByID(int StaffID)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetStaffDataByID(ref errMsg, StaffID);

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
        [ActionName("SetStaffData")]
        public async Task<HttpResponseMessage> SetStaff(StaffModels item)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                int iCount = repository.GetCheckUniqeLogin(
                 " Staffs ",
                 " StaffCode ",
                 " and StaffCode=" + repository.ReplaceString(item.StaffCode), "StaffID", item.StaffID);

                bool ExistOK = false;

                if (iCount > 0) ExistOK = true;

                if (ExistOK)
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "รหัสพนักงานซ้ำ";
                    //throw new Exception(" รหัสแผนก ซ้ำ !!!");
                }
                else
                {
                    iCount = repository.GetCheckUniqeLogin(
                     " Staffs ",
                     " StaffLogin ",
                     " and StaffLogin=" + repository.ReplaceString(item.StaffLogin), "StaffID", item.StaffID);

                    ExistOK = false;
                    if (iCount > 0) ExistOK = true;

                    if (ExistOK)
                    {
                        response.StatusCode = (int)StatusCodes.Error;
                        response.Messages = "รหัส User login ซ้ำ";
                        //throw new Exception(" รหัสแผนก ซ้ำ !!!");
                    }
                    else
                    {
                        ret = await repository.SetStaffData(item);

                        if (ret)
                        {
                            response.StatusCode = (int)StatusCodes.Succuss;
                            response.Messages = "บันทึกข้อมูลเรียบร้อยแล้ว";
                        }
                        else
                        {
                            response.StatusCode = (int)StatusCodes.Error;
                            response.Messages = "";

                        }
                    }
                        
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
        [ActionName("DeleteStaff")]
        public async Task<HttpResponseMessage> DeleteStaff(int id)
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
                    response.Messages = "ลบรหัสพนักงานเรียบร้อยแล้ว";
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
        #endregion

        #region " แผนก "
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
        [ActionName("DepartmentRoleDataByID")]
        public HttpResponseMessage DepartmentRoleDataByID(int DEPid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetDepartmentDataByID(ref errMsg, DEPid);

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
                int iCount = repository.GetCheckUniqe(
                 " Department ",
                 " DEPcode ",
                 " and DEPcode=" + repository.ReplaceString(item.DEPcode), "DEPid", item.DEPid);

                bool ExistOK = false;

                if (iCount > 0) ExistOK = true;

                if (ExistOK)
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "รหัสแผนกซ้ำ";
                    //throw new Exception(" รหัสแผนก ซ้ำ !!!");
                }
                else
                {
                    ret = await repository.SetDepartmentData(item);

                    if (ret)
                    {
                        response.StatusCode = (int)StatusCodes.Succuss;
                        response.Messages = "บันทึกข้อมูลเรียบร้อยแล้ว";
                    }
                    else
                    {
                        response.StatusCode = (int)StatusCodes.Error;
                        response.Messages = "";

                    }
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
                    response.Messages = "ลบรหัสแผนกเรียบร้อยแล้ว";
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
        #endregion

        #region " บทบาทหน้าที่ "
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
        [ActionName("PermissionRoleDataByID")]
        public HttpResponseMessage GETPermissionRoleDataByID(int Authorizeid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetStaffAuthorizeDataByID(ref errMsg, Authorizeid);

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
        [ActionName("DeleteStaffAuthorize")]
        public async Task<HttpResponseMessage> DeleteStaffAuthorize(int id)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ret = await repository.DeleteStaffAuthorize(id);

                if (ret)
                {
                    response.StatusCode = (int)StatusCodes.Succuss;
                    response.Messages = "ลบบทบาทหน้าที่เรียบร้อยแล้ว";
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
        [ActionName("SetStaffAuthorize")]
        public async Task<HttpResponseMessage> SetStaffAuthorize(StaffAuthorize item)
        {
            Result response = new Result();
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                int iCount = repository.GetCheckUniqeAuthorize(
                 " StaffAuthorize ",
                 " StaffCode ",
                 " and StaffID=" + item.StaffID + " and DEPid=" + item.DEPid + " and PositionPermissionId=" + item.PositionPermissionId, "Authorizeid", item.Authorizeid);

                bool ExistOK = false;

                if (iCount > 0) ExistOK = true;

                if (ExistOK)
                {
                    response.StatusCode = (int)StatusCodes.Error;
                    response.Messages = "รหัสบทบาทหน้าที่ซ้ำ";
                    //throw new Exception(" รหัสแผนก ซ้ำ !!!");
                }
                else
                {
                    ret = await repository.SetStaffAuthorize(item);

                    if (ret)
                    {
                        response.StatusCode = (int)StatusCodes.Succuss;
                        response.Messages = "บันทึกข้อมูลเรียบร้อยแล้ว";
                    }
                    else
                    {
                        response.StatusCode = (int)StatusCodes.Error;
                        response.Messages = "";

                    }
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
        #endregion




    }
}
