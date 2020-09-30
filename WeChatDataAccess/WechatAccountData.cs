using System.Collections.Generic;
using System.Linq;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class WeChatAccountData : BaseData<long, WeChatAccountModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(WeChatAccountModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, WeChatAccountModel>(saveModel);
                }
                else
                {
                    //修改
                    conn.Update(saveModel);
                }
            }
        }

        /// <summary>
        /// 根据openid查询用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeChatAccountModel GetModelByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WeChatAccountModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), OpenId = openId })?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据accountid获取用户信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public WeChatAccountModel GetByAccountId(long accountId)
        {
            if (accountId < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WeChatAccountModel>(new
                { IsDel = FlagEnum.HadZore.GetHashCode(), AccountId = accountId })?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据accountIds获取用户信息
        /// </summary>
        /// <param name="accountIds"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> GetByAccountIds(List<long> accountIds)
        {
            if (accountIds == null || accountIds.Count < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WeChatAccountModel>("where IsDel=@IsDel and AccountId in @AccountId", new
                { IsDel = FlagEnum.HadZore.GetHashCode(), AccountId = accountIds.ToArray() }).ToList();
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
        /// 获取多个用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> Get(List<long> userIds)
        {
            if (userIds == null || userIds.Count < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Query<WeChatAccountModel>("select * from wechataccount where AccountId in @AccountId and IsDel=@IsDel ", new
                { IsDel = FlagEnum.HadZore.GetHashCode(), AccountId = userIds.ToArray() }).ToList();
            }
        }
    }
}
