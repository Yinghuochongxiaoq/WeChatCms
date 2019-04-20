using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class MenuData
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysMenuModel> GetMenuModels(int pageIndex, int pageSize)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysMenuModel>(pageIndex, pageSize, " where IsDel=@IsDel ", null,
                    new { IsDel = FlagEnum.HadZore.GetHashCode() })?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetMenuCount()
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysMenuModel>(new { IsDel = FlagEnum.HadZore.GetHashCode() });
            }
        }

        /// <summary>
        /// 获取单个的菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMenuModel GetMenuModel(int id)
        {
            if (id < 1)
            {
                return null;
            }

            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<SysMenuModel>(id);
            }
        }

        /// <summary>
        /// 获取所有的菜单信息
        /// </summary>
        /// <returns></returns>
        public List<SysMenuModel> GetAllMenuModels()
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<SysMenuModel>("where IsDel=@IsDel", new { IsDel = FlagEnum.HadZore.GetHashCode() })?.OrderBy(f => f.OrderNo).ToList();
            }
        }

        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="sysMenuModel"></param>
        public void SaveMenuModel(SysMenuModel sysMenuModel)
        {
            if (sysMenuModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (sysMenuModel.Id > 0)
                {
                    conn.Update(sysMenuModel);
                }
                else
                {
                    conn.Insert(sysMenuModel);
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DelMenuModel(int id)
        {
            if (id < 1) return;
            var model = GetMenuModel(id);
            if (model != null)
            {
                model.IsDel = FlagEnum.HadOne;
                using (var conn = SqlConnectionHelper.GetOpenConnection())
                {
                    conn.Update(model);
                    conn.Execute("update sysusermenu set IsDel=@Del where MenuId=@MenuId and IsDel=@NotDel",
                        new
                        {
                            Del = FlagEnum.HadOne.GetHashCode(),
                            NotDel = FlagEnum.HadZore.GetHashCode(),
                            MenuId = id
                        });
                }
            }

        }

        /// <summary>
        /// 获取多层级菜单
        /// </summary>
        /// <returns></returns>
        public List<SysMenuModel> GetAllMenuListTree()
        {
            var sysUserMenus = GetAllMenuModels();
            var resultMemuList = new List<SysMenuModel>();
            if (sysUserMenus != null && sysUserMenus.Any())
            {
                var ids = sysUserMenus.Select(f => f.Id).ToList();
                var dictionMenu = sysUserMenus.ToDictionary(f => f.Id, f => f);
                foreach (var r in sysUserMenus)
                {
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

        /// <summary>
        /// 获取个人权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sysusermenu> GetPowersById(int userId)
        {
            if (userId < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var userMenus = conn.GetList<Sysusermenu>(new { IsDel = FlagEnum.HadZore.GetHashCode(), UserId = userId })?.ToList();
                return userMenus;
            }
        }

        /// <summary>
        /// 批量更新个人权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="powerIds"></param>
        public void UpdateUserPowers(int userId, List<int> powerIds)
        {
            if (userId < 1 || powerIds == null || powerIds.Count < 1) return;
            var powerList = new List<Sysusermenu>();
            powerIds.ForEach(f =>
            {
                var tempPower = new Sysusermenu { UserId = userId, IsDel = FlagEnum.HadZore.GetHashCode(), MenuId = f };
                powerList.Add(tempPower);
            });
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var delOldPower = "update sysusermenu set IsDel=@IsDel where UserId=@UserId";
                        conn.Execute(delOldPower, new { IsDel = FlagEnum.HadOne.GetHashCode(), UserId = userId },
                            transaction);
                        var addNewPower = "INSERT into sysusermenu(UserId,MenuId,Isdel) VALUES(@UserId,@MenuId,@Isdel)";
                        conn.Execute(addNewPower, powerList, transaction);
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        //回滚事务
                        transaction.Rollback();
                        conn.Close();
                        conn.Dispose();
                        Trace.WriteLine(e);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }
    }
}
