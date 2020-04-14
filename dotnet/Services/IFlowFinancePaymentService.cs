using System.Collections.Generic;
using System.Threading.Tasks;
using FlowFinance.Data;
using FlowFinance.GraphQL.Types;
using FlowFinance.Models;

namespace FlowFinance.Services
{
    public interface IFlowFinancePaymentService
    {
        Task<CreatePaymentResponse> VerifyLoanAsync(string paymentIdentifier, string loanId, int accountId, string callbackUrl, int amount);
        Task<CancelPaymentResponse> CancelPaymentAsync(CancelPaymentRequest cancelPaymentRequest);
        Task<CapturePaymentResponse> CapturePaymentAsync(CapturePaymentRequest capturePaymentRequest);
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest);
        Task<CreatePaymentRequest> GetCreatePaymentRequestAsync(string paymentIdentifier);
        Task<MerchantSettings> GetSettingsAsync();
        Task<CreatePaymentResponse> ReadChargeAsync(string paymentId);
        Task<RefundPaymentResponse> RefundPaymentAsync(RefundPaymentRequest refundPaymentRequest);

        Task<bool> PreQualify(string cnpj);
        Task<Models.CreateAccountResponse.RootObject> CreateAccount(Models.CreateAccountRequest.RootObject createAccountRequest);
        Task<GetLoanOptionsResponse> GetLoanOptions(GetLoanOptionsRequest getLoanOptionsRequest);
        Task SetAccountId(FlowFinanceShopper shopper);
        Task ProcessCallback(Models.WebhookPayload.RootObject callbackPayload);
        Task<ApplicationResult> ProcessApplication(ApplicationInput applicationInput);
        Task<Models.ListAccountsResponse.RootObject> ListAccounts(int page, int limit);
        Task<Models.ListPersonsResponse.RootObject> ListPersons(int accountId);
        Task<string> SignLoan(Models.SignLoanRequest.RootObject signLoanRequest, string loanId, int accountId);
        Task<Models.CreateWebhookEndpointResponse.RootObject> CreateWebhook(Models.CreateWebhookEndpointRequest.RootObject createWebhookRequest);
        Task<Models.CreateLoanResponse.RootObject> CreateLoan(string offerToken, int accountId);
        Task<Models.LoanPreviewResponse.RootObject> LoanPreview(int amount, int accountId);
        Task<Models.RetrieveLoanByIdResponse.RootObject> RetrieveLoanById(string loanId, int accountId);
        Task<string> DeleteWebhookEndpoint(int webhookId);
        Task<string> InitWebhooks(string siteRoot);

        Task<IList<FlowFinanceShopper>> ListShoppers();
        Task<string> SendEmail(string to, MailTemplateType templateType);
        Task<string> GetShopperIp();
    }
}