using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    [Permission(EnumBusinessPermission.AdvertiseManage)]
    public class SysAdvertiseController : AdminControllerBase
    {
        #region [1、广告列表]
        /// <summary>
        /// 广告列表
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.AdvertiseList)]
        public ActionResult SysAdvertiseList()
        {
            return View();
        }

        /// <summary>
        /// 获取列表信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.AdvertiseList)]
        public ActionResult ResourceListPage(string type = null, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            pageSize = pageSize < 1 ? PageSize : pageSize;
            var server = new SysAdvertiseService();
            var dataList = server.GetList(type, pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, dataList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取内容的配置信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.AdvertiseList)]
        public ActionResult GetContentType()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new SysDicService();
            var data = server.GetAllDict("AdvertiseType");
            resultMode.Data = data?.Select(f => new
            {
                f.Id,
                f.Lable,
                f.Type
            }).Distinct().ToList();
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.AdvertiseList)]
        public ActionResult SaveResourceInfo(SysadvertisementModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new SysAdvertiseService();
            var saveModel = new SysadvertisementModel();
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
            saveModel.AdvertiName = model.AdvertiName;
            saveModel.AdvertiTip = model.AdvertiTip;
            saveModel.ResourceUrl = model.ResourceUrl;
            saveModel.Sort = model.Sort;
            saveModel.AdvertiType = model.AdvertiType;
            saveModel.Remarks = model.Remarks;
            saveModel.AdvertiUrl = model.AdvertiUrl;
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
            var server = new SysAdvertiseService();
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
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ResourceList)]
        public ActionResult GetModel(long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new SysAdvertiseService();
            var data = server.Get(id);
            resultMode.Data = data;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}