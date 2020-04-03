using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.FlowFinance.Common
{
    public class Documents
    {
        [JsonProperty("virtual")]
        public List<Virtual> virtualDocuments { get; set; }

        [JsonProperty("physical")]
        public List<Physical> physicalDocuments { get; set; }
    }
}
