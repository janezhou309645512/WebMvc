using System.Web.Mvc;

namespace WebMvc.Areas.KanBan
{
    public class KanBanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KanBan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KanBan_default",
                "KanBan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}