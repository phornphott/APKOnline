using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APKOnline.Controllers
{
    public class ManageStaffController : Controller
    {
        // GET: ManageStaff
        public ActionResult PermissionStaff()
        {
            return View();
        }
        public ActionResult ManageStaff()
        {
            return View();
        }
        public ActionResult ManageDept()
        {
            return View();
        }
        public ActionResult ManageRole()
        {
            return View();
        }
    }
}