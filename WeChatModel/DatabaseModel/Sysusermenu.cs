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
    /// Sysusermenu
    /// </summary>
    public class Sysusermenu
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [Key]
        public Int32 Id { get; set; }
        /// <summary>
        /// UserId 用户id
        /// </summary>
        public Int32 UserId { get; set; }
        /// <summary>
        /// MenuId 菜单id
        /// </summary>
        public Int32 MenuId { get; set; }
        /// <summary>
        /// IsDel 是否已经删除0：未删除，1：已经删除
        /// </summary>
        public Int32 IsDel { get; set; }
    }
}