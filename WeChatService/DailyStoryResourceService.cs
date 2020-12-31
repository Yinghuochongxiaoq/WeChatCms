using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class DailyStoryResourceService
    {
        private DailyStoryResourceData _dataAccess = new DailyStoryResourceData();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(DailyStoryResourceModel saveModel)
        {
            return _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 获取指定记录的资源集合
        /// </summary>
        /// <param name="detailIdList"></param>
        /// <returns></returns>
        public List<DailyStoryResourceModel> GetDailyStoryResourceModels(List<long> detailIdList) => _dataAccess.GetDailyStoryResourceModels(detailIdList);

        /// <summary>
        /// 根据id删除资源
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public int DeleteResource(long storyId) => _dataAccess.DeleteResource(storyId);
    }
}
