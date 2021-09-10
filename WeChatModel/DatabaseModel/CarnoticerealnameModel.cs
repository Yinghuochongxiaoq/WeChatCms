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
    /// carnoticerealname table in MySQL5
    /// </summary>
    [Table("carnoticerealname")]
    public class CarNoticeRealNameModel
    {
        /// <summary>
        /// Id 自增主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// creattime 创建时间
        /// </summary>
        public DateTime creattime { get; set; }
        /// <summary>
        /// AccountId 关联的用户id
        /// </summary>
        public Int64 AccountId { get; set; }
        /// <summary>
        /// realname 关联的用户名称
        /// </summary>
        public String realname { get; set; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public FlagEnum IsSuper { get; set; }
    }
}