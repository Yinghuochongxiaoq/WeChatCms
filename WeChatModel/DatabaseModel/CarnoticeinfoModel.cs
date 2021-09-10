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
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// carnoticeinfo table in MySQL5
    /// </summary>
    [Table("carnoticeinfo")]
    public class CarnoticeinfoModel
    {
        /// <summary>
        /// id 主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// nickname 名称
        /// </summary>
        public String Nickname { get; set; }
        /// <summary>
        /// start 开始时间
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// end 结束时间
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// createtime 创建时间
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
        /// interimswich 临时开关，开启后不受时间限制
        /// </summary>
        public FlagEnum Interimswich { get; set; }
        /// <summary>
        /// 时间段设置
        /// </summary>
        public List<CarnoticedetailModel> DateTimeRange { get; set; }
    }
}