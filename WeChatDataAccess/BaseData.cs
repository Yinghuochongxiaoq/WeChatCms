using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatDataAccess
{
    /// <summary>
    /// 基础数据访问
    /// </summary>
    public class BaseData<TK, T> where T : new()
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> GetModels(int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<T>(pageIndex, pageSize, where.ToString(), "id desc", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<T>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(TK id)
        {
            var typeKey = typeof(TK);
            if (typeKey == typeof(int) || typeKey == typeof(long) || typeKey == typeof(float) || typeKey == typeof(double) ||
                typeKey == typeof(short) || typeKey == typeof(byte) || typeKey == typeof(Int16))
            {
                if (DataTypeConvertHelper.ToLong(id, 0) < 1L) return default(T);
            }
            else if (typeKey == typeof(string) || typeKey == typeof(Guid))
            {
                if (string.IsNullOrEmpty(id.ToString())) return default(T);
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<T>(id);
            }
        }
    }
}
