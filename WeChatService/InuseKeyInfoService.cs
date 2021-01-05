using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class InuseKeyInfoService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private readonly InuseKeyInfoData _dataAccess = new InuseKeyInfoData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="keyInfo"></param>
        /// <param name="useYear"></param>
        /// <param name="useMonth"></param>
        /// <returns></returns>
        public List<InusekeyinfoModel> GetModels(string keyType, string keyInfo, int useYear, int useMonth)
        {
            return _dataAccess.GetModels(keyType, keyInfo, useYear, useMonth);
        }

        /// <summary>
        /// 获取所有的字典数据
        /// </summary>
        /// <param name="keyType">类型</param>
        /// <returns></returns>
        public List<InusekeyinfoModel> GetAllTypeList(string keyType = null)
        {
            return _dataAccess.GetAllType(keyType);
        }

        /// <summary>
        /// 删除记录
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
        public InusekeyinfoModel Get(string id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne) return null;
            return data;

        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(InusekeyinfoModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}
