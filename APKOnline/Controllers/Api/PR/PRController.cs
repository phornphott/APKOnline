﻿using APKOnline.DBHelper;
using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using System.Web.Http;
using System.IO;

namespace APKOnline
{
    public class PRController : ApiController
    {
        static readonly IPRData repository = new PRData();

      
        [HttpGet]
        [ActionName("PreparePageData")]
        public HttpResponseMessage GETPreparePageData(int id,int type)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();
            DataTable dtPRdetail = new DataTable();
            DataTable dtDocumentGroup = new DataTable();


            //if (type == 0)
            //{
            //    id = repository.InsertHeader(ref errMsg);
            //}




            dtDocumentGroup.Columns.Add("ID",typeof(int));
            dtDocumentGroup.Columns.Add("Name", typeof(string));

            for (int i = 0; i <= 6; i++)
            {
                DataRow nrow = dtDocumentGroup.NewRow();
                nrow[0] = i;
                string name = "";
                switch (i)
                {
                    case 0:
                        name = "- กรุณาเลือก -";
                        break;
                    case 1:
                        name = "สินค้าทั่วไป-คลังสินค้า";
                        break;
                    case 2:
                        name = "สินค้าทั่วไป-อื่นๆ";
                        break;
                    case 3:
                        name = "สินทรัพย์ถาวร";
                        break;
                    case 4:
                        name = "เชื้อเพลิง-วัตถุดิบ";
                        break;
                    case 5:
                        name = "ค่าใช้จ่ายทั่วไป";
                        break;
                    case 6:
                        name = "ค่าใช้จ่ายเงินสดย่อย";
                        break;
                    default:
                        break;
                }
                nrow[1] = name;
                dtDocumentGroup.Rows.Add(nrow);
            }
            dtDocumentGroup.TableName = "DocumentGroup";

            DataTable dtDocumentCate = repository.GetCategoryData(ref errMsg);
            DataTable dtDocumentObj = repository.GetObjectiveData(0, ref errMsg);
            DataTable dtJob = repository.GetJobData(ref errMsg);
            DataTable dtAccount = repository.GetAccountData(ref errMsg);
            DataTable dtdept = repository.GetDepData(ref errMsg);
            DataTable dtHeader = repository.GetHeaderData(id, type, ref errMsg);
            DataTable dtDetail = repository.GetDetailData(id, type, ref errMsg);

            ds.Tables.Add(dtDocumentGroup);
            ds.Tables.Add(dtDocumentCate);
            ds.Tables.Add(dtDocumentObj);
            ds.Tables.Add(dtJob);
            ds.Tables.Add(dtAccount);
            ds.Tables.Add(dtdept);
            ds.Tables.Add(dtHeader);
            ds.Tables.Add(dtDetail);

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
        [ActionName("PRDetailData")]
        public HttpResponseMessage GETPRDetailData(int id, int type)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            DataTable dtDetail = repository.GetDetailData(id, type, ref errMsg);


            ds.Tables.Add(dtDetail);

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
        [ActionName("ViewPRData")]
        public HttpResponseMessage GETViewPRData(int id,int staffid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            DataTable dtAccount = repository.GetAccountData(ref errMsg);
            DataTable dtHeaderData = repository.GetHeaderData(id, staffid, ref errMsg);
            DataTable dtDetail = repository.GetDetailData(id, 1, ref errMsg);
            ds.Tables.Add(dtAccount);
            ds.Tables.Add(dtHeaderData);
            ds.Tables.Add(dtDetail);

            //Get Pathfile
            DataTable dtfile = new DataTable();
            dtfile.Columns.Add("filename");
            dtfile.Columns.Add("path");


            string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/" + id.ToString() + "/");
            if (System.IO.Directory.Exists(targetpath))
            {
                string[] files = System.IO.Directory.GetFiles(targetpath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    DataRow dr = dtfile.NewRow();



                    // Use static Path methods to extract only the file name from the path.
                    dr[0] = System.IO.Path.GetFileName(s);
                    dr[1] = id.ToString() + "/"+ dr[0];

                    dtfile.Rows.Add(dr);
                    //string destFile = System.IO.Path.Combine(targetpath, fileName);

                }

            }
            dtfile.TableName = "FileUpload";
            ds.Tables.Add(dtfile);






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
        [ActionName("ListPRByStaff")]
        public HttpResponseMessage GETListPRByStaff(int id )
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPRData(id, ref errMsg);


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
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("ListPRForApprove")]
        public HttpResponseMessage GETListPRForApprove(int id,int deptid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPRDataForApprove(id,deptid, ref errMsg);


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
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("ListPROverForApprove")]
        public HttpResponseMessage GETListPROverForApprove(int id,int depid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPROverDataForApprove(id, depid, ref errMsg);


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
            return Request.CreateResponse(HttpStatusCode.OK, resData);
        }
        [HttpGet]
        [ActionName("CancelPRTmpDetail")]
        public HttpResponseMessage CancelPRTmpDetail(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            repository.DeleteTmpDetail(id, ref errMsg);


          

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
        [ActionName("OjectiveData")]
        public HttpResponseMessage GETOjectiveDate(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            
            DataTable dtDocumentObj = repository.GetObjectiveData(id, ref errMsg);
            ds.Tables.Add(dtDocumentObj);

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
        [HttpPost]
        [ActionName("AddPRDetail")]
        public HttpResponseMessage AddPRDetail(PRDetailModels detail)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


           int id = repository.InserttmpDetail(detail, ref errMsg);

            //ds.Tables.Add(dtDocumentVnos);
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

        [HttpPost]
        [ActionName("SavePRData")]
        public HttpResponseMessage SavePRData(PRHeaderModels Header)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int id = repository.InsertHeader(Header, ref errMsg);

            //ds.Tables.Add(dtDocumentVnos);
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

        [HttpPost]
        [ActionName("DeletePRData")]
        public async Task<HttpResponseMessage> DeletePRData(int id)
        {
            string errMsg = "";
            bool ret = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result response = new Result();
            bool POok = false;

            try
            {
                POok = await repository.CheckDeletePRData(id);
                if (POok)
                {
                    response.StatusCode = (int)(StatusCodes.Error);
                    response.Messages = "ไม่สามารถลบฝบขออนุมัตินี้ได้" + Environment.NewLine + "ใบขออนุมัตินี้ถูกอนุมัติเรียบร้อยแล้ว";
                }
                else
                {
                    ret = await repository.DeletePRData(id);

                    //ds.Tables.Add(dtDocumentVnos);
                    if (ret)
                    {
                        response.StatusCode = (int)(StatusCodes.Succuss);
                        response.Messages = "ลบใบขออนุมัติเรียบร้อยแล้ว";

                    }
                    else
                    {
                        response.StatusCode = (int)(StatusCodes.Error);
                        response.Messages = errMsg;
                    }
                }
            }
            catch (Exception e)
            {
                response.StatusCode = (int)StatusCodes.Error;
                response.Messages = e.Message;
            }


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [ActionName("GeneratePRNo")]
        public HttpResponseMessage GETGeneratePRID(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtDocumentVnos = repository.GeneratePRNo(id, ref errMsg);

            ds.Tables.Add(dtDocumentVnos);
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
        [HttpPost]
        [ActionName("ApprovePRData")]
        public HttpResponseMessage ApprovePRData(PRHeaderModels Header)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int id = repository.ApprovePR(Header.Document_Id,Header.Document_CreateUser, ref errMsg);

            //ds.Tables.Add(dtDocumentVnos);
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
        [HttpPost]
        [ActionName("ApprovePROverBudget")]
        public HttpResponseMessage ApprovePROverBudget(PRHeaderModels Header)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int id = repository.ApprovePROverBudget(Header.Document_Id, Header.Document_CreateUser, ref errMsg);

            //ds.Tables.Add(dtDocumentVnos);
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

        [HttpPost]
        [ActionName("UploadFiles")]
        public HttpResponseMessage UploadFiles(string tmppath)
        {
            //Create the Directory.
            string path = HttpContext.Current.Server.MapPath("~/tmpUpload/" + tmppath + "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Save the Files.
            foreach (string key in HttpContext.Current.Request.Files)
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[key];
                postedFile.SaveAs(path + postedFile.FileName);
            }

            //Send OK Response to Client.
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
