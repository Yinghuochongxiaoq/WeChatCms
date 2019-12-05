using System.Web.Mvc;
using WeChatCmsCommon.CustomerAttribute;

namespace WeChatCms
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CompressAttribute());
        }
    }
}
