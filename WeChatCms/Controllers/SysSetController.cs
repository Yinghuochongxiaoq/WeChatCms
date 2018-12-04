using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreshCommonUtility.Security;
using Newtonsoft.Json;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatService;

namespace WeChatCms.Controllers
{
    [Permission(EnumBusinessPermission.SysSetManage)]
    public class SysSetController : AdminControllerBase
    {
        #region [1.菜单权限设置]
        /// <summary>
        /// 菜单权限设置
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuSet)]
        public ActionResult MenuSet()
        {
            return View();
        }

        /// <summary>
        /// 获取个人权限集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuSet)]
        public string GetUserPowerList(int id)
        {
            var resultData = new MenuService().GetUserPowerListJson(id);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = resultData
            };
            return JsonConvert.SerializeObject(resultMode);
        }

        /// <summary>
        /// 保存权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listIds"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuSet)]
        public ActionResult SaveUserPower(int id, List<int> listIds)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
            };
            if (id < 1 || listIds == null || listIds.Count < 1)
            {
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                resultMode.Message = "参数错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            new MenuService().SaveUserPower(id, listIds);
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [2.菜单管理]
        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult MenuAdmin()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult GetMenuList(int pageIndex, int pageSize = 0)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            pageSize = pageSize < 1 ? PageSize : pageSize;
            var menuList = new MenuService().GetMenuList(pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, menuList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult SaveMenuInfo(SysMenuModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new MenuService();
            if (model != null && model.Id > 0)
            {
                var tempModel = menuServer.GetMenuModel(model.Id);
                if (tempModel == null)
                {
                    resultMode.Message = "该菜单已经被删除";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                menuServer.SaveMenuModel(model);
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
        /// 获取所有的菜单项
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult GetAllMenuList()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new MenuService();
            var list = menuServer.GetAllMenuModels();
            resultMode.Data = list?.Select(f => new { f.Id, f.Title });
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult GetMenuModel(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new MenuService();
            var data = menuServer.GetMenuModel(id);
            resultMode.Data = data;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuAdmin)]
        public ActionResult DelMenuModel(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var menuServer = new MenuService();
            try
            {
                menuServer.DelMenuModel(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [3.管理员列表]
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult MenuUsers()
        {
            return View();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult GetUserList(int pageIndex, int pageSize = 0)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            pageSize = pageSize < 1 ? PageSize : pageSize;
            var dataList = new AccountService().GetSysUsers(pageIndex, pageSize, out var count);
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功",
                Data = new { count, dataList }
            };
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取单个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult GetUserModel(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new AccountService();
            var data = server.GetSysUser(id);
            resultMode.Data = data;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult SaveDataInfo(SysUser model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new AccountService();
            var saveModel = new SysUser();
            if (model == null)
            {
                Debug.WriteLine("请求参数为空");
                resultMode.Message = "保存失败";
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            if (model.Id > 0)
            {
                saveModel = server.GetSysUser(model.Id);
                if (saveModel == null)
                {
                    resultMode.Message = "该菜单已经被删除";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                if (saveModel.UserName != model.UserName)
                {
                    resultMode.Message = "登录名不允许修改";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var list = server.GetSysUsersByUserName(model.UserName);
                if (list != null && list.Count > 0)
                {
                    resultMode.Message = "用户名已经被占用";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                saveModel.CreateAuth = CurrentModel.Id;
                saveModel.CreateTime = DateTime.Now;
                saveModel.Password = AesHelper.AesEncrypt("123456");
            }

            saveModel.Id = model.Id;
            saveModel.Birthday = model.Birthday;
            saveModel.Sex = model.Sex;
            saveModel.TelPhone = model.TelPhone;
            saveModel.IsDel = FlagEnum.HadZore;
            saveModel.TrueName = model.TrueName;
            saveModel.UserType = model.UserType;
            saveModel.UpdateAuth = CurrentModel.Id;
            saveModel.UpdateTime = DateTime.Now;
            saveModel.UserName = model.UserName;
            try
            {
                server.SaveUserModel(saveModel);
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
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult DelModel(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var server = new AccountService();
            try
            {
                if (id == CurrentModel.Id)
                {
                    Trace.WriteLine("存在自杀现象");
                    resultMode.Message = "保存失败,不能自己删除自己";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                server.DelUserModel(id);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(EnumBusinessPermission.MenuUsers)]
        public ActionResult ResetPassword(int id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Success,
                Message = "响应成功"
            };
            var newPassword = AesHelper.AesEncrypt("123456");
            var server = new AccountService();
            try
            {
                var currentModel = server.GetSysUser(CurrentModel.Id);
                if (currentModel == null)
                {
                    resultMode.Message = "登录超时，请退出重新登录";
                    resultMode.ResultCode = ResponceCodeEnum.Fail;
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                if (currentModel.UserType == UserTypeEnum.SuperAdmin ||
                    currentModel.UserType == UserTypeEnum.UsuallyAdmin)
                {
                    var resetUser = server.GetSysUser(id);
                    if (resetUser == null)
                    {
                        resultMode.Message = "用户无效";
                        resultMode.ResultCode = ResponceCodeEnum.Fail;
                        return Json(resultMode, JsonRequestBehavior.AllowGet);
                    }

                    resetUser.Password = newPassword;
                    server.SaveUserModel(resetUser);
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                resultMode.Message = "还不是管理员，不能重置密码";
                resultMode.ResultCode = ResponceCodeEnum.Fail;
                return Json(resultMode, JsonRequestBehavior.AllowGet);
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