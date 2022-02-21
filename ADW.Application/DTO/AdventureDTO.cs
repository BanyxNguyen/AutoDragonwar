using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.DTO
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdventureDTO
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("payload")]
        public List<int> Payload { get; set; }
    }


}
