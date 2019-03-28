using System.ComponentModel;

namespace WeChatCmsCommon.EnumBusiness
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceTypeEnum
    {
        /// <summary>
        /// 未知 0
        /// </summary>
        [Description("未知")]
        None = 0,

        /// <summary>
        /// 图片 1
        /// </summary>
        [Description("图片")]
        Image=1,

        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video=2,
    }
}
