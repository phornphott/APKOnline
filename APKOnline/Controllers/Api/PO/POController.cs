using APKOnline.DBHelper;
using APKOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APKOnline
{
    public class POController : ApiController
    {
        static readonly IPOData repository = new POData();

      
        //[HttpGet]
        //[ActionName("PreparePageData")]
        //public HttpResponseMessage GETPreparePageData(int id,int type)
        //{
        //    string errMsg = "";
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    Result resData = new Result();
        //    DataTable dtPRdetail = new DataTable();
        //    DataTable dtDocumentGroup = new DataTable();


        //    //if (type == 0)
        //    //{
        //    //    id = repository.InsertHeader(ref errMsg);
        //    //}




        //    dtDocumentGroup.Columns.Add("ID",typeof(int));
        //    dtDocumentGroup.Columns.Add("Name", typeof(string));

        //    for (int i = 0; i <= 4; i++)
        //    {
        //        DataRow nrow = dtDocumentGroup.NewRow();
        //        nrow[0] = i;
        //        string name = "";
        //        switch (i)
        //        {
        //            case 0:
        //                name = "- กรุณาเลือก -";
        //                break;
        //            case 1:
        //                name = "สินค้าทั่วไป";
        //                break;
        //            case 2:
        //                name = "เชื่อเพลิง";
        //                break;
        //            case 3:
        //                name = "ค่าใช้จ่ายทั่วไป";
        //                break;
        //            case 4:
        //                name = "ค่าใช้จ่ายเงินสดย่อย";
        //                break;
        //            default:
        //                break;
        //        }
        //        nrow[1] = name;
        //        dtDocumentGroup.Rows.Add(nrow);
        //    }
        //    dtDocumentGroup.TableName = "DocumentGroup";

        //    DataTable dtDocumentCate = repository.GetCategoryData(ref errMsg);
        //    DataTable dtDocumentObj = repository.GetObjectiveData(0, ref errMsg);
        //    DataTable dtJob = repository.GetJobData(ref errMsg);
        //    DataTable dtAccount = repository.GetAccountData(ref errMsg);
        //    DataTable dtdept = repository.GetDepData(ref errMsg);
        //    DataTable dtHeader = repository.GetHeaderData(id, type, ref errMsg);
        //    DataTable dtDetail = repository.GetDetailData(id, type, ref errMsg);

        //    ds.Tables.Add(dtDocumentGroup);
        //    ds.Tables.Add(dtDocumentCate);
        //    ds.Tables.Add(dtDocumentObj);
        //    ds.Tables.Add(dtJob);
        //    ds.Tables.Add(dtAccount);
        //    ds.Tables.Add(dtdept);
        //    ds.Tables.Add(dtHeader);
        //    ds.Tables.Add(dtDetail);

        //    if (errMsg != "")
        //    {
        //        resData.StatusCode = (int)(StatusCodes.Error);
        //        resData.Messages = errMsg;
        //    }
        //    else
        //    {
        //        resData.StatusCode = (int)(StatusCodes.Succuss);
        //        resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
        //    }

        //    resData.Results = ds;
        //    return Request.CreateResponse(HttpStatusCode.OK, resData);
        //}
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
        [ActionName("CreatePOData")]
        public HttpResponseMessage GETCreatePOData(int id,int tmpid)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            
            DataTable dtHeaderData = repository.GetPRHeaderData(id, 1, ref errMsg);

           
            DataTable dtDetail = repository.GetDetailData(id, 0, ref errMsg);
            if (dtDetail.Rows.Count <= 0)
            {
                repository.InserttmpDetail(id, tmpid, ref errMsg);
                dtDetail = repository.GetDetailData(id, 0, ref errMsg);
            }
            DataTable dtDocumentVnos = repository.GeneratePONo(id, ref errMsg);
            ds.Tables.Add(dtDocumentVnos);
            ds.Tables.Add(dtHeaderData);
            ds.Tables.Add(dtDetail);
            DataTable dtCus = repository.GetCustomer(0, ref errMsg);
            ds.Tables.Add(dtCus);


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
                    dr[1] = id.ToString() + "/" + dr[0];

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
        [ActionName("GETApprovePO")]
        public HttpResponseMessage GETApprovePO(int id,int StaffID)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtHeaderData = repository.GetPOHeaderData(id, StaffID, ref errMsg);


            DataTable dtDetail = repository.GetDetailData(id, ref errMsg);
            
           
            ds.Tables.Add(dtHeaderData);
            ds.Tables.Add(dtDetail);


            foreach (DataRow row in dtHeaderData.Rows)
            {
                //Get Pathfile
                DataTable dtfile = new DataTable();
                dtfile.Columns.Add("filename");
                dtfile.Columns.Add("path");

                string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/" + row["Document_PRID"].ToString() + "/");
                if (System.IO.Directory.Exists(targetpath))
                {
                    string[] files = System.IO.Directory.GetFiles(targetpath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        DataRow dr = dtfile.NewRow();



                        // Use static Path methods to extract only the file name from the path.
                        dr[0] = System.IO.Path.GetFileName(s);
                        dr[1] = id.ToString() + "/" + dr[0];

                        dtfile.Rows.Add(dr);
                        //string destFile = System.IO.Path.Combine(targetpath, fileName);

                    }

                }
                dtfile.TableName = "FileUpload";
                ds.Tables.Add(dtfile);
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
        [ActionName("GETViewPO")]
        public HttpResponseMessage GETViewPO(int id, int StaffID)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtHeaderData = repository.GetPOHeaderData(id, StaffID, ref errMsg);


            DataTable dtDetail = repository.GetDetailData(id, ref errMsg);


            ds.Tables.Add(dtHeaderData);
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
        [ActionName("ListPO")]
        public HttpResponseMessage ListPO(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetListPO(id,ref errMsg);


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
        [ActionName("ListPOApprove")]
        public HttpResponseMessage ListPOApprove(int  id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetListPOForApprove(id,ref errMsg);


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
        [ActionName("ListPRForCreatePO")]
        public HttpResponseMessage GETListPRForCreatePO(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();

            dt = repository.GetPRDataForCreatePO(id, ref errMsg);


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
        [ActionName("CancelPOTmpDetail")]
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
        //[HttpPost]
        //[ActionName("AddPRDetail")]
        //public HttpResponseMessage AddPRDetail(PRDetailModels detail)
        //{
        //    string errMsg = "";
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    Result resData = new Result();


        //   int id = repository.InserttmpDetail(detail, ref errMsg);

        //    //ds.Tables.Add(dtDocumentVnos);
        //    if (errMsg != "")
        //    {
        //        resData.StatusCode = (int)(StatusCodes.Error);
        //        resData.Messages = errMsg;
        //    }
        //    else
        //    {
        //        resData.StatusCode = (int)(StatusCodes.Succuss);
        //        resData.Messages = (String)EnumString.GetStringValue(StatusCodes.Succuss);
        //    }

        //    resData.Results = ds;
        //    return Request.CreateResponse(HttpStatusCode.OK, resData);
        //}

        [HttpPost]
        [ActionName("SavePOData")]
        public HttpResponseMessage SavePRData(PRHeaderModels Header,int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int docid = repository.InsertHeader(Header,id, ref errMsg);

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
        [ActionName("UpdateDetailData")]
        public HttpResponseMessage UpdateDetailData(PRDetailModels detail, int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int docid = repository.UpdatetmpDetail(detail,  ref errMsg);

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
        [HttpGet]
        [ActionName("GeneratePONo")]
        public HttpResponseMessage GETGeneratePRID(int id)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            DataTable dtDocumentVnos = repository.GeneratePONo(id, ref errMsg);

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
        [ActionName("ApprovePOData")]
        public HttpResponseMessage ApprovePRData(PRHeaderModels Header)
        {
            string errMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Result resData = new Result();


            int id = repository.ApprovePO(Header.Document_Id,Header.Document_CreateUser, ref errMsg);

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
    }
}
