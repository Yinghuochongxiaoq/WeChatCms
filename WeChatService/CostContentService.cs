using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.CustomerModel;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CostContentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CostContentData _dataAccess = new CostContentData();

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostContentModel GetContentModel(long id)
        {
            if (id < 1)
            {
                return null;
            }

            return _dataAccess.Get(id);
        }

        /// <summary>
        /// 添加内容信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long AddAndUpdateContentInfo(CostContentModel model)
        {
            if (model == null) return 0;
            if (model.SpendType == 2)
            {
                //验证
                if (model.Id > 0)
                {
                    var linkModel = GetContentModel(model.LinkCostId);
                    if (linkModel == null || linkModel.LinkCostId != model.Id)
                    {
                        if (Debugger.IsAttached)
                        {
                            Trace.WriteLine("关联入账信息不匹配，更新失败");
                        }
                        return 0;
                    }
                }
                var inModel = new CostContentModel
                {
                    Cost = model.Cost,
                    CostAddress = model.CostAddress,
                    SpendType = model.SpendType,
                    Id = model.LinkCostId,
                    CostTime = model.CostTime,
                    CostInOrOut = CostInOrOutEnum.In,
                    CostChannel = model.LinkCostChannel,
                    CostChannelNo = model.LinkCostChannelNo,
                    CostChannelName = model.LinkCostChannelName,
                    LinkCostChannel = model.CostChannel,
                    LinkCostChannelNo = model.CostChannelNo,
                    LinkCostChannelName = model.CostChannelName,
                    UserId = model.UserId,
                    CostType = model.CostType,
                    LinkCostId = model.Id,
                    CostThing = model.CostThing,
                    CreateTime = model.CreateTime,
                    CostMonth = model.CostMonth,
                    UpdateTime = model.UpdateTime,
                    CostYear = model.CostYear,
                    UpdateUserId = model.UpdateUserId,
                    CostTypeName = model.CostTypeName,
                    CreateUserId = model.CreateUserId,
                    IsDel = model.IsDel
                };
                return _dataAccess.SaveLinkCostModel(model, inModel);
            }

            return _dataAccess.SaveModel(model);
        }

        /// <summary>
        /// 更新两个对象，通过事务
        /// </summary>
        /// <param name="modelItem1"></param>
        /// <param name="modelItem2"></param>
        /// <returns></returns>
        public long UpdateLinkCostContentInfo(CostContentModel modelItem1, CostContentModel modelItem2)
        {
            switch (modelItem1)
            {
                case null when modelItem2 == null:
                    return 0;
                case null when modelItem2.Id > 0:
                    return _dataAccess.SaveModel(modelItem2);
            }

            if (modelItem2 == null && modelItem1.Id > 0)
            {
                return _dataAccess.SaveModel(modelItem1);
            }
            return _dataAccess.SaveLinkCostModel(modelItem1, modelItem2);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="costType"></param>
        /// <param name="costchannel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <returns></returns>
        public List<CostContentModel> GetList(List<long> userIds, int spendType, string address, string costThing, int costType, long costchannel, DateTime startTime, DateTime endTime, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(userIds, spendType, address, costThing, costType, costchannel, startTime, endTime);
            var list = _dataAccess.GetModels(userIds, spendType, address, costThing, costType, costchannel, startTime, endTime, indexPage, pageSize);
            return list;
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <param name="costType"></param>
        /// <param name="costchannel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetStatisticsCost(List<long> userIds, int spendType, string address, string costThing,
            int costType, long costchannel, DateTime startTime, DateTime endTime)
        {
            return _dataAccess.GetStatisticsCost(userIds, spendType, address, costThing, costType, costchannel,
                startTime, endTime);
        }

        /// <summary>
        /// 获取总的可支配账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public dynamic GetStatisticsCanPay(long userId, DateTime starTime, DateTime endTime, long channelId)
        {
            List<CanPayAcountModel> channelCanPayList = _dataAccess.GetStatisticsCanPay(new List<long> { userId });
            var inCostList = channelCanPayList.Where(f => f.CostInOrOut == CostInOrOutEnum.In);
            var outCostList = channelCanPayList.Where(e => e.CostInOrOut == CostInOrOutEnum.Out);
            var allCanPay = inCostList.Sum(r => r.CostCount) - outCostList.Sum(s => s.CostCount);
            var channelAcount = new Dictionary<string, decimal>();
            channelCanPayList.ForEach(h =>
            {
                if (channelAcount.ContainsKey(h.CostChannelName))
                {
                    channelAcount[h.CostChannelName] = channelAcount[h.CostChannelName] +
                                                       (h.CostInOrOut == CostInOrOutEnum.In
                                                           ? h.CostCount
                                                           : h.CostCount * -1);
                }
                else
                {
                    channelAcount.Add(h.CostChannelName, h.CostInOrOut == CostInOrOutEnum.In
                        ? h.CostCount
                        : h.CostCount * -1);
                }
            });
            var data = channelAcount.Where(r => r.Value != 0).Select(f => new CanPayAcountModel { CostCount = f.Value, CostChannelName = f.Key }).ToList();

            var costTypeList = GetCostTypeStatistics(starTime, endTime, new List<long> { userId }, CostInOrOutEnum.Out, channelId);
            var allTypeCost = costTypeList.Sum(f => f.CostCount);

            var costDayList = _dataAccess.GetStatisticsCostDayPay(starTime, endTime, userId, CostInOrOutEnum.Out, channelId);
            var costDayDic = new Dictionary<string, decimal>();
            costDayList.ForEach(f =>
            {
                costDayDic.Add(f.CostDay.ToString("yyyy-MM-dd"), f.CostCount);
            });
            var costBeginTime = "";
            var costEndTime = "";
            if (costDayList.Count > 0)
            {
                costBeginTime = costDayList[0].CostDay.ToString("yyyy-MM-01");
                var itemEndTime = costDayList[costDayList.Count - 1].CostDay;
                costEndTime = new DateTime(itemEndTime.Year, itemEndTime.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            return new { allCanPay, allTypeCost, channelAcount = data, costTypeList, costDayDic, costBeginTime, costEndTime };
        }

        /// <summary>
        /// 获取用户的所有账户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<dynamic> GetStatisticsAllChannel(List<long> userIds)
        {
            var channelServer = new CostChannelService();
            //获取所有用户的统计信息
            List<CanPayAcountModel> channelCanPayList = _dataAccess.GetStatisticsCanPay(userIds);
            //获取所有用户账户
            var channelList = channelServer.GetList(-1, userIds, 1, 10000, out _);
            var result = new List<dynamic>();
            if (channelList == null || channelList.Count < 1)
            {
                return result;
            }

            #region [家庭成员所有]

            if (userIds.Count > 1)
            {
                var allOutCost = channelCanPayList.Where(e => e.CostInOrOut == CostInOrOutEnum.Out)
                    .Sum(e => e.CostCount);
                var allInCost = channelCanPayList.Where(f => f.CostInOrOut == CostInOrOutEnum.In).Sum(s => s.CostCount);
                var allCouldCost = allInCost - allOutCost;
                var channelAccount = new Dictionary<string, decimal>();
                if (channelList.Any())
                {
                    channelList.ForEach(f =>
                        {
                            if (!channelAccount.ContainsKey(f.CostChannelName))
                            {
                                channelAccount.Add(f.CostChannelName, 0);
                            }
                        }
                    );
                }

                channelCanPayList.ForEach(h =>
                {
                    if (channelAccount.ContainsKey(h.CostChannelName))
                    {
                        channelAccount[h.CostChannelName] = channelAccount[h.CostChannelName] +
                                                            (h.CostInOrOut == CostInOrOutEnum.In
                                                                ? h.CostCount
                                                                : h.CostCount * -1);
                    }
                    else
                    {
                        channelAccount.Add(h.CostChannelName, h.CostInOrOut == CostInOrOutEnum.In
                            ? h.CostCount
                            : h.CostCount * -1);
                    }
                });
                var data = channelAccount.Select(f => new CanPayAcountModel
                {
                    CostCount = f.Value,
                    CostChannelName = f.Key,
                    CostChannel = channelList.FirstOrDefault(r => r.CostChannelName == f.Key)?.Id
                }).ToList();
                result.Add(new
                {
                    StatisticsModel = new
                    {
                        allCouldCost = $"{allCouldCost:N2}",
                        allInCost = $"{allInCost:N2}",
                        allOutCost = $"{allOutCost:N2}"
                    },
                    channelAcount = data,
                    userId = -1
                });
            }

            #endregion

            #region [计算成员的统计信息]
            foreach (var canPayAccountModels in channelCanPayList.GroupBy(f => f.UserId))
            {
                var itemOutCost = canPayAccountModels.Where(e => e.CostInOrOut == CostInOrOutEnum.Out).Sum(e => e.CostCount);
                var itemInCost = canPayAccountModels.Where(f => f.CostInOrOut == CostInOrOutEnum.In).Sum(s => s.CostCount);
                var itemCouldCost = itemInCost - itemOutCost;
                var itemChannelAccount = new Dictionary<string, decimal>();
                var itemChannelList = channelList.Where(r => r.UserId == canPayAccountModels.Key).ToList();
                if (itemChannelList.Any())
                {
                    itemChannelList.ForEach(r => itemChannelAccount.Add(r.CostChannelName, 0));
                }
                foreach (var h in canPayAccountModels)
                {
                    if (itemChannelAccount.ContainsKey(h.CostChannelName))
                    {
                        itemChannelAccount[h.CostChannelName] = itemChannelAccount[h.CostChannelName] +
                                                               (h.CostInOrOut == CostInOrOutEnum.In
                                                                   ? h.CostCount
                                                                   : h.CostCount * -1);
                    }
                    else
                    {
                        itemChannelAccount.Add(h.CostChannelName, h.CostInOrOut == CostInOrOutEnum.In
                            ? h.CostCount
                            : h.CostCount * -1);
                    }
                }
                var itemData = itemChannelAccount.Select(f => new CanPayAcountModel { CostCount = f.Value, CostChannelName = f.Key, CostChannel = itemChannelList.FirstOrDefault(r => r.CostChannelName == f.Key)?.Id }).ToList();
                result.Add(new
                {
                    StatisticsModel = new
                    {
                        allCouldCost = $"{itemCouldCost:N2}",
                        allInCost = $"{itemInCost:N2}",
                        allOutCost = $"{itemOutCost:N2}"
                    },
                    channelAcount = itemData,
                    userId = canPayAccountModels.Key
                });
                userIds.Remove(canPayAccountModels.Key);
            }
            #endregion

            #region 处理没有数据的情况

            if (userIds.Count > 0)
            {
                userIds.ForEach(f =>
                {
                    result.Add(new
                    {
                        StatisticsModel = new
                        {
                            allCouldCost = $"{0:N2}",
                            allInCost = $"{0:N2}",
                            allOutCost = $"{0:N2}"
                        },
                        channelAcount = new List<CanPayAcountModel>(),
                        userId = f
                    });
                });
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 获取分类统计
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userIds"></param>
        /// <param name="inOrOut"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public List<CanPayAcountModel> GetCostTypeStatistics(DateTime starTime, DateTime endTime, List<long> userIds,
            CostInOrOutEnum inOrOut, long channelId)
        {
            var costTypeList = _dataAccess.GetStatisticsCostTypePay(starTime, endTime, userIds, CostInOrOutEnum.Out, channelId);
            return costTypeList;
        }

        /// <summary>
        /// 获取分类统计
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userIds"></param>
        /// <param name="inOrOut"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetCostMonthStatistics(DateTime starTime, DateTime endTime, List<long> userIds,
            CostInOrOutEnum inOrOut, long channelId)
        {
            var costDic = _dataAccess.GetStatisticsCostMonth(starTime, endTime, userIds, inOrOut, channelId);
            return costDic;
        }
    }
}
