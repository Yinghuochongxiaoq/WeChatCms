using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    /// <summary>
    /// 处理记录的身份证号
    /// </summary>
    public class SysIdCardContentData : BaseData<long, SysIdCardContentModel>
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(SysIdCardContentModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, SysIdCardContentModel>(saveModel);
                }
                else
                {
                    //修改
                    conn.Update(saveModel);
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="preCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="name"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysIdCardContentModel> GetModels(string preCode, string cardNumber, string name, int indexPage, int pageSize)
        {
            var where = new StringBuilder(" where 1=1 ");

            if (!string.IsNullOrEmpty(preCode))
            {
                where.Append(" AND CardNumber like @preCardNumber ");
            }
            if (!string.IsNullOrEmpty(cardNumber))
            {
                where.Append(" AND CardNumber like @CardNumber ");
            }

            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and Name like @Name ");
            }
            var param = new
            {
                Name = "%" + name + "%",
                preCardNumber = preCode + "%",
                CardNumber = "%" + cardNumber + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysIdCardContentModel>(indexPage, pageSize, where.ToString(), " Id desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="preCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetCount(string preCode, string cardNumber, string name)
        {
            var where = new StringBuilder(" where 1=1 ");

            if (!string.IsNullOrEmpty(preCode))
            {
                where.Append(" AND CardNumber like @preCardNumber ");
            }
            if (!string.IsNullOrEmpty(cardNumber))
            {
                where.Append(" AND CardNumber like @CardNumber ");
            }

            if (!string.IsNullOrEmpty(name))
            {
                where.Append(" and Name like @Name ");
            }
            var param = new
            {
                Name = "%" + name + "%",
                preCardNumber = preCode + "%",
                CardNumber = "%" + cardNumber + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysIdCardContentModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 根据CardNumber查询用户
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public SysIdCardContentModel GetModelByCardNumberId(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length > 18)
            {
                return null;
            }
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<SysIdCardContentModel>(new { CardNumber = cardNumber })?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 删除表记录
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(long id)
        {
            if (id < 1) return;
            var model = Get(id);
            if (model == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Delete(model);
            }
        }
    }
}
