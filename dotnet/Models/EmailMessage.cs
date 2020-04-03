using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class JsonData
    {
        public string to { get; set; }
    }

    public class EmailMessage
    {
        public object providerName { get; set; }
        public string templateName { get; set; }
        public JsonData jsonData { get; set; }
    }
}
