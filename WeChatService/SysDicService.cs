using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    /// <summary>
    /// 数据字典服务
    /// </summary>
    public class SysDicService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysDicData _dataAccess = new SysDicData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="label"></param>
        /// <param name="type"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysdictModel> GetList(string label, string type, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(label, type);
            return _dataAccess.GetModels(label, type, indexPage, pageSize);
        }

        /// <summary>
        /// 获取所有的字典数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public List<SysdictModel> GetAllDict(string type = null)
        {
            return _dataAccess.GetAllDicType(type);
        }

        /// <summary>
        /// 删除字典记录
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(string id)
        {
            _dataAccess.DelModel(id);
        }

        /// <summary>
        /// 获取单个字典数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysdictModel Get(string id)
        {
            var data=_dataAccess.Get(id);
            if (data == null || data.IsDel==FlagEnum.HadOne.GetHashCode()) return null;
            return data;

        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(SysdictModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}
