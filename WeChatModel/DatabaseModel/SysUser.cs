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
using Newtonsoft.Json;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// SysUser table in MySQL5
    /// </summary>
    public class SysUser
    {
        /// <summary>
        /// Id 主键，用户id
        /// </summary>
        [Key]
        public Int32 Id { get; set; }
        /// <summary>
        /// UserName 用户名
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        /// Password 密码
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// Sex 性别：0保密；1：男；2：女
        /// </summary>
        public SexEnum Sex { get; set; }
        /// <summary>
        /// Birthday 出生日期
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// HeadUrl 头像地址
        /// </summary>
        public String HeadUrl { get; set; }
        /// <summary>
        /// TelPhone 手机号
        /// </summary>
        public String TelPhone { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// UpdateTime 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// CreateAuth 创建人id
        /// </summary>
        public Int32 CreateAuth { get; set; }
        /// <summary>
        /// UpdateAuth 更新人id
        /// </summary>
        public Int32 UpdateAuth { get; set; }
        /// <summary>
        /// UserType 用户类型0：普通用户；1：超级管理员；2：普通管理员
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// 标记 1：已经删除；0：未删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
    }
}