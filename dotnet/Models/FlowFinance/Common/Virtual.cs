using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.FlowFinance.Common
{
    public class Virtual
    {
        public string id { get; set; }
        public string value { get; set; }
        public DateTime timestamp { get; set; }
        public string type { get; set; }
        public string exp { get; set; }
        public string issuer { get; set; }
    }
}
