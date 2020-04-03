namespace FlowFinance.Models
{
    public class RefundPaymentRequest
    {
        public string transactionId { get; set; }
        public string paymentId { get; set; }
        public string settleId { get; set; }
        public decimal value { get; set; }
        public string requestId { get; set; }
        public string authorizationId { get; set; }
    }
}
