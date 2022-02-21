using ADW.Application.DTO;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.RequestFilters
{
    public class RequestFilterMyDragonPage : RequestFilterBase
    {

        public delegate void OnResponseMyPageDragon(ResponseCreatedEventArgs e, MyPageDragonDTO myPageDragonDTO);
        public delegate void OnProcessRequestMyPageDragon(RequestFilterBase a1, RequestEventArgs a2);
        public event OnResponseMyPageDragon OnResponseListener;
        public event OnProcessRequestMyPageDragon OnRequestListener;
        public RequestFilterMyDragonPage()
        {
            var url = new Uri("https://devabcde-api.dragonwars.game/v1/dragons/my-dragon");
            Key = url.AbsolutePath.ToLower();
            IsCaptureReponse = true;
            IsCaptureRequest = true;
            OnResponse = ProcessResponse;
            OnRequest = ProcessRequest;
        }

        private Task ProcessRequest(RequestFilterBase arg1, RequestEventArgs arg2)
        {
            OnRequestListener?.Invoke(arg1, arg2);
            return Task.CompletedTask;
        }

        private async Task ProcessResponse(RequestFilterBase arg1, ResponseCreatedEventArgs arg2)
        {
            if (arg2.Response.Status == System.Net.HttpStatusCode.OK)
            {
                var res = await arg2.Response.JsonAsync<MyPageDragonDTO>();
                OnResponseListener?.Invoke(arg2, res);
            }
        }
    }
}
