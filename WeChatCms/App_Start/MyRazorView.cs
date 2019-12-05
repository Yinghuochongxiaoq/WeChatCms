using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace WeChatCms
{
    public class MyRazorView : RazorView
    {
        public MyRazorView(ControllerContext controllerContext, string viewPath, string layoutPath,
            bool runViewStartPages, IEnumerable<string> viewStartFileExtensions) : base(controllerContext, viewPath,
            layoutPath, runViewStartPages, viewStartFileExtensions)
        {
        }

        public MyRazorView(ControllerContext controllerContext, string viewPath, string layoutPath,
            bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator) :
            base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        private static readonly Minifier Minifier = new Minifier();
        private static readonly CodeSettings CodeSettings = new CodeSettings
        {
            IgnoreAllErrors = false,
            MinifyCode = true,
            LocalRenaming = LocalRenaming.CrunchAll
        };
        private static readonly CssSettings CssSettings = new CssSettings
        {
            IgnoreAllErrors = false,
            OutputMode = OutputMode.SingleLine,
            MinifyExpressions = true
        };
        /// <summary>
        /// js压缩正则表达式
        /// </summary>
        private static readonly Regex RegexJs = new Regex("(?<=<script(.)*?>)([\\s\\S](?!<script))*?(?=</script>)", RegexOptions.IgnoreCase);

        /// <summary>
        /// css内联样式压缩正则表达式
        /// </summary>
        private static readonly Regex RegexStyle = new Regex("(?<=<style(.)*?>)([\\s\\S](?!<script))*?(?=</style>)", RegexOptions.IgnoreCase);

        private const string KeyCss = "[css*.256.843.56.745.h*J.]";
        private const string KeyJs = "[js*.869.218.839.*W.]";

        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter textWriter = new HtmlTextWriter(stringWriter);
            base.RenderView(viewContext, textWriter, instance);
            var html = stringWriter.ToString();

            ////将html里面的简体中文转换为繁体,末尾参数设置为1033为了防止页面产生乱码
            //html = Strings.StrConv(html, VbStrConv.TraditionalChinese, 1033);

            //压缩js
            Parallel.Invoke(() =>
                {
                    var marchCollection = RegexJs.Matches(html);
                    if (marchCollection.Count > 0)
                    {
                        foreach (Match itemMatch in marchCollection)
                        {
                            if (string.IsNullOrWhiteSpace(itemMatch.Value))
                            {
                                continue;
                            }

                            html = html.Replace(itemMatch.Value, KeyJs);
                            //压缩js
                            var minJs = Minifier.MinifyJavaScript(itemMatch.Value, CodeSettings);
                            html = html.Replace(KeyJs, minJs);
                        }
                    }
                },
                //压缩css
                () =>
                {
                    var marchList = RegexStyle.Matches(html);
                    if (marchList.Count > 0)
                    {
                        foreach (Match item in marchList)
                        {
                            if (string.IsNullOrWhiteSpace(item.Value))
                            {
                                continue;
                            }

                            html = html.Replace(item.Value, KeyCss);
                            //压缩style
                            var minCss = Minifier.MinifyStyleSheet(item.Value, CssSettings, CodeSettings);
                            html = html.Replace(KeyCss, minCss);
                        }
                    }
                });
            //压缩html，移除html标签之间的空格
            html = Regex.Replace(html, @"(?<=>)[\s|\n|\t]*(?=<)", string.Empty);
            //移除多余空格和换行符
            html = new Regex(@"\n+\s+").Replace(html, string.Empty);
            //输出到页面
            writer.Write(html);
        }
    }
}