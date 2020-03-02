using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class SysIdCardContentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysIdCardContentData _idCardDataAccess = new SysIdCardContentData();

        /// <summary>
        /// 查询相关信息
        /// </summary>
        /// <param name="preCode">前缀code</param>
        /// <param name="cardNumber">身份证号码</param>
        /// <param name="name">姓名</param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysIdCardContentModel> GetList(string preCode, string cardNumber, string name, int indexPage, int pageSize, out int count)
        {
            if (preCode == "0")
            {
                preCode = null;
            }
            count = _idCardDataAccess.GetCount(preCode, cardNumber, name);
            return _idCardDataAccess.GetModels(preCode, cardNumber, name, indexPage, pageSize);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        public void DelModel(List<long> ids)
        {
            if (ids == null || ids.Count < 1) return;
            if (ids.Count == 1)
            {
                _idCardDataAccess.DelModel(ids[0]);
            }
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysIdCardContentModel Get(long id)
        {
            var data = _idCardDataAccess.Get(id);
            return data;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public SysIdCardContentModel GetByCardNumber(string cardNumber)
        {
            var data = _idCardDataAccess.GetModelByCardNumberId(cardNumber);
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(SysIdCardContentModel saveModel)
        {
            _idCardDataAccess.SaveModel(saveModel);
        }
    }
}
