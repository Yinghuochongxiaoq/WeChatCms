﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class WechatAccountData : BaseData<long, WechatAccountModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(WechatAccountModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, WechatAccountModel>(saveModel);
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
        public WechatAccountModel GetModelByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WechatAccountModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), OpenId = openId })?.FirstOrDefault();
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
    }
}