/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// sysdict table in MySQL5
    /// </summary>
    [Table("sysdict")]
    public class SysdictModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        [Required]
        public String Id { get; set; }
        /// <summary>
        /// Value 字典值
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// Lable 标签
        /// </summary>
        public String Lable { get; set; }
        /// <summary>
        /// Type 类型
        /// </summary>
        public String Type { get; set; }
        /// <summary>
        /// Description 描述
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Sort 排序值
        /// </summary>
        public Decimal Sort { get; set; }
        /// <summary>
        /// ParentId 上级id
        /// </summary>
        public String ParentId { get; set; }
        /// <summary>
        /// CreateBy 创建人
        /// </summary>
        public String CreateBy { get; set; }
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
    }
}