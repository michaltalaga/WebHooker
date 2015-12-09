using System.Collections.Generic;

namespace WebHookerClient
{
    public class Request
    {
        public string HttpMethod { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Body { get; set; }
    }
}
