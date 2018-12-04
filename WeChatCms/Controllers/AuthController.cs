using System.Web.Mvc;
using FreshCommonUtility.Cookie;
using FreshCommonUtility.Security;
using Newtonsoft.Json;
using WeChatCmsCommon.CustomerAttribute;
using WeChatCmsCommon.Unit;
using WeChatService;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 登录操作
    /// </summary>
    public class AuthController : ControllerBase
    {
        private readonly AccountService _accountService = new AccountService();

        [AuthorizeIgnore]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(string username, string password)
        {
            password = AesHelper.AesEncrypt(password);
            var loginInfo = _accountService.UserLogin(username, password);

            if (loginInfo != null && loginInfo.IsLogin)
            {
                string data = JsonConvert.SerializeObject(loginInfo);
                CookieHelper.SetCookie(StaticFileHelper.UserCookieStr, AesHelper.AesEncrypt(DesHelper.DesEnCode(data)));
                return Redirect(ViewBag.RootNode + "/Home/WelCome");
            }
            ModelState.AddModelError("error", "用户名或密码错误");
            return View();
        }

        public ActionResult Logout()
        {
            CookieHelper.SetCookie(StaticFileHelper.UserCookieStr, string.Empty);
            return RedirectToAction("Login");
        }

        public ActionResult O404()
        {
            return View("404");
        }
    }
}
