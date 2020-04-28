using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APKOnline.Controllers
{
    public class PurchaseRequestController : Controller
    {
        // GET: PurchaseRequest
        public ActionResult ListPurchaseRequest()
        {
            return View();
        }

        public ActionResult ApprovePurchaseRequest()
        {
            return View();
        }
        public ActionResult ViewPurchaseRequest()
        {
            return View();
        }
        public ActionResult ListApprovePurchaseRequest()
        {
            return View();
        }
        public ActionResult ApprovePR()
        {
            return View();
        }
        public ActionResult ListOverBGPurchaseRequest()
        {
            return View();
        }
        public ActionResult ApprovePROver()
        {
            return View();
        }
    }
}