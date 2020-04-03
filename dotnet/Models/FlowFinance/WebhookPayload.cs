using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.WebhookPayload
{
    public class Entity
    {
        public int id { get; set; }
        public string status { get; set; }

        [JsonProperty("line-of-credit")]
        public object lineOfCredit { get; set; }

        [JsonProperty("tos-acceptance")]
        public object tosAcceptance { get; set; }

        [JsonProperty("created-a")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updated-at")]
        public DateTime updatedAt { get; set; }
    }

    public class Data
    {
        public string event_id { get; set; }

        [JsonProperty("event")]
        public string eventType { get; set; }
        public string entity_type { get; set; }
        public Entity entity { get; set; }
        public DateTime created_at { get; set; }
    }

    public class RootObject
    {
        public Data data { get; set; }
    }
}
