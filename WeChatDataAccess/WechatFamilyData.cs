using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    /// <summary>
    /// 家庭成员信息数据服务
    /// </summary>
    public class WechatFamilyData : BaseData<long, WechatFamilyModel>
    {
        /// <summary>
        /// 插入家庭成员，并更新成员信息
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
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (familyModels != null && familyModels.Count > 0)
                    {
                        foreach (var wechatFamilyModel in familyModels)
                        {
                            conn.Insert<long, WechatFamilyModel>(wechatFamilyModel, transaction);
                        }
                    }

                    if (weChatAccountModels != null && weChatAccountModels.Count > 0)
                    {
                        foreach (var weChatAccountModel in weChatAccountModels)
                        {
                            conn.Update(weChatAccountModel);
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached)
                    {
                        Trace.WriteLine("事务处理失败：" + e.Message);
                    }
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取家庭成员列表
        /// </summary>
        /// <param name="familyCode"></param>
        /// <returns></returns>
        public List<WechatFamilyModel> GetFamilyMembers(string familyCode)
        {
            if (string.IsNullOrEmpty(familyCode))
            {
                return null;
            }

            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WechatFamilyModel>(new { IsDel = FlagEnum.HadOne.GetHashCode(), FamilyCode = familyCode })
                    .ToList();
            }
        }
    }
}
