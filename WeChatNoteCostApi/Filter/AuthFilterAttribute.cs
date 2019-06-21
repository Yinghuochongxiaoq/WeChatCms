using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Dynamic;
using Newtonsoft.Json;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatNoteCostApi.WeChatInnerModel;

namespace WeChatNoteCostApi.Filter
{
    public class AuthFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //如果用户方位的Action带有AllowAnonymousAttribute，则不进行授权验证
            if (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                .Any())
            {
                return;
            }
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            //TODO:验证权限
            var method = actionContext.Request.Method;
            object token = null;
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            if (method == HttpMethod.Post)
            {
                var requestContent = actionContext.Request.Content.ReadAsStringAsync();//读取post提交的json数据
                requestContent.Wait();//等待异步读取结束
                var inputJson = requestContent.Result;
                var model = JsonConvert.DeserializeObject<DynamicDataEntity>(inputJson);
                token = model?["token"];
            }
            else if (method == HttpMethod.Get)
            {
                var queryNameValuePairs = actionContext.Request.GetQueryNameValuePairs();
                var nameValuePairs = queryNameValuePairs.ToList();
                if (nameValuePairs.Any(f => f.Key == "token"))
                {
                    token = nameValuePairs.FirstOrDefault(r => r.Key == "token").Value;
                }
            }

            #region 判断TOKEN信息
            if (token != null && !string.IsNullOrEmpty(token.ToString()))
            {
                try
                {
                    var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
                    var tempUserId = userData?.AccountId;
                    if (tempUserId == null || tempUserId < 1)
                    {
                        resultMode.Message = "登录失效，请重新登录";
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, resultMode);
                    }
                    return;
                }
                catch (Exception e)
                {
                    resultMode.Message = "登录失效，请重新登录";
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, resultMode);
                }
            }
            resultMode.Message = "token无效";
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, resultMode);
            #endregion
        }
    }
}