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
    /// 消费类型
    /// </summary>
    public class CostTypeData : BaseData<long, CostTypeModel>
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="spendType"></param>
        /// <param name="userIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostTypeModel> GetModels(int spendType, List<long> userIds, int pageIndex, int pageSize, string name)
        {
            var where = new StringBuilder(" where UserId in @UserId ");
            where.Append(" and IsDel=@IsDel ");
            if (spendType != -1)
            {
                where.Append(" and SpendType= @Type ");
            }
            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and Name like @Name ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = spendType,
                UserId = userIds.ToArray(),
                Name = "%" + name + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<CostTypeModel>(pageIndex, pageSize, where.ToString(), " Sort desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(int spendType, List<long> userIds, string name)
        {
            var where = new StringBuilder(" where UserId in @UserId ");
            where.Append(" and IsDel=@IsDel ");
            if (spendType != -1)
            {
                where.Append(" and SpendType= @Type ");
            }

            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and Name like @Name ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = spendType,
                UserId = userIds.ToArray(),
                Name = "%" + name + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<CostTypeModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(CostTypeModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, CostTypeModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
            }
        }

        /// <summary>
        /// 查询指定用户的类型，名称
        /// </summary>
        /// <param name="spendType"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostTypeModel> GetModels(int spendType, long userId, string name)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CostTypeModel>(new { SpendType = spendType, UserId = userId, Name = name, IsDel = FlagEnum.HadZore.GetHashCode() }).ToList();
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
