using System.Collections.Generic;
using System.Web.Http;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;

namespace WeChatNoteCostApi.Controllers
{
    public class ConfigFunctionController : ApiControllerBase
    {
        /// <summary>
        /// 绑定账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> MyConfigInfo()
        {
            return new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "",
                Data = new List<dynamic>{
                //new {
                //    title = "账户设置",
                //    link = "/pages/costchannel/costchannel"
                //},
                    new {
                        title = "消费类型设置",
                        link = "/pages/costtype/costtype"
                    },
                    new {
                        title = "绑定家庭成员",
                        link = "/pages/bindfamily/index"
                    },
                    new {
                        title = "打卡记录",
                        link = "/pages/calendar/calendar"
                    },
                    new {
                        title = "轻松一刻",
                        link = "/pages/relax/relax?couldInGame=true"
                        //link = "/pages/relax/relax"
                    },
                    new {
                        title = "关于我们",
                        link = "./about"
                    }
                }
            };
        }
    }
}