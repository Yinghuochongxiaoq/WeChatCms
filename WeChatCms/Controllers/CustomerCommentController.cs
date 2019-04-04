using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using FreshCommonUtility.DataConvert;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 留言管理
    /// </summary>
    [Permission(EnumBusinessPermission.CustomerCommentManage)]
    public class CustomerCommentController : AdminControllerBase
    {
        /// <summary>
        /// 留言列表
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CustomerCommentList)]
        public ActionResult CustomerCommentList()
        {
            return View();
        }

        /// <summary>
        /// 获取列表信息
        /// </summary>
        /// <param name="hasDeal"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CustomerCommentList)]
        public ActionResult CustomerCommentListPage(FlagEnum hasDeal, string beginTime, string endTime, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            pageSize = pageSize < 1 ? PageSize : pageSize;
            var beginDateTime = DataTypeConvertHelper.ToDateTime(beginTime, new DateTime(1900, 1, 1));
            var endDateTime = DataTypeConvertHelper.ToDateTime(endTime, new DateTime(1900, 1, 1));
            var server = new CustomerCommentService();
            var dataList = server.GetList(beginDateTime, endDateTime, hasDeal, pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, dataList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CustomerCommentList)]
        public ActionResult DelResourceModels(List<long> ids)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            if (ids == null || ids.Count < 1)
            {
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            var server = new CustomerCommentService();
            try
            {
                server.DelModel(ids);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dealResult"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CustomerCommentList)]
        public ActionResult DealResultModels(long id, string dealResult)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            if (id < 1)
            {
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            var server = new CustomerCommentService();
            try
            {
                server.DealComment(id, dealResult);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
    }
}