using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.LoanPreviewResponse
{
    public class Datum
    {
        public string offer_token { get; set; }
        public decimal? amount { get; set; }
        public decimal? interest_rate { get; set; }
        public int term { get; set; }
        public decimal? total_debt { get; set; }
        public decimal? installment_amount { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
    }
}
