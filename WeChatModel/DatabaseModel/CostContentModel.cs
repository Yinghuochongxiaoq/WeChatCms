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
    /// costcontent table in MySQL5
    /// </summary>
    [Table("costcontent")]
    public class CostContentModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// Cost 
        /// </summary>
        public Decimal Cost { get; set; }
        /// <summary>
        /// UserId 
        /// </summary>
        public Int64 UserId { get; set; }
        /// <summary>
        /// CostAddress 
        /// </summary>
        public String CostAddress { get; set; }
        /// <summary>
        /// CostTime 
        /// </summary>
        public DateTime CostTime { get; set; }
        /// <summary>
        /// CostThing 
        /// </summary>
        public String CostThing { get; set; }
        /// <summary>
        /// 消费类型快照
        /// </summary>
        public String CostTypeName { get; set; }
        /// <summary>
        /// CostType 
        /// </summary>
        public Int32 CostType { get; set; }
        /// <summary>
        /// CostYear 
        /// </summary>
        public Int32 CostYear { get; set; }
        /// <summary>
        /// CostMonth 
        /// </summary>
        public Int32 CostMonth { get; set; }
        /// <summary>
        /// SpendType 2:转移；1:收入；0：支出
        /// </summary>
        public Int32 SpendType { get; set; }
        /// <summary>
        /// CostChannel 渠道id
        /// </summary>
        public Int64 CostChannel { get; set; }
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
        /// 渠道名称快照
        /// </summary>
        public String CostChannelName { get; set; }
        /// <summary>
        /// 支付账号/收款账号
        /// </summary>
        public String CostChannelNo { get; set; }
        /// <summary>
        /// 1:入账；0：出账
        /// </summary>
        public CostInOrOutEnum CostInOrOut { get; set; }
        /// <summary>
        /// 关联入账账户
        /// </summary>
        public Int64 LinkCostChannel { get; set; }
        /// <summary>
        /// 关联记录id
        /// </summary>
        public Int64 LinkCostId { get; set; }
        /// <summary>
        /// 关联记录渠道名称
        /// </summary>
        public String LinkCostChannelName { get; set; }
        /// <summary>
        /// 关联记录渠道账号
        /// </summary>
        public String LinkCostChannelNo { get; set; }
    }
}