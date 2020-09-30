/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/
using System;
using System.Collections.Generic;
using FreshCommonUtility.Dapper;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// humanstorydetail table in MySQL5
    /// </summary>
    [Table("humanstorydetail")]
    public class HumanstorydetailModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// CreateBy 创建人
        /// </summary>
        public Int64 CreateBy { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Content 发表内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// Address 地址
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        /// IsDel 1:启用；0：删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// 记录涉及的资源
        /// </summary>
        public List<HumanstoryresourceModel> MediaList { get; set; }
        /// <summary>
        /// token
        /// </summary>
        [IgnoreInsert,IgnoreUpdate,IgnoreSelect]
        public string Token { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [IgnoreInsert, IgnoreUpdate, IgnoreSelect]
        public string NikeName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [IgnoreInsert, IgnoreUpdate, IgnoreSelect]
        public string HeadImageUrl { get; set; }
    }
}