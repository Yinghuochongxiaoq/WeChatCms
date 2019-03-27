using System.ComponentModel;

namespace WeChatCmsCommon.EnumBusiness
{
    public enum EnumBusinessPermission
    {
        //[EnumTitle("[无]", IsDisplay = false)]
        None = 0,
        #region [1、系统设置]
        /// <summary>
        /// 系统设置
        /// </summary>
        [Description("系统设置")]
        SysSetManage = 1001,

        /// <summary>
        /// 菜单权限设置
        /// </summary>
        [Description("菜单权限设置")]
        MenuSet = 1002,

        /// <summary>
        /// 菜单配置
        /// </summary>
        [Description("菜单配置")]
        MenuAdmin = 1003,

        /// <summary>
        /// 管理员列表
        /// </summary>
        [Description("管理员列表")]
        MenuUsers = 1004,

        /// <summary>
        /// 字典管理
        /// </summary>
        [Description("字典管理")]
        SysDicManage=1005,
        #endregion

        #region [2、内容管理]
        /// <summary>
        /// 内容管理
        /// </summary>
        [Description("内容管理")]
        ContentManage=2001,

        /// <summary>
        /// 内容编辑
        /// </summary>
        [Description("内容编辑")]
        ContentEditPage=2002,

        /// <summary>
        /// 内容列表
        /// </summary>
        [Description("内容列表")]
        ContentEditList=2003,
        #endregion
    }
}
