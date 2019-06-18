using System.Web.Http;
using FreshCommonUtility.Dapper;
using WeChatService;

namespace WeChatNoteCostApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            DbLinkTestService.DbLink();
        }
    }
}
