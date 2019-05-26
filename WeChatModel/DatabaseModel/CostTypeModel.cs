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
    /// costtype table in MySQL5
    /// </summary>
    [Table("costtype")]
    public class CostTypeModel
    {
        /// <summary>
        /// Id 主键，类型id
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// Name 类型名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// IsValid 是否有效
        /// </summary>
        public FlagEnum IsValid { get; set; }
        /// <summary>
        /// IsDelete 是否删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// Sort 排序值
        /// </summary>
        public Int32 Sort { get; set; }
        /// <summary>
        /// SpendType 1:收入；0：支出
        /// </summary>
        public Int32 SpendType { get; set; }
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
    }
}