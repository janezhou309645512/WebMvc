using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Areas.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Web/Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowTable()
        {
            return View();
        }

        public string QueryList(int start,int limit)
        {
        



            return Dal.Test.MQueryList(start,limit);

        }









    }
}