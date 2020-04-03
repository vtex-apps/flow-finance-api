using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class GetLoanOptionsRequest
    {
        public string email { get; set; }
        public decimal total { get; set; }
    }
}
