using System;
using System.Web;
using System.Web.Mvc;
using FreshCommonUtility.Configure;

namespace WeChatCmsCommon.Unit
{
    /// <summary>
    ///  文件服务器分离，需要得到文件服务器上文件的地址
    /// </summary>
    public static class StaticFileHelper
    {
        /// <summary>
        /// 取得静态服务器的网址
        /// 如果是https网站，跨域调用静态资源需要欺骗浏览器如：http://content..../.png 改成 //content..../.png
        /// </summary>
        /// <returns></returns>
        private static string _staticServiceUri;
        public static string GetStaticServiceUri()
        {
            //使用本地图片，而不做资源分离，暂时取本地地址：
            if (_staticServiceUri == null)
                _staticServiceUri = HttpContext.Current.Request.Url.Scheme+"://" + HttpContext.Current.Request.Url.Authority;

            return _staticServiceUri;
        }

        /// <summary>
        /// 得到静态文件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StaticFile(this UrlHelper helper, string path)
        {
            var rootUrl = AppConfigurationHelper.GetString("ReferenceKey.RootNode") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            if (path.StartsWith("~"))
                return helper.Content(path);
            else
                return GetStaticServiceUri() + rootUrl + path;
        }

        public static string JsCssFile(this UrlHelper helper, string path)
        {
            var jsAndCssFileEdition = AppConfigurationHelper.GetString("JsAndCssFileEdition");
            if (string.IsNullOrEmpty(jsAndCssFileEdition))
                jsAndCssFileEdition = Guid.NewGuid().ToString();

            path += string.Format("?v={0}", jsAndCssFileEdition);

            return helper.StaticFile(path);
        }

        /// <summary>
        /// 得到图片文件，以及缩略图
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ImageFile(this UrlHelper helper, string path, string size = null)
        {
            if (string.IsNullOrEmpty(path))
                return helper.StaticFile(@"/content/images/no_picture.jpg");

            if (size == null)
                return helper.StaticFile(path);

            var ext = path.Substring(path.LastIndexOf('.'));
            var head = path.Substring(0, path.LastIndexOf('.'));
            var url = string.Format("{0}{1}_{2}{3}", GetStaticServiceUri(), head, size, ext);
            return url;
        }

        /// <summary>
        /// 得到文件服务器根网址
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string StaticFile(this UrlHelper helper)
        {
            return GetStaticServiceUri();
        }

        /// <summary>
        /// 用户cookie key
        /// </summary>
        public static string UserCookieStr = "Context_UserInfo";
    }
}
