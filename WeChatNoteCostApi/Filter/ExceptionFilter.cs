using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;

namespace WeChatNoteCostApi.Filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //如果截获异常为我们自定义，可以处理的异常则通过我们自己的规则处理
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = actionExecutedContext.Exception.Message
            };
            //TODO:记录日志
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, resultMode);
        }
    }
}