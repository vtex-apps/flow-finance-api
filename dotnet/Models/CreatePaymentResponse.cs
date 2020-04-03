namespace FlowFinance.Models
{
    public class PaymentAppData
    {
        public string appName { get; set; }
        public string payload { get; set; }
    }

    public class CreatePaymentResponse
    {
        /// <summary>
        /// The same paymentId sent in the request
        /// </summary>
        public string paymentId { get; set; }

        /// <summary>
        /// Provider's payment status:
        /// • approved
        /// • denied
        /// • undefined
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Provider's unique identifier for the transaction
        /// </summary>
        public string tid { get; set; }

        /// <summary>
        /// Provider's unique identifier for the authorization
        /// </summary>
        public string authorizationId { get; set; }

        /// <summary>
        /// Provider's unique sequential number for the transaction
        /// </summary>
        public string nsu { get; set; }

        /// <summary>
        /// Acquirer name (mostly used for card payments)
        /// </summary>
        public string acquirer { get; set; }

        /// <summary>
        /// The bank invoice URL to be presented to the end user,
        /// or the URL the end user needs to be redirected to (external authentication, 3DS, etc.)
        /// </summary>
        public string paymentUrl { get; set; }

        /// <summary>
        /// The bank invoice unformatted identification number
        /// </summary>
        public string identificationNumber { get; set; }

        /// <summary>
        /// The bank invoice formatted identification number that will be presented to the end user
        /// </summary>
        public string identificationNumberFormatted { get; set; }

        /// <summary>
        /// The bank invoice barcode image type: 
        /// • i25 for Brazilian Boleto Bancário
        /// </summary>
        public string barCodeImageType { get; set; }

        /// <summary>
        /// The bank invoice number to generate a barcode (must follow any regulations/specifications for targeted countries)
        /// </summary>
        public string barCodeImageNumber { get; set; }

        /// <summary>
        /// Provider's operation/error code to be logged
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Provider's operation/error message to be logged
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Total time (in seconds) before we make and automatic call to /settlements no mather if the payment was approved by merchant's antifraud or not
        /// </summary>
        public int delayToAutoSettle { get; set; }

        /// <summary>
        /// Total time (in seconds) before we make and automatic call to /settlements after merchant's antifraud approval
        /// </summary>
        public int delayToAutoSettleAfterAntifraud { get; set; }

        /// <summary>
        /// Total time (in seconds) to wait for an authorization and make and automatic call to /cancellations to cancel the payment
        /// </summary>
        public int delayToCancel { get; set; }

        /// <summary>
        /// Indicate the app that will handle the payment flow at Checkout
        /// </summary>
        public PaymentAppData paymentAppData { get; set; }
    }
}
