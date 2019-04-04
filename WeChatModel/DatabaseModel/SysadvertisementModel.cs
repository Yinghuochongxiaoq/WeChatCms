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

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// sysadvertisement table in MySQL5
    /// </summary>
    [Table("sysadvertisement")]
    public class SysadvertisementModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// AdvertiName 广告名称
        /// </summary>
        public String AdvertiName { get; set; }
        /// <summary>
        /// AdvertiType 广告类型(对应字典表中的主键)
        /// </summary>
        public String AdvertiType { get; set; }
        /// <summary>
        /// AdvertiTip 广告语
        /// </summary>
        public String AdvertiTip { get; set; }
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
        /// <summary>
        /// Sort 排序值
        /// </summary>
        public Decimal Sort { get; set; }
        /// <summary>
        /// ResourceUrl 关联资源url
        /// </summary>
        public String ResourceUrl { get; set; }
        /// <summary>
        /// AdvertiUrl 广告链接地址
        /// </summary>
        public String AdvertiUrl { get; set; }
    }
}