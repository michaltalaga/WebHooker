using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHookerClient
{
    public class CommandLineOptions
    {
        [Option('t', "target", Required = true, HelpText = "Forward to url. Example: http://localhost:8080 or http://hostname")]
        public string ForwardToUrl { get; set; }
        [Option('s', "server", Required = false, HelpText = "Public WebHooker server url", DefaultValue = Program.DefaultWebHookerServerUrl)]
        public string ServerUrl { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
