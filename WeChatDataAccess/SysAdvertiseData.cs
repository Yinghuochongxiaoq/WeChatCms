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
    /// 广告数据服务
    /// </summary>
    public class SysAdvertiseData : BaseData<long, SysadvertisementModel>
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysadvertisementModel> GetModels(string type, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and AdvertiType= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysadvertisementModel>(pageIndex, pageSize, where.ToString(), " Sort desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string type)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and AdvertiType= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysadvertisementModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(SysadvertisementModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, SysadvertisementModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
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
            model.IsDel = FlagEnum.HadOne.GetHashCode();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }
    }
}
