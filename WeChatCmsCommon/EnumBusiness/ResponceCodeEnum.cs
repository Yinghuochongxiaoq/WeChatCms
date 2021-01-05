namespace WeChatCmsCommon.EnumBusiness
{
    /// <summary>
    /// 响应代码
    /// </summary>
    public enum ResponceCodeEnum
    {
        /// <summary>
        /// 成功 0
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败-1
        /// </summary>
        Fail = -1,

        /// <summary>
        /// 需要登录
        /// </summary>
        NeedLogin = 201,

        /// <summary>
        /// 404
        /// </summary>
        Page404 = 404,

        /// <summary>
        /// 500
        /// </summary>
        Page500 = 500,
    }
}
