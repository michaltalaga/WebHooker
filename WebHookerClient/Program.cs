using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.Linq;

namespace WebHookerClient
{
    class Program
    {
        public const string DefaultWebHookerServerUrl = "http://webhooker.dotcraft.net";

        static void Main(string[] args)
        {
            //args = new[] { "-t", "http://localhost:123" };
            CommandLineOptions options = GetOptionsOrExit(args);
            
            var serverUrl = options.ServerUrl;

            var requestForwarder = new WebRequestRequestForwarder(options.ForwardToUrl);
            var consoleForwarder = new ConsoleRequestForwarder();

            var webHookerClient = new WebHookerClient(serverUrl, requestForwarder, consoleForwarder);
            webHookerClient.Connected += WebHookerClient_Connected;
            webHookerClient.Error += WebHookerClient_Error;
            webHookerClient.Start();


            Console.ReadLine();
        }

        private static CommandLineOptions GetOptionsOrExit(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(0);
            }
            Uri uri;
            if (!Uri.TryCreate(options.ForwardToUrl, UriKind.Absolute, out uri))
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(0);
            }
            if (options.ServerUrl != null && !Uri.TryCreate(options.ServerUrl, UriKind.Absolute, out uri))
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(0);
            }

            return options;
        }

        private static void WebHookerClient_Error(object sender, WebHookerClient.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
            Environment.Exit(0);
        }

        private static void WebHookerClient_Connected(object sender, WebHookerClient.ConnectedEventArgs e)
        {
            Console.WriteLine("Public url: " + e.Url);
        }
    }
  

    
    public class WebHookerClient
    {
        List<IRequestForwarder> forwarders;
        string url;
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<ErrorEventArgs> Error;

        public WebHookerClient(string url, params IRequestForwarder[] forwarders) 
        {
            this.forwarders = new List<IRequestForwarder>(forwarders);
            this.url = url;
        }
        public void AddForwarder(IRequestForwarder forwarder)
        {

        }
        public void Start()
        {
            var hubConnection = new HubConnection(this.url);
            hubConnection.Error += HubConnection_Error;
            IHubProxy proxy = hubConnection.CreateHubProxy("ClientHub");
            proxy.On<Request>(nameof(IClient.Forward), data =>
            {
                foreach (var forwarder in forwarders)
                {
                    forwarder.Forward(data);
                }
            });
            proxy.On<string>(nameof(IClient.Registered), url =>
            {
                if (Connected != null) Connected(this, new ConnectedEventArgs(url));
            });
            hubConnection.Start().Wait();
        }

        private void HubConnection_Error(Exception obj)
        {
            if (Error != null) Error(this, new ErrorEventArgs(obj));
            else throw obj;
        }
        public class ConnectedEventArgs : EventArgs
        {
            public string Url { get; private set; }
            public ConnectedEventArgs(string url)
            {
                Url = url;
            }
        }
        public class ErrorEventArgs : EventArgs
        {
            public Exception Exception { get; private set; }
            public ErrorEventArgs(Exception exception)
            {
                Exception = exception;
            }
        }
    }

}
