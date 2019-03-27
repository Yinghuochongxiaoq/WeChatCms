using System;
using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel.DatabaseModel;

namespace WeChatModel
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class LoginInfoModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string LoginName { get; set; }

        public string TrueName { get; set; }

        public DateTime LastAccessTime { get; set; }

        public string ClientIp { get; set; }

        public bool IsLogin { get; set; }
        public string ErrMessage { get; set; }

        /// <summary>
        /// 用户拥有的菜单权限
        /// </summary>
        private List<SysMenuModel> _menuList = new List<SysMenuModel>();
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<SysMenuModel> MenuList
        {
            get { return _menuList; }
            set { _menuList = value; }
        }
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<EnumBusinessPermission> BusinessPermissionList { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// HeadUrl 头像地址
        /// </summary>
        public string HeadUrl { get; set; }
    }
}
