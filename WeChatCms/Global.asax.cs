using System.Web.Mvc;
using System.Web.Routing;
using FreshCommonUtility.Dapper;
using WeChatService;

namespace WeChatCms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //自定义加载引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MyRazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            DbLinkTestService.DbLink();
        }
    }
}
