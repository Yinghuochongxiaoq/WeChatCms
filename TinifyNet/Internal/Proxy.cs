using System;
using System.Net;

namespace TinifyNet.Internal
{
    internal class Proxy : IWebProxy
    {
        Uri Uri { get; }

        public ICredentials Credentials { get; set; }

        public Proxy(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var _url))
            {
                throw new ConnectionException($"Invalid proxy: cannot parse '{url}'");
            }

            if (!string.IsNullOrEmpty(_url.UserInfo))
            {
                var user = _url.UserInfo.Split(':');
                Credentials = new NetworkCredential(user[0], user[1]);
            }
        }

        public Uri GetProxy(Uri destination)
        {
            return Uri;
        }

        public bool IsBypassed(Uri host)
        {
            return host.IsLoopback;
        }
    }
}
