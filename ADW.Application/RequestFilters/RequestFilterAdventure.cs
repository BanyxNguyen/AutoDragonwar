using ADW.Application.DTO;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.RequestFilters
{
    public class RequestFilterAdventure : RequestFilterBase
    {

        public delegate void OnResponseMyPageDragon(ResponseCreatedEventArgs e, AdventureDTO myPageDragonDTO);
        public event OnResponseMyPageDragon OnResponseListener;
        public RequestFilterAdventure()
        {
            var url = new Uri("https://devabcde-api.dragonwars.game/v1/dragons/get-dragon-adventure");
            Key = url.AbsolutePath.ToLower();
            IsCaptureReponse = true;
            OnResponse = ProcessResponse;
        }

        private async Task ProcessResponse(RequestFilterBase arg1, ResponseCreatedEventArgs arg2)
        {
            if(arg2.Response.Status == System.Net.HttpStatusCode.OK)
            {
                var res = await arg2.Response.JsonAsync<AdventureDTO>();
                OnResponseListener?.Invoke(arg2,res);
            }
        }
    }
}
