using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Dapper;
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
                            conn.Update(weChatAccountModel, transaction);
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
        /// 解除用户绑定
        /// </summary>
        /// <param name="familyModel"></param>
        /// <param name="weChatAccountModel"></param>
        /// <returns></returns>
        public bool UnBindFamilyAndUser(WechatFamilyModel familyModel, WeChatAccountModel weChatAccountModel)
        {
            if (familyModel == null || weChatAccountModel == null)
            {
                return false;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();

                try
                {
                    conn.Update(familyModel, transaction);
                    conn.Update(weChatAccountModel, transaction);
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
                return conn.GetList<WechatFamilyModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), FamilyCode = familyCode })
                    .ToList();
            }
        }

        /// <summary>
        /// 获取家庭成员
        /// </summary>
        /// <param name="familyCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WechatFamilyModel GetFamilyMember(string familyCode, long userId)
        {
            if (string.IsNullOrEmpty(familyCode))
            {
                return null;
            }

            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WechatFamilyModel>(new { UserId = userId, FamilyCode = familyCode })?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取家庭成员列表
        /// </summary>
        /// <param name="familyCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WeChatAccountModel> GetMembersAccount(string familyCode, long userId)
        {
            if (string.IsNullOrEmpty(familyCode))
            {
                return null;
            }

            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Query<WeChatAccountModel>(@"SELECT
	wa.AccountId,
	wa.AvatarUrl,
	wa.CreateTime,
	wa.FamilyCode,
	wa.Gender,
	wa.HadBindFamily,
	wa.Id,
	wa.Id,
	wa.IsDel,
	wa.IsDel,
	wa.NickName,
	wa.OpenId,
	wa.Remarks,
	wf.UnBindTime UpdateTime 
FROM
	wechatfamily wf
	LEFT JOIN wechataccount wa ON wa.AccountId = wf.UserId 
WHERE
	wf.FamilyCode = @FamilyCode 
	AND wa.IsDel = @IsDel 
	AND ( wf.IsDel = @IsDel OR wf.UserId = @CurrentUserId )", new
                { IsDel = FlagEnum.HadZore.GetHashCode(), FamilyCode = familyCode, CurrentUserId = userId }).ToList();
            }
        }
    }
}
