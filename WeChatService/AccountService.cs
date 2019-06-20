using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel;
using WeChatModel.DatabaseModel;

namespace WeChatService
{
    /// <summary>
    /// 用户验证服务
    /// </summary>
    public class AccountService
    {
        private static AccountData _accountData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccountService()
        {
            if (_accountData == null)
            {
                _accountData = new AccountData();
            }
        }

        #region [1、登录部分]
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginInfoModel UserLogin(string userName, string password)
        {
            return _accountData.UserLogin(userName, password);
        }

        /// <summary>
        /// 获取用户菜单信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static LoginInfoModel GetUserMenuInfo(LoginInfoModel userInfo)
        {
            return new AccountData().GetUserInfo(userInfo);
        }
        #endregion

        #region [2、处理管理员相关信息]
        /// <summary>
        /// 获取列表用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysUser> GetSysUsers(int pageIndex, int pageSize, out int count)
        {
            count = _accountData.GetSysUserCount();
            return _accountData.GetSysUsers(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取单个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysUser GetSysUser(long id)
        {
            return _accountData.GetSysUser(id);
        }

        /// <summary>
        /// 保存单个用户
        /// </summary>
        /// <param name="model"></param>
        public void SaveUserModel(SysUser model)
        {
            if (model == null) return;
            _accountData.SaveUserModel(model);
        }

        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SysUser> GetSysUsersByUserName(string userName)
        {
            return _accountData.GetSysUsersByUserName(userName);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        public void DelUserModel(int id)
        {
            _accountData.DelUserModel(id);
        }
        #endregion

        #region [3、添加并绑定一个新微信用户]

        /// <summary>
        /// 绑定新用户
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public long InsertWeChatUserAndBind(SysUser sysUser, string openId)
        {
            return _accountData.InsertAndBindWechatUser(sysUser, openId);
        }

        #endregion
    }
}
