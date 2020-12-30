using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TinifyNet
{
    public class Result : ResultMeta
    {
        protected HttpContentHeaders Content;
        protected byte[] Data;

        internal Result(HttpResponseHeaders meta, HttpContentHeaders content, byte[] data) : base(meta)
        {
            Content = content;
            Data = data;
        }

        public async Task ToFile(string path)
        {
            using (var file = File.Create(path))
            {
                await file.WriteAsync(Data, 0, Data.Length).ConfigureAwait(false);
            }
        }

        public byte[] ToBuffer()
        {
            return Data;
        }

        public ulong? Size => (ulong?) Content.ContentLength;

        public string MediaType
        {
            get
            {
                var header = Content.ContentType;
                return header?.MediaType;
            }
        }

        public string ContentType => MediaType;
    }
}
