using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class App
    {
        public string id { get; set; }
        public List<string> fields { get; set; }
    }

    public class OrderConfigurationApps
    {
        public List<App> apps { get; set; }
    }
}
