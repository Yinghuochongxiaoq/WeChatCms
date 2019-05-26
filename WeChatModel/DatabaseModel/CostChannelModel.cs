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
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// costchannel table in MySQL5
    /// </summary>
    [Table("costchannel")]
    public class CostChannelModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// CostChannelName 渠道名称
        /// </summary>
        public String CostChannelName { get; set; }
        /// <summary>
        /// CostChannelNo 渠道账号
        /// </summary>
        public String CostChannelNo { get; set; }
        /// <summary>
        /// IsDel 1:启用；0：删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// IsValid 1:启用；0：停用
        /// </summary>
        public FlagEnum IsValid { get; set; }
        /// <summary>
        /// UserId 用户id
        /// </summary>
        public Int64 UserId { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// UpdateTime 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// CreateUserId 
        /// </summary>
        public Int64 CreateUserId { get; set; }
        /// <summary>
        /// UpdateUserId 
        /// </summary>
        public Int64 UpdateUserId { get; set; }
        /// <summary>
        /// Sort 排序值
        /// </summary>
        public Int32 Sort { get; set; }
    }
}