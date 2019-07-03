using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.CustomerModel;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class CostContentData : BaseData<long, CostContentModel>
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <param name="costType"></param>
        /// <param name="costchannel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<CostContentModel> GetModels(long userId, int spendType, string address, string costThing, int costType, long costchannel, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where UserId=@UserId and IsDel=@IsDel ");

            if (spendType != -1)
            {
                where.Append(" AND SpendType = @SpendType ");
            }
            if (costType != -1)
            {
                where.Append(" AND CostType = @CostType ");
            }

            if (costchannel != -1)
            {
                where.Append(" and CostChannel=@CostChannel ");
            }

            if (startTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime >= @StartTime ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime <= @EndTime ");
            }
            if (!string.IsNullOrEmpty(address))
            {
                where.Append(" AND CostAddress LIKE @CostAddress ");
            }

            if (!string.IsNullOrEmpty(costThing))
            {
                where.Append(" AND CostThing LIKE @CostThing ");
            }
            var param = new
            {
                UserId = userId,
                SpendType = spendType,
                CostType = costType,
                CostChannel = costchannel,
                StartTime = startTime,
                EndTime = endTime,
                CostAddress = "%" + address + "%",
                CostThing = "%" + costThing + "%",
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<CostContentModel>(pageIndex, pageSize, where.ToString(), " CostTime desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(long userId, int spendType, string address, string costThing, int costType, long costchannel, DateTime startTime, DateTime endTime)
        {
            var where = new StringBuilder(" where UserId=@UserId and IsDel=@IsDel ");

            if (spendType != -1)
            {
                where.Append(" AND SpendType = @SpendType ");
            }
            if (costType != -1)
            {
                where.Append(" AND CostType = @CostType ");
            }

            if (costchannel != -1)
            {
                where.Append(" and CostChannel=@CostChannel ");
            }

            if (startTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime >= @StartTime ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime <= @EndTime ");
            }
            if (!string.IsNullOrEmpty(address))
            {
                where.Append(" AND CostAddress LIKE @CostAddress ");
            }

            if (!string.IsNullOrEmpty(costThing))
            {
                where.Append(" AND CostThing LIKE @CostThing ");
            }
            var param = new
            {
                UserId = userId,
                SpendType = spendType,
                CostType = costType,
                CostChannel = costchannel,
                StartTime = startTime,
                EndTime = endTime,
                CostAddress = "%" + address + "%",
                CostThing = "%" + costThing + "%",
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<CostContentModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="spendType"></param>
        /// <param name="address"></param>
        /// <param name="costThing"></param>
        /// <param name="costType"></param>
        /// <param name="costchannel"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetStatisticsCost(long userId, int spendType, string address, string costThing, int costType, long costchannel, DateTime startTime, DateTime endTime)
        {
            var select = "select CostInOrOut,sum(cost) Sum from costcontent ";
            var where = new StringBuilder(" where UserId=@UserId and IsDel=@IsDel ");

            if (spendType != -1)
            {
                where.Append(" AND SpendType = @SpendType ");
            }
            if (costType != -1)
            {
                where.Append(" AND CostType = @CostType ");
            }

            if (costchannel != -1)
            {
                where.Append(" and CostChannel=@CostChannel ");
            }

            if (startTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime >= @StartTime ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" AND CostTime <= @EndTime ");
            }
            if (!string.IsNullOrEmpty(address))
            {
                where.Append(" AND CostAddress LIKE @CostAddress ");
            }

            if (!string.IsNullOrEmpty(costThing))
            {
                where.Append(" AND CostThing LIKE @CostThing ");
            }

            var groupby = " GROUP BY CostInOrOut ";
            var param = new
            {
                UserId = userId,
                SpendType = spendType,
                CostType = costType,
                CostChannel = costchannel,
                StartTime = startTime,
                EndTime = endTime,
                CostAddress = "%" + address + "%",
                CostThing = "%" + costThing + "%",
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            var result = new Dictionary<int, decimal>();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<dynamic> query = conn.Query(select + where + groupby, param);
                foreach (var rows in query)
                {
                    if (!(rows is IDictionary<string, object> fields)) continue;
                    var sum = fields["Sum"];
                    var inOrOut = fields["CostInOrOut"];
                    result.Add(DataTypeConvertHelper.ToInt(inOrOut), DataTypeConvertHelper.ToDecimal(sum));
                }
            }

            return result;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(CostContentModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, CostContentModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
            }
        }

        /// <summary>
        /// 保存关联消费信息
        /// </summary>
        /// <param name="outModel"></param>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public long SaveLinkCostModel(CostContentModel outModel, CostContentModel inModel)
        {
            if (outModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                //新增
                if (outModel.Id < 1)
                {
                    try
                    {
                        var outId = conn.Insert<long, CostContentModel>(outModel, transaction);
                        var inId = conn.Insert<long, CostContentModel>(inModel, transaction);
                        outModel.Id = outId;
                        outModel.LinkCostId = inId;

                        inModel.Id = inId;
                        inModel.LinkCostId = outId;
                        conn.Update(outModel, transaction);
                        conn.Update(inModel, transaction);
                        transaction.Commit();
                        return outId;
                    }
                    catch (Exception e)
                    {
                        if (Debugger.IsAttached)
                        {
                            Trace.WriteLine("事务处理失败：" + e.Message);
                        }
                        transaction.Rollback();
                        return 0;
                    }
                }

                try
                {
                    //修改
                    conn.Update(outModel, transaction);
                    conn.Update(inModel, transaction);
                    transaction.Commit();
                    return outModel.Id;
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached)
                    {
                        Trace.WriteLine("事务处理失败：" + e.Message);
                    }
                    transaction.Rollback();
                    return 0;
                }

            }
        }

        /// <summary>
        /// 获取余额统计数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CanPayAcountModel> GetStatisticsCanPay(long userId)
        {
            var select = @"	SELECT CostInOrOut, CostChannel, sum( cost ) CostCount,CostChannelName
	FROM costcontent WHERE userid = @UserId and IsDel=@IsDel GROUP BY CostInOrOut, CostChannel ,CostChannelName";
            var param = new
            {
                UserId = userId,
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<CanPayAcountModel> query = conn.Query<CanPayAcountModel>(select, param);
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取消费分类记录
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <param name="inOrOut"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public List<CanPayAcountModel> GetStatisticsCostTypePay(DateTime starTime, DateTime endTime, long userId, CostInOrOutEnum inOrOut, long channelId)
        {
            var select = @"SELECT
	CostTypeName,
	sum( cost ) CostCount 
FROM
	costcontent ";
            var groupby = " GROUP BY CostTypeName ORDER BY CostCount DESC";
            var where = new StringBuilder("WHERE UserId = @UserId and IsDel=@IsDel ");
            where.Append(" AND SpendType!=2 ");
            where.Append(" AND CostInOrOut = @CostInOrOut  ");
            if (starTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime>@StartTime  ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime<=@EndTime  ");
            }

            if (channelId > 0)
            {
                where.Append(" and  CostChannel=@CostChannel  ");
            }
            var param = new
            {
                UserId = userId,
                CostInOrOut = inOrOut.GetHashCode(),
                StartTime = starTime,
                EndTime = endTime,
                CostChannel = channelId,
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<CanPayAcountModel> query = conn.Query<CanPayAcountModel>(select + where + groupby, param);
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取消费天记录
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <param name="inOrOut"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public List<CanPayAcountModel> GetStatisticsCostDayPay(DateTime starTime, DateTime endTime, long userId, CostInOrOutEnum inOrOut, long channelId)
        {
            var select = @"select DATE(CostTime) as CostDay,sum(cost) as CostCount from costcontent ";
            var groupby = " group by CostDay ORDER BY CostDay";
            var where = new StringBuilder("WHERE UserId = @UserId and IsDel=@IsDel ");
            where.Append(" AND SpendType!=2 ");
            where.Append(" AND CostInOrOut = @CostInOrOut  ");
            if (starTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime>@StartTime  ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime<=@EndTime  ");
            }

            if (channelId > 0)
            {
                where.Append(" and  CostChannel=@CostChannel  ");
            }
            var param = new
            {
                UserId = userId,
                CostInOrOut = inOrOut.GetHashCode(),
                StartTime = starTime,
                EndTime = endTime,
                CostChannel = channelId,
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<CanPayAcountModel> query = conn.Query<CanPayAcountModel>(select + where + groupby, param);
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取消费分类记录
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <param name="inOrOut"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetStatisticsCostMonth(DateTime starTime, DateTime endTime, long userId, CostInOrOutEnum inOrOut, long channelId)
        {
            var select = @"SELECT
	CostYear,CostMonth,
	sum( cost ) CostCount 
FROM 
	costcontent ";
            var groupby = " GROUP BY CostYear,CostMonth ORDER BY CostYear desc,CostMonth desc";
            var where = new StringBuilder("WHERE UserId = @UserId and IsDel=@IsDel ");
            where.Append(" AND SpendType!=2 ");
            where.Append(" AND CostInOrOut = @CostInOrOut  ");
            if (starTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime>@StartTime  ");
            }

            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and  CostTime<=@EndTime  ");
            }

            if (channelId > 0)
            {
                where.Append(" and  CostChannel=@CostChannel  ");
            }
            var param = new
            {
                UserId = userId,
                CostInOrOut = inOrOut.GetHashCode(),
                StartTime = starTime,
                EndTime = endTime,
                CostChannel = channelId,
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            var resultMap = new Dictionary<int, decimal>();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<dynamic> query = conn.Query(select + where + groupby, param);
                foreach (var rows in query)
                {
                    if (!(rows is IDictionary<string, object> fields)) continue;
                    var sum = fields["CostCount"];
                    var costYear = fields["CostYear"];
                    var costMonth = fields["CostMonth"];
                    resultMap.Add(DataTypeConvertHelper.ToInt(costYear) * 100 + DataTypeConvertHelper.ToInt(costMonth), DataTypeConvertHelper.ToDecimal(sum));
                }
            }

            return resultMap;
        }
    }
}
