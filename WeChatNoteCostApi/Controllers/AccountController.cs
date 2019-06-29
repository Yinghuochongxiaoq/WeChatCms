using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Http;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Configure;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.Enum;
using FreshCommonUtility.Security;
using WeChatCmsCommon.CheckCodeHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatModel.WeChatAuthModel;
using WeChatNoteCostApi.WeChatHelper;
using WeChatNoteCostApi.WeChatInnerModel;
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
        public ResponseBaseModel<WeChatAuthResponseModel> GetUserOpenId([FromBody]WeChatLoginInfo loginInfo)
        {
            var data = RedisCacheHelper.Get<WeChatAuthResponseModel>(RedisCacheKey.AuthInfoKey + loginInfo.code);
            if (data != null)
            {
                return new ResponseBaseModel<WeChatAuthResponseModel> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证成功", Data = data };
            }
            var weChatCheck = new WeChatAppDecrypt(AppConfigurationHelper.GetString("XcxAppID", ""), AppConfigurationHelper.GetString("XcxAppSecrect", ""));
            var openIdAndSessionKeyModel = weChatCheck.DecodeOpenIdAndSessionKey(loginInfo);
            if (openIdAndSessionKeyModel == null)
            {
                return new ResponseBaseModel<WeChatAuthResponseModel> { ResultCode = ResponceCodeEnum.Fail, Message = "微信认证失败" };
            }
            var isValidData = weChatCheck.VaildateUserInfo(loginInfo, openIdAndSessionKeyModel);
            if (!isValidData)
            {
                return new ResponseBaseModel<WeChatAuthResponseModel> { ResultCode = ResponceCodeEnum.Fail, Message = "请求信息验签失败" };
            }
            var responseData = weChatCheck.Decrypt(loginInfo.encryptedData, loginInfo.iv, openIdAndSessionKeyModel.session_key);
            if (responseData == null)
            {
                return new ResponseBaseModel<WeChatAuthResponseModel> { ResultCode = ResponceCodeEnum.Fail, Message = "微信认证失败" };
            }
            var server = new WechatAccountService();
            var searchOpenIdModel = server.GetByOpenId(responseData.openId);
            //TODO:新的访问者
            if (searchOpenIdModel == null)
            {
                var newModel = new WeChatAccountModel
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

                var newSysModel = new SysUser
                {
                    Birthday = "1900-01-01 00:00:00",
                    CreateTime = DateTime.Now,
                    IsDel = FlagEnum.HadZore,
                    CreateAuth = 1,
                    HeadUrl = newModel.AvatarUrl,
                    Password = AesHelper.AesEncrypt("123456"),
                    Sex = EnumHelper.GetEnumByValue<SexEnum>(newModel.Gender),
                    UpdateAuth = 1,
                    UpdateTime = DateTime.Now,
                    UserType = UserTypeEnum.Usually,
                    UserName = Guid.NewGuid().ToString()
                };
                var accountService = new AccountService();
                var resultId = accountService.InsertWeChatUserAndBind(newSysModel, newModel.OpenId);
                newModel.AccountId = resultId;
                searchOpenIdModel = newModel;
            }
            else if (searchOpenIdModel.AccountId < 1)
            {
                var newSysModel = new SysUser
                {
                    Birthday = "1900-01-01 00:00:00",
                    CreateTime = DateTime.Now,
                    IsDel = FlagEnum.HadZore,
                    CreateAuth = 1,
                    HeadUrl = searchOpenIdModel.AvatarUrl,
                    Password = AesHelper.AesEncrypt("123456"),
                    Sex = EnumHelper.GetEnumByValue<SexEnum>(searchOpenIdModel.Gender),
                    UpdateAuth = 1,
                    UpdateTime = DateTime.Now,
                    UserType = UserTypeEnum.Usually,
                    UserName = Guid.NewGuid().ToString()
                };
                var accountService = new AccountService();
                var resultId = accountService.InsertWeChatUserAndBind(newSysModel, searchOpenIdModel.OpenId);
                searchOpenIdModel.AccountId = resultId;
            }

            var resultModel = new WeChatAuthResponseModel
            {
                Token = Guid.NewGuid().ToString(),
                CodeTimeSpan = responseData.watermark?.timestamp,
                AvatarUrl = responseData.avatarUrl,
                AccountId = searchOpenIdModel.AccountId,
                NickName = responseData.nickName
            };
            //TODO：记录Token信息
            RedisCacheHelper.AddSet(RedisCacheKey.AuthInfoKey + loginInfo.code, resultModel,DateTime.Now.AddHours(2));
            RedisCacheHelper.AddSet(RedisCacheKey.AuthTokenKey + resultModel.Token, searchOpenIdModel, DateTime.Now.AddHours(2));
            return new ResponseBaseModel<WeChatAuthResponseModel> { ResultCode = ResponceCodeEnum.Success, Message = "微信认证成功", Data = resultModel };
        }

        /// <summary>
        /// 绑定账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseBaseModel<dynamic> BindWeChatUser(string name, string password, string checkcode, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "token失效" };
            }
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            if (userData == null || userData.AccountId > 0)
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "token失效或者已经绑定过信息" };
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "用户名和密码不能为空" };
            }

            if (string.IsNullOrEmpty(checkcode))
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "验证码不能为空" };
            }
            var oldCheckCode = RedisCacheHelper.Get<string>(RedisCacheKey.AuthCheckCodeKey + token);
            RedisCacheHelper.Remove(RedisCacheKey.AuthCheckCodeKey + token);
            if (oldCheckCode != checkcode)
            {
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "验证码错误" };
            }
            var accountService = new AccountService();
            password = AesHelper.AesEncrypt(password);
            var loginInfo = accountService.GetSysUsersByUserName(name)?.FirstOrDefault();
            //没有该用户则注册一个用户
            if (loginInfo == null || loginInfo.Id < 1)
            {
                var newModel = new SysUser
                {
                    Birthday = "1900-01-01 00:00:00",
                    CreateTime = DateTime.Now,
                    IsDel = FlagEnum.HadZore,
                    CreateAuth = 1,
                    HeadUrl = userData.AvatarUrl,
                    Password = password,
                    Sex = EnumHelper.GetEnumByValue<SexEnum>(userData.Gender),
                    UpdateAuth = 1,
                    UpdateTime = DateTime.Now,
                    UserType = UserTypeEnum.Usually,
                    UserName = name
                };
                var resultId = accountService.InsertWeChatUserAndBind(newModel, userData.OpenId);
                //处理成功
                if (resultId > 0)
                {
                    userData.AccountId = resultId;
                    RedisCacheHelper.AddSet(RedisCacheKey.AuthTokenKey + token, userData, DateTime.Now.AddDays(1));
                }
                var resultModel = new WeChatAuthResponseModel
                {
                    Token = token,
                    CodeTimeSpan = DateTime.Now.AddDays(1).ToLongTimeString(),
                    AvatarUrl = userData.AvatarUrl,
                    AccountId = resultId
                };
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "", Data = resultModel };
            }
            //有该用户，查看是否已经绑定过别人
            else
            {
                if (loginInfo.Password != password)
                {
                    return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "密码错误" };
                }
                var wechatServer = new WechatAccountService();
                var otherData = wechatServer.GetByAccountId(loginInfo.Id);
                //有人已经绑定了
                if (otherData != null)
                {
                    if (otherData.OpenId == userData.OpenId)
                    {
                        var resultModeltemp = new WeChatAuthResponseModel
                        {
                            Token = token,
                            CodeTimeSpan = DateTime.Now.AddDays(1).ToLongTimeString(),
                            AvatarUrl = userData.AvatarUrl,
                            AccountId = loginInfo.Id
                        };
                        RedisCacheHelper.AddSet(RedisCacheKey.AuthTokenKey + token, userData, DateTime.Now.AddDays(1));
                        return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "", Data = resultModeltemp };
                    }
                    return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Fail, Message = "非法绑定" };
                }
                userData.AccountId = loginInfo.Id;
                var oldModel = wechatServer.GetByOpenId(userData.OpenId);
                if (oldModel == null)
                {
                    wechatServer.SaveModel(userData);
                }
                else
                {
                    oldModel.AccountId = userData.AccountId;
                    wechatServer.SaveModel(oldModel);
                }
                RedisCacheHelper.AddSet(RedisCacheKey.AuthTokenKey + token, userData, DateTime.Now.AddDays(1));
                var resultModel = new WeChatAuthResponseModel
                {
                    Token = token,
                    CodeTimeSpan = DateTime.Now.AddDays(1).ToLongTimeString(),
                    AvatarUrl = userData.AvatarUrl,
                    AccountId = loginInfo.Id
                };
                return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "", Data = resultModel };
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="token">token</param>
        [AllowAnonymous]
        [HttpGet]
        public ResponseBaseModel<dynamic> CheckCodeBaseStr(string token)
        {
            var yzm = new YzmHelper();
            yzm.CreateImage();
            var code = yzm.Text;
            Bitmap img = yzm.Image;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            string str = Convert.ToBase64String(ms.ToArray());
            RedisCacheHelper.Remove(RedisCacheKey.AuthCheckCodeKey + token);
            RedisCacheHelper.AddSet(RedisCacheKey.AuthCheckCodeKey + token, code, DateTime.Now.AddMinutes(3));
            return new ResponseBaseModel<dynamic> { ResultCode = ResponceCodeEnum.Success, Message = "", Data = "data:image/png;base64," + str };
        }
        #endregion
    }
}