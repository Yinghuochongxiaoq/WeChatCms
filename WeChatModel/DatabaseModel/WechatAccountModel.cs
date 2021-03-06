﻿/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// wechataccount table in MySQL5
    /// </summary>
    [Table("wechataccount")]
    public class WeChatAccountModel
    {
        /// <summary>
        /// Id 自增主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// OpenId 微信OpenId
        /// </summary>
        public String OpenId { get; set; }
        /// <summary>
        /// NickName 昵称获取微信授权的昵称或重命名
        /// </summary>
        public String NickName { get; set; }
        /// <summary>
        /// AvatarUrl 头像
        /// </summary>
        public String AvatarUrl { get; set; }
        /// <summary>
        /// Gender 性别1:女；2:男
        /// </summary>
        public Int32 Gender { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Remarks 备注
        /// </summary>
        public String Remarks { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
        /// <summary>
        /// AccountId 关联的用户id
        /// </summary>
        public Int64 AccountId { get; set; }
        /// <summary>
        /// 关联的家庭code
        /// </summary>
        public String FamilyCode { get; set; }
        /// <summary>
        /// 是否关联了家庭0：未关联；1：已经关联
        /// </summary>
        public FlagEnum HadBindFamily { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpDateTime { get; set; }
    }
}