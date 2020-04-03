using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.SignLoanRequest
{
    public class Signature
    {
        public DateTime date { get; set; }
        public string ip { get; set; }

        [JsonProperty("user-agent")]
        public string userAgent { get; set; }
    }

    public class RootObject
    {
        public Signature signature { get; set; }
    }
}
