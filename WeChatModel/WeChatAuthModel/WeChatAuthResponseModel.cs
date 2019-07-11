using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatModel.WeChatAuthModel
{
    public class WeChatAuthResponseModel
    {
        /// <summary>
        /// Token值
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string CodeTimeSpan { get; set; }

        /// <summary>
        /// AccountId 关联的用户id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 家庭成员
        /// </summary>
        public List<WeChatAuthResponseModel> WechatMemberList { get; set; }
    }
}
