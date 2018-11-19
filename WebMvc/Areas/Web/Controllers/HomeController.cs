using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Areas.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Web/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();


        }
        public string UserLogin(string UserName, string Pwd)
        {


            return Dal.UserOperation.UserLogin(UserName, Pwd);


        }




    }
}