using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebHookerClient
{
    public class Request
    {
        public string HttpMethod { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Body { get; set; }
        public Dictionary<string, string> QueryString { get; set; }
    }
}
