using System;
using System.Linq;
using System.Web.Mvc;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.Web;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 内容编辑
    /// </summary>
    [Permission(EnumBusinessPermission.ContentManage)]
    public class ContentController : AdminControllerBase
    {
        #region [1、内容编辑]
        /// <summary>
        /// 内容编辑页面
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ContentEditPage)]
        public ActionResult ContentEdit(long id = 0)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 获取内容信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ContentEditPage)]
        public ActionResult GetContentInfo(long id)
        {
            var resultMode = new ResponseBaseModel<Syscontent>
            {
                ResultCode = ResponceCodeEnum.Fail
            };
            var model = new Syscontent();
            if (id < 1)
            {
                resultMode.Message = "响应成功";
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Data = model;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            model = new ContentService().GetContentModel(id);
            resultMode.Message = "响应成功";
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Data = model;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Permission(EnumBusinessPermission.ContentEditPage)]
        public ActionResult AddContentInfo(Syscontent model)
        {
            var resultMode = new ResponseBaseModel<Syscontent>
            {
                ResultCode = ResponceCodeEnum.Fail
            };
            if (string.IsNullOrEmpty(model.Content))
            {
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Introduction))
            {
                var introduction = FilterHtmlHelper.NoHtml(model.Content);
                model.Introduction = introduction != null && introduction.Length > 200 ? introduction.Substring(0, 200) : introduction;
            }
            var server = new ContentService();
            long id;
            if (model.Id > 0)
            {
                var oldModel = server.GetContentModel(model.Id);
                if (oldModel == null)
                {
                    resultMode.Message = "不存在该内容记录";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                oldModel.Content = model.Content;
                oldModel.ContentSource = model.ContentSource;
                oldModel.ContentType = model.ContentType;
                oldModel.ContentFlag = model.ContentFlag;
                oldModel.Introduction = model.Introduction;
                oldModel.Title = model.Title;
                oldModel.ContentDisImage = model.ContentDisImage;
                oldModel.AttachmentFile = model.AttachmentFile;
                oldModel.AttachmentFileName = model.AttachmentFileName;
                oldModel.AttachmentFileSize = model.AttachmentFileSize;
                id = server.AddAndUpdateContentInfo(oldModel);
            }
            else
            {
                model.CreateTime = DateTime.Now;
                model.CreateUserId = CurrentModel.UserId;
                model.IsDel = FlagEnum.HadZore.GetHashCode();
                id = server.AddAndUpdateContentInfo(model);
            }
            if (id > 0)
            {
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Message = "处理成功";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            resultMode.Message = "处理失败";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取内容的配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetContentType()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new SysDicService();
            var data = server.GetAllDict("ContentType");
            resultMode.Data = data?.Select(f => new
            {
                f.Id,
                f.Lable,
                f.Type
            }).Distinct().ToList();
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [2、内容列表]
        /// <summary>
        /// 内容列表
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ContentEditList)]
        public ActionResult ContentList()
        {
            return View();
        }

        /// <summary>
        /// 获取内容项
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="contentType"></param>
        /// <param name="contentSource"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ContentEditList)]
        public ActionResult GetList(int pageIndex, int pageSize = 0, string title = null, string starttime = null, string endtime = null, string contentType = null, string contentSource = null)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            pageSize = pageSize < 1 ? PageSize : pageSize;
            var dataList = new ContentService().GetList(title, starttime, endtime, contentType, contentSource, pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, dataList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.ContentEditList)]
        public ActionResult DeleteId(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new ContentService();
            try
            {
                menuServer.DelModel(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}