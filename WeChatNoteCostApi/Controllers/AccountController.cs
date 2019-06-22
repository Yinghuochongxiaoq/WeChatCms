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
                searchOpenIdModel = newModel;
            }

            var resultModel = new WeChatAuthResponseModel
            {
                Token = Guid.NewGuid().ToString(),
                CodeTimeSpan = responseData.watermark?.timestamp,
                AvatarUrl = responseData.avatarUrl,
                AccountId = searchOpenIdModel.AccountId
            };
            //TODO：记录Token信息
            RedisCacheHelper.AddSet(RedisCacheKey.AuthInfoKey + loginInfo.code, resultModel,
                new TimeSpan(DataTypeConvertHelper.ToLong(responseData.watermark?.timestamp)));
            RedisCacheHelper.AddSet(RedisCacheKey.AuthTokenKey + resultModel.Token, searchOpenIdModel, new TimeSpan(DataTypeConvertHelper.ToLong(responseData.watermark?.timestamp)));
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
                    UserName = userData.NickName
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

        #region [2、获取账户流水记录]
        /// <summary>
        /// 获取消费记录异步数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCostPage(string token, int pageIndex = 1, int pageSize = 10, int costType = -1, int spendType = -1, long costchannel = -1)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            DateTime starttime = new DateTime(1900, 1, 1);
            DateTime endtime = new DateTime(1900, 1, 1);
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
            }
            else
            {
                var userId = tempUserId.Value;
                int count;
                var server = new CostContentService();
                var dataList = server.GetList(userId, spendType, null, null, costType, costchannel, starttime, endtime,
                    pageIndex, pageSize, out count);
                var dic = server.GetStatisticsCost(userId, spendType, null, null, costType, costchannel,
                    starttime, endtime);
                var allOutCost = dic.ContainsKey(CostInOrOutEnum.Out.GetHashCode())
                    ? dic[CostInOrOutEnum.Out.GetHashCode()]
                    : 0;
                var allInCost = dic.ContainsKey(CostInOrOutEnum.In.GetHashCode())
                    ? dic[CostInOrOutEnum.In.GetHashCode()]
                    : 0;
                var statisticsModel = new
                {
                    allCouldCost = $"{allInCost - allOutCost:N2}",
                    allOutCost = $"{allOutCost:N2}",
                    allInCost = $"{allInCost:N2}"
                };
                resultMode.Data = new { pageCount = count / pageSize + (count % pageSize > 0 ? 1 : 0), dataList, statisticsModel };
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }

            return resultMode;
        }

        /// <summary>
        /// 获取消费类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCostChannelType(string token)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }

            var _ = 0;
            var server = new CostTypeService();
            var userId = userData.AccountId;
            var costTypeData = server.GetList(-1, userId, 1, 100000, out _);
            var channelServer = new CostChannelService();
            var channelData = channelServer.GetList(FlagEnum.HadOne.GetHashCode(), userId, 1, 100000, out _);
            resultMode.Data = new
            {
                costTypeData,
                channelData
            };
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        ///// <summary>
        ///// 获取账户信息
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> GetCostChannel()
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var server = new CostChannelService();
        //    var userId = CurrentModel.UserId;
        //    var data = server.GetList(FlagEnum.HadOne.GetHashCode(), userId, 1, 100000, out _);
        //    resultMode.Data = data;
        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 添加信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> AddCostInfo(CostContentModel model)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //    };
        //    if (model == null)
        //    {
        //        resultMode.Message = "参数错误";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    var userId = CurrentModel.UserId;
        //    var server = new CostContentService();
        //    CostContentModel newModel = new CostContentModel();
        //    if (model.Id > 0)
        //    {
        //        newModel = server.GetContentModel(model.Id);
        //        //验证权限
        //        if (newModel == null || newModel.UserId != userId)
        //        {
        //            resultMode.Message = "非法访问";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }

        //        if (newModel.SpendType == 2 && (model.SpendType != 2 || model.LinkCostId < 0))
        //        {
        //            resultMode.Message = "转移记录类型不能修改或无关联入账信息";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }

        //        if (newModel.SpendType == 2 && newModel.CostInOrOut == CostInOrOutEnum.In)
        //        {
        //            resultMode.Message = "转移记录入账信息不能修改";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    //验证参数
        //    newModel.Cost = Math.Round(Math.Abs(model.Cost), 2);
        //    newModel.CostAddress = model.CostAddress;
        //    newModel.CostChannel = model.CostChannel;
        //    newModel.CostType = model.SpendType == 2 ? -1 : model.CostType;
        //    newModel.UserId = userId;
        //    newModel.CostInOrOut = model.CostInOrOut;
        //    newModel.CostThing = model.CostThing;
        //    newModel.CostTime = model.CostTime;
        //    newModel.CostMonth = newModel.CostTime.Month;
        //    newModel.CostYear = newModel.CostTime.Year;
        //    newModel.CreateTime = newModel.Id > 0 ? newModel.CreateTime : DateTime.Now;
        //    newModel.CreateUserId = userId;
        //    newModel.SpendType = model.SpendType;
        //    newModel.UpdateTime = DateTime.Now;
        //    newModel.UpdateUserId = userId;
        //    newModel.LinkCostChannel = model.LinkCostChannel;
        //    if (newModel.Cost < (decimal)0.01)
        //    {
        //        resultMode.Message = "金额设置错误";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.SpendType != 2)
        //    {
        //        var costTypeInfo = new CostTypeService().Get(newModel.CostType);
        //        if (costTypeInfo == null
        //            || costTypeInfo.UserId != userId
        //            || costTypeInfo.IsDel == FlagEnum.HadOne
        //            || costTypeInfo.IsValid == FlagEnum.HadZore)
        //        {
        //            resultMode.Message = "类型选择无效";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }
        //        newModel.CostTypeName = costTypeInfo.Name;
        //    }

        //    var costChannelServer = new CostChannelService();
        //    var costChannelInfo = costChannelServer.Get(newModel.CostChannel);
        //    if (costChannelInfo == null
        //        || costChannelInfo.UserId != userId
        //        || costChannelInfo.IsDel == FlagEnum.HadOne
        //        || costChannelInfo.IsValid == FlagEnum.HadZore)
        //    {
        //        resultMode.Message = "账户选择无效";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    newModel.CostChannelName = costChannelInfo.CostChannelName;
        //    newModel.CostChannelNo = costChannelInfo.CostChannelNo;
        //    newModel.CostInOrOut = newModel.SpendType == 1 ? CostInOrOutEnum.In : CostInOrOutEnum.Out;

        //    if (newModel.SpendType == 2)
        //    {
        //        if (newModel.LinkCostChannel < 1)
        //        {
        //            resultMode.Message = "转入账户无效";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }
        //        var linkChannelInfo = costChannelServer.Get(newModel.LinkCostChannel);
        //        if (linkChannelInfo == null
        //            || linkChannelInfo.UserId != userId
        //            || linkChannelInfo.IsDel == FlagEnum.HadOne
        //            || linkChannelInfo.IsValid == FlagEnum.HadZore)
        //        {
        //            resultMode.Message = "转入账户无效";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }

        //        newModel.LinkCostChannelName = linkChannelInfo.CostChannelName;
        //        newModel.LinkCostChannelNo = linkChannelInfo.CostChannelNo;
        //    }
        //    else
        //    {
        //        newModel.LinkCostChannel = 0;
        //        newModel.LinkCostId = 0;
        //        newModel.LinkCostChannelNo = "";
        //        newModel.LinkCostChannelName = "";
        //    }
        //    var costContentServer = new CostContentService();
        //    costContentServer.AddAndUpdateContentInfo(newModel);
        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取记录信息
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> GetCostModel(long id)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var server = new CostContentService();
        //    var userId = CurrentModel.UserId;
        //    var data = server.GetContentModel(id);
        //    if (data != null && data.UserId == userId)
        //    {
        //        resultMode.Data = data;
        //        resultMode.ResultCode = ResponceCodeEnum.Success;
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    resultMode.Message = "查询失败";
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取账户设置异步数据
        ///// </summary>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="name"></param>
        ///// <param name="isValid"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> GetCostChannelPage(int pageIndex = 1, int pageSize = 10, string name = "", int isValid = -1)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var userId = CurrentModel.UserId;
        //    if (userId < 1)
        //    {
        //        resultMode.Message = "登录失效，请重新登录";
        //    }
        //    else
        //    {
        //        var server = new CostChannelService();
        //        var dataList = server.GetList(isValid, userId, pageIndex, pageSize, out var count, name);
        //        resultMode.Data = new { count, dataList };
        //        resultMode.ResultCode = ResponceCodeEnum.Success;
        //    }

        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取信息
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> GetCostChannelModel(long id)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var server = new CostChannelService();
        //    var userId = CurrentModel.UserId;
        //    var data = server.Get(id);
        //    if (data != null && data.UserId == userId)
        //    {
        //        resultMode.Data = data;
        //        resultMode.ResultCode = ResponceCodeEnum.Success;
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    resultMode.Message = "查询失败";
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 保存信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="costChannelName"></param>
        ///// <param name="costChannelNo"></param>
        ///// <param name="sort"></param>
        ///// <param name="isValid"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> SaveCostChannelInfo(long id, string costChannelName, string costChannelNo, int sort, int isValid)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //    };
        //    if (string.IsNullOrEmpty(costChannelName))
        //    {
        //        resultMode.Message = "名称不能为空";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    var userId = CurrentModel.UserId;
        //    var server = new CostChannelService();
        //    CostChannelModel newModel = new CostChannelModel();
        //    if (id > 0)
        //    {
        //        newModel = server.Get(id);
        //        //验证权限
        //        if (newModel == null || newModel.UserId != userId)
        //        {
        //            resultMode.Message = "非法访问";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    //验证参数
        //    newModel.Id = id;
        //    newModel.IsDel = FlagEnum.HadZore;
        //    newModel.IsValid = EnumHelper.GetEnumByValue<FlagEnum>(isValid);
        //    newModel.CostChannelName = costChannelName;
        //    newModel.CostChannelNo = costChannelNo;
        //    newModel.UserId = userId;
        //    newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
        //    newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
        //    newModel.Sort = sort;
        //    newModel.UpdateTime = DateTime.Now;
        //    newModel.UpdateUserId = userId;

        //    var costTypeInfoList = server.GetList(costChannelName, userId);
        //    if (costTypeInfoList != null
        //        && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
        //    {
        //        resultMode.Message = "账号名称已经存在";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    server.SaveModel(newModel);
        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 删除类型
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="isValid"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> DelCostChannelModel(long id, int isValid)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Success,
        //        Message = "响应成功"
        //    };
        //    var userId = CurrentModel.UserId;
        //    var server = new CostChannelService();
        //    var oldModel = server.Get(id);
        //    if (oldModel == null || oldModel.UserId != userId)
        //    {
        //        resultMode.ResultCode = ResponceCodeEnum.Fail;
        //        resultMode.Message = "参数错误";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    oldModel.IsDel = FlagEnum.HadZore;
        //    oldModel.IsValid = EnumHelper.GetEnumByValue<FlagEnum>(isValid);
        //    oldModel.UpdateUserId = userId;
        //    oldModel.UpdateTime = DateTime.Now;
        //    try
        //    {
        //        server.SaveModel(oldModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine(e);
        //    }
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 初始化账户信息
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> InitCostChannelModel()
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Success,
        //        Message = "响应成功"
        //    };
        //    var userId = CurrentModel.UserId;
        //    var server = new CostChannelService();
        //    var modelList = server.GetList(-1, userId, 1, 10, out _);
        //    if (modelList != null && modelList.Count > 0)
        //    {
        //        resultMode.ResultCode = ResponceCodeEnum.Fail;
        //        resultMode.Message = "已经初始化过";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    server.InitCostChannel(userId);

        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    resultMode.Message = "初始化成功";
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取消费类型设置异步数据
        ///// </summary>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="name"></param>
        ///// <param name="spendType"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> GetCostTypePage(int pageIndex = 1, int pageSize = 10, string name = "", int spendType = -1)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var userId = CurrentModel.UserId;
        //    if (userId < 1)
        //    {
        //        resultMode.Message = "登录失效，请重新登录";
        //    }
        //    else
        //    {
        //        var server = new CostTypeService();
        //        var dataList = server.GetList(spendType, userId, pageIndex, pageSize, out var count, name);
        //        resultMode.Data = new { count, dataList };
        //        resultMode.ResultCode = ResponceCodeEnum.Success;
        //    }

        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取类型信息
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> Result GetCostTypeModel(long id)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //        Message = ""
        //    };
        //    var server = new CostTypeService();
        //    var userId = CurrentModel.UserId;
        //    var data = server.Get(id);
        //    if (data != null && data.UserId == userId)
        //    {
        //        resultMode.Data = data;
        //        resultMode.ResultCode = ResponceCodeEnum.Success;
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    resultMode.Message = "查询失败";
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 保存信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="name"></param>
        ///// <param name="sort"></param>
        ///// <param name="spendType"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> SaveTypeInfo(long id, string name, int sort, int spendType)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Fail,
        //    };
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        resultMode.Message = "名称不能为空";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    var userId = CurrentModel.UserId;
        //    var server = new CostTypeService();
        //    CostTypeModel newModel = new CostTypeModel();
        //    if (id > 0)
        //    {
        //        newModel = server.Get(id);
        //        //验证权限
        //        if (newModel == null || newModel.UserId != userId)
        //        {
        //            resultMode.Message = "非法访问";
        //            return Json(resultMode, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    //验证参数
        //    newModel.Id = id;
        //    newModel.SpendType = spendType;
        //    newModel.IsDel = FlagEnum.HadZore;
        //    newModel.IsValid = FlagEnum.HadOne;
        //    newModel.Name = name;
        //    newModel.UserId = userId;
        //    newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
        //    newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
        //    newModel.Sort = sort;
        //    newModel.UpdateTime = DateTime.Now;
        //    newModel.UpdateUserId = userId;

        //    var costTypeInfoList = server.GetList(spendType, userId, name);
        //    if (costTypeInfoList != null
        //        && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
        //    {
        //        resultMode.Message = "类型名称已经存在";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    server.SaveModel(newModel);
        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 删除类型
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> InvalidCostTypeModel(long id)
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Success,
        //        Message = "响应成功"
        //    };
        //    var userId = CurrentModel.UserId;
        //    var server = new CostTypeService();
        //    var oldModel = server.Get(id);
        //    if (oldModel == null || oldModel.UserId != userId)
        //    {
        //        resultMode.ResultCode = ResponceCodeEnum.Fail;
        //        resultMode.Message = "参数错误";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }

        //    oldModel.IsDel = FlagEnum.HadOne;
        //    oldModel.IsValid = FlagEnum.HadZore;
        //    oldModel.UpdateUserId = userId;
        //    oldModel.UpdateTime = DateTime.Now;
        //    try
        //    {
        //        server.SaveModel(oldModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine(e);
        //    }
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 初始化消费类型
        ///// </summary>
        ///// <returns></returns>
        //public ResponseBaseModel<dynamic> InitCostTypeModel()
        //{
        //    var resultMode = new ResponseBaseModel<dynamic>
        //    {
        //        ResultCode = ResponceCodeEnum.Success,
        //        Message = "响应成功"
        //    };
        //    var userId = CurrentModel.UserId;
        //    var server = new CostTypeService();
        //    var modelList = server.GetList(-1, userId, 1, 10, out _);
        //    if (modelList != null && modelList.Count > 0)
        //    {
        //        resultMode.ResultCode = ResponceCodeEnum.Fail;
        //        resultMode.Message = "已经初始化过";
        //        return Json(resultMode, JsonRequestBehavior.AllowGet);
        //    }
        //    server.InitCostType(userId);
        //    resultMode.ResultCode = ResponceCodeEnum.Success;
        //    resultMode.Message = "初始化成功";
        //    return Json(resultMode, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}