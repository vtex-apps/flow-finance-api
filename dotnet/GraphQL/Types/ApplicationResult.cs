using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    public class ApplicationResult
    {
        public bool success { get; set; }
        public string error { get; set; }
    }
}
