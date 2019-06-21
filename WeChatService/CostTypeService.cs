using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatCmsCommon.EnumBusiness;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    public class CostTypeService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CostTypeData _dataAccess = new CostTypeData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostTypeModel> GetList(int type, long userId, int indexPage, int pageSize, out int count, string name = "")
        {
            count = _dataAccess.GetCount(type, userId, name);
            return _dataAccess.GetModels(type, userId, indexPage, pageSize, name);
        }

        /// <summary>
        /// 查询全名匹配内容
        /// </summary>
        /// <param name="spendType"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CostTypeModel> GetList(int spendType, long userId, string name)
        {
            return _dataAccess.GetModels(spendType, userId, name);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void DelModel(long id)
        {
            _dataAccess.DelModel(id);
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostTypeModel Get(long id)
        {
            var data = _dataAccess.Get(id);
            if (data == null || data.IsDel == FlagEnum.HadOne) return null;
            return data;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(CostTypeModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="userId"></param>
        public void InitCostType(long userId)
        {
            if(userId<1) return;
            List<string> outTypeList = new List<string>
            {
                "餐饮","乘车","旅游","服饰","奢侈品","送礼","取现","住宿","充话费","水电气费","物管费","发红包"
            };
            List<string> inTypeList = new List<string>
            {
                "工资","结存","收红包"
            };
            int i = 1;
            foreach (var s in outTypeList)
            {
                var oldModel = new CostTypeModel()
                {
                    IsDel = FlagEnum.HadZore,
                    IsValid = FlagEnum.HadOne,
                    UpdateUserId = userId,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    Sort = i++,
                    UserId = userId,
                    Name = s,
                    SpendType = CostInOrOutEnum.Out.GetHashCode()
                };
                try
                {
                    SaveModel(oldModel);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
            foreach (var s in inTypeList)
            {
                var oldModel = new CostTypeModel()
                {
                    IsDel = FlagEnum.HadZore,
                    IsValid = FlagEnum.HadOne,
                    UpdateUserId = userId,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreateUserId = userId,
                    Sort = i++,
                    UserId = userId,
                    Name = s,
                    SpendType = CostInOrOutEnum.In.GetHashCode()
                };
                try
                {
                    SaveModel(oldModel);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
        }
    }
}
