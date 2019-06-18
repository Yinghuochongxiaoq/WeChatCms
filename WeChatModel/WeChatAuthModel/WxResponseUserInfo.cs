using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatModel.WeChatAuthModel
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WxResponseUserInfo
    {
        /// <summary>
        /// 单平台用户唯一ID
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 多平台用户公用唯一ID
        /// </summary>
        public string unionId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string avatarUrl { get; set; }
        public string sessionKey { get; set; }
    }
}
