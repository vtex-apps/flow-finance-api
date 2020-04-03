using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class Payload
    {
        public string inboundRequestsUrl { get; set; }
        public string callbackUrl { get; set; }
        public string paymentIdentifier { get; set; }
        public string publicKey { get; set; }
    }
}
