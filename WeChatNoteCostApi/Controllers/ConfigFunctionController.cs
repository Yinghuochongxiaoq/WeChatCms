using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FreshCommonUtility.Cache;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatNoteCostApi.WeChatInnerModel;
using WeChatService;

namespace WeChatNoteCostApi.Controllers
{
    public class ConfigFunctionController : ApiControllerBase
    {
        readonly SysDicService _dicServer = new SysDicService();
        /// <summary>
        /// 绑定账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> MyConfigInfo()
        {
            var myPageMenuList = RedisCacheHelper.Get<List<SysdictModel>>(RedisCacheKey.WeChatMinProgramMyPage);
            var resultData = new List<dynamic>();
            if (myPageMenuList != null && myPageMenuList.Count > 0)
            {
                resultData.AddRange(
                    myPageMenuList.OrderByDescending(r => r.Sort).Select(f => new
                    {
                        title = f.Lable,
                        link = f.Value
                    }).ToList());
            }
            else
            {
                var data = _dicServer.GetAllDict("WeChatMyMenu");
                if (data != null && data.Count > 0)
                {
                    RedisCacheHelper.AddSet(RedisCacheKey.WeChatMinProgramMyPage, data, DateTime.Now.AddMinutes(10));
                    resultData.AddRange(
                        data.OrderByDescending(r => r.Sort).Select(f => new
                        {
                            title = f.Lable,
                            link = f.Value
                        }).ToList());
                }
            }

            return new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "",
                Data = resultData
            };
        }
    }
}