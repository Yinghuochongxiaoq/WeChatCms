using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class SysAdvertiseService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysAdvertiseData _dataAccess = new SysAdvertiseData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysadvertisementModel> GetList(string type, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(type);
            return _dataAccess.GetModels(type, indexPage, pageSize);
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
                _dataAccess.DelModel(ids[0]);
            }
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysadvertisementModel Get(long id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne.GetHashCode()) return null;
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(SysadvertisementModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}
