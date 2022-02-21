using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.RequestFilters
{
    public class RequestFilterBase
    {
        public string Key { get; set; }
        public Func<RequestFilterBase, RequestEventArgs, Task> OnRequest { get; set; }
        public Func<RequestFilterBase, ResponseCreatedEventArgs,Task> OnResponse { get; set; }
        public bool IsCaptureRequest { get; set; }
        public bool IsCaptureReponse { get; set; }
    }
}
