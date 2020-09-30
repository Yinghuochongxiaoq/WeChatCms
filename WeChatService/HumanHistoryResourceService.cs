using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class HumanHistoryResourceService
    {
        private HumanHistoryResourceData _dataAccess = new HumanHistoryResourceData();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(HumanstoryresourceModel saveModel)
        {
            return _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 获取指定记录的资源集合
        /// </summary>
        /// <param name="detailIdList"></param>
        /// <returns></returns>
        public List<HumanstoryresourceModel> GetHumanstoryresourceModels(List<long> detailIdList) => _dataAccess.GetHumanstoryresourceModels(detailIdList);
    }
}
