using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class CarNoticeInfoData : BaseData<long, CarnoticeinfoModel>
    {
        /// <summary>
        /// 获取所有有效的配置数据
        /// </summary>
        /// <returns></returns>
        public List<CarnoticeinfoModel> GetAllCarNoticeInfoModels()
        {
            var where = new StringBuilder(" where IsDel=@IsDel order by id asc");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CarnoticeinfoModel>(where.ToString(), param)?.ToList();
            }
        }

        /// <summary>
        /// 获取所有有效的配置数据
        /// </summary>
        /// <returns></returns>
        public List<CarnoticeinfoModel> GetCarNoticeInfoModelsByIds(List<long> idsList)
        {
            var where = new StringBuilder(" where IsDel=@IsDel and id in @CarId");
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                CarId = idsList.ToArray()
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<CarnoticeinfoModel>(where.ToString(), param)?.ToList();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(CarnoticeinfoModel saveModel)
        {
            if (saveModel == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    return conn.Insert<long, CarnoticeinfoModel>(saveModel);
                }

                //修改
                conn.Update(saveModel);
                return saveModel.Id;
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
            model.IsDel = FlagEnum.HadOne;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
            }
        }
    }
}
