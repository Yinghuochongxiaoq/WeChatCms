using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Enum;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatNoteCostApi.WeChatInnerModel;
using WeChatService;

namespace WeChatNoteCostApi.Controllers
{
    public class CostNoteController : ApiControllerBase
    {
        readonly WechatFamilyService _familyServer = new WechatFamilyService();

        #region [1、消费流水页面记录]
        /// <summary>
        /// 获取消费记录异步数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCostPage(string token, int pageIndex = 1, int pageSize = 10, int costType = -1, int spendType = -1, long costchannel = -1, long memberId = -1)
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
                var userIds = new List<long> { userId };
                if (userData.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(userData.FamilyCode))
                {
                    var members = _familyServer.GetFamilyMembers(userData.FamilyCode);
                    if (members != null && members.Count > 0)
                    {
                        userIds.AddRange(members.Select(f => f.UserId));
                    }
                }
                //查询成员的信息
                if (memberId > 0 && userIds.Contains(memberId))
                {
                    userIds.Clear();
                    userIds.Add(memberId);
                }
                userIds = userIds.Distinct().ToList();

                var server = new CostContentService();
                var dataList = server.GetList(userIds, spendType, null, null, costType, costchannel, starttime, endtime,
                    pageIndex, pageSize, out count);
                var dic = server.GetStatisticsCost(userIds, spendType, null, null, costType, costchannel,
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
        public ResponseBaseModel<dynamic> GetCostChannelType(string token, long memberId = -1)
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

            var userIds = new List<long> { userId };
            if (userData.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(userData.FamilyCode))
            {
                var members = _familyServer.GetFamilyMembers(userData.FamilyCode);
                if (members != null && members.Count > 0)
                {
                    userIds.AddRange(members.Select(f => f.UserId));
                }
            }

            //查询成员的信息
            if (memberId > 0 && userIds.Contains(memberId))
            {
                userIds.Clear();
                userIds.Add(memberId);
            }
            userIds = userIds.Distinct().ToList();

            var costTypeData = server.GetList(-1, userIds, 1, 100000, out _);
            var channelServer = new CostChannelService();
            var channelData = channelServer.GetList(FlagEnum.HadOne.GetHashCode(), userIds, 1, 100000, out _);
            resultMode.Data = new
            {
                costTypeData,
                channelData
            };
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [2、获取统计信息]
        /// <summary>
        /// 统计用户账户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> StatisticsCostChannel(string token)
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

            var userIds = new List<long> { tempUserId.Value };
            if (userData.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(userData.FamilyCode))
            {
                var members = _familyServer.GetFamilyMembers(userData.FamilyCode);
                if (members != null && members.Count > 0)
                {
                    userIds.AddRange(members.Select(f => f.UserId));
                }
            }
            userIds = userIds.Distinct().ToList();

            var server = new CostContentService();
            var resultData = server.GetStatisticsAllChannel(userIds);
            resultMode.Data = resultData;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [3、编辑消费记录]
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseBaseModel<dynamic> AddCostInfo([FromBody]CostContentModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (model == null)
            {
                resultMode.Message = "参数错误";
                return resultMode;
            }
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + model.Token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var userId = tempUserId;
            var server = new CostContentService();
            CostContentModel newModel = new CostContentModel();
            if (model.Id > 0)
            {
                newModel = server.GetContentModel(model.Id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "只能修改自己的账户哦~";
                    return resultMode;
                }

                if (newModel.SpendType == 2 && (model.SpendType != 2 || model.LinkCostId < 0))
                {
                    resultMode.Message = "转移记录类型不能修改或无关联入账信息";
                    return resultMode;
                }

                if (newModel.SpendType == 2 && newModel.CostInOrOut == CostInOrOutEnum.In)
                {
                    resultMode.Message = "转移记录入账信息不能修改";
                    return resultMode;
                }
            }
            //验证参数
            newModel.Cost = Math.Round(Math.Abs(model.Cost), 2);
            newModel.CostAddress = model.CostAddress;
            newModel.CostChannel = model.CostChannel;
            newModel.CostType = model.SpendType == 2 ? -1 : model.CostType;
            newModel.UserId = userId.Value;
            newModel.CostInOrOut = model.CostInOrOut;
            newModel.CostThing = model.CostThing;
            newModel.CostTime = model.CostTime;
            newModel.CostMonth = newModel.CostTime.Month;
            newModel.CostYear = newModel.CostTime.Year;
            newModel.CreateTime = newModel.Id > 0 ? newModel.CreateTime : DateTime.Now;
            newModel.CreateUserId = userId.Value;
            newModel.SpendType = model.SpendType;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId.Value;
            newModel.LinkCostChannel = model.LinkCostChannel;
            newModel.IsDel = FlagEnum.HadZore;
            if (newModel.Cost < (decimal)0.01)
            {
                resultMode.Message = "金额设置错误";
                return resultMode;
            }

            if (model.SpendType != 2)
            {
                var costTypeInfo = new CostTypeService().Get(newModel.CostType);
                if (costTypeInfo == null
                    || costTypeInfo.UserId != userId
                    || costTypeInfo.IsDel == FlagEnum.HadOne
                    || costTypeInfo.IsValid == FlagEnum.HadZore)
                {
                    resultMode.Message = "类型选择无效";
                    return resultMode;
                }
                newModel.CostTypeName = costTypeInfo.Name;
            }

            var costChannelServer = new CostChannelService();
            var costChannelInfo = costChannelServer.Get(newModel.CostChannel);
            if (costChannelInfo == null
                || costChannelInfo.UserId != userId
                || costChannelInfo.IsDel == FlagEnum.HadOne
                || costChannelInfo.IsValid == FlagEnum.HadZore)
            {
                resultMode.Message = "账户选择无效";
                return resultMode;
            }
            newModel.CostChannelName = costChannelInfo.CostChannelName;
            newModel.CostChannelNo = costChannelInfo.CostChannelNo;
            newModel.CostInOrOut = newModel.SpendType == 1 ? CostInOrOutEnum.In : CostInOrOutEnum.Out;

            if (newModel.SpendType == 2)
            {
                if (newModel.LinkCostChannel < 1)
                {
                    resultMode.Message = "转入账户无效";
                    return resultMode;
                }
                var linkChannelInfo = costChannelServer.Get(newModel.LinkCostChannel);
                if (linkChannelInfo == null
                    || linkChannelInfo.UserId != userId
                    || linkChannelInfo.IsDel == FlagEnum.HadOne
                    || linkChannelInfo.IsValid == FlagEnum.HadZore)
                {
                    resultMode.Message = "转入账户无效";
                    return resultMode;
                }

                newModel.LinkCostChannelName = linkChannelInfo.CostChannelName;
                newModel.LinkCostChannelNo = linkChannelInfo.CostChannelNo;
            }
            else
            {
                newModel.LinkCostChannel = 0;
                newModel.LinkCostId = 0;
                newModel.LinkCostChannelNo = "";
                newModel.LinkCostChannelName = "";
            }
            var costContentServer = new CostContentService();
            costContentServer.AddAndUpdateContentInfo(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> DeleteCostInfo(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }

            if (id < 1)
            {
                resultMode.Message = "参数错误";
                return resultMode;
            }
            var userId = tempUserId.Value;

            var server = new CostContentService();
            var oldModel = server.GetContentModel(id);
            CostContentModel linkModel = null;
            //验证权限
            if (oldModel == null || oldModel.UserId != userId)
            {
                resultMode.Message = "只能本人删除";
                return resultMode;
            }

            if (oldModel.SpendType == 2 && oldModel.LinkCostId < 1)
            {
                Trace.WriteLine("转移记录" + oldModel.Id + "异常，没有与之关联的转移记录");
            }
            else if (oldModel.SpendType == 2 && oldModel.LinkCostId > 0)
            {
                linkModel = server.GetContentModel(oldModel.LinkCostId);
            }

            if (linkModel != null && linkModel.UserId != userId)
            {
                resultMode.Message = "非法访问,关联记录鉴权失败";
                return resultMode;
            }

            if (linkModel != null)
            {
                linkModel.IsDel = FlagEnum.HadOne;
                linkModel.UpdateUserId = userId;
                linkModel.UpdateTime = DateTime.Now;
            }

            oldModel.IsDel = FlagEnum.HadOne;
            oldModel.UpdateUserId = userId;
            oldModel.UpdateTime = DateTime.Now;

            var costContentServer = new CostContentService();
            costContentServer.UpdateLinkCostContentInfo(oldModel, linkModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [4、编辑账户信息]
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCostChannelModel(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostChannelService();
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var userId = tempUserId;
            var data = server.Get(id);
            if (data != null && data.UserId == userId)
            {
                resultMode.Data = data;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return resultMode;
            }
            resultMode.Message = "查询失败";
            return resultMode;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="costChannelName"></param>
        /// <param name="costChannelNo"></param>
        /// <param name="sort"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> SaveCostChannelInfo(string token, long id, string costChannelName, string costChannelNo, int sort, int isValid)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (string.IsNullOrEmpty(costChannelName))
            {
                resultMode.Message = "名称不能为空";
                return resultMode;
            }

            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var userId = tempUserId.Value;
            var server = new CostChannelService();
            CostChannelModel newModel = new CostChannelModel();
            if (id > 0)
            {
                newModel = server.Get(id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "非法访问";
                    return resultMode;
                }
            }
            //验证参数
            newModel.Id = id;
            newModel.IsDel = FlagEnum.HadZore;
            newModel.IsValid = EnumHelper.GetEnumByValue<FlagEnum>(isValid);
            newModel.CostChannelName = costChannelName;
            newModel.CostChannelNo = costChannelNo;
            newModel.UserId = userId;
            newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
            newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
            newModel.Sort = sort;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId;

            var costTypeInfoList = server.GetList(costChannelName, userId);
            if (costTypeInfoList != null
                && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
            {
                resultMode.Message = "账号名称已经存在";
                return resultMode;
            }
            server.SaveModel(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [5、编辑类型信息]
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCostTypeModel(string token, long id)
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
            var server = new CostTypeService();
            var userId = tempUserId.Value;
            var data = server.Get(id);
            if (data != null && data.UserId == userId)
            {
                resultMode.Data = data;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return resultMode;
            }
            resultMode.Message = "查询失败";
            return resultMode;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <param name="spendType"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> SaveTypeInfo(string token, long id, string name, int sort, int spendType)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (string.IsNullOrEmpty(name))
            {
                resultMode.Message = "名称不能为空";
                return resultMode;
            }
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var userId = tempUserId.Value;
            var server = new CostTypeService();
            CostTypeModel newModel = new CostTypeModel();
            if (id > 0)
            {
                newModel = server.Get(id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "非法访问";
                    return resultMode;
                }
            }
            //验证参数
            newModel.Id = id;
            newModel.SpendType = spendType;
            newModel.IsDel = FlagEnum.HadZore;
            newModel.IsValid = FlagEnum.HadOne;
            newModel.Name = name;
            newModel.UserId = userId;
            newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
            newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
            newModel.Sort = sort;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId;

            var costTypeInfoList = server.GetList(spendType, userId, name);
            if (costTypeInfoList != null
                && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
            {
                resultMode.Message = "类型名称已经存在";
                return resultMode;
            }
            server.SaveModel(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> DeleteCostTypeModel(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var userId = tempUserId.Value;
            var server = new CostTypeService();
            var oldModel = server.Get(id);
            if (oldModel == null || oldModel.UserId != userId)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "参数错误";
                return resultMode;
            }

            oldModel.IsDel = FlagEnum.HadOne;
            oldModel.IsValid = FlagEnum.HadZore;
            oldModel.UpdateUserId = userId;
            oldModel.UpdateTime = DateTime.Now;
            try
            {
                server.SaveModel(oldModel);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return resultMode;
        }
        #endregion

        #region [6、获取统计表格]

        /// <summary>
        /// 获取统计表格的数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pieIndex"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> ChartPieData(string token, int pieIndex, long memberId = -1)
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
            var server = new CostContentService();
            var userId = tempUserId.Value;
            var currentTime = DateTime.Now;
            var endTime = new DateTime(currentTime.Year, currentTime.Month + 1, 1).AddSeconds(-1);
            var pieStartTime = currentTime;
            //获取成员信息
            var userIds = new List<long> { userId };
            if (userData.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(userData.FamilyCode))
            {
                var members = _familyServer.GetFamilyMembers(userData.FamilyCode);
                if (members != null && members.Count > 0)
                {
                    userIds.AddRange(members.Select(f => f.UserId));
                }
            }
            //查询成员的信息
            if (memberId > 0 && userIds.Contains(memberId))
            {
                userIds.Clear();
                userIds.Add(memberId);
            }
            userIds = userIds.Distinct().ToList();
            //本月
            if (pieIndex == 0)
            {
                pieStartTime = endTime.AddMonths(-1).AddSeconds(1);
            }
            //季度
            else if (pieIndex == 1)
            {
                pieStartTime = endTime.AddMonths(-3).AddSeconds(1);
            }
            //半年
            else if (pieIndex == 2)
            {
                pieStartTime = endTime.AddMonths(-6).AddSeconds(1);
            }
            else if (pieIndex == 3)
            {
                pieStartTime = endTime.AddMonths(-12).AddSeconds(1);
            }

            var costTypeList = server.GetCostTypeStatistics(pieStartTime, endTime, userIds, CostInOrOutEnum.Out, 0);
            var resultData = costTypeList?.Select(f => new { name = f.CostTypeName, data = f.CostCount }).ToList();
            resultMode.Data = resultData;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 获取统计消费折线图数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lineIndex"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> ChartLineData(string token, int lineIndex, long memberId = -1)
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
            var server = new CostContentService();
            var userId = tempUserId.Value;
            var currentTime = DateTime.Now;
            var endTime = new DateTime(currentTime.Year, currentTime.Month + 1, 1).AddSeconds(-1);
            var pieStartTime = currentTime;

            //查询成员的信息
            var userIds = new List<long> { userId };
            if (userData.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(userData.FamilyCode))
            {
                var members = _familyServer.GetFamilyMembers(userData.FamilyCode);
                if (members != null && members.Count > 0)
                {
                    userIds.AddRange(members.Select(f => f.UserId));
                }
            }
            if (memberId > 0 && userIds.Contains(memberId))
            {
                userIds.Clear();
                userIds.Add(memberId);
            }
            userIds = userIds.Distinct().ToList();
            //季度
            if (lineIndex == 1)
            {
                pieStartTime = endTime.AddMonths(-3).AddSeconds(1);
            }
            //半年
            else if (lineIndex == 2)
            {
                pieStartTime = endTime.AddMonths(-6).AddSeconds(1);
            }
            else if (lineIndex == 3)
            {
                pieStartTime = endTime.AddMonths(-12).AddSeconds(1);
            }

            var costOutList = server.GetCostMonthStatistics(pieStartTime, endTime, userIds, CostInOrOutEnum.Out, 0);
            var costIntList = server.GetCostMonthStatistics(pieStartTime, endTime, userIds, CostInOrOutEnum.In, 0);
            var resultMap = new Dictionary<int, Tuple<decimal, decimal>>();
            foreach (var outItem in costOutList)
            {
                resultMap.Add(outItem.Key, new Tuple<decimal, decimal>(outItem.Value, 0));
            }
            foreach (var inItem in costIntList)
            {
                if (resultMap.ContainsKey(inItem.Key))
                {
                    var tempValue = resultMap[inItem.Key];
                    var secondValue = new Tuple<decimal, decimal>(tempValue.Item1, inItem.Value);
                    resultMap[inItem.Key] = secondValue;
                }
                else
                {
                    resultMap.Add(inItem.Key, new Tuple<decimal, decimal>(0, inItem.Value));
                }
            }

            resultMap = resultMap.OrderBy(f => f.Key).ToDictionary(k => k.Key, v => v.Value);
            var nameArray = resultMap.Keys.Select(e => e / 100 + "年" + e % 100 + "月").ToList();
            var dataOutArray = resultMap.Values.Select(s => s.Item1).ToList();
            var dataInArray = resultMap.Values.Select(s => s.Item2).ToList();
            resultMode.Data = new
            {
                nameArray,
                dataOutArray,
                dataInArray
            };
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [7、绑定家庭信息]
        /// <summary>
        /// 获取家庭邀请码
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetInviteCode(string token)
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

            var cacheKey = RedisCacheKey.InviteCodeKey + tempUserId;
            var inviteCode = RedisCacheHelper.Get<string>(cacheKey);
            //缓存还没有过期
            if (!string.IsNullOrEmpty(inviteCode))
            {
                resultMode.Data = inviteCode;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return resultMode;
            }
            //缓存过期了，重新生成
            inviteCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 26).ToUpper();
            RedisCacheHelper.AddSet(cacheKey, inviteCode, new TimeSpan(0, 0, 5, 0));
            RedisCacheHelper.AddSet(inviteCode, userData.OpenId, new TimeSpan(0, 0, 5, 0));
            resultMode.Data = inviteCode;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 加入家庭中
        /// </summary>
        /// <param name="token"></param>
        /// <param name="inviteCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> AddSelfToFamily(string token, string inviteCode)
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

            if (string.IsNullOrEmpty(inviteCode))
            {
                resultMode.Message = "邀请码不能为空";
                return resultMode;
            }

            if (userData.HadBindFamily == FlagEnum.HadOne)
            {
                resultMode.Message = "您已经是家庭成员无需再次绑定";
                return resultMode;
            }

            var wechatAccountService = new WechatAccountService();
            var wechatFamilyService = new WechatFamilyService();
            //获取邀请人id
            var masterOpenId = RedisCacheHelper.Get<string>(inviteCode);
            var masterInfo = wechatAccountService.GetByOpenId(masterOpenId);
            if (masterInfo == null)
            {
                RedisCacheHelper.Remove(inviteCode);
                resultMode.Message = "邀请码已过期，请重新输入";
                return resultMode;
            }

            if (masterInfo.AccountId == tempUserId.Value)
            {
                resultMode.Message = "不能自己邀请自己";
                return resultMode;
            }

            var masterInviteCode = RedisCacheHelper.Get<string>(RedisCacheKey.InviteCodeKey + masterInfo.AccountId);
            if (!masterInviteCode.Equals(inviteCode, StringComparison.OrdinalIgnoreCase))
            {
                resultMode.Message = "邀请码错误，请重新输入";
                return resultMode;
            }
            var currentUserInfo = wechatAccountService.GetByOpenId(userData.OpenId);
            //接受邀请，并写入家庭数据
            if (masterInfo.HadBindFamily == FlagEnum.HadOne && !string.IsNullOrEmpty(masterInfo.FamilyCode))
            {
                //邀请者已经是家庭成员了
                currentUserInfo.FamilyCode = masterInfo.FamilyCode;
                currentUserInfo.HadBindFamily = masterInfo.HadBindFamily;
                var wechatFamilyInfo = new WechatFamilyModel
                {
                    CreateTime = DateTime.Now,
                    FamilyCode = currentUserInfo.FamilyCode,
                    IsDel = FlagEnum.HadZore,
                    Remarks = "",
                    UserId = currentUserInfo.AccountId
                };
                var familys = new List<WechatFamilyModel> { wechatFamilyInfo };
                var wechatUsers = new List<WeChatAccountModel> { currentUserInfo };
                var result = wechatFamilyService.BindFamilyAndUser(familys, wechatUsers);
                if (result)
                {
                    resultMode.Message = "绑定成功";
                    resultMode.ResultCode = ResponceCodeEnum.Success;
                }
                else
                {
                    resultMode.Message = "绑定失败";
                }
            }
            else
            {
                //邀请者还不是家庭成员，首次邀请
                var newFaimlyCode = Guid.NewGuid().ToString();
                masterInfo.FamilyCode = newFaimlyCode;
                masterInfo.HadBindFamily = FlagEnum.HadOne;
                currentUserInfo.FamilyCode = newFaimlyCode;
                currentUserInfo.HadBindFamily = FlagEnum.HadOne;

                var masterFamilyInfo = new WechatFamilyModel
                {
                    CreateTime = DateTime.Now,
                    FamilyCode = masterInfo.FamilyCode,
                    IsDel = FlagEnum.HadZore,
                    Remarks = "",
                    UserId = masterInfo.AccountId
                };
                var newUserFamilyInfo = new WechatFamilyModel
                {
                    CreateTime = DateTime.Now,
                    FamilyCode = currentUserInfo.FamilyCode,
                    IsDel = FlagEnum.HadZore,
                    Remarks = "",
                    UserId = currentUserInfo.AccountId
                };
                var familys = new List<WechatFamilyModel> { masterFamilyInfo, newUserFamilyInfo };
                var wechatUsers = new List<WeChatAccountModel> { currentUserInfo, masterInfo };
                var result = wechatFamilyService.BindFamilyAndUser(familys, wechatUsers);
                if (result)
                {
                    resultMode.Message = "绑定成功";
                    resultMode.ResultCode = ResponceCodeEnum.Success;
                }
                else
                {
                    resultMode.Message = "绑定失败";
                }
            }
            RedisCacheHelper.Remove(inviteCode);
            RedisCacheHelper.Remove(RedisCacheKey.InviteCodeKey + masterInfo.AccountId);
            return resultMode;
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> UnBindFamily(string token)
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

            if (userData.HadBindFamily == FlagEnum.HadZore)
            {
                resultMode.Message = "您已经解除绑定";
                return resultMode;
            }

            var weChatAccountService = new WechatAccountService();
            var weChatFamilyService = new WechatFamilyService();

            var currentUserInfo = weChatAccountService.GetByOpenId(userData.OpenId);
            var weChatFamilyInfo =
                weChatFamilyService.GetFamilyMember(currentUserInfo?.FamilyCode, currentUserInfo?.AccountId ?? 0);
            //解除绑定
            if (currentUserInfo == null || weChatFamilyInfo == null)
            {
                resultMode.Message = "用户信息获取失败";
            }
            else
            {
                currentUserInfo.HadBindFamily = FlagEnum.HadZore;
                currentUserInfo.UpDateTime = DateTime.Now;
                weChatFamilyInfo.IsDel = FlagEnum.HadOne;
                weChatFamilyInfo.UnBindTime = DateTime.Now;
                weChatFamilyService.UnBindFamilyAndUser(weChatFamilyInfo, currentUserInfo);
                resultMode.Message = "解除绑定成功";
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }
            return resultMode;
        }

        /// <summary>
        /// 重新绑定绑定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> ReBindFamily(string token)
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

            if (userData.HadBindFamily == FlagEnum.HadOne)
            {
                resultMode.Message = "您已经绑定成功";
                return resultMode;
            }

            var weChatAccountService = new WechatAccountService();
            var weChatFamilyService = new WechatFamilyService();

            var currentUserInfo = weChatAccountService.GetByOpenId(userData.OpenId);
            var weChatFamilyInfo =
                weChatFamilyService.GetFamilyMember(currentUserInfo?.FamilyCode, currentUserInfo?.AccountId ?? 0);
            //重新绑定
            if (currentUserInfo == null || weChatFamilyInfo == null)
            {
                resultMode.Message = "用户信息获取失败";
            }
            else if (weChatFamilyInfo.UnBindTime < DateTime.Now.AddDays(-3))
            {
                resultMode.Message = "72小时时限已过，请让原家庭成员重新邀请吧~";
            }
            else
            {
                currentUserInfo.HadBindFamily = FlagEnum.HadOne;
                currentUserInfo.UpDateTime = DateTime.Now;
                weChatFamilyInfo.IsDel = FlagEnum.HadZore;
                weChatFamilyInfo.UnBindTime = new DateTime(1900, 1, 1);
                weChatFamilyService.UnBindFamilyAndUser(weChatFamilyInfo, currentUserInfo);
                resultMode.Message = "重新绑定成功";
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }
            return resultMode;
        }
        #endregion

        #region [8、获取用户的家庭成员]

        /// <summary>
        /// 获取用户的家庭成员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCurrentUserFamilyMembers(string token)
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

            if (!string.IsNullOrEmpty(userData.FamilyCode))
            {
                var members = _familyServer.GetMemberInfoByCode(userData.FamilyCode, tempUserId.Value);
                if (members != null && members.Count > 0)
                {
                    if (userData.HadBindFamily == FlagEnum.HadOne)
                    {
                        resultMode.Data = new { members, state = 1 };
                        resultMode.ResultCode = ResponceCodeEnum.Success;
                    }
                    else
                    {
                        var currentIndex = members.FindIndex(f => f.AccountId == tempUserId.Value);
                        if (currentIndex > 0)
                        {
                            var tempUser = members[currentIndex];
                            members[currentIndex] = members[0];
                            members[0] = tempUser;
                        }
                        resultMode.Data = new
                        {
                            members = members[0].UpDateTime > DateTime.Now.AddDays(-3) ? members : new List<WeChatAccountModel>(),
                            state = members[0].UpDateTime > DateTime.Now.AddDays(-3) ? 2 : 0
                        };
                        resultMode.ResultCode = ResponceCodeEnum.Success;
                    }
                }
            }
            else
            {
                resultMode.Data = new { members = new List<int>(), state = 0 };
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "还没有绑定家庭哦";
            }
            return resultMode;
        }
        #endregion

        #region [9、获取首页滚动通知]

        /// <summary>
        /// 获取通知信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetHomePageNotice(string token)
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

            var server = new CostNoticeService();
            var noticeList=server.GetCurrentTimeNoticeList(DateTime.Now);
            resultMode.Data = new
            {
                noticeList
            };
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        #endregion
    }
}