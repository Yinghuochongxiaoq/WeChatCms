using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class HumanHistoryResourceData : BaseData<long, HumanstoryresourceModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(HumanstoryresourceModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, HumanstoryresourceModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
            }
        }

        /// <summary>
        /// 获取资源详情
        /// </summary>
        /// <param name="detailIdList"></param>
        /// <returns></returns>
        public List<HumanstoryresourceModel> GetHumanstoryresourceModels(List<long> detailIdList)
        {
            if (detailIdList == null || detailIdList.Count < 1)
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<HumanstoryresourceModel>("where StoryDetailId in @StoryDetailId", new
                    { StoryDetailId = detailIdList.ToArray() }).ToList();
            }
        }
    }
}
