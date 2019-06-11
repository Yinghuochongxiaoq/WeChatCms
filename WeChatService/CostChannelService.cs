﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CostChannelService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CostChannelData _dataAccess = new CostChannelData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="userId"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostChannelModel> GetList(int isValid, long userId, int indexPage, int pageSize, out int count, string name = "")
        {
            count = _dataAccess.GetCount(userId, isValid, name);
            return _dataAccess.GetModels(userId, isValid, indexPage, pageSize, name);
        }

        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="costChannelName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CostChannelModel> GetList(string costChannelName, long userId)
        {
            return _dataAccess.GetModels(costChannelName, userId);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        public void DelModel(List<long> ids)
        {
            if (ids == null || ids.Count < 1) return;
            if (ids.Count == 1)
            {
                _dataAccess.DelModel(ids[0]);
            }
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostChannelModel Get(long id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne) return null;
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(CostChannelModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}