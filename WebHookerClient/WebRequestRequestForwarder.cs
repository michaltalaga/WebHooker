using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebHookerClient
{
    public class WebRequestRequestForwarder : IRequestForwarder
    {
        string targetUrl;

        public WebRequestRequestForwarder(string targetUrl)
        {
            this.targetUrl = targetUrl;
        }
        public void Forward(Request data)
        {
            var request = HttpWebRequest.CreateHttp(this.targetUrl);
            request.Method = data.HttpMethod;
            foreach (var header in data.Headers)
            {
                if (WebHeaderCollection.IsRestricted(header.Key))
                {
                    AddRestrictedHeader(request, header);
                }
                else
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            if (data.Body != null && data.Body.Length > 0)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data.Body, 0, data.Body.Length);
                }
            }

            using (var response = request.GetResponse())
            {

            }
        }
        private static void AddRestrictedHeader(HttpWebRequest request, KeyValuePair<string, string> header)
        {
            if (string.Equals(header.Key, "Date", StringComparison.InvariantCultureIgnoreCase))
            {
                request.Date = DateTime.ParseExact(header.Value, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToLocalTime();
            }
            else if (string.Equals(header.Key, "If-Modified-Since", StringComparison.InvariantCultureIgnoreCase))
            {
                request.IfModifiedSince = DateTime.ParseExact(header.Value, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToLocalTime();
            }
            else if (string.Equals(header.Key, "Proxy-Connection", StringComparison.InvariantCultureIgnoreCase) || string.Equals(header.Key, "Connection", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(header.Value, "keep-alive", StringComparison.InvariantCultureIgnoreCase))
                {
                    request.KeepAlive = true;
                }
                else if (string.Equals(header.Value, "close", StringComparison.InvariantCultureIgnoreCase))
                {
                    request.KeepAlive = false;
                }
            }
            else if (string.Equals(header.Key, "Accept", StringComparison.InvariantCultureIgnoreCase))
            {
                request.Accept = header.Value;
            }
            else if (string.Equals(header.Key, "Content-Length", StringComparison.InvariantCultureIgnoreCase))
            {
                request.ContentLength = long.Parse(header.Value);
            }
            else if (string.Equals(header.Key, "Content-Type", StringComparison.InvariantCultureIgnoreCase))
            {
                request.ContentType = header.Value;
            }
            else if (string.Equals(header.Key, "Expect", StringComparison.InvariantCultureIgnoreCase))
            {
                request.Expect = header.Value;
            }
            else if (string.Equals(header.Key, "Host", StringComparison.InvariantCultureIgnoreCase))
            {
                //request.Host = header.Value;
            }
            else if (string.Equals(header.Key, "Referer", StringComparison.InvariantCultureIgnoreCase))
            {
                request.Referer = header.Value;
            }
            else if (string.Equals(header.Key, "Transfer-Encoding", StringComparison.InvariantCultureIgnoreCase))
            {
                request.TransferEncoding = header.Value;
            }
            else if (string.Equals(header.Key, "User-Agent", StringComparison.InvariantCultureIgnoreCase))
            {
                request.UserAgent = header.Value;
            }
        }
    }
}
