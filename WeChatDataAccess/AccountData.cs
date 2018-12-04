using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatCmsCommon.Unit;
using WeChatModel;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    public class AccountData
    {
        private readonly string CacheMenuListKey = "LoginInfo.";

        #region [1、登录处理]
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginInfoModel UserLogin(string userName, string password)
        {
            var userInfo = new LoginInfoModel();
            List<SysUser> userList;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                userList = conn.GetList<SysUser>(new { UserName = userName, IsDel = FlagEnum.HadZore.GetHashCode(), Password = password })?.ToList();
            }
            if (userList == null || userList.Count < 1)
            {
                userInfo.IsLogin = false;
                userInfo.ErrMessage = "用户名或密码错误";
                return userInfo;
            }

            var u = userList.First();
            userInfo.ClientIp = Fetch.UserIp;

            userInfo.IsLogin = true;
            userInfo.Id = u.Id;
            userInfo.UserId = u.Id;
            userInfo.LoginName = userName;
            userInfo.TrueName = u.TrueName;
            userInfo.DepartName = string.Empty;
            userInfo.JobNumber = string.Empty;
            return userInfo;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public LoginInfoModel GetUserInfo(LoginInfoModel userInfo)
        {
            if (userInfo == null || userInfo.Id < 1)
            {
                return null;
            }
            var key = CacheMenuListKey + userInfo.Id;
            var model = RedisCacheHelper.Get<LoginInfoModel>(key);
            if (model != null)
            {
                return model;
            }
            List<int> authorityList = new List<int>();
            userInfo.MenuList = GetMenuList(userInfo.UserId, ref authorityList);
            if (authorityList != null && authorityList.Any())
            {
                userInfo.BusinessPermissionList = authorityList.Select(p => p).Cast<EnumBusinessPermission>().ToList();
            }
            else
            {
                userInfo.BusinessPermissionList = new List<EnumBusinessPermission>();
            }
            if (userInfo.MenuList == null)
            {
                userInfo.MenuList = new List<SysMenuModel>();
            }

            RedisCacheHelper.AddSet(CacheMenuListKey + userInfo.Id, userInfo, new TimeSpan(0, 0, 10));
            return userInfo;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        public void DelUserModel(int id)
        {
            if (id < 1) return;
            var model = GetSysUser(id);
            if (model == null) return;
            model.IsDel = FlagEnum.HadOne;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Update(model);
                conn.Execute("update sysusermenu set IsDel=@Del where UserId=@UserId and IsDel=@NotDel",
                    new
                    {
                        Del = FlagEnum.HadOne.GetHashCode(),
                        NotDel = FlagEnum.HadZore.GetHashCode(),
                        UserId = id
                    });
            }
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityList"></param>
        /// <returns></returns>
        public List<SysMenuModel> GetMenuList(int userId, ref List<int> authorityList)
        {
            if (userId < 1)
            {
                return null;
            }

            var sysUserMenus = new List<SysMenuModel>();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var userMenus = conn.GetList<Sysusermenu>(new { IsDel = FlagEnum.HadZore.GetHashCode(), UserId = userId })?.ToList();
                if (userMenus != null && userMenus.Any())
                {
                    sysUserMenus = conn.GetList<SysMenuModel>("where id in @Id and IsDel=@IsDel", new { Id = userMenus.Select(item => item.MenuId).ToArray(), IsDel = FlagEnum.HadZore.GetHashCode() })?.ToList();
                }
            }

            var resultMemuList = new List<SysMenuModel>();
            if (sysUserMenus != null && sysUserMenus.Any())
            {
                var ids = sysUserMenus.Select(f => f.Id).ToList();
                var dictionMenu = sysUserMenus.ToDictionary(f => f.Id, f => f);
                foreach (var r in sysUserMenus)
                {
                    authorityList.Add(DataTypeConvertHelper.ToInt(r.MenuType));
                    if (r.ParentId < 1)
                    {
                        resultMemuList.Add(r);
                    }

                    else if (ids.Contains(r.ParentId))
                    {
                        if (dictionMenu[r.ParentId].SubMenuModel == null)
                        {
                            dictionMenu[r.ParentId].SubMenuModel = new List<SysMenuModel>();
                        }
                        dictionMenu[r.ParentId].SubMenuModel.Add(r);
                    }
                    else
                    {
                        resultMemuList.Add(r);
                    }
                }
            }
            return resultMemuList;
        }
        #endregion

        #region [2、用户处理]

        /// <summary>
        /// 获取列表用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysUser> GetSysUsers(int pageIndex, int pageSize)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysUser>(pageIndex, pageSize, " where IsDel=@IsDel ", null,
                    new { IsDel = FlagEnum.HadZore.GetHashCode() })?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetSysUserCount()
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysUser>(new { IsDel = FlagEnum.HadZore.GetHashCode() });
            }
        }

        /// <summary>
        /// 获取单个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysUser GetSysUser(int id)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<SysUser>(id);
            }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model"></param>
        public void SaveUserModel(SysUser model)
        {
            if (model == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (model.Id > 0)
                {
                    conn.Update(model);
                }
                else
                {
                    conn.Insert(model);
                }
            }
        }

        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SysUser> GetSysUsersByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<SysUser>(new { UserName = userName })?.ToList();
            }
        }
        #endregion
    }
}
