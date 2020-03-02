using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class SysAreaData : BaseData<long, SysAreaModel>
    {
        /// <summary>
        /// 根据CardNumber查询用户
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public List<SysAreaModel> GetModelsByPCode(string pCode)
        {
            if (string.IsNullOrEmpty(pCode))
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<SysAreaModel>(new { PCODE = pCode }).ToList();
            }
        }
    }
}
