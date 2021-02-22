using Newtonsoft.Json;
using System.Collections.Generic;

namespace Trueman.Core.Clients.TemporaryAccessPass
{
    public class TapListRoot
    {
        [JsonProperty("value")]
        public List<TapData> Items { get; set; }
    }
}
