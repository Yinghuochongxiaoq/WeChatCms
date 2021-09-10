using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class CarNoticeDetailData : BaseData<long, CarnoticedetailModel>
    {
        /// <summary>
        /// 获取预约详情
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="carId">主表</param>
        /// <param name="searchTime">查询时间</param>
        /// <returns></returns>
        public List<CarnoticedetailModel> GetCarNoticeDetailModelsByCarId(long userId, long carId, DateTime searchTime)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (userId > 0)
            {
                where.Append(" and CarUserId=@CarUserId ");
            }

            if (searchTime > new DateTime(1970, 1, 1))
            {
                where.Append(" and DailyYear=@DailyYear and DailyMonth=@DailyMonth and DailyDay=@DailyDay ");
            }
            if (carId > 0)
            {
                where.Append("  and CarId=@CarId ");
            }

            where.Append(" order by id asc ");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                CarId = carId,
                DailyYear = searchTime.Year,
                DailyMonth = searchTime.Month,
                DailyDay = searchTime.Day,
                CarUserId = userId
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CarnoticedetailModel>(where.ToString(), param)?.ToList();
            }
        }

        /// <summary>
        /// 后期某个时间点以后的安排
        /// </summary>
        /// <param name="carId">主键</param>
        /// <param name="searchTime">时间点</param>
        /// <returns></returns>
        public List<CarnoticedetailModel> GetAfterNowCarNoticeDetailModels(long carId, DateTime searchTime)
        {
            if (carId < 1 || searchTime < new DateTime(1970, 1, 1))
            {
                return null;
            }
            var where = new StringBuilder(" where IsDel=@IsDel and CarId=@CarId and dailyDate>@DailyDate");
            where.Append(" order by id asc ");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                CarId = carId,
                DailyDate = searchTime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CarnoticedetailModel>(where.ToString(), param)?.ToList();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(CarnoticedetailModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, CarnoticedetailModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
            }
        }

        /// <summary>
        /// 删除表记录
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(long id)
        {
            if (id < 1) return;
            var model = Get(id);
            if (model == null) return;
            model.IsDel = FlagEnum.HadOne;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }
    }
}
