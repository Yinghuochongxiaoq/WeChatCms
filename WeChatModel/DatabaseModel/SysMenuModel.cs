using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class SysMenuModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 菜单（权限）类别
        /// </summary>
        public string MenuType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 是否已经删除
        /// </summary>
        public FlagEnum IsDel { get; set; }

        /// <summary>
        /// 子菜单实体
        /// </summary>
        public List<SysMenuModel> SubMenuModel { get; set; }
    }
}
