using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using TinifyNet;
using WeChatCmsCommon.EnumBusiness;
using WeChatCmsCommon.Unit;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class DailyStoryResourceService
    {
        private readonly DailyStoryResourceData _dataAccess = new DailyStoryResourceData();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public long SaveModel(DailyStoryResourceModel saveModel)
        {
            return _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 获取指定记录的资源集合
        /// </summary>
        /// <param name="detailIdList"></param>
        /// <returns></returns>
        public List<DailyStoryResourceModel> GetDailyStoryResourceModels(List<long> detailIdList) => _dataAccess.GetDailyStoryResourceModels(detailIdList);

        /// <summary>
        /// 根据关联id删除资源
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public int DeleteResourceByStoryId(long storyId) => _dataAccess.DeleteResourceByStoryId(storyId);

        /// <summary>
        /// 根据id删除资源
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteResourceById(List<long> ids) => _dataAccess.DeleteResourceById(ids);

        /// <summary>
        /// 处理新上传的图片资源
        /// </summary>
        /// <param name="idList"></param>
        public async Task DealNewResourceAsync(List<long> idList)
        {
            if (idList.Count < 1) return;
            var resourceList = _dataAccess.GetDailyStoryResourceModelsById(idList);
            if (resourceList.Count < 1) return;
            var dictServer = new SysDicService();
            var inUserService = new InuseKeyInfoService();
            var dictList = dictServer.GetAllDict("FtpConfig");
            if (dictList == null || dictList.Count < 1) return;
            var ftpUrl = dictList.FirstOrDefault(f => f.Lable == "FtpUrl")?.Value;
            var ftpUserName = dictList.FirstOrDefault(f => f.Lable == "FtpUserName")?.Value;
            var ftpPassword = dictList.FirstOrDefault(f => f.Lable == "FtpPassword")?.Value;
            var ftpViewPre = dictList.FirstOrDefault(f => f.Lable == "FtpViewPre")?.Value;
            var tinifyCodeList = dictServer.GetAllDict("TinifyKey")?.OrderBy(f => f.Sort).ToList();
            if (tinifyCodeList == null || tinifyCodeList.Count < 1) return;
            var newTime = DateTime.Now;
            var tiniCodeUseLimit = 500;
            var inUseModel = inUserService.GetModels("TinifyKey", null, newTime.Year, newTime.Month)?.FirstOrDefault(f => f.UseCount < tiniCodeUseLimit);
            if (inUseModel == null || tinifyCodeList.All(f => f.Value != inUseModel.KeyInfo))
            {
                inUseModel = new InusekeyinfoModel { CreateTime = newTime, IsDel = FlagEnum.HadZore, KeyInfo = tinifyCodeList[0].Value, KeyType = "TinifyKey", UseCount = 0, UseDate = newTime };
                inUserService.SaveModel(inUseModel);
                if (inUseModel.Id < 1)
                {
                    Console.WriteLine("保存失败");
                    return;
                }
            }
            foreach (var model in tinifyCodeList)
            {
                if (model.Value == inUseModel.KeyInfo)
                {
                    model.IsDel = FlagEnum.HadOne.GetHashCode();
                }
            }
            Tinify.Key = inUseModel.KeyInfo;
            var ftpServer = new FtpUpLoadFiles(ftpUrl, ftpUserName, ftpPassword);
            var allowExtensions = new List<string> { ".jpeg", ".png",".jpg" };
            foreach (var f in resourceList)
            {
                //压缩图片
                if (string.IsNullOrEmpty(f.Url)) continue;
                var tempPath = HostingEnvironment.MapPath(f.Url);
                if (string.IsNullOrEmpty(tempPath)) continue;
                var fileExtension = Path.GetExtension(tempPath)?.ToLower();
                if (fileExtension == null) continue;
                var sourceData = File.ReadAllBytes(tempPath);
                byte[] resultData = null;
                //判断文件扩展名是否能够进行压缩
                var couldCompress = allowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
                if (couldCompress)
                {
                    try
                    {
                        resultData = await Tinify.FromBuffer(sourceData).ToBuffer();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }
                    //该code已经用完了
                    if (Tinify.CompressionCount >= tiniCodeUseLimit)
                    {
                        //变更原有的状态
                        inUseModel.IsDel = FlagEnum.HadOne;
                        inUseModel.UseCount = (int)Tinify.CompressionCount;
                        inUserService.SaveModel(inUseModel);

                        //重新生成
                        var tempKey = tinifyCodeList.FirstOrDefault(r => r.IsDel == FlagEnum.HadZore.GetHashCode());
                        if (tempKey == null)
                        {
                            continue;
                        }
                        tempKey.IsDel = FlagEnum.HadOne.GetHashCode();
                        inUseModel = new InusekeyinfoModel { CreateTime = newTime, IsDel = FlagEnum.HadZore, KeyInfo = tempKey.Value, KeyType = "TinifyKey", UseCount = 0, UseDate = newTime };
                        inUserService.SaveModel(inUseModel);
                    }
                    else
                    {
                        inUseModel.UseCount = (int)(Tinify.CompressionCount ?? 0);
                        inUserService.SaveModel(inUseModel);
                    }
                }
                //上传
                var indexOfName = f.Url.LastIndexOf("/", StringComparison.Ordinal);
                if (indexOfName < 0) continue;
                var fileName = f.Url.Substring(indexOfName);
                var transferFinish = ftpServer.UploadFile("", couldCompress ? resultData : sourceData, fileName);
                if (transferFinish)
                {
                    //上传完成
                    f.FullUrl = ftpViewPre + fileName;
                    SaveModel(f);
                }
            }
        }
    }
}
