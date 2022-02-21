using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.Options
{
    public class PuppeteerBrowserOption 
    {
        public string UserData { get; set; }
        public bool Headless { get; set; }
        public string Profile { get; set; }
        public string MetaMaskExtension { get; set; }
        public string ChromePath { get; set; }
        public string MetaMaskExtensionId { get; set; }
        public string Wallet { get; set; }
    }
}
