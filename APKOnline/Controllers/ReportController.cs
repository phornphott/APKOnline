using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APKOnline.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ReportBudget()
        {
            return View();
        }
        public ActionResult PurchaseRequestReport()
        {
            return View();
        }
    }
}