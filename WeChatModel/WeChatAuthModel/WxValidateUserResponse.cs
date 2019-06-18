using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatModel.WeChatAuthModel
{
    /// <summary>
    /// 微信授权用户反馈类
    /// </summary>
    public class WxValidateUserResponse
    {
        /// <summary>
        /// session_key
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 单平台用户唯一ID
        /// </summary>
        public string openid { get; set; }
    }
}
