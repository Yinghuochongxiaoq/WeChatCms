using System.Collections.Generic;
using System.Linq;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class DailyStoryResourceData : BaseData<long, DailyStoryResourceModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(DailyStoryResourceModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, DailyStoryResourceModel>(saveModel);
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
        public List<DailyStoryResourceModel> GetDailyStoryResourceModels(List<long> detailIdList)
        {
            if (detailIdList == null || detailIdList.Count < 1)
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<DailyStoryResourceModel>("where StoryDetailId in @StoryDetailId and IsDel=@IsDel", new
                { StoryDetailId = detailIdList.ToArray(), IsDel = FlagEnum.HadZore.GetHashCode() })?.OrderBy(f => f.Sort).ToList();
            }
        }

        /// <summary>
        /// 获取资源详情
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public List<DailyStoryResourceModel> GetDailyStoryResourceModelsById(List<long> idList)
        {
            if (idList == null || idList.Count < 1)
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<DailyStoryResourceModel>("where Id in @Ids and IsDel=@IsDel", new
                { Ids = idList.ToArray(), IsDel = FlagEnum.HadZore.GetHashCode() })?.OrderBy(f => f.Sort).ToList();
            }
        }

        /// <summary>
        /// 根据关联id删除资源信息
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public int DeleteResourceByStoryId(long storyId)
        {
            var sql = @"update dailystoryresource set IsDel=@IsDel where StoryDetailId = @StoryId and IsDel!=@IsDel";
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Execute(sql, new { StoryId = storyId, IsDel = FlagEnum.HadOne.GetHashCode() });
            }
        }

        /// <summary>
        /// 根据id删除资源信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteResourceById(List<long> ids)
        {
            var sql = @"update dailystoryresource set IsDel=@IsDel where Id in @Ids and IsDel!=@IsDel";
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Execute(sql, new { Ids = ids.ToArray(), IsDel = FlagEnum.HadOne.GetHashCode() });
            }
        }
    }
}
