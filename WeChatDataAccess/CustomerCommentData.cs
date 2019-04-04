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
    /// <summary>
    /// 留言数据处理
    /// </summary>
    public class CustomerCommentData : BaseData<long, CustomercommentModel>
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="hasDeal"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<CustomercommentModel> GetModels(DateTime beginTime, DateTime endTime, FlagEnum hasDeal, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where HasDeal=@HasDeal and IsDel=@IsDel ");

            if (beginTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and CreateTime>=@BeginTime ");
            }
            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and CreateTime<@EndTime ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                HasDeal = hasDeal.GetHashCode(),
                BeginTime = beginTime,
                EndTime = endTime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<CustomercommentModel>(pageIndex, pageSize, where.ToString(), " CreateTime desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(DateTime beginTime, DateTime endTime, FlagEnum hasDeal)
        {
            var where = new StringBuilder(" where HasDeal=@HasDeal and IsDel=@IsDel ");

            if (beginTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and CreateTime>=@BeginTime ");
            }
            if (endTime > new DateTime(1900, 1, 1))
            {
                where.Append(" and CreateTime<@EndTime ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                HasDeal = hasDeal.GetHashCode(),
                BeginTime = beginTime,
                EndTime = endTime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<CustomercommentModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(CustomercommentModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, CustomercommentModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
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
            model.IsDel = FlagEnum.HadOne.GetHashCode();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }

        /// <summary>
        /// 处理记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dealResult"></param>
        public void DealComment(long id, string dealResult)
        {
            if (id < 1) return;
            var model = Get(id);
            if (model == null) return;
            model.HasDeal = FlagEnum.HadOne;
            model.DealResult = dealResult;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }
    }
}
