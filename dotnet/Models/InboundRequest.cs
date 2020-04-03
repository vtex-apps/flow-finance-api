namespace FlowFinance.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RequestData
    {
        public string body { get; set; }
    }

    public class InboundRequest
    {
        public string requestId { get; set; }
        public string transactionId { get; set; }
        public string paymentId { get; set; }
        public string authorizationId { get; set; }
        public string tid { get; set; }
        public RequestData requestData { get; set; }
    }
}
