using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FreshCommonUtility.Security;
using FreshCommonUtility.Web;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [Permission(EnumBusinessPermission.SysIdCardContentList)]
    public class SysIdCardContentController : AdminControllerBase
    {
        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult IdCardContentList()
        {
            return View();
        }

        /// <summary>
        /// 获取省信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProvinceType(string pCode = "0")
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new SysAreaService();
            var data = server.GetList(pCode);
            resultMode.Data = data?.Select(f => new { code = f.CODE, name = f.NAME }).Distinct().ToList();
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取内容信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSysIdCardInfo(long id)
        {
            var resultMode = new ResponseBaseModel<SysIdCardContentModel>
            {
                ResultCode = ResponceCodeEnum.Fail
            };
            var model = new SysIdCardContentModel();
            if (id < 1)
            {
                resultMode.Message = "响应成功";
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Data = model;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            model = new SysIdCardContentService().Get(id);
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
        public ActionResult SaveIdCardNumberInfo(SysIdCardContentModel model)
        {
            var resultMode = new ResponseBaseModel<SysIdCardContentModel>
            {
                ResultCode = ResponceCodeEnum.Fail
            };
            if (model == null)
            {
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                resultMode.Message = "姓名不能为空";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.CardNumber))
            {
                resultMode.Message = "身份证号不能为空";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(model.Remarks))
            {
                var introduction = FilterHtmlHelper.NoHtml(model.Remarks);
                model.Remarks = introduction != null && introduction.Length > 200 ? introduction.Substring(0, 200) : introduction;
            }

            if (!IdCardValidatorHelper.CheckIdCard(model.CardNumber))
            {
                resultMode.Message = "身份证号有效性验证不通过";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            var cardModel = new IdCardNumber(model.CardNumber);
            var newModel = new SysIdCardContentModel
            {
                Id = model.Id,
                Age = cardModel.Age,
                CardNumber = cardModel.CardNumber,
                Name = model.Name,
                Remarks = model.Remarks,
                City = cardModel.City,
                Area = cardModel.Area,
                CreateTime = DateTime.Now,
                Province = cardModel.Province,
                Sex = cardModel.Sex == 1 ? SexEnum.Boy : SexEnum.Grill
            };
            var server = new SysIdCardContentService();

            if (model.Id > 0)
            {
                var oldModel = server.Get(model.Id);
                if (oldModel == null)
                {
                    resultMode.Message = "不存在该内容记录";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                newModel.CreateTime = oldModel.CreateTime;
            }
            else
            {
                var hadAdd = server.GetByCardNumber(model.CardNumber);
                if (hadAdd != null && hadAdd.CardNumber == model.CardNumber)
                {
                    resultMode.Message = "已经存在该身份证号码";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                server.SaveModel(newModel);
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Message = "处理成功";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                resultMode.Message = "处理失败";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="cardNumber"></param>
        /// <param name="provinceCode"></param>
        /// <param name="cityCode"></param>
        /// <param name="countyCode"></param>
        /// <returns></returns>
        public ActionResult GetList(int pageIndex, int pageSize = 0, string name = null, string cardNumber = null, string provinceCode = null, string cityCode = null, string countyCode = null)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            pageSize = pageSize < 1 ? PageSize : pageSize;
            var preCode = "";
            if (!string.IsNullOrEmpty(provinceCode) && provinceCode.Length > 2)
            {
                preCode += provinceCode.Substring(0, 2);
            }
            if (!string.IsNullOrEmpty(cityCode) && cityCode.Length > 4)
            {
                preCode += cityCode.Substring(2, 2);
            }
            preCode = string.IsNullOrEmpty(countyCode) ? preCode : countyCode;
            var dataList = new SysIdCardContentService().GetList(preCode, cardNumber, name, pageIndex, pageSize, out var count);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteId(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new SysIdCardContentService();
            try
            {
                menuServer.DelModel(new List<long> { id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
    }
}