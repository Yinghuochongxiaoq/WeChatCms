/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using WeChatCmsCommon.EnumBusiness;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// humanstoryresource table in MySQL5
    /// </summary>
    [Table("humanstoryresource")]
    public class HumanstoryresourceModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// Type 媒体类型：image,video
        /// </summary>
        public String Type { get; set; }
        /// <summary>
        /// Size 大小，字节
        /// </summary>
        public Int64 Size { get; set; }
        /// <summary>
        /// TempFilePath 路径
        /// </summary>
        public String TempFilePath { get; set; }
        /// <summary>
        /// Duration 码率
        /// </summary>
        public String Duration { get; set; }
        /// <summary>
        /// Height 高度
        /// </summary>
        public Int32 Height { get; set; }
        /// <summary>
        /// Width 宽度
        /// </summary>
        public String Width { get; set; }
        /// <summary>
        /// ThumbTempFilePath 视频缩略图
        /// </summary>
        public String ThumbTempFilePath { get; set; }
        /// <summary>
        /// CreateBy 创建人
        /// </summary>
        public Int64 CreateBy { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Sort 排序值
        /// </summary>
        public Int32 Sort { get; set; }
        /// <summary>
        /// StoryDetailId 关联记录信息
        /// </summary>
        public Int64 StoryDetailId { get; set; }
        /// <summary>
        /// IsDel 1:启用；0：删除
        /// </summary>
        public FlagEnum IsDel { get; set; }
    }
}