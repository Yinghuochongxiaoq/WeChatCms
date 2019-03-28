using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    [Permission(EnumBusinessPermission.ResourceManage)]
    public class SysResourceController : AdminControllerBase
    {
        #region [1、资源列表]
        /// <summary>
        /// 资源列表页
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ResourceList)]
        public ActionResult ResourceList()
        {
            return View();
        }

        /// <summary>
        /// 获取资源列表信息
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ResourceList)]
        public ActionResult ResourceListPage(ResourceTypeEnum resourceType = 0, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            pageSize = pageSize < 1 ? PageSize : pageSize;
            var server = new ResourceService();
            var dataList = server.GetList(resourceType, pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, dataList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存图片资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ResourceList)]
        public ActionResult SaveResourceInfo(SysresourceModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new ResourceService();
            var saveModel = new SysresourceModel();
            if (model == null)
            {
                Debug.WriteLine("请求参数为空");
                resultMode.Message = "保存失败";
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            if (model.Id > 0)
            {
                saveModel = server.Get(model.Id);
                if (saveModel == null)
                {
                    resultMode.Message = "该记录已经被删除";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                saveModel.CreateBy = CurrentModel.Id.ToString();
                saveModel.CreateTime = DateTime.Now;
            }

            saveModel.Id = model.Id;
            saveModel.IsDel = FlagEnum.HadZore.GetHashCode();
            saveModel.ResourceRemark = model.ResourceRemark;
            saveModel.ResourceType = model.ResourceType;
            saveModel.ResourceUrl = model.ResourceUrl;
            saveModel.Sort = model.Sort;
            try
            {
                server.SaveModel(saveModel);
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                resultMode.Message = "保存失败";
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Data = e.Message;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ResourceList)]
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
            var server = new ResourceService();
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
        #endregion
    }
}