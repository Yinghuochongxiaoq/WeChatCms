using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class HumanHistoryDetailService
    {
        private HumanHistoryDetailData _dataAccess = new HumanHistoryDetailData();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(HumanstorydetailModel saveModel)
        {
            return _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <returns></returns>
        public List<HumanstorydetailModel> GetList(int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount();
            var list = _dataAccess.GetModels(indexPage, pageSize);
            return list;
        }
    }
}
