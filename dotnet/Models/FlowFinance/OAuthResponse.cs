using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.OAuthResponse
{
    public class RootObject
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}