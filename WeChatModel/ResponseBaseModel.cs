using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel
{
    public class ResponseBaseModel<T>
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        public ResponceCodeEnum ResultCode { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
    }
}
