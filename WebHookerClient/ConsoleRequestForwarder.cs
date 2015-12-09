using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHookerClient
{
    public class ConsoleRequestForwarder : IRequestForwarder
    {
        public void Forward(Request data)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
