namespace FlowFinance.Models
{
    public class CancelPaymentResponse
    {
        public string paymentId { get; set; }
        public string cancellationId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
    }
}
