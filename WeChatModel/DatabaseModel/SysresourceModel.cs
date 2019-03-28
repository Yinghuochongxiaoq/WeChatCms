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
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// sysresource table in MySQL5
    /// </summary>
    [Table("sysresource")]
    public class SysresourceModel
    {
        /// <summary>
        /// Id 资源id
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// ResourceUrl 资源链接
        /// </summary>
        public String ResourceUrl { get; set; }
        /// <summary>
        /// ResourceType 资源类型1：图片；2：视频
        /// </summary>
        public ResourceTypeEnum ResourceType { get; set; }
        /// <summary>
        /// ResourceRemark 资源备注
        /// </summary>
        public String ResourceRemark { get; set; }
        /// <summary>
        /// CreateBy 创建人
        /// </summary>
        public String CreateBy { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public Int32 Sort { get; set; }
    }
}