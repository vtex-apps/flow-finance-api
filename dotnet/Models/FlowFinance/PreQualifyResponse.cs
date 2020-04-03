using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.PreQualifyResponse
{
    public class Data
    {
        public string cnpj { get; set; }
        public bool eligible { get; set; }
    }

    public class RootObject
    {
        public Data data { get; set; }
    }
}
