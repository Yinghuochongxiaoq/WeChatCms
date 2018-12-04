using System.Web.Mvc;
using System.Web.Routing;
using FreshCommonUtility.Configure;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatService;

namespace WeChatCms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            DbLinkTestService.DbLink();
        }
    }
}
