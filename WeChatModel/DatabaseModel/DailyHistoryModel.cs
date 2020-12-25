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
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// dailyhistory table in MySQL5
    /// </summary>
    [Table("dailyhistory")]
    public class DailyHistoryModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// DailyNumber 工作量
        /// </summary>
        public Decimal DailyNumber { get; set; }
        /// <summary>
        /// DailyDate 工作时间
        /// </summary>
        public DateTime DailyDate { get; set; }
        /// <summary>
        /// DailyContent 工作内容
        /// </summary>
        public String DailyContent { get; set; }
        /// <summary>
        /// Createtime 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// UserId 用户id
        /// </summary>
        public Int64 UserId { get; set; }
        /// <summary>
        /// UpdateTime 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// DailyYear 时间年
        /// </summary>
        public Int32 DailyYear { get; set; }
        /// <summary>
        /// DailyMonth 时间月
        /// </summary>
        public Int32 DailyMonth { get; set; }
        /// <summary>
        /// DailyDay 时间日
        /// </summary>
        public Int32 DailyDay { get; set; }
    }
}