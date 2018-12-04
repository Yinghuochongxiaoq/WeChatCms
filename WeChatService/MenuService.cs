using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeChatDataAccess;
using WeChatModel;
using WeChatModel.DatabaseModel;


namespace WeChatService
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysMenuModel> GetMenuList(int indexPage, int pageSize, out int count)
        {
            var dataAccess = new MenuData();
            count = dataAccess.GetMenuCount();
            return dataAccess.GetMenuModels(indexPage, pageSize);
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

            return new MenuData().GetMenuModel(id);
        }

        /// <summary>
        /// 获取所有的菜单信息
        /// </summary>
        /// <returns></returns>
        public List<SysMenuModel> GetAllMenuModels()
        {
            return new MenuData().GetAllMenuModels();
        }

        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="sysMenuModel"></param>
        public void SaveMenuModel(SysMenuModel sysMenuModel)
        {
            if (sysMenuModel == null) return;
            new MenuData().SaveMenuModel(sysMenuModel);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DelMenuModel(int id)
        {
            if (id < 1)
            {
                return;
            }
            new MenuData().DelMenuModel(id);
        }

        /// <summary>
        /// 获取个人权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PowerTreeModel> GetUserPowerListJson(int userId)
        {
            var dataAccess = new MenuData();
            var userPower = dataAccess.GetPowersById(userId);
            var allMenu = dataAccess.GetAllMenuListTree();
            var userPowerDictionary = new Dictionary<int, int>();
            if (userPower != null && userPower.Count > 0)
            {
                userPowerDictionary = userPower.ToDictionary(f => f.MenuId, r => r.MenuId);
            }
            return GetMenuToJObject(allMenu, userPowerDictionary);
        }

        /// <summary>
        /// 递归函数 
        /// [{ id: 1, pId: 0, name: "test 1", open: false, checked: true },{ id: 1, pId: 0, name: "test 1", open: false, checked: true }]
        /// </summary>
        /// <param name="itemMemu"></param>
        /// <param name="userPowerDictionary"></param>
        /// <returns></returns>
        private List<PowerTreeModel> GetMenuToJObject(List<SysMenuModel> itemMemu, Dictionary<int, int> userPowerDictionary)
        {
            var resultData = new List<PowerTreeModel>();
            itemMemu.ForEach(e =>
            {
                var tempObject = new PowerTreeModel
                {
                    Id = e.Id,
                    PId = e.ParentId,
                    Name = e.Title,
                    Open = true,
                    Children = new List<PowerTreeModel>()
                };
                tempObject.Children = e.SubMenuModel != null && e.SubMenuModel.Count > 0 ? GetMenuToJObject(e.SubMenuModel, userPowerDictionary) : new List<PowerTreeModel>();
                tempObject.Checked = userPowerDictionary.ContainsKey(e.Id) || (tempObject.Children?.Any(f => f.Checked) ?? false);
                resultData.Add(tempObject);
            });
            return resultData;
        }

        /// <summary>
        /// 保存权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listIds"></param>
        public void SaveUserPower(int id, List<int> listIds)
        {
            if (id < 1 || listIds == null || listIds.Count < 1) return;
            var model = new AccountService().GetSysUser(id);
            if (model == null) return;
            listIds = listIds.Distinct().ToList();
            new MenuData().UpdateUserPowers(id, listIds);
        }
    }
}
