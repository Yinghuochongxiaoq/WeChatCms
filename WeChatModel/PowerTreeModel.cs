using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeChatModel
{
    /// <summary>
    /// 权限树集合
    /// </summary>
    public class PowerTreeModel
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [JsonProperty("pId")]
        public int PId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        [JsonProperty("open")]
        public bool Open { get; set; }

        /// <summary>
        /// children
        /// </summary>
        [JsonProperty("children")]
        public List<PowerTreeModel> Children { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [JsonProperty("checked")]
        public bool Checked { get; set; }
    }
}
