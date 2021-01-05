using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class InuseKeyInfoData
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="keyInfo"></param>
        /// <param name="useYear"></param>
        /// <param name="useMonth"></param>
        /// <returns></returns>
        public List<InusekeyinfoModel> GetModels(string keyType, string keyInfo, int useYear, int useMonth)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(keyType))
            {
                where.Append(" and keyType like @keyType ");
            }

            if (!string.IsNullOrEmpty(keyInfo))
            {
                where.Append(" and keyInfo= @keyInfo ");
            }

            if (useYear > 0)
            {
                where.Append(" and UseYear= @useYear ");
            }

            if (useMonth > 0)
            {
                where.Append(" and UseMonth= @useMonth ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                keyType,
                keyInfo,
                useYear,
                useMonth
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<InusekeyinfoModel>(where.ToString(), param)?.OrderByDescending(f => f.CreateTime).ToList();
            }
        }

        /// <summary>
        /// 获取字典记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InusekeyinfoModel Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<InusekeyinfoModel>(id);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(InusekeyinfoModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                saveModel.UseYear = saveModel.UseDate.Year;
                saveModel.UseMonth = saveModel.UseDate.Month;
                saveModel.UseDay = saveModel.UseDate.Day;
                if (saveModel.Id < 1)
                {
                    //新增
                    var newId = conn.Insert(saveModel);
                    if (newId != null) saveModel.Id = newId.Value;
                }

                //修改
                conn.Update(saveModel);
            }
        }

        /// <summary>
        /// 删除表记录
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            var model = Get(id);
            if (model == null) return;
            model.IsDel = FlagEnum.HadOne;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }

        /// <summary>
        /// 获取所有的类型数据
        /// </summary>
        /// <param name="keyType">类型</param>
        /// <returns></returns>
        public List<InusekeyinfoModel> GetAllType(string keyType)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return string.IsNullOrEmpty(keyType)
                    ? conn.GetList<InusekeyinfoModel>(new { IsDel = FlagEnum.HadZore.GetHashCode() })?.OrderByDescending(f => f.CreateTime).ToList()
                    : conn.GetList<InusekeyinfoModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), KeyType = keyType })?.OrderByDescending(f => f.CreateTime).ToList();
            }
        }
    }
}
