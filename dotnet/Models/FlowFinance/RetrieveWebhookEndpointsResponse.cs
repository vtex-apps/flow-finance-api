using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.RetrieveWebhookEndpointsResponse
{
    public class Datum
    {
        public int id { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public List<string> events { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
    }
}
