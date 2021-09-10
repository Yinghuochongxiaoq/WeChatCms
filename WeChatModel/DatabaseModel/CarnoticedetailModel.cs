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
    /// carnoticedetail table in MySQL5
    /// </summary>
    [Table("carnoticedetail")]
    public class  CarnoticedetailModel
    {
        /// <summary>
        /// id 
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
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
        /// carid carnoticeinfo主键
        /// </summary>
        public Int64 Carid { get; set; }
                /// <summary>
        /// timerange 提示内容
        /// </summary>
        public String Timerange { get; set; }
                /// <summary>
        /// hadflag 是否已经被占用1：占用；0：未占用
        /// </summary>
        public FlagEnum Hadflag { get; set; }
                /// <summary>
        /// caruserid 被谁占用
        /// </summary>
        public Int64 Caruserid { get; set; }
                /// <summary>
        /// caruserheadimage 占用者头像
        /// </summary>
        public String Caruserheadimage { get; set; }
                /// <summary>
        /// carnickname 昵称
        /// </summary>
        public String Carnickname { get; set; }
                /// <summary>
        /// carrealname 真实名称
        /// </summary>
        public String Carrealname { get; set; }
                /// <summary>
        /// DailyDate 预约日期
        /// </summary>
        public DateTime DailyDate { get; set; }
                /// <summary>
        /// DailyYear 预约年份
        /// </summary>
        public Int32 DailyYear { get; set; }
                /// <summary>
        /// DailyMonth 预约月份
        /// </summary>
        public Int32 DailyMonth { get; set; }
                /// <summary>
        /// DailyDay 预约日
        /// </summary>
        public Int32 DailyDay { get; set; }
            }
}