using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebHookerServer.Hubs;

namespace WebHookerServer.Controllers
{
    public class HookController : Controller
    {
        public ActionResult Handle(string id)
        {
            if (ClientHub.ClientConnectionIdMap.ContainsKey(id))
            {
                var connectionId = ClientHub.ClientConnectionIdMap[id]; 
                using (var ms = new MemoryStream())
                {
                    Request.InputStream.Position = 0;
                    Request.InputStream.CopyTo(ms);
                    var request = new WebHookerClient.Request()
                    {
                        HttpMethod = Request.HttpMethod,
                        Headers = Request.Headers.AllKeys.ToDictionary(k => k, k => Request.Headers[k]),
                        Body = ms.ToArray(),
                        QueryString = Request.QueryString.AllKeys.ToDictionary(key => key, key => Request.QueryString[key]),

                    };
                    var hub = GlobalHost.ConnectionManager.GetHubContext<ClientHub, WebHookerClient.IClient>();
                    hub.Clients.Client(connectionId).Forward(request);
                }
                
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}