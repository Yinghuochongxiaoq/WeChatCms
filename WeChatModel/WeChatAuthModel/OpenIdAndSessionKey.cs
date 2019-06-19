namespace WeChatModel.WeChatAuthModel
{
    /// <summary>
    /// 微信授权用户反馈类
    /// </summary>
    public class OpenIdAndSessionKey
    {
        /// <summary>
        /// session_key
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 单平台用户唯一ID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 错误code
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
