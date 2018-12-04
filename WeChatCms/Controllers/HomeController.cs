using System.Web.Mvc;
using WeChatCmsCommon.CustomerAttribute;

namespace WeChatCms.Controllers
{
    public class HomeController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        /// 欢迎页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WelCome()
        {
            ViewBag.Html = "欢迎光临";
            return View();
        }
    }
}
