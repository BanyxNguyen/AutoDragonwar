using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.DTO
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Image
    {
        [JsonProperty("250")]
        public string _250 { get; set; }

        [JsonProperty("500")]
        public string _500 { get; set; }

        [JsonProperty("768")]
        public string _768 { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }
    }

    public class Parts
    {
        [JsonProperty("horns")]
        public string Horns { get; set; }

        [JsonProperty("backcales")]
        public string Backcales { get; set; }

        [JsonProperty("tail")]
        public string Tail { get; set; }

        [JsonProperty("head")]
        public string Head { get; set; }

        [JsonProperty("eyes")]
        public string Eyes { get; set; }

        [JsonProperty("wings")]
        public string Wings { get; set; }

        [JsonProperty("chest")]
        public string Chest { get; set; }

        [JsonProperty("middlehorns")]
        public string Middlehorns { get; set; }
    }

    public class Stats
    {
        [JsonProperty("mana")]
        public int Mana { get; set; }

        [JsonProperty("health")]
        public int Health { get; set; }

        [JsonProperty("attack")]
        public int Attack { get; set; }

        [JsonProperty("defend")]
        public int Defend { get; set; }

        [JsonProperty("speed")]
        public int Speed { get; set; }

        [JsonProperty("morale")]
        public int Morale { get; set; }
    }

    public class Datum
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("parts")]
        public Parts Parts { get; set; }

        [JsonProperty("generation")]
        public int Generation { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("skills")]
        public List<object> Skills { get; set; }

        [JsonProperty("potential")]
        public int Potential { get; set; }

        [JsonProperty("birth")]
        public object Birth { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("isReady")]
        public bool IsReady { get; set; }

        [JsonProperty("isGestating")]
        public bool IsGestating { get; set; }

        [JsonProperty("timeLock")]
        public int TimeLock { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("sale")]
        public object Sale { get; set; }

        [JsonProperty("cooldownIndex")]
        public int CooldownIndex { get; set; }

        [JsonProperty("nextActionAt")]
        public int NextActionAt { get; set; }

        [JsonProperty("xp")]
        public int Xp { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("mutant")]
        public bool Mutant { get; set; }

        [JsonProperty("startLock")]
        public object StartLock { get; set; }

        [JsonProperty("favorite")]
        public bool Favorite { get; set; }
    }

    public class PayloadDragonPage
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
    }

    public class MyPageDragonDTO
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("payload")]
        public PayloadDragonPage Payload { get; set; }
    }


}
