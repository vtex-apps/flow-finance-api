using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.CreateWebhookEndpointRequest
{
    public class RootObject
    {
        public string url { get; set; }
        public List<string> events { get; set; }
    }
}
