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

        #region [3、资源管理]

        /// <summary>
        /// 资源管理
        /// </summary>
        [Description("资源管理")]
        ResourceManage=3001,

        /// <summary>
        /// 资源列表
        /// </summary>
        [Description("资源列表")]
        ResourceList=3002,

        #endregion

        #region [4、广告配置]

        /// <summary>
        /// 广告配置
        /// </summary>
        [Description("广告配置")]
        AdvertiseManage = 4001,

        /// <summary>
        /// 广告列表
        /// </summary>
        [Description("广告列表")]
        AdvertiseList = 4002,
        #endregion

        #region [5、留言管理]

        /// <summary>
        /// 留言管理
        /// </summary>
        [Description("留言管理")]
        CustomerCommentManage = 5001,

        /// <summary>
        /// 留言列表
        /// </summary>
        [Description("留言列表")]
        CustomerCommentList = 5002,

        /// <summary>
        /// 收集身份证号码
        /// </summary>
        [Description("客户信息")]
        SysIdCardContentList=5003,
        #endregion

        #region [6、消费记录]

        /// <summary>
        /// 消费记录
        /// </summary>
        [Description("消费记录")]
        CostNoteManager = 6001,

        /// <summary>
        /// 消费记录列表
        /// </summary>
        [Description("消费记录列表")]
        CostNoteList=6002,

        /// <summary>
        /// 消费记录统计
        /// </summary>
        [Description("消费记录统计")]
        CostStatistical = 6003,

        /// <summary>
        /// 支付类型设置
        /// </summary>
        [Description("支付类型设置")]
        CostTypePage=6004,

        /// <summary>
        /// 账户设置
        /// </summary>
        [Description("账户设置")]
        CostChannel=6005,
        #endregion
    }
}
