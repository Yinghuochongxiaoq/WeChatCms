﻿using System;
using System.Collections.Generic;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class DailyHistoryService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private DailyHistoryData _dataAccess = new DailyHistoryData();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="number">工作量</param>
        /// <param name="dailyDateTime">工作时间</param>
        /// <param name="dailyContent">工作内容</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public long SaveModel(long id, decimal number, DateTime dailyDateTime, string dailyContent, long userId)
        {
            var model = new DailyHistoryModel();
            if (id > 0)
            {
                model = _dataAccess.Get(id);
            }
            else
            {
                model.Createtime = DateTime.Now;
            }

            model.DailyDate = dailyDateTime;
            model.DailyContent = dailyContent.Length > 500 ? dailyContent.Substring(0, 500) : dailyContent;
            model.DailyMonth = dailyDateTime.Month;
            model.DailyNumber = number;
            model.DailyYear = dailyDateTime.Year;
            model.IsDel = FlagEnum.HadZore;
            model.UpdateTime = DateTime.Now;
            model.UserId = userId;
            model.DailyDay = dailyDateTime.Day;
            return _dataAccess.SaveModel(model);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long SaveModel(DailyHistoryModel model)
        {
            return _dataAccess.SaveModel(model);
        }

        /// <summary>
        /// 查询用户的日志信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">天</param>
        /// <returns>日志记录</returns>
        public List<DailyHistoryModel> GetDailyHistoryModels(long userId, int year, int month,int day=0)
        {
            return _dataAccess.GetDailyHistoryListByUserId(userId, year, month, day);
        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DailyHistoryModel GetDailyHistoryDetailInfo(long id)
        {
            DailyHistoryModel model= _dataAccess.Get(id);
            if (model != null && model.IsDel == FlagEnum.HadOne)
            {
                model = null;
            }

            return model;
        }
    }
}
