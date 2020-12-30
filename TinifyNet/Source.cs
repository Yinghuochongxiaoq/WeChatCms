using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinifyNet
{
    using Method = HttpMethod;

    public class Source
    {
        public static async Task<Source> FromFile(string path)
        {
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var buffer = new MemoryStream())
            {
                await file.CopyToAsync(buffer).ConfigureAwait(false);
                return await FromBuffer(buffer.ToArray()).ConfigureAwait(false);
            }
        }

        public static async Task<Source> FromBuffer(byte[] buffer)
        {
            var response = await Tinify.Client.Request(Method.Post, "/shrink", buffer).ConfigureAwait(false);
            var location = response.Headers.Location;

            return new Source(location);
        }

        public static async Task<Source> FromUrl(string url)
        {
            var body = new Dictionary<string, object>();
            body.Add("source", new { url });

            var response = await Tinify.Client.Request(Method.Post, "/shrink", body).ConfigureAwait(false);
            var location = response.Headers.Location;

            return new Source(location);
        }

        Uri url;
        Dictionary<string, object> commands;

        internal Source(Uri url, Dictionary<string, object> commands = null)
        {
            this.url = url;
            if (commands == null) commands = new Dictionary<string, object>();
            this.commands = commands;
        }

        public Source Preserve(params string[] options)
        {
            return new Source(url, MergeCommands("preserve", options));
        }

        public Source Resize(object options)
        {
            return new Source(url, MergeCommands("resize", options));
        }

        public async Task<ResultMeta> Store(object options)
        {
            var commandsDic = MergeCommands("store", options);
            var response = await Tinify.Client.Request(Method.Post, url, commandsDic).ConfigureAwait(false);

            return new ResultMeta(response.Headers);
        }

        public async Task<Result> GetResult()
        {
            HttpResponseMessage response;
            if (commands.Count == 0) {
                response = await Tinify.Client.Request(Method.Get, url).ConfigureAwait(false);
            } else {
                response = await Tinify.Client.Request(Method.Post, url, commands).ConfigureAwait(false);
            }

            var body = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return new Result(response.Headers, response.Content.Headers, body);
        }

        public async Task ToFile(string path) {
            await GetResult().ToFile(path).ConfigureAwait(false);
        }

        public async Task<byte[]> ToBuffer()  {
            return await GetResult().ToBuffer().ConfigureAwait(false);
        }

        private Dictionary<string, object> MergeCommands(string key, object options)
        {
            var commandsDictionary = new Dictionary<string, object>(commands);
            commandsDictionary.Add(key, options);
            return commandsDictionary;
        }
    }
}
