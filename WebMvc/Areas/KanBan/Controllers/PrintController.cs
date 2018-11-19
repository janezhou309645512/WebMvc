using FastReport.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Areas.KanBan.Controllers
{
    public class PrintController : Controller
    {
        // GET: KanBan/Print
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Fr()
        {
            WebReport webReport = new WebReport();

            string filename = @"F:\fxfile\test.frx";
            webReport.Report.Load(filename);
            webReport.Report.SetParameterValue("Test", "MY");

            ViewBag.WebReport = webReport;



            return View();
        }

    }
}