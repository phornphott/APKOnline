using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APKOnline.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult ListPurchaseOrder()
        {
            return View();
        }

        public ActionResult ViewPurchaseOrder()
        {
            return View();
        }
        public ActionResult ListPurchaseOrderApprove()
        {
            return View();
        }
        public ActionResult ApprovePO()
        {
            return View();
        }
        public ActionResult ViewPO()
        {
            return View();
        }
    }
}