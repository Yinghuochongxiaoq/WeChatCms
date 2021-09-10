using System;
using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CarNoticeService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CarNoticeInfoData _dataAccess = new CarNoticeInfoData();
        private CarNoticeDetailData _carNoticeDetailData = new CarNoticeDetailData();
        private CarNoticeRealNameData _carNoticeRealNameData = new CarNoticeRealNameData();

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="carInfoId">配置主键</param>
        /// <returns></returns>
        public CarnoticeinfoModel GetCarnoticeinfoModelById(long carInfoId)
        {
            CarnoticeinfoModel resultModel = _dataAccess.Get(carInfoId);
            DateTime nowTime = DateTime.Now;

            resultModel.Start = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, resultModel.Start.Hour, resultModel.Start.Minute, resultModel.Start.Second);
            resultModel.End = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, resultModel.End.Hour, resultModel.End.Minute, resultModel.End.Second);
            return resultModel;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long SaveCarnoticeinfoModel(CarnoticeinfoModel model)
        {
            return _dataAccess.SaveModel(model);
        }

        /// <summary>
        /// 获取所有的配置信息
        /// </summary>
        /// <returns></returns>
        public List<CarnoticeinfoModel> GetAllCarNoticeInfoModels(DateTime searchTime)
        {
            List<CarnoticeinfoModel> resultList = _dataAccess.GetAllCarNoticeInfoModels();
            if (resultList != null && resultList.Count > 0)
            {
                DateTime nowTime = DateTime.Now;
                resultList.ForEach(f =>
                {
                    f.Start = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, f.Start.Hour, f.Start.Minute, f.Start.Second);
                    f.End = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, f.End.Hour, f.End.Minute, f.End.Second);
                    if (searchTime > new DateTime(1970, 1, 1))
                    {
                        f.DateTimeRange = _carNoticeDetailData.GetCarNoticeDetailModelsByCarId(0, f.Id, searchTime);
                    }
                });
            }

            return resultList;
        }

        /// <summary>
        /// 根据id获取配置信息
        /// </summary>
        /// <param name="carIds">ids</param>
        /// <returns></returns>
        public List<CarnoticeinfoModel> GetCarNoticeInfoById(List<long> carIds)
        {
            return _dataAccess.GetCarNoticeInfoModelsByIds(carIds);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CarNoticeRealNameModel GetCarNoticeRealNameByAccountId(long? userId)
        {
            if (userId == null)
            {
                return null;
            }
            return _carNoticeRealNameData.GetCarNoticeRealNameByAccountId(userId.Value);
        }

        /// <summary>
        /// 添加用户实名
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public long AddCarNoticeRealName(CarNoticeRealNameModel newModel)
        {
            return _carNoticeRealNameData.SaveModel(newModel);
        }

        /// <summary>
        /// 保存详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long SaveCarNoticeDetail(CarnoticedetailModel model)
        {
            return _carNoticeDetailData.SaveModel(model);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="detailId">详细id</param>
        /// <returns></returns>
        public CarnoticedetailModel GetCarnoticedetailModelById(long detailId)
        {
            return _carNoticeDetailData.Get(detailId);
        }

        /// <summary>
        /// 查询已经预约的信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="carId">车id</param>
        /// <param name="searchTime">查询时间</param>
        /// <returns></returns>
        public List<CarnoticedetailModel> GetCarNoticeDetailListByCarIdAccount(long userId, long carId,
            DateTime searchTime)
        {
            return _carNoticeDetailData.GetCarNoticeDetailModelsByCarId(userId, carId, searchTime);
        }

        /// <summary>
        /// 获取车的某个时间的后续安排
        /// </summary>
        /// <param name="carId">主键id</param>
        /// <param name="searchTime">时间点</param>
        /// <returns></returns>
        public List<CarnoticedetailModel> GetAfterNewCarNoticeDetailListByCarId(long carId, DateTime searchTime)
        {
            return _carNoticeDetailData.GetAfterNowCarNoticeDetailModels(carId, searchTime);
        }
    }
}
