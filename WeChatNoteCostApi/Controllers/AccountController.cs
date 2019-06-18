using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using FreshCommonUtility.Configure;
using FreshCommonUtility.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.WeChatAuthModel;

namespace WeChatNoteCostApi.Controllers
{
    public class AccountController : ApiControllerBase
    {
        #region [1、小程序模式注册登录]
        /// <summary>
        /// 获取微信小程序授权信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public ResponseBaseModel<dynamic> GetUserOpenId(string code, string encryptedData, string iv)
        {
            StringBuilder urlStr = new StringBuilder();
            urlStr.AppendFormat(@"https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}"
                    + "&grant_type=authorization_code", AppConfigurationHelper.GetString("XCXAppID", ""),
                AppConfigurationHelper.GetString("XCXAppSecrect", ""),
                    code
                );
            string retString = FreshCommonUtility.Web.WebHttpHelper.HttpGet(urlStr.ToString());
            WxValidateUserResponse vdModel = JsonConvert.DeserializeObject<WxValidateUserResponse>(retString);
            if (vdModel != null)
            {
                string result = AesHelper.AesDecrypt(encryptedData, vdModel.session_key, iv);
                JObject usrInfo = (JObject)JsonConvert.DeserializeObject(result);
                WxResponseUserInfo responseData = new WxResponseUserInfo
                {
                    nickName = usrInfo["nickName"].ToString(),
                    gender = usrInfo["gender"].ToString(),
                    city = usrInfo["city"].ToString(),
                    province = usrInfo["province"].ToString(),
                    country = usrInfo["country"].ToString(),
                    avatarUrl = usrInfo["avatarUrl"].ToString(),
                    sessionKey = vdModel.session_key,
                    openId = usrInfo["openId"].ToString()
                };
                try
                {
                    responseData.unionId = usrInfo["unionId"].ToString();
                }
                catch (Exception)
                {
                    responseData.unionId = "null";
                }
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证成功", Data = responseData };
            }
            else
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证失败" };
            }
        }

        /// <summary>
        /// 小程序注册
        /// </summary>
        /// <param name="eneity"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> PostWxRegister()
        {
            return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证失败", Data = DateTime.Now };
        }
        #endregion
    }
}