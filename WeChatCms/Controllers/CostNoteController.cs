using FreshCommonUtility.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using FreshCommonUtility.Enum;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    /// <summary>
    /// 消费记录信息
    /// </summary>
    [Permission(EnumBusinessPermission.CostNoteManager)]
    public class CostNoteController : AdminControllerBase
    {
        #region [1、消费记录]
        /// <summary>
        /// 消费记录首页
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取消费记录异步数据
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult GetCostPage()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            int pageIndex = System.Web.HttpContext.Current.GetIntFromParameters("pageindex");
            int pageSize = System.Web.HttpContext.Current.GetIntFromParameters("pagesize");
            string costaddress = System.Web.HttpContext.Current.GetStringFromParameters("costaddress");
            DateTime starttime = System.Web.HttpContext.Current.GetDateTimeFromParameters("starttime");
            DateTime endtime = System.Web.HttpContext.Current.GetDateTimeFromParameters("endtime");
            int costtype = System.Web.HttpContext.Current.GetIntFromParameters("costtype");
            int spendtype = System.Web.HttpContext.Current.GetIntFromParameters("spendtype");
            string costthing = System.Web.HttpContext.Current.GetStringFromParameters("costthing");
            long costchannel = System.Web.HttpContext.Current.GetIntFromParameters("costchannel");
            var userId = CurrentModel.UserId;
            if (userId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
            }
            else
            {
                var server = new CostContentService();
                var dataList = server.GetList(userId, spendtype, costaddress, costthing, costtype, costchannel, starttime, endtime,
                    pageIndex, pageSize, out var count);
                var dic = server.GetStatisticsCost(userId, spendtype, costaddress, costthing, costtype, costchannel,
                    starttime, endtime);
                var allOutCost = dic.ContainsKey(CostInOrOutEnum.Out.GetHashCode())
                    ? dic[CostInOrOutEnum.Out.GetHashCode()]
                    : 0;
                var allInCost = dic.ContainsKey(CostInOrOutEnum.In.GetHashCode())
                    ? dic[CostInOrOutEnum.In.GetHashCode()]
                    : 0;
                var statisticsModel = new
                {
                    allCouldCost = $"{allInCost - allOutCost:N2}",
                    allOutCost = $"{allOutCost:N2}",
                    allInCost = $"{allInCost:N2}"
                };
                resultMode.Data = new { count, dataList, statisticsModel };
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }

            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取消费类型
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult GetCostType(int spendType)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostTypeService();
            var userId = CurrentModel.UserId;
            var data = server.GetList(spendType, userId, 1, 100000, out _);
            resultMode.Data = data;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult GetCostChannel()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostChannelService();
            var userId = CurrentModel.UserId;
            var data = server.GetList(FlagEnum.HadOne.GetHashCode(), userId, 1, 100000, out _);
            resultMode.Data = data;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult AddCostInfo(CostContentModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (model == null)
            {
                resultMode.Message = "参数错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            var userId = CurrentModel.UserId;
            var server = new CostContentService();
            CostContentModel newModel = new CostContentModel();
            if (model.Id > 0)
            {
                newModel = server.GetContentModel(model.Id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "非法访问";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                if (newModel.SpendType == 2 && (model.SpendType != 2 || model.LinkCostId < 0))
                {
                    resultMode.Message = "转移记录类型不能修改或无关联入账信息";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                if (newModel.SpendType == 2 && newModel.CostInOrOut == CostInOrOutEnum.In)
                {
                    resultMode.Message = "转移记录入账信息不能修改";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }
            //验证参数
            newModel.Cost = Math.Round(Math.Abs(model.Cost), 2);
            newModel.CostAddress = model.CostAddress;
            newModel.CostChannel = model.CostChannel;
            newModel.CostType = model.CostType;
            newModel.UserId = userId;
            newModel.CostInOrOut = model.CostInOrOut;
            newModel.CostThing = model.CostThing;
            newModel.CostTime = model.CostTime;
            newModel.CostMonth = newModel.CostTime.Month;
            newModel.CostYear = newModel.CostTime.Year;
            newModel.CreateTime = newModel.Id > 0 ? newModel.CreateTime : DateTime.Now;
            newModel.CreateUserId = userId;
            newModel.SpendType = model.SpendType;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId;
            newModel.LinkCostChannel = model.LinkCostChannel;
            if (newModel.Cost < (decimal)0.01)
            {
                resultMode.Message = "金额设置错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            if (model.SpendType != 2)
            {
                var costTypeInfo = new CostTypeService().Get(newModel.CostType);
                if (costTypeInfo == null
                    || costTypeInfo.UserId != userId
                    || costTypeInfo.IsDel == FlagEnum.HadOne
                    || costTypeInfo.IsValid == FlagEnum.HadZore)
                {
                    resultMode.Message = "类型选择无效";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                newModel.CostTypeName = costTypeInfo.Name;
            }

            var costChannelServer = new CostChannelService();
            var costChannelInfo = costChannelServer.Get(newModel.CostChannel);
            if (costChannelInfo == null
                || costChannelInfo.UserId != userId
                || costChannelInfo.IsDel == FlagEnum.HadOne
                || costChannelInfo.IsValid == FlagEnum.HadZore)
            {
                resultMode.Message = "账户选择无效";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            newModel.CostChannelName = costChannelInfo.CostChannelName;
            newModel.CostChannelNo = costChannelInfo.CostChannelNo;
            newModel.CostInOrOut = newModel.SpendType == 1 ? CostInOrOutEnum.In : CostInOrOutEnum.Out;

            if (newModel.SpendType == 2)
            {
                if (newModel.LinkCostChannel < 1)
                {
                    resultMode.Message = "转入账户无效";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                var linkChannelInfo = costChannelServer.Get(newModel.LinkCostChannel);
                if (linkChannelInfo == null
                    || linkChannelInfo.UserId != userId
                    || linkChannelInfo.IsDel == FlagEnum.HadOne
                    || linkChannelInfo.IsValid == FlagEnum.HadZore)
                {
                    resultMode.Message = "转入账户无效";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                newModel.LinkCostChannelName = linkChannelInfo.CostChannelName;
                newModel.LinkCostChannelNo = linkChannelInfo.CostChannelNo;
            }
            else
            {
                newModel.LinkCostChannel = 0;
                newModel.LinkCostId = 0;
                newModel.LinkCostChannelNo = "";
                newModel.LinkCostChannelName = "";
            }
            var costContentServer = new CostContentService();
            costContentServer.AddAndUpdateContentInfo(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取记录信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostNoteList)]
        public ActionResult GetCostModel(long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostContentService();
            var userId = CurrentModel.UserId;
            var data = server.GetContentModel(id);
            if (data != null && data.UserId == userId)
            {
                resultMode.Data = data;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            resultMode.Message = "查询失败";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [2、账户设置]

        /// <summary>
        /// 账户设置
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostChannel)]
        public ActionResult CostChannelAccount()
        {
            return View();
        }

        /// <summary>
        /// 获取账户设置异步数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostChannel)]
        public ActionResult GetCostChannelPage(int pageIndex = 1, int pageSize = 10, string name = "", int isValid = -1)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userId = CurrentModel.UserId;
            if (userId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
            }
            else
            {
                var server = new CostChannelService();
                var dataList = server.GetList(isValid, userId, pageIndex, pageSize, out var count, name);
                resultMode.Data = new { count, dataList };
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }

            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostChannel)]
        public ActionResult GetCostChannelModel(long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostChannelService();
            var userId = CurrentModel.UserId;
            var data = server.Get(id);
            if (data != null && data.UserId == userId)
            {
                resultMode.Data = data;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            resultMode.Message = "查询失败";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="costChannelName"></param>
        /// <param name="costChannelNo"></param>
        /// <param name="sort"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostChannel)]
        public ActionResult SaveCostChannelInfo(long id, string costChannelName, string costChannelNo, int sort, int isValid)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (string.IsNullOrEmpty(costChannelName))
            {
                resultMode.Message = "名称不能为空";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            var userId = CurrentModel.UserId;
            var server = new CostChannelService();
            CostChannelModel newModel = new CostChannelModel();
            if (id > 0)
            {
                newModel = server.Get(id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "非法访问";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }
            //验证参数
            newModel.Id = id;
            newModel.IsDel = FlagEnum.HadZore;
            newModel.IsValid = EnumHelper.GetEnumByValue<FlagEnum>(isValid);
            newModel.CostChannelName = costChannelName;
            newModel.CostChannelNo = costChannelNo;
            newModel.UserId = userId;
            newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
            newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
            newModel.Sort = sort;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId;

            var costTypeInfoList = server.GetList(costChannelName, userId);
            if (costTypeInfoList != null
                && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
            {
                resultMode.Message = "账号名称已经存在";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            server.SaveModel(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult DelCostChannelModel(long id, int isValid)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var userId = CurrentModel.UserId;
            var server = new CostChannelService();
            var oldModel = server.Get(id);
            if (oldModel == null || oldModel.UserId != userId)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "参数错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            oldModel.IsDel = FlagEnum.HadZore;
            oldModel.IsValid = EnumHelper.GetEnumByValue<FlagEnum>(isValid);
            oldModel.UpdateUserId = userId;
            oldModel.UpdateTime = DateTime.Now;
            try
            {
                server.SaveModel(oldModel);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化账户信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult InitCostChannelModel()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var userId = CurrentModel.UserId;
            var server = new CostChannelService();
            var modelList = server.GetList(-1, userId, 1, 10, out _);
            if (modelList != null && modelList.Count > 0)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "已经初始化过";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            List<string> channelList = new List<string>
            {
                "现金账户","支付宝账户","微信账户"
            };
            int i = 1;
            foreach (var s in channelList)
            {
                var oldModel = new CostChannelModel
                {
                    IsDel = FlagEnum.HadZore,
                    IsValid = FlagEnum.HadOne,
                    UpdateUserId = userId,
                    UpdateTime = DateTime.Now,
                    CostChannelName = s,
                    CostChannelNo = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    Sort = i++,
                    UserId = userId
                };
                try
                {
                    server.SaveModel(oldModel);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }

            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "初始化成功";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [3、消费类型设置]

        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult CostTypeSet()
        {
            return View();
        }

        /// <summary>
        /// 获取消费类型设置异步数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="spendType"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult GetCostTypePage(int pageIndex = 1, int pageSize = 10, string name = "", int spendType = -1)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userId = CurrentModel.UserId;
            if (userId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
            }
            else
            {
                var server = new CostTypeService();
                var dataList = server.GetList(spendType, userId, pageIndex, pageSize, out var count, name);
                resultMode.Data = new { count, dataList };
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }

            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult GetCostTypeModel(long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var server = new CostTypeService();
            var userId = CurrentModel.UserId;
            var data = server.Get(id);
            if (data != null && data.UserId == userId)
            {
                resultMode.Data = data;
                resultMode.ResultCode = ResponceCodeEnum.Success;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            resultMode.Message = "查询失败";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <param name="spendType"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult SaveTypeInfo(long id, string name, int sort, int spendType)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            if (string.IsNullOrEmpty(name))
            {
                resultMode.Message = "名称不能为空";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            var userId = CurrentModel.UserId;
            var server = new CostTypeService();
            CostTypeModel newModel = new CostTypeModel();
            if (id > 0)
            {
                newModel = server.Get(id);
                //验证权限
                if (newModel == null || newModel.UserId != userId)
                {
                    resultMode.Message = "非法访问";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }
            //验证参数
            newModel.Id = id;
            newModel.SpendType = spendType;
            newModel.IsDel = FlagEnum.HadZore;
            newModel.IsValid = FlagEnum.HadOne;
            newModel.Name = name;
            newModel.UserId = userId;
            newModel.CreateTime = newModel.CreateTime < new DateTime(1900, 1, 1) ? DateTime.Now : newModel.CreateTime;
            newModel.CreateUserId = newModel.CreateUserId < 1 ? userId : newModel.CreateUserId;
            newModel.Sort = sort;
            newModel.UpdateTime = DateTime.Now;
            newModel.UpdateUserId = userId;

            var costTypeInfoList = server.GetList(spendType, userId, name);
            if (costTypeInfoList != null
                && (costTypeInfoList.Count > 1 || costTypeInfoList.Count == 1 && costTypeInfoList[0].Id != id))
            {
                resultMode.Message = "类型名称已经存在";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            server.SaveModel(newModel);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult InvalidCostTypeModel(long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var userId = CurrentModel.UserId;
            var server = new CostTypeService();
            var oldModel = server.Get(id);
            if (oldModel == null || oldModel.UserId != userId)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "参数错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            oldModel.IsDel = FlagEnum.HadOne;
            oldModel.IsValid = FlagEnum.HadZore;
            oldModel.UpdateUserId = userId;
            oldModel.UpdateTime = DateTime.Now;
            try
            {
                server.SaveModel(oldModel);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化消费类型
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostTypePage)]
        public ActionResult InitCostTypeModel()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var userId = CurrentModel.UserId;
            var server = new CostTypeService();
            var modelList = server.GetList(-1, userId, 1, 10, out _);
            if (modelList != null && modelList.Count > 0)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "已经初始化过";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            List<string> outTypeList = new List<string>
            {
                "餐饮","乘车","旅游","服饰","奢侈品","送礼","外借","取现","住宿","充话费","水电气费","物管费","发红包"
            };
            List<string> inTypeList = new List<string>
            {
                "工资","归还","结余","收红包"
            };
            int i = 1;
            foreach (var s in outTypeList)
            {
                var oldModel = new CostTypeModel()
                {
                    IsDel = FlagEnum.HadZore,
                    IsValid = FlagEnum.HadOne,
                    UpdateUserId = userId,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    Sort = i++,
                    UserId = userId,
                    Name = s,
                    SpendType = CostInOrOutEnum.Out.GetHashCode()
                };
                try
                {
                    server.SaveModel(oldModel);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
            foreach (var s in inTypeList)
            {
                var oldModel = new CostTypeModel()
                {
                    IsDel = FlagEnum.HadZore,
                    IsValid = FlagEnum.HadOne,
                    UpdateUserId = userId,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    Sort = i++,
                    UserId = userId,
                    Name = s,
                    SpendType = CostInOrOutEnum.In.GetHashCode()
                };
                try
                {
                    server.SaveModel(oldModel);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "初始化成功";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [4、统计消费情况]

        /// <summary>
        /// 统计消费情况
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.CostStatistical)]
        public ActionResult CostStatistical()
        {
            return View();
        }

        #endregion
    }
}