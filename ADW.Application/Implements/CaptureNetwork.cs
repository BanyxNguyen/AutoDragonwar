using ADW.Application.RequestFilters;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.Implements
{
    public class CaptureNetwork
    {
        private readonly Page _Page;
        public CaptureNetwork(Page page)
        {
            _Page = page;
            _Page.Request += new EventHandler<RequestEventArgs>(onRequest);
            _Page.Response += new EventHandler<ResponseCreatedEventArgs>(onResponse);
            MapFilterRequest = new Dictionary<string, RequestFilters.RequestFilterBase>();
            MapFilterResponse = new Dictionary<string, RequestFilters.RequestFilterBase>();
        }
        public IDictionary<string, RequestFilters.RequestFilterBase> MapFilterRequest { get; set; }
        public IDictionary<string, RequestFilters.RequestFilterBase> MapFilterResponse { get; set; }
        public void AddFilter(RequestFilterBase filter)
        {
            if (filter.IsCaptureReponse)
            {
                MapFilterResponse.Add(filter.Key.ToLower(), filter);
            }
            if (filter.IsCaptureRequest)
            {
                MapFilterRequest.Add(filter.Key.ToLower(), filter);
            }
        }
        private void onResponse(object sender, ResponseCreatedEventArgs e)
        {
            var url = new Uri(e.Response.Url);
            if (MapFilterResponse.ContainsKey(url.AbsolutePath.ToLower()))
            {
                var filter = MapFilterResponse[url.AbsolutePath.ToLower()];
                _ = filter.OnResponse?.Invoke(filter, e);
            }
            Console.WriteLine(url.AbsolutePath);
        }

        private void onRequest(object sender, RequestEventArgs e)
        {
            var url = new Uri(e.Request.Url);
            if (MapFilterRequest.ContainsKey(url.AbsolutePath.ToLower()))
            {
                var filter = MapFilterRequest[url.AbsolutePath.ToLower()];
                _ = filter.OnRequest?.Invoke(filter, e);
            }
            Console.WriteLine(url.AbsolutePath);
        }
    }
}
