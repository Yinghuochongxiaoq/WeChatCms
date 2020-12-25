using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class DailyHistoryData : BaseData<long, DailyHistoryModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(DailyHistoryModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, DailyHistoryModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
            }
        }

        /// <summary>
        /// 根据用户id查询日志记录
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="dailyYear">年份</param>
        /// <param name="dailyMonth">月份</param>
        /// <param name="dailyDay">天</param>
        /// <returns>查询日志记录</returns>
        public List<DailyHistoryModel> GetDailyHistoryListByUserId(long userId, int dailyYear, int dailyMonth, int dailyDay)
        {
            List<DailyHistoryModel> dataList;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                dataList = dailyDay < 1
                    ? conn
                        .GetList<DailyHistoryModel>(new
                            { IsDel=0,UserId = userId, DailyYear = dailyYear, DailyMonth = dailyMonth})
                        ?.OrderBy(f => f.DailyDate).ToList()
                    : conn
                        .GetList<DailyHistoryModel>(new
                            { IsDel = 0, UserId = userId, DailyYear = dailyYear, DailyMonth = dailyMonth, DailyDay = dailyDay})
                        ?.OrderBy(f => f.DailyDate).ToList();
            }

            return dataList;
        }
    }
}
