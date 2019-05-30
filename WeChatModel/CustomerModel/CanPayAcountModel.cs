using System;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.CustomerModel
{
    public class CanPayAcountModel
    {
        /// <summary>
        /// 1入账；0：出账
        /// </summary>
        public CostInOrOutEnum CostInOrOut { get; set; }
        /// <summary>
        /// 消费账户id
        /// </summary>
        public long CostChannel { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string CostChannelName { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal CostCount { get; set; }

        /// <summary>
        /// 消费类型名称
        /// </summary>
        public string CostTypeName { get; set; }
        /// <summary>
        /// 统计消费天
        /// </summary>
        public DateTime CostDay { get; set; }
    }
}
