using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class OrderInformation
    {
        public bool hasError { get; set; }
        public string message { get; set; }

        public string offerToken { get; set; }
        public string email { get; set; }
    }
}
