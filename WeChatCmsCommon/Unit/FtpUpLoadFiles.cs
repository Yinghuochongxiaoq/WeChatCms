using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WeChatCmsCommon.Unit
{
    /// <summary>
    /// ftp文件服务帮助类
    /// </summary>
    public class FtpUpLoadFiles
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="ftpConStr"></param>
        /// <param name="ftpUserName"></param>
        /// <param name="ftpPassWord"></param>
        public FtpUpLoadFiles(string ftpConStr, string ftpUserName, string ftpPassWord)
        {
            _ftpconstr = ftpConStr;
            _ftpusername = ftpUserName;
            _ftppassword = ftpPassWord;
        }

        /// <summary>
        /// FTP的服务器地址，格式为ftp://192.168.1.234:8021/
        /// </summary>
        private readonly string _ftpconstr;

        /// <summary>
        /// FTP服务器的用户名
        /// </summary>
        private readonly string _ftpusername;

        /// <summary>
        /// FTP服务器的密码
        /// </summary>
        private readonly string _ftppassword;

        #region [1、本地文件上传到FTP服务器]

        /// <summary>
        /// 上传文件到远程ftp
        /// </summary>
        /// <param name="ftpPath">ftp上的文件路径</param>
        /// <param name="path">本地的文件目录</param>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        public bool UploadFile(string ftpPath, string path, string name)
        {
            FileInfo f = new FileInfo(path);
            if (!f.Exists)
            {
                return false;
            }

            ftpPath = ftpPath.Replace("\\", "/");
            bool b = MakeDir(ftpPath);
            if (b == false)
            {
                return false;
            }
            path = _ftpconstr + ftpPath + "/" + name;
            var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(path));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.ContentLength = f.Length;
            var buffLength = 2048;
            var buff = new byte[buffLength];
            var fs = f.OpenRead();
            try
            {
                var contentLen = fs.Read(buff, 0, buffLength);
                var stream = reqFtp.GetRequestStream();
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                stream.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("因{0},无法完成上传", ex.Message);
                return false;
            }
        }
        #endregion

        #region [2、从ftp服务器下载文件]

        /// <summary>
        /// 从ftp服务器下载文件的功能
        /// </summary>
        /// <param name="ftpFilepath">ftp下载的地址,相对路径</param>
        /// <param name="filePath">存放到本地的目录路径</param>
        /// <param name="fileName">保存的文件名称</param>
        /// <returns></returns>
        public bool Download(string ftpFilepath, string filePath, string fileName)
        {
            FtpWebRequest reqFtp = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;
            try
            {
                string onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + "\\" + onlyFileName;
                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }
                ftpFilepath = ftpFilepath.Replace("\\", "/");
                var url = _ftpconstr + ftpFilepath;
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
                response = (FtpWebResponse)reqFtp.GetResponse();
                ftpStream = response.GetResponseStream();
                if (ftpStream == null)
                {
                    Console.WriteLine("数据流读取失败");
                    return false;
                }
                var bufferSize = 2048;
                var buffer = new byte[bufferSize];
                var readCount = ftpStream.Read(buffer, 0, bufferSize);
                outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("因{0},无法下载", ex.Message);
                return false;
            }
            finally
            {
                reqFtp?.Abort();
                response?.Close();
                ftpStream?.Close();
                outputStream?.Close();
            }
        }
        #endregion

        #region [3、获得文件的大小]
        /// <summary>
        /// 获得文件大小
        /// </summary>
        /// <param name="url">FTP文件的相对路径</param>
        /// <returns></returns>
        public long GetFileSize(string url)
        {
            url = _ftpconstr + url;
            long fileSize = 0;
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("因{0},无法获得文件大小", ex.Message);
                return fileSize;
            }
            return fileSize;
        }
        #endregion

        #region [4、在ftp服务器上创建文件目录]

        /// <summary>
        ///在ftp服务器上创建文件目录
        /// </summary>
        /// <param name="dirName">文件目录</param>
        /// <returns></returns>
        public bool MakeDir(string dirName)
        {
            try
            {
                bool b = RemoteFtpDirExists(dirName);
                if (b)
                {
                    return true;
                }
                string url = _ftpconstr + dirName;
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("因{0},无法创建目录", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断ftp上的文件目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool RemoteFtpDirExists(string path)
        {
            path = _ftpconstr + path;
            FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(path));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
            reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse resFtp = null;
            try
            {
                resFtp = (FtpWebResponse)reqFtp.GetResponse();
                FtpStatusCode code = resFtp.StatusCode;
                Console.WriteLine(code);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("因{0},无法检测是否存在", exception.Message);
                return false;
            }
            finally
            {
                resFtp?.Close();
            }
        }

        /// <summary>
        /// 获取目录的文件列表
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public List<string> GetFileList(string path)
        {
            path = _ftpconstr + path;
            var results = new List<string>();
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(path));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;

                WebResponse response = reqFtp.GetResponse();
                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    return null;
                }
                StreamReader reader = new StreamReader(stream);
                string line = reader.ReadLine();
                while (line != null)
                {
                    results.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return results;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region [4、从ftp服务器删除文件的功能]
        /// <summary>
        /// 从ftp服务器删除文件的功能
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeleteFile(string fileName)
        {
            var url = _ftpconstr + fileName;
            FtpWebResponse response = null;
            try
            {
                var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFtp.Credentials = new NetworkCredential(_ftpusername, _ftppassword);
                response = (FtpWebResponse)reqFtp.GetResponse();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("因{0},无法删除文件", ex.Message);
                return false;
            }
            finally
            {
                response?.Close();
            }
        }
        #endregion
    }
}
