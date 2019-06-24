using System;
using System.Security.Cryptography;
using System.Text;
using FreshCommonUtility.Web;
using Newtonsoft.Json;
using WeChatModel.WeChatAuthModel;

namespace WeChatNoteCostApi.WeChatHelper
{
    public class WeChatAppDecrypt
    {
        #region [1、微信小程序用户数据解密]
        private string appId;
        private string appSecret;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">应用程序的AppId</param>
        /// <param name="appSecret">应用程序的AppSecret</param>
        public WeChatAppDecrypt(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        /// <summary>
        /// 获取OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">客户端发来的code</param>
        /// <returns>Json数据包</returns>
        private string GetOpenIdAndSessionKeyString(string code)
        {
            string temp = "https://api.weixin.qq.com/sns/jscode2session?" +
                "appid=" + appId
                + "&secret=" + appSecret
                + "&js_code=" + code
                + "&grant_type=authorization_code";

            return WebHttpHelper.HttpGet(temp);
        }

        /// <summary>
        /// 反序列化包含OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="loginInfo">Json数据包</param>
        /// <returns>包含OpenId和SessionKey的类</returns>
        public OpenIdAndSessionKey DecodeOpenIdAndSessionKey(WeChatLoginInfo loginInfo)
        {
            OpenIdAndSessionKey oiask = JsonConvert.DeserializeObject<OpenIdAndSessionKey>(GetOpenIdAndSessionKeyString(loginInfo.code));
            if (!string.IsNullOrEmpty(oiask.errcode))
                return null;
            return oiask;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="rawData">公开的用户资料</param>
        /// <param name="signature">公开资料携带的签名信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(string rawData, string signature, string sessionKey)
        {
            //创建SHA1签名类
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //编码用于SHA1验证的源数据
            byte[] source = Encoding.UTF8.GetBytes(rawData + sessionKey);
            //生成签名
            byte[] target = sha1.ComputeHash(source);
            //转化为string类型，注意此处转化后是中间带短横杠的大写字母，需要剔除横杠转小写字母
            string result = BitConverter.ToString(target).Replace("-", "").ToLower();
            //比对，输出验证结果
            return signature == result;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(WeChatLoginInfo loginInfo, string sessionKey)
        {
            return VaildateUserInfo(loginInfo.rawData, loginInfo.signature, sessionKey);
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="idAndKey">包含OpenId和SessionKey的类</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        public bool VaildateUserInfo(WeChatLoginInfo loginInfo, OpenIdAndSessionKey idAndKey)
        {
            return VaildateUserInfo(loginInfo, idAndKey.session_key);
        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        public WeChatUserInfo Decrypt(string encryptedData, string iv, string sessionKey)
        {
            WeChatUserInfo userInfo;
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //格式化待处理字符串
            byte[] byteEncryptedData = Convert.FromBase64String(encryptedData);
            byte[] byteIv = Convert.FromBase64String(iv);
            byte[] byteSessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byteIv;
            aes.Key = byteSessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byteEncryptedData, 0, byteEncryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);

            //反序列化结果，生成用户信息实例
            userInfo = JsonConvert.DeserializeObject<WeChatUserInfo>(result);

            return userInfo;

        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据，推荐直接使用此方法
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <returns>用户信息</returns>
        public WeChatUserInfo Decrypt(WeChatLoginInfo loginInfo)
        {
            if (loginInfo == null)
                return null;

            if (string.IsNullOrEmpty(loginInfo.code))
                return null;

            OpenIdAndSessionKey oiask = DecodeOpenIdAndSessionKey(loginInfo);

            if (oiask == null)
                return null;

            if (!VaildateUserInfo(loginInfo, oiask))
                return null;

            WeChatUserInfo userInfo = Decrypt(loginInfo.encryptedData, loginInfo.iv, oiask.session_key);

            return userInfo;
        }
        #endregion
    }
}
