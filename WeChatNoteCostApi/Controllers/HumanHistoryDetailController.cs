using System;
using System.Linq;
using System.Web.Http;
using FreshCommonUtility.Cache;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatNoteCostApi.WeChatInnerModel;
using WeChatService;

namespace WeChatNoteCostApi.Controllers
{
    public class HumanHistoryDetailController : ApiControllerBase
    {
        #region [1、编辑历史记录]
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseBaseModel<dynamic> AddStoryDetail(HumanstorydetailModel model)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + model.Token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }

            if (string.IsNullOrEmpty(model.Content) && !model.MediaList.Any())
            {
                resultMode.Message = "参数错误";
                return resultMode;
            }
            var userId = tempUserId;
            var server = new HumanHistoryDetailService();
            HumanstorydetailModel newModel = new HumanstorydetailModel
            {
                CreateBy = userId.Value,
                Address = model.Address,
                Content = model.Content,
                CreateTime = DateTime.Now
            };
            long id = server.SaveModel(newModel);
            if (id > 0 && model.MediaList.Any())
            {
                var index = 0;
                model.MediaList.ForEach(f =>
                {
                    var tmp = new HumanstoryresourceModel
                    {
                        CreateBy = userId.Value,
                        CreateTime = DateTime.Now,
                        Duration = f.Duration,
                        Height = f.Height,
                        Size = f.Size,
                        Sort = index++,
                        StoryDetailId = id,
                        TempFilePath = f.TempFilePath,
                        ThumbTempFilePath = f.ThumbTempFilePath,
                        Type = f.Type,
                        Width = f.Width
                    };
                    new HumanHistoryResourceService().SaveModel(tmp);
                });
            }

            resultMode.Data = id;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }
        #endregion

        #region [2、获取历史数据]
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ResponseBaseModel<dynamic> GetStoryPage(string token, int pageIndex = 1, int pageSize = 10)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            var userData = string.IsNullOrEmpty(token) ? null : RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;

            int count;
            var server = new HumanHistoryDetailService();
            var resourceServer = new HumanHistoryResourceService();
            var wechatAccountserver = new WechatAccountService();
            //获取详情数据
            var dataList = server.GetList(pageIndex, pageSize, out count);
            if (dataList != null && dataList.Count > 0)
            {
                //获取资源
                var detailIdList = dataList.Select(f => f.Id).ToList();
                var resourceList = resourceServer.GetHumanstoryresourceModels(detailIdList);
                //获取用户信息
                var accountIdList = dataList.Select(h => h.CreateBy).ToList();
                var accountList = wechatAccountserver.GetByAccountIds(accountIdList);

                if (resourceList != null && resourceList.Count > 0 || accountList != null && accountList.Count > 0)
                {
                    dataList.ForEach(r =>
                    {
                        var tmpList = resourceList?
                            .Where(e => e.StoryDetailId == r.Id)
                            .OrderBy(s => s.Sort)
                            .ToList();
                        var tmpAccountList = accountList?
                            .FirstOrDefault(h => h.AccountId == r.CreateBy);
                        r.MediaList = tmpList;
                        r.NikeName = tmpAccountList?.NickName;
                        r.HeadImageUrl = tmpAccountList?.AvatarUrl;
                    });
                }
            }
            resultMode.Data = new { pageCount = count / pageSize + (count % pageSize > 0 ? 1 : 0), dataList };
            resultMode.ResultCode = ResponceCodeEnum.Success;


            return resultMode;
        }
        #endregion
    }
}