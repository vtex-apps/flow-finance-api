using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models
{
    public class ResponseData
    {
        public string statusCode { get; set; }
        public string body { get; set; }
    }

    public class InboundResponse
    {
        public string requestId { get; set; }
        public string paymentId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public ResponseData responseData { get; set; }
    }
}
