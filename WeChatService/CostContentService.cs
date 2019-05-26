﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
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
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <returns></returns>
        public List<CostContentModel> GetList(long userId, int spendType, string address, string costThing, int costType, DateTime startTime, DateTime endTime, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(userId, spendType, address, costThing, costType, startTime, endTime);
            var list = _dataAccess.GetModels(userId, spendType, address, costThing, costType, startTime, endTime, indexPage, pageSize);
            return list;
        }
    }
}
