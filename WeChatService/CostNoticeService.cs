using System;
using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CostNoticeService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private readonly CostNoticeData _dataAccess = new CostNoticeData();

        /// <summary>
        /// 获取多个提示信息
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public List<CostNoticeModel> GetCurrentTimeNoticeList(DateTime currentTime)
        {
            if (currentTime < new DateTime(1900, 1, 1)) return null;
            return _dataAccess.GetCurrentTimeNoticeList(currentTime);
        }
    }
}
