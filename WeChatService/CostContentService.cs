using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    CreateUserId = model.CreateUserId
                };
                return _dataAccess.SaveLinkCostModel(model, inModel);
            }

            return _dataAccess.SaveModel(model);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="userId"></param>
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
        public List<CostContentModel> GetList(long userId, int spendType, string address, string costThing, int costType, long costchannel, DateTime startTime, DateTime endTime, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(userId, spendType, address, costThing, costType, costchannel, startTime, endTime);
            var list = _dataAccess.GetModels(userId, spendType, address, costThing, costType, costchannel, startTime, endTime, indexPage, pageSize);
            return list;
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <param name="costType"></param>
        /// <param name="costchannel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetStatisticsCost(long userId, int spendType, string address, string costThing,
            int costType, long costchannel, DateTime startTime, DateTime endTime)
        {
            return _dataAccess.GetStatisticsCost(userId, spendType, address, costThing, costType, costchannel,
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
            List<CanPayAcountModel> channelCanPayList = _dataAccess.GetStatisticsCanPay(userId);
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
            var data = channelAcount.Select(f => new CanPayAcountModel { CostCount = f.Value, CostChannelName = f.Key }).ToList();

            var costTypeList = _dataAccess.GetStatisticsCostTypePay(starTime, endTime, userId, CostInOrOutEnum.Out, channelId);
            var allTypeCost = costTypeList.Sum(f => f.CostCount);
            return new { allCanPay, allTypeCost, channelAcount = data, costTypeList };
        }
    }
}
