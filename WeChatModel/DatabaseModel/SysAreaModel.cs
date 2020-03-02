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

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// sysarea table in MySQL5
    /// </summary>
    [Table("sysarea")]
    public class SysAreaModel
    {
        /// <summary>
        /// id 
        /// </summary>
        [Key]
        public Int32 id { get; set; }
        /// <summary>
        /// CODE 
        /// </summary>
        public String CODE { get; set; }
        /// <summary>
        /// PCODE 
        /// </summary>
        public String PCODE { get; set; }
        /// <summary>
        /// PNAME 
        /// </summary>
        public String PNAME { get; set; }
        /// <summary>
        /// NAME 
        /// </summary>
        public String NAME { get; set; }
        /// <summary>
        /// DESC1 
        /// </summary>
        public String DESC1 { get; set; }
        /// <summary>
        /// poverty 
        /// </summary>
        public String poverty { get; set; }
        /// <summary>
        /// x 
        /// </summary>
        public String x { get; set; }
        /// <summary>
        /// y 
        /// </summary>
        public String y { get; set; }
    }
}