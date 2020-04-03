namespace FlowFinance.Models
{
    public class CapturePaymentRequest
    {
        public string transactionId { get; set; }
        public string paymentId { get; set; }
        public decimal value { get; set; }
        public string requestId { get; set; }
        public string authorizationId { get; set; }
    }
}
