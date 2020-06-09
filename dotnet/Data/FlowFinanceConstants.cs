namespace FlowFinance.Data
{
    public class FlowFinanceConstants
    {
        public const string AppToken = "X-VTEX-API-AppToken";
        public const string AppKey = "X-VTEX-API-AppKey";
        public const string IsProduction = "X-Vtex-Workspace-Is-Production";
        public const string VtexCookieHeader = "Cookie";
        public const string VtexIdCookie = "VtexIdclientAutCookie";

        public const string MailService = "http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail";
        public const string PaymentApp = "vtex.flow-finance-payment";

        public const string FlowFinaceApiUrl = "http://api.flowfinance.com.br";
        public const string FlowFinaceStgApiUrl = "http://stg-gpp.flowfinance.com.br";
        public const string FlowFinaceApiUrlSecure = "https://api.flowfinance.com.br";
        public const string FlowFinaceStgApiUrlSecure = "https://stg-gpp.flowfinance.com.br";
        public const string FlowFinaceApiVersion = "/api/v1/";
        public const string OAuthLogin = "oauth/login";
        public const string OAuthToken = "oauth/token";
        public const string Accounts = "accounts";
        public const string Persons = "persons";
        public const string PreQualify = "pre-qualify";
        public const string LoanPreview = "loan-preview";
        public const string Loans = "loans";
        public const string WebhookEndpoints = "webhook-endpoints";

        public const string Acquirer = "FlowFinance";

        public const string CustomTokenField = "chosenLoanToken";
        public const string CustomTokenId = "flowfinance";

        public const string Success = "[Success]";

        public const string TokenUsedErrorMessage = "A loan matching this offer-token already exists.";
        public const string CreditLimitExceededErrorMessage = "Operation declined: exceeds your credit limit.";

        public const string FORWARDED_HEADER = "X-Forwarded-For";
        public const string FORWARDED_HOST = "X-Forwarded-Host";
        public const string APPLICATION_JSON = "application/json";
        public const string HEADER_VTEX_CREDENTIAL = "X-Vtex-Credential";
        public const string AUTHORIZATION_HEADER_NAME = "Authorization";
        public const string ACCOUNT_ID_HEADER_NAME = "account-id";
        public const string PROXY_AUTHORIZATION_HEADER_NAME = "Proxy-Authorization";
        public const string USE_HTTPS_HEADER_NAME = "X-Vtex-Use-Https";
        public const string PROXY_TO_HEADER_NAME = "X-Vtex-Proxy-To";
        public const string VTEX_ACCOUNT_HEADER_NAME = "X-Vtex-Account";
        public const string TEMPLATE_NAME_SUBMITTED = "flow-finance-submitted";
        public const string TEMPLATE_NAME_APPROVED = "flow-finance-approved";
        public const string TEMPLATE_NAME_DENIED = "flow-finance-denied";

        public class Inbound
        {
            public const string ActionLoanAcceptance = "loanAcceptance";
        }

        public class Vtex
        {
            public const string Approved = "approved";
            public const string Denied = "denied";
            public const string Undefined = "undefined";
        }

        // "account.created" "person.updated" "account.updated" "loan.updated" "loan.created" "person.created"
        public class WebHookNotification
        {
            public const string AccountCreated = "account.created";
            public const string AccountUpdated = "account.updated";
            public const string BusinessCreated = "business.created";
            public const string BusinessUpdated = "business.updated";
            public const string PersonCreated = "person.created";
            public const string PersonUpdated = "person.updated";
            public const string LoanCreated = "loan.created";
            public const string LoanUpdated = "loan.updated";
        }

        public class FlowFinanceStatus
        {
            public const string Pending = "pending";
            public const string Approved = "approved";
            public const string UnderReview = "under-review";
            public const string Denied = "rejected";
        }

        public class LoanStatus
        {
            public const string None = "none";  // This is not an expected status from FF but is being used to represent no account.
            public const string Pending = "pending";
            public const string Approved = "approved";
            public const string UnderReview = "under-review";
            public const string Denied = "rejected";
        }

        public class CultureInfo
        {
            public const string Brazil = "pt-BR";
        }
    }

    public enum MailTemplateType
    {
        Submitted,
        Approved,
        Denied
    }
}
