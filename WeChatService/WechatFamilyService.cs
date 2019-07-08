using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class WechatFamilyService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private WechatFamilyData _dataAccess = new WechatFamilyData();

        /// <summary>
        /// 处理绑定家庭数据，更新用户事务
        /// </summary>
        /// <param name="familyModels"></param>
        /// <param name="weChatAccountModels"></param>
        /// <returns></returns>
        public bool BindFamilyAndUser(List<WechatFamilyModel> familyModels,
            List<WeChatAccountModel> weChatAccountModels)
        {
            if (weChatAccountModels?.Count < 0 && familyModels?.Count < 1)
            {
                return false;
            }

            return _dataAccess.BindFamilyAndUser(familyModels, weChatAccountModels);
        }

        /// <summary>
        /// 根据家庭code获得家庭成员信息
        /// </summary>
        /// <param name="familyCode"></param>
        /// <returns></returns>
        public List<WechatFamilyModel> GetFamilyMembers(string familyCode)
        {
            if (string.IsNullOrEmpty(familyCode))
            {
                return null;
            }

            return _dataAccess.GetFamilyMembers(familyCode);
        }
    }
}
