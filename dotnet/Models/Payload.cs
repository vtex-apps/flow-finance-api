using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class Payload
    {
        public string inboundRequestsUrl { get; set; }
        public string callbackUrl { get; set; }
        public string paymentIdentifier { get; set; }
        public string publicKey { get; set; }
        public string pdfUrl { get; set; }
        public string loanId { get; set; }
        public string transactionId { get; set; }
        public int accountId { get; set; }
    }
}
