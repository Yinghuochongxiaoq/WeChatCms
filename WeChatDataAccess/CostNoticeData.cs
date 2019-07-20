using System;
using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class CostNoticeData : BaseData<long, CostNoticeModel>
    {
        /// <summary>
        /// 获取多个提示信息
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public List<CostNoticeModel> GetCurrentTimeNoticeList(DateTime currentTime)
        {
            if (currentTime < new DateTime(1900, 1, 1)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CostNoticeModel>("where IsDel=@IsDel and BeginTime<=@BeginTime and EndTime>=@EndTime", new
                { IsDel = FlagEnum.HadZore.GetHashCode(), BeginTime = currentTime, EndTime = currentTime }).ToList();
            }
        }
    }
}
