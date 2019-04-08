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
    /// 内容数据服务类
    /// </summary>
    public class SysContentData
    {
        /// <summary>
        /// 根据id获取内容信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Syscontent GetContentModel(long id)
        {
            if (id < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<Syscontent>(id);
            }
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long AddAndUpdateContentInfo(Syscontent model)
        {
            if (model == null) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (model.Id > 0)
                {
                    return conn.Update(model);
                }
                return conn.Insert<long, Syscontent>(model);
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="contentType"></param>
        /// <param name="contentSource"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Syscontent> GetModels(string title, string starttime, string endtime, string contentType, string contentSource, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(title))
            {
                where.Append(" and Title like @Title ");
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                where.Append(" and CreateTime> @StartTime ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                where.Append(" and CreateTime< @EndTime ");
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                where.Append(" and ContentType=@ContentType ");
            }

            if (!string.IsNullOrEmpty(contentSource))
            {
                where.Append(" and ContentSource=@ContentSource ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Title = "%" + title + "%",
                ContentSource = contentSource,
                ContentType = contentType,
                StartTime = starttime,
                EndTime = endtime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<Syscontent>(pageIndex, pageSize, where.ToString(), " CreateTime desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string title, string starttime, string endtime, string contentType, string contentSource)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(title))
            {
                where.Append(" and Title like @Title ");
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                where.Append(" and CreateTime> @StartTime ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                where.Append(" and CreateTime< @EndTime ");
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                where.Append(" and ContentType=@ContentType ");
            }

            if (!string.IsNullOrEmpty(contentSource))
            {
                where.Append(" and ContentSource=@ContentSource ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Title = "%" + title + "%",
                ContentSource = contentSource,
                ContentType = contentType,
                StartTime = starttime,
                EndTime = endtime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<Syscontent>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(int id)
        {
            if (id < 1) return;
            var model = GetContentModel(id);
            if (model != null)
            {
                model.IsDel = FlagEnum.HadOne.GetHashCode();
                using (var conn = SqlConnectionHelper.GetOpenConnection())
                {
                    conn.Update(model);
                }
            }

        }
    }
}
