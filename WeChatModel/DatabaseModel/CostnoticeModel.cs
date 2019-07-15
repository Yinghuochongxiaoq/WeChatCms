/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/
using System;
using FreshCommonUtility.Dapper;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// costnotice table in MySQL5
    /// </summary>
    [Table("costnotice")]
    public class CostNoticeModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// NoticeUrl 连接地址
        /// </summary>
        public String NoticeUrl { get; set; }
        /// <summary>
        /// NoticeTitle 通知标题
        /// </summary>
        public String NoticeTitle { get; set; }
        /// <summary>
        /// NoticeContent 内容
        /// </summary>
        public String NoticeContent { get; set; }
        /// <summary>
        /// BeginTime 通知开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// EndTime 通知结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Remarks 备注
        /// </summary>
        public String Remarks { get; set; }
    }
}