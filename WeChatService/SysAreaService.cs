using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class SysAreaService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysAreaData _dataAccess = new SysAreaData();

        /// <summary>
        /// 查询pCode
        /// </summary>
        /// <param name="pCode">父级编码</param>
        /// <returns></returns>
        public List<SysAreaModel> GetList(string pCode)
        {
            return _dataAccess.GetModelsByPCode(pCode);
        }
    }
}
