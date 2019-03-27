using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 自定义jsonresult类
    /// </summary>
    public class VMEJsonResult : JsonResult
    {
        /// <summary>
        /// 重写JsonResult中的序列化方法
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("拒绝GET访问");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null)
            {
                return;
            }
            // 设置日期序列化的格式
            JsonSerializerSettings setting = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss", };
            if (RecursionLimit.HasValue)
            {
                setting.MaxDepth = RecursionLimit.Value;
            }
            response.Write(JsonConvert.SerializeObject(Data, setting));
        }
    }
}