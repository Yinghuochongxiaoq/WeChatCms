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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// inusekeyinfo table in MySQL5
    /// </summary>
    [Table("inusekeyinfo")]
    public class InusekeyinfoModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [Key]
        public Int32 Id { get; set; }
        /// <summary>
        /// IsDel 是否删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// UseCount 使用次数
        /// </summary>
        public Int32 UseCount { get; set; }
        /// <summary>
        /// UseYear 
        /// </summary>
        public Int32 UseYear { get; set; }
        /// <summary>
        /// UseMonth 
        /// </summary>
        public Int32 UseMonth { get; set; }
        /// <summary>
        /// UseDay 
        /// </summary>
        public Int32 UseDay { get; set; }
        /// <summary>
        /// UseDate 最后使用时间
        /// </summary>
        public DateTime UseDate { get; set; }
        /// <summary>
        /// KeyInfo 关键信息
        /// </summary>
        public String KeyInfo { get; set; }
        /// <summary>
        /// KeyType 关键信息分类
        /// </summary>
        public String KeyType { get; set; }
        /// <summary>
        /// Remark 备注信息
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
