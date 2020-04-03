using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class FlowFinanceShopper
    {
        public string email { get; set; }
        public string accountId { get; set; }
        public DateTime createdAt { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string idNumber { get; set; }
    }
}
