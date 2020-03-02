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
    /// sysidcardcontent table in MySQL5
    /// </summary>
    [Table("sysidcardcontent")]
    public class SysIdCardContentModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// Name 姓名
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// CardNumber 身份证号码
        /// </summary>
        public String CardNumber { get; set; }
        /// <summary>
        /// Province 所在省份信息
        /// </summary>
        public String Province { get; set; }
        /// <summary>
        /// Area 所在地区信息
        /// </summary>
        public String Area { get; set; }
        /// <summary>
        /// City 所在区县信息
        /// </summary>
        public String City { get; set; }
        /// <summary>
        /// Age 出生年月
        /// </summary>
        public DateTime Age { get; set; }
        /// <summary>
        /// Sex 性别，0为女，1为男
        /// </summary>
        public SexEnum Sex { get; set; }
        /// <summary>
        /// Remarks 备注
        /// </summary>
        public String Remarks { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}