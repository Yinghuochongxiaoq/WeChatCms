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
        /// 解除绑定/重新绑定
        /// </summary>
        /// <param name="familyModel"></param>
        /// <param name="weChatAccountModel"></param>
        /// <returns></returns>
        public bool UnBindFamilyAndUser(WechatFamilyModel familyModel, WeChatAccountModel weChatAccountModel)
        {
            return _dataAccess.UnBindFamilyAndUser(familyModel, weChatAccountModel);
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

        /// <summary>
        /// 根据家庭code获得家庭成员信息
        /// </summary>
        /// <param name="familyCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WechatFamilyModel GetFamilyMember(string familyCode, long userId)
        {
            return _dataAccess.GetFamilyMember(familyCode, userId);
        }

        /// <summary>
        /// 获取用户的家庭成员信息
        /// </summary>
        /// <param name="familyCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> GetMemberInfoByCode(string familyCode,long userId)
        {
            if (string.IsNullOrEmpty(familyCode)) return null;
            return _dataAccess.GetMembersAccount(familyCode, userId);
        }
    }
}
