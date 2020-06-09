using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class FlowFinanceToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
