namespace WeChatNoteCostApi.WeChatInnerModel
{
    public class RedisCacheKey
    {
        /// <summary>
        /// 用户响应信息缓存key
        /// </summary>
        public static string AuthInfoKey = "AuthInfoKey_";

        /// <summary>
        /// 验证码缓存
        /// </summary>
        public static string AuthCheckCodeKey = "AuthCheckCodeKey_";

        /// <summary>
        /// token信息
        /// </summary>
        public static string AuthTokenKey = "AuthTokenKey_";

        /// <summary>
        /// 缓存邀请码缓存key
        /// </summary>
        public static string InviteCodeKey = "GetInviteCode_";
    }
}