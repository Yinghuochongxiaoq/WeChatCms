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

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// syscontent table in MySQL5
    /// </summary>
    public class Syscontent
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// CreateUserId 创建者id（sysuser表id）
        /// </summary>
        public Int64 CreateUserId { get; set; }
        /// <summary>
        /// Title 标题
        /// </summary>
        public String Title { get; set; }
        /// <summary>
        /// Content 文章内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
        /// <summary>
        /// ContentSource 文章来源
        /// </summary>
        public String ContentSource { get; set; }
        /// <summary>
        /// ContentType 内容类型，关联文章类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string ContentFlag { get; set; }
        /// <summary>
        /// 文章配图
        /// </summary>
        public string ContentDisImage { get; set; }
        /// <summary>
        /// 附件文件地址
        /// </summary>
        public string AttachmentFile { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string AttachmentFileSize { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string AttachmentFileName { get; set; }
    }
}