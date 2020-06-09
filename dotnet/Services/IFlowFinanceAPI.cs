using FlowFinance.Models;
using System.Threading.Tasks;

namespace FlowFinance.Services
{
    internal interface IFlowFinanceAPI
    {
        Task<ResponseWrapper> PreQualify(string cnpj);
        Task<ResponseWrapper> CreateAccount(Models.CreateAccountRequest.RootObject createAccountRequest);
        Task<ResponseWrapper> LoanPreview(decimal amount, int accountId);
        Task<ResponseWrapper> CreateLoan(string offerToken, int accountId);
        Task<ResponseWrapper> RetrieveAccountById(int accountId);
        Task<Models.OAuthResponse.RootObject> OAuthLogin();
        Task<string> GetAccessToken(string refreshToken);
        Task<ResponseWrapper> SignLoan(Models.SignLoanRequest.RootObject signLoanRequest, string id, int accountId);
        Task<ResponseWrapper> CreatePerson(Models.CreatePersonRequest.RootObject createPersonRequest, int accountId);
        Task<ResponseWrapper> UpdatePerson(Models.CreatePersonRequest.RootObject updatePersonRequest, int accountId, int personId);
        Task<ResponseWrapper> ListAccounts(int page, int limit);
        Task<ResponseWrapper> ListPersons(int accountId);
        Task<ResponseWrapper> CreateWebhookEndpoint(Models.CreateWebhookEndpointRequest.RootObject createWebhookEndpointRequest);
        Task<ResponseWrapper> RetrieveWebhookEndpoints();
        Task<ResponseWrapper> UpdateAccount(Models.UpdateAccountRequest.RootObject updateAccountRequest, int accountId);
        Task<ResponseWrapper> RetrieveLoanById(string loanId, int accountId);
        Task<ResponseWrapper> DeleteWebhookEndpoint(int webhookId);
        Task<ResponseWrapper> DeleteAccount(int accountId);
        Task<ResponseWrapper> DeletePerson(int accountId, string personId);
        Task<ResponseWrapper> RetrieveAllLoans(int accountId);
        Task GetToken();
    }
}