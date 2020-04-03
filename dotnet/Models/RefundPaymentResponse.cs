namespace FlowFinance.Models
{
    public class RefundPaymentResponse
    {
        public string paymentId { get; set; }
        public string refundId { get; set; }
        public decimal value { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
    }
}
