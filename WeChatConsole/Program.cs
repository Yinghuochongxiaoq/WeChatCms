using System;
using System.Threading.Tasks;
using TinifyNet;
using WeChatCmsCommon.Unit;

namespace WeChatConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var ftpServer = new FtpUpLoadFiles("ftp://23.224.53.118", "s9292981", "mbBA44waOT");
            //ftpServer.UploadFile("", "D:\\12582Work\\Document\\EmailContent.txt", "a.txt");
            //ftpServer.Download("/a/a.txt", "D:\\12582Work\\Document", "a.txt");
            //ftpServer.GetFileSize("/a/a.txt");
            //ftpServer.GetFileList("");
            //ftpServer.DeleteFile("/a.txt");
            Task task = Yasuotupian();
            Console.WriteLine("测试完成" + Tinify.CompressionCount);
            Console.ReadKey();
        }

        private static async Task Yasuotupian()
        {
            Tinify.Key = "bQNd3HT7CKBLz6wVCvM73MwGM9B79vvQ";
            var source = Tinify.FromFile("C:\\Users\\QinFreshMan\\Pictures\\0.jpg");
            await source.ToFile("optimized.jpg");
        }
    }
}
