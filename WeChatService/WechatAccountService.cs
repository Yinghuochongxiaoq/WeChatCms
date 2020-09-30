using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class WechatAccountService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private WeChatAccountData _dataAccess = new WeChatAccountData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> GetList(string type, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount();
            return _dataAccess.GetModels(indexPage, pageSize);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        public void DelModel(List<long> ids)
        {
            if (ids == null || ids.Count < 1) return;
            if (ids.Count == 1)
            {
                _dataAccess.DelModel(ids[0]);
            }
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WeChatAccountModel Get(long id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne.GetHashCode()) return null;
            return data;
        }

        /// <summary>
        /// 获取多个用户的信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> Get(List<long> userIds)
        {
            var data = _dataAccess.Get(userIds);
            return data;
        }

        /// <summary>
        /// 通过绑定的id获取用户信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public WeChatAccountModel GetByAccountId(long accountId)
        {
            var data = _dataAccess.GetByAccountId(accountId);
            if (data == null || data.IsDel == FlagEnum.HadOne.GetHashCode()) return null;
            return data;
        }

        /// <summary>
        /// 通过绑定的id获取用户信息
        /// </summary>
        /// <param name="accountIds"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> GetByAccountIds(List<long> accountIds)
        {
            var data = _dataAccess.GetByAccountIds(accountIds);
            return data;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeChatAccountModel GetByOpenId(string openId)
        {
            var data = _dataAccess.GetModelByOpenId(openId);
            if (data == null || data.IsDel == FlagEnum.HadOne.GetHashCode()) return null;
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(WeChatAccountModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}
