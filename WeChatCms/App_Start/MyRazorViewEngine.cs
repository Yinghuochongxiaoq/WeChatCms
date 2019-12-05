using System.Web.Mvc;

namespace WeChatCms
{
    /// <summary>
    /// 重写视图引擎
    /// </summary>
    public class MyRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// 压缩页面css，js，html
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        /// <param name="masterPast"></param>
        /// <returns></returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPast)
        {
            //压缩输出
            return new MyRazorView(controllerContext, viewPath, masterPast, true, FileExtensions);
        }
    }
}