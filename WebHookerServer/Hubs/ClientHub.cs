using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebHookerServer.Controllers;

namespace WebHookerServer.Hubs
{
    public class ClientHub : Hub<WebHookerClient.IClient>
    {
        public static Dictionary<string, string> ClientConnectionIdMap = new Dictionary<string, string>();
        private static object syncRoot = new object();

        public override Task OnConnected()
        {
            string id;
            lock (syncRoot)
            {
                while (true)
                {
                    id = GetUniqueId();
                    if (!ClientConnectionIdMap.ContainsKey(id))
                    {
                        ClientConnectionIdMap[id] = Context.ConnectionId;
                        break;
                    }
                }
            }
            var uri = new Uri(Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority);
            uri = new Uri(uri, nameof(HookController).Replace("Controller", "") + "/" + id);
            Clients.Caller.Registered(uri.ToString());
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            lock(syncRoot)
            {
                if (ClientConnectionIdMap.ContainsValue(Context.ConnectionId))
                {
                    var entry = ClientConnectionIdMap.Single(e => e.Value == Context.ConnectionId);
                    ClientConnectionIdMap.Remove(entry.Key);
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        private static string GetUniqueId()
        {
            var guid = Guid.NewGuid();
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "");
            enc = enc.Replace("+", "");
            enc = enc.Replace("=", "");
            return enc.ToLower().Substring(0, 10);
        }


    }
}