using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class CostChannelData : BaseData<long, CostChannelModel>
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="isValid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostChannelModel> GetModels(List<long> userIds, int isValid, int pageIndex, int pageSize, string name)
        {
            var where = new StringBuilder(" where UserId in @UserId ");
            where.Append(" and IsDel=@IsDel ");
            if (isValid >= 0)
            {
                where.Append(" and IsValid=@IsValid ");
            }
            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and CostChannelName like @CostChannelName ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                UserId = userIds.ToArray(),
                IsValid = isValid,
                CostChannelName = "%" + name + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<CostChannelModel>(pageIndex, pageSize, where.ToString(), " Sort desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(List<long> userIds, int isValid, string name)
        {
            var where = new StringBuilder(" where UserId in @UserId ");
            where.Append(" and IsDel=@IsDel ");
            if (isValid >= 0)
            {
                where.Append(" and IsValid=@IsValid ");
            }

            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and CostChannelName like @CostChannelName ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                UserId = userIds.ToArray(),
                IsValid = isValid,
                CostChannelName = "%" + name + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<CostChannelModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="costChannelName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CostChannelModel> GetModels(string costChannelName, long userId)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CostChannelModel>(new
                { CostChannelName = costChannelName, UserId = userId, IsDel = FlagEnum.HadOne.GetHashCode() })
                    .ToList();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(CostChannelModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, CostChannelModel>(saveModel);
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
