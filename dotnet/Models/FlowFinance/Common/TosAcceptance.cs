using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.FlowFinance.Common
{
    public class TosAcceptance
    {
        public DateTime date { get; set; }
        public string ip { get; set; }
        public string user_agent { get; set; }
    }
}
