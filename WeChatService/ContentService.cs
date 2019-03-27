using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    /// <summary>
    /// 内容服务
    /// </summary>
    public class ContentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysContentData _dataAccess = new SysContentData();

        /// <summary>
        /// 获取单个的菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Syscontent GetContentModel(long id)
        {
            if (id < 1)
            {
                return null;
            }

            return _dataAccess.GetContentModel(id);
        }

        /// <summary>
        /// 添加内容信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long AddAndUpdateContentInfo(Syscontent model)
        {
            if (model == null) return 0;
            return _dataAccess.AddAndUpdateContentInfo(model);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="contentType"></param>
        /// <param name="contentSource"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Syscontent> GetList(string title, string starttime , string endtime , int contentType, string contentSource ,int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(title,starttime,endtime,contentType,contentSource);
            return _dataAccess.GetModels(title, starttime, endtime, contentType, contentSource,indexPage, pageSize);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(int id)
        {
            if (id < 1)
            {
                return;
            }
            _dataAccess.DelModel(id);
        }
    }
}
