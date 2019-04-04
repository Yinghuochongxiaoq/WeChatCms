using System;
using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CustomerCommentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CustomerCommentData _dataAccess = new CustomerCommentData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="hasDeal"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CustomercommentModel> GetList(DateTime beginTime, DateTime endTime, FlagEnum hasDeal, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(beginTime, endTime, hasDeal);
            return _dataAccess.GetModels(beginTime, endTime, hasDeal, indexPage, pageSize);
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
        public CustomercommentModel Get(long id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne.GetHashCode()) return null;
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(CustomercommentModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 处理结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dealResult"></param>
        public void DealComment(long id, string dealResult)
        {
            _dataAccess.DealComment(id, dealResult);
        }
    }
}
