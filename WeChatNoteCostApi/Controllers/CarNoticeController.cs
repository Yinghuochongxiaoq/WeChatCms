using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Enum;
using WeChatCmsCommon.EnumBusiness;
using WeChatModel;
using WeChatModel.DatabaseModel;
using WeChatNoteCostApi.WeChatInnerModel;
using WeChatService;

namespace WeChatNoteCostApi.Controllers
{
    public class CarNoticeController : ApiControllerBase
    {
        /// <summary>
        /// 服务
        /// </summary>
        private readonly CarNoticeService _carNoticeService = new CarNoticeService();

        private readonly SysDicService _dicServer = new SysDicService();

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="id">主键</param>
        /// <param name="nickName">名称</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> SaveCarConfigInfo(string token, long id, string nickName, string start,
            string end)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null || realNameModel.IsSuper != FlagEnum.HadOne)
            {
                resultMode.Message = "非管理员，禁止访问";
                return resultMode;
            }
            CarnoticeinfoModel model = new CarnoticeinfoModel();
            if (id > 0)
            {
                model = _carNoticeService.GetCarnoticeinfoModelById(id);
            }
            else
            {
                model.IsDel = FlagEnum.HadOne;
                model.Interimswich = FlagEnum.HadZore;
                model.Createtime = DateTime.Now;
                model.UserId = tempUserId.Value;
            }

            model.Id = id;
            model.Nickname = nickName;
            model.UpdateTime = DateTime.Now;
            model.Start = DateTime.ParseExact(start, "HH:mm", CultureInfo.CurrentCulture);
            model.End = DateTime.ParseExact(end, "HH:mm", CultureInfo.CurrentCulture);

            var newId = _carNoticeService.SaveCarnoticeinfoModel(model);
            model.Id = newId;

            var detailList = _carNoticeService.GetAfterNewCarNoticeDetailListByCarId(id, DateTime.Now);
            resultMode.Message = "保存成功";
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Data = new
            {
                model.Id,
                model.Nickname,
                start = model.Start.ToString("HH:mm"),
                end = model.End.ToString("HH:mm"),
                detailList = detailList.Select(f => new { f.Id, DailyDate = f.DailyDate.ToString("yyyy-MM-dd"), f.Timerange }).GroupBy(r => r.DailyDate).Select(e => new
                {
                    name = e.Key,
                    timeRange = e
                })
            };
            return resultMode;
        }

        /// <summary>
        /// 获取通知信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCarNotice(string token)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }

            var noticeList = _carNoticeService.GetAllCarNoticeInfoModels(DateTime.Now.AddDays(1));
            var carNoticeRealNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            resultMode.Data = new
            {
                noticeList,
                carNoticeRealNameModel
            };
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCarNoticeById(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var carConfigInfo = _carNoticeService.GetCarnoticeinfoModelById(id);
            if (carConfigInfo == null)
            {
                resultMode.Message = "配置查询失败，请刷新页面";
                return resultMode;
            }

            var detailList = _carNoticeService.GetAfterNewCarNoticeDetailListByCarId(id, DateTime.Now);
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Data = new
            {
                carConfigInfo.Id,
                carConfigInfo.Nickname,
                start = carConfigInfo.Start.ToString("HH:mm"),
                end = carConfigInfo.End.ToString("HH:mm"),
                detailList = detailList.Select(f => new { f.Id, DailyDate = f.DailyDate.ToString("yyyy-MM-dd"), f.Timerange }).GroupBy(r => r.DailyDate).Select(e => new
                {
                    name = e.Key,
                    timeRange = e
                })
            };
            resultMode.Message = "查询成功";
            return resultMode;
        }

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <param name="id">详细id</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="askCode">邀请码</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> AddCarNoticeDetail(string token, long id, string realName, string askCode)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }

            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null)
            {
                List<SysdictModel> askCodeList = _dicServer.GetAllDict("CarAskCode");
                if (askCodeList == null || askCodeList.Count < 1)
                {
                    resultMode.Message = "配置错误";
                    return resultMode;
                }

                if (string.IsNullOrEmpty(realName))
                {
                    resultMode.Message = "名字不能为空";
                    return resultMode;
                }

                if (askCodeList.All(f => f.Value != askCode))
                {
                    resultMode.Message = "邀请码错误";
                    return resultMode;
                }
                CarNoticeRealNameModel newModel = new CarNoticeRealNameModel
                {
                    AccountId = tempUserId.Value,
                    creattime = DateTime.Now,
                    IsDel = FlagEnum.HadZore,
                    IsSuper = FlagEnum.HadZore,
                    realname = realName.Trim()
                };
                _carNoticeService.AddCarNoticeRealName(newModel);
                realNameModel = newModel;
            }
            //验证配置
            var detailModel = _carNoticeService.GetCarnoticedetailModelById(id);
            if (detailModel == null || detailModel.IsDel == FlagEnum.HadOne)
            {
                resultMode.Message = "页面已失效，请刷新重新预约";
                return resultMode;
            }
            //验证是否已经被占用
            if (detailModel.Hadflag == FlagEnum.HadOne)
            {
                resultMode.Message = "已经被抢占了，请选择其他的时间";
                return resultMode;
            }
            //验证配置
            var carConfigInfo = _carNoticeService.GetCarnoticeinfoModelById(detailModel.Carid);
            if (carConfigInfo == null || carConfigInfo.IsDel == FlagEnum.HadOne)
            {
                resultMode.Message = "页面已失效，请刷新重新预约";
                return resultMode;
            }

            if (realNameModel.IsSuper != FlagEnum.HadOne)
            {
                //验证是否已经选了其他时间段了
                var selfDetailList = _carNoticeService.GetCarNoticeDetailListByCarIdAccount(tempUserId.Value, 0,
                    DateTime.Now.AddDays(1));
                if (selfDetailList != null && selfDetailList.Count > 0)
                {
                    resultMode.Message = "已经预约了其他时段，先取消再来吧";
                    return resultMode;
                }
                //验证时间
                if (carConfigInfo.Interimswich == FlagEnum.HadZore && (carConfigInfo.Start > DateTime.Now || carConfigInfo.End < DateTime.Now))
                {
                    resultMode.Message = "还没有到开通时间，请等待";
                    return resultMode;
                }
            }
            //处理添加
            detailModel.Hadflag = FlagEnum.HadOne;
            detailModel.Carnickname = userData.NickName;
            detailModel.Carrealname = realNameModel.realname;
            detailModel.Caruserheadimage = userData.AvatarUrl;
            detailModel.Caruserid = userData.AccountId;
            _carNoticeService.SaveCarNoticeDetail(detailModel);
            resultMode.Message = "预约成功";
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 保存详细时间配置信息
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="id">主键</param>
        /// <param name="rangeTime">时间段</param>
        /// <param name="carId">配置id</param>
        /// <param name="dayTime">时间天</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> SaveCarDetailInfo(string token, long id, string rangeTime, long carId, string dayTime)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null || realNameModel.IsSuper != FlagEnum.HadOne)
            {
                resultMode.Message = "非管理员，禁止访问";
                return resultMode;
            }
            CarnoticedetailModel model = new CarnoticedetailModel();
            if (id > 0)
            {
                model = _carNoticeService.GetCarnoticedetailModelById(id);
                if (model == null)
                {
                    resultMode.Message = "页面已过期，请刷新";
                    return resultMode;
                }
            }
            else
            {
                model.IsDel = FlagEnum.HadOne;
                model.Carid = carId;
                model.Createtime = DateTime.Now;
                model.UserId = tempUserId.Value;
                model.Hadflag = FlagEnum.HadZore;
                model.DailyDate = DateTime.ParseExact(dayTime, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                model.DailyDay = model.DailyDate.Day;
                model.DailyMonth = model.DailyDate.Month;
                model.DailyYear = model.DailyDate.Year;
            }


            model.Id = id;
            model.Timerange = rangeTime;
            model.UpdateTime = DateTime.Now;

            var newId = _carNoticeService.SaveCarNoticeDetail(model);
            model.Id = newId;
            resultMode.Message = "保存成功";
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 删除一个时间段
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> DeleteDetailInfoById(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null || realNameModel.IsSuper != FlagEnum.HadOne)
            {
                resultMode.Message = "非管理员，禁止访问";
                return resultMode;
            }
            if (id < 1)
            {
                resultMode.Message = "参数错误";
                return resultMode;
            }
            CarnoticedetailModel model = _carNoticeService.GetCarnoticedetailModelById(id);
            if (model == null)
            {
                resultMode.Message = "页面已过期，请刷新";
                return resultMode;
            }

            if (model.IsDel != FlagEnum.HadOne)
            {
                model.IsDel = FlagEnum.HadOne;
                _carNoticeService.SaveCarNoticeDetail(model);
            }

            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "处理成功";
            return resultMode;
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="token">用户token</param>
        /// <param name="id">记录主键</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> CancleCarnoticeDetail(string token, long id)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            var detailModel = _carNoticeService.GetCarnoticedetailModelById(id);
            if (detailModel == null)
            {
                resultMode.Message = "记录不存在";
                return resultMode;
            }
            if (detailModel.Caruserid != tempUserId && (realNameModel == null || realNameModel.IsSuper == FlagEnum.HadZore))
            {
                resultMode.Message = "不能修改其他数据";
                return resultMode;
            }
            //时间段验证
            var carConfigInfo = _carNoticeService.GetCarnoticeinfoModelById(detailModel.Carid);
            if (carConfigInfo == null || carConfigInfo.IsDel == FlagEnum.HadOne)
            {
                resultMode.Message = "页面已失效，请刷新重新操作";
                return resultMode;
            }
            if (carConfigInfo.Interimswich == FlagEnum.HadZore && (carConfigInfo.Start > DateTime.Now || carConfigInfo.End < DateTime.Now))
            {
                resultMode.Message = "该时段不可操作，请等待";
                return resultMode;
            }
            //处理添加
            detailModel.Hadflag = FlagEnum.HadZore;
            detailModel.Carnickname = "";
            detailModel.Carrealname = "";
            detailModel.Caruserheadimage = "";
            detailModel.Caruserid = 0;
            _carNoticeService.SaveCarNoticeDetail(detailModel);
            resultMode.Message = "取消成功";
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return resultMode;
        }

        /// <summary>
        /// 获取预约历史记录
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetCarHistory(string token)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var selfDetailList = _carNoticeService.GetCarNoticeDetailListByCarIdAccount(tempUserId.Value, 0,
                new DateTime());
            if (selfDetailList != null && selfDetailList.Count > 0)
            {
                selfDetailList = selfDetailList.OrderByDescending(f => f.DailyDate).ToList();
                List<CarnoticeinfoModel> carNoticeInfoModels = _carNoticeService.GetCarNoticeInfoById(selfDetailList.Select(r => r.Carid).ToList());
                var resultDate = new List<dynamic>();

                foreach (var t in selfDetailList)
                {
                    resultDate.Add(new
                    {
                        day = t.DailyDate.ToString("yyyy-MM-dd"),
                        t.Timerange,
                        carNoticeInfoModels.FirstOrDefault(r => r.Id == t.Carid)?.Nickname
                    });
                }

                resultMode.Data = resultDate;
            }

            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "获取成功";
            return resultMode;
        }

        /// <summary>
        /// 获取所有的配置信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> GetAllCarNoticeConfig(string token)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null || realNameModel.IsSuper != FlagEnum.HadOne)
            {
                resultMode.Message = "无访问权限";
                return resultMode;
            }

            var carConfigInfoList = _carNoticeService.GetAllCarNoticeInfoModels(new DateTime());
            var resultDate = new List<dynamic>();

            foreach (var t in carConfigInfoList)
            {
                resultDate.Add(new
                {
                    start = t.Start.ToString("HH:mm"),
                    end = t.End.ToString("HH:mm"),
                    t.Nickname,
                    t.Interimswich,
                    t.Id
                });
            }


            resultMode.Data = resultDate;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "获取成功";
            return resultMode;
        }

        /// <summary>
        /// 打开临时通道
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="id">主键</param>
        /// <param name="swich">开关</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseBaseModel<dynamic> SwichCarNoticeInterim(string token, long id, int swich)
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = ""
            };
            var userData = RedisCacheHelper.Get<WeChatAccountModel>(RedisCacheKey.AuthTokenKey + token);
            var tempUserId = userData?.AccountId;
            if (tempUserId == null || tempUserId < 1)
            {
                resultMode.Message = "登录失效，请重新登录";
                return resultMode;
            }
            var realNameModel = _carNoticeService.GetCarNoticeRealNameByAccountId(tempUserId);
            if (realNameModel == null || realNameModel.IsSuper != FlagEnum.HadOne)
            {
                resultMode.Message = "无访问权限";
                return resultMode;
            }

            var carConfigInfo = _carNoticeService.GetCarnoticeinfoModelById(id);
            if (carConfigInfo == null)
            {
                resultMode.Message = "配置查询失败，请刷新页面";
                return resultMode;
            }

            if (carConfigInfo.Interimswich.GetHashCode() == swich)
            {
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Message = "设置成功";
                return resultMode;
            }

            FlagEnum swichFlagEnum;
            if (EnumHelper.TryToEnum(swich, out swichFlagEnum))
            {
                carConfigInfo.Interimswich = swichFlagEnum;
                _carNoticeService.SaveCarnoticeinfoModel(carConfigInfo);
                resultMode.ResultCode = ResponceCodeEnum.Success;
                resultMode.Message = "设置成功";
                return resultMode;
            }

            resultMode.Message = "参数异常";
            return resultMode;

        }
    }
}