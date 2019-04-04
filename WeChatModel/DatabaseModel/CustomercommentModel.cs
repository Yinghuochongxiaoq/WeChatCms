/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/

using System;
using System.ComponentModel.DataAnnotations.Schema;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// customercomment table in MySQL5
    /// </summary>
    [Table("customercomment")]
    public class CustomercommentModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
        /// <summary>
        /// Content 内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// CustomerName 姓名
        /// </summary>
        public String CustomerName { get; set; }
        /// <summary>
        /// CustomerPhone 电话
        /// </summary>
        public String CustomerPhone { get; set; }
        /// <summary>
        /// CustomerEmail 邮箱
        /// </summary>
        public String CustomerEmail { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已经处理
        /// </summary>
        public FlagEnum HasDeal { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        public String DealResult { get; set; }
    }
}