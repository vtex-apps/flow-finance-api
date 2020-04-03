using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class ResponseWrapper
    {
        public string responseMessage { get; set; }
        public string errorMessage { get; set; }
        public bool success { get; set; }
        public object responseObject { get; set; }
    }
}
