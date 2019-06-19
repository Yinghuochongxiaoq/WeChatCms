using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using FreshCommonUtility.Configure;
using FreshCommonUtility.DataConvert;
using WeChatCmsCommon.CheckCodeHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatModel.WeChatAuthModel;
using WeChatNoteCostApi.WeChatHelper;
using WeChatService;

namespace WeChatNoteCostApi.Controllers
{
    public class AccountController : ApiControllerBase
    {
        #region [1、小程序注册登录绑定]

        /// <summary>
        /// 获取微信小程序授权信息
        /// </summary>
        /// <param name="loginInfo">微信登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> GetUserOpenId([FromBody]WechatLoginInfo loginInfo)
        {
            var weChatCheck = new WeChatAppDecrypt(AppConfigurationHelper.GetString("XcxAppID", ""), AppConfigurationHelper.GetString("XcxAppSecrect", ""));
            var openIdAndSessionKeyModel = weChatCheck.DecodeOpenIdAndSessionKey(loginInfo);
            if (openIdAndSessionKeyModel == null)
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证失败" };
            }
            var isValidData = weChatCheck.VaildateUserInfo(loginInfo, openIdAndSessionKeyModel);
            if (!isValidData)
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "请求信息验签失败" };
            }
            var responseData = weChatCheck.Decrypt(loginInfo.encryptedData, loginInfo.iv, openIdAndSessionKeyModel.session_key);
            var server = new WechatAccountService();
            var searchOpenIdModel = server.GetByOpenId(loginInfo.code);
            //TODO:新的访问者
            if (searchOpenIdModel == null)
            {
                var newModel = new WechatAccountModel
                {
                    AvatarUrl = responseData.avatarUrl,
                    CreateTime = DateTime.Now,
                    Gender = DataTypeConvertHelper.ToInt(responseData.gender, 1),
                    IsDel = FlagEnum.HadZore.GetHashCode(),
                    NickName = responseData.nickName,
                    OpenId = responseData.openId,
                    Remarks = "新访问用户"
                };
                server.SaveModel(newModel);
                searchOpenIdModel = newModel;
            }
            return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证成功", Data = searchOpenIdModel };
        }

        /// <summary>
        /// 绑定账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> BindWeChatUser(string name, string password)
        {
            return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证失败", Data = DateTime.Now };
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ResponseBaseModel<dynamic> CheckCodeBaseStr()
        {
            var yzm = new YzmHelper();
            yzm.CreateImage();
            var code = yzm.Text;
            Bitmap img = yzm.Image;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            string str = Convert.ToBase64String(ms.ToArray());
            return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "", Data = "data:image/png;base64,"+str };
            }
        #endregion
    }
}