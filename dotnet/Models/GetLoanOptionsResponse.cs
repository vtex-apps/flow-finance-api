using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class LoanOption
    {
        public string offer_token { get; set; }
        public decimal amount { get; set; }
        public decimal interest_rate { get; set; }
        public int term { get; set; }
        public decimal total_debt { get; set; }
        public decimal installment_amount { get; set; }
    }

    public class GetLoanOptionsResponse
    {
        public string accountStatus { get; set; }
        public decimal availableCredit { get; set; }
        public List<LoanOption> loanOptions { get; set; }
    }
}
