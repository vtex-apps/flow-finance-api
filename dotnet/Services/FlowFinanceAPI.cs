using FlowFinance.Data;
using FlowFinance.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlowFinance.Services
{
    public class FlowFinanceAPI : IFlowFinanceAPI
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        private readonly string client_id;
        private readonly string client_secret;
        private readonly string accessToken;
        private readonly string flowFinaceApiUrl;
        //private readonly string flowFinaceApiUrlSecure;

        public FlowFinanceAPI(IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, MerchantSettings merchantSettings)
        {
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            this.client_id = merchantSettings.clientId ?? throw new ArgumentNullException(nameof(merchantSettings.clientId));
            this.client_secret = merchantSettings.clientSecret ?? throw new ArgumentNullException(nameof(merchantSettings.clientSecret));
            this.flowFinaceApiUrl = $"{(merchantSettings.isLive ? FlowFinanceConstants.FlowFinaceApiUrl : FlowFinanceConstants.FlowFinaceStgApiUrl)}{FlowFinanceConstants.FlowFinaceApiVersion}";
            //this.flowFinaceApiUrlSecure = $"{(merchantSettings.isLive ? FlowFinanceConstants.FlowFinaceApiUrlSecure : FlowFinanceConstants.FlowFinaceStgApiUrlSecure)}{FlowFinanceConstants.FlowFinaceApiVersion}";
            //this.flowFinaceApiUrlSecure = merchantSettings.isLive ? FlowFinanceConstants.FlowFinaceApiUrlSecure : FlowFinanceConstants.FlowFinaceStgApiUrlSecure;

            this.accessToken = GetAccessToken().Result;
        }

        /// <summary>
        /// Send transaction request to Flow Finance
        /// </summary>
        /// <param name="method">HttpMethod to use when sending</param>
        /// <param name="endpoint">Url to send to</param>
        /// <param name="message">Optional message body to be sent</param>
        /// <param name="accountId">Optional account Id</param>
        /// <returns></returns>
        public async Task<ResponseWrapper> SendRequest(HttpMethod method, string endpoint, string message = null, string accountId = null)
        {
            //Console.WriteLine($" <=]|[=> SendRequest :{method.ToString()}: :{endpoint}: <=]|[=> ");
            //Console.WriteLine($" <=]|[=> :{message}: <=]|[=> ");

            ResponseWrapper responseWrapper = new ResponseWrapper();
            string responseContent = string.Empty;

            if (string.IsNullOrEmpty(this.accessToken))
            {
                responseWrapper.success = false;
                responseWrapper.errorMessage = "Token Failure.";
            }
            else
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = method,
                        RequestUri = new Uri($"{flowFinaceApiUrl}{endpoint}")
                    };

                    if (!string.IsNullOrEmpty(message))
                    {
                        request.Content = new StringContent(message, Encoding.UTF8, FlowFinanceConstants.APPLICATION_JSON);
                    }
                    //else
                    //{
                    //    Console.WriteLine("Empty Request Body");
                    //}

                    // Vtex headers
                    request.Headers.Add(FlowFinanceConstants.USE_HTTPS_HEADER_NAME, "true");
                    //request.Headers.Add(PROXY_TO_HEADER_NAME, flowFinaceApiUrlSecure);
                    request.Headers.Add(FlowFinanceConstants.PROXY_AUTHORIZATION_HEADER_NAME, _httpContextAccessor.HttpContext.Request.Headers[FlowFinanceConstants.HEADER_VTEX_CREDENTIAL].ToString());

                    // Flow Headers
                    request.Headers.Add(FlowFinanceConstants.AUTHORIZATION_HEADER_NAME, accessToken);
                    //Console.WriteLine($"Header '{AUTHORIZATION_HEADER_NAME}' set to '{accessToken}'");

                    if (!string.IsNullOrEmpty(accountId))
                    {
                        request.Headers.Add(FlowFinanceConstants.ACCOUNT_ID_HEADER_NAME, accountId);
                        //Console.WriteLine($"Set Account Id [{accountId}]");
                    }

                    //Console.WriteLine($" <=]|[=> Request : {request.Headers} : {request.Content.Headers} : {request.Method} : {request.Properties} : {request.RequestUri} <=]|[=> ");

                    HttpClient client = _clientFactory.CreateClient();
                    HttpResponseMessage responseMessage = await client.SendAsync(request);
                    //_httpContextAccessor.HttpContext.Response.StatusCode = (int)responseMessage.StatusCode;
                    //_httpContextAccessor.HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
                    responseContent = await responseMessage.Content.ReadAsStringAsync();

                    //Console.WriteLine($" <=]|[=> Response : {responseMessage.ReasonPhrase} : {responseMessage.Content.Headers} : {responseMessage.Headers} : {responseMessage.TrailingHeaders} : {responseContent} <=]|[=> ");
                    Console.WriteLine($" <=]|[=> Response : {responseMessage.ReasonPhrase} :|: {responseContent} <=]|[=> ");

                    responseWrapper.success = responseMessage.IsSuccessStatusCode;
                    responseWrapper.responseMessage = responseContent;

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        //Console.WriteLine($"Request - Headers :{request.Headers}: :{request.Method}:{request.RequestUri}:");

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            try
                            {
                                Models.ErrorResponse.RootObject errorResponse = JsonConvert.DeserializeObject<Models.ErrorResponse.RootObject>(responseContent);
                                if (errorResponse.error != null)
                                {
                                    //Console.WriteLine($"Flow Finance Response: Error: {errorResponse.error}");
                                    responseWrapper.errorMessage = $"Flow Finance Response: Error: {errorResponse.error}";
                                }

                                if (errorResponse.errors != null)
                                {
                                    responseWrapper.errorMessage = $"Flow Finance Response: Error(s): {errorResponse.errors}";

                                    //StringBuilder sb = new StringBuilder();
                                    //Dictionary<string, string> errorDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorResponse.errors.ToString());
                                    //foreach (var errorMessage in errorDictionary)
                                    //{
                                    //    Console.WriteLine($" <=]|[=> {errorMessage.Key} <=]|[=> ");
                                    //    sb.Append($" - {errorMessage.Key} {errorMessage.Value}");
                                    //}


                                    //foreach (Models.ErrorResponse.Error error in errorResponse.errors)
                                    //{
                                    //    //Console.WriteLine($"Flow Finance Response: Error(s): {error.title} {error.field} {error.message}");
                                    //    sb.Append($"{error.title} ({error.field})");
                                    //    Dictionary<string, string> messageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(error.message.ToString());
                                    //    foreach(var errorMessage in messageDictionary)
                                    //    {
                                    //        sb.Append($" - {errorMessage.Key} {errorMessage.Value}");
                                    //    }
                                    //}

                                    //responseWrapper.errorMessage = sb.ToString();
                                }

                                if (errorResponse.value != null)
                                {
                                    responseWrapper.responseMessage = errorResponse.value.ToString();
                                    //responseWrapper.success = true;
                                }

                                if (errorResponse.type != null)
                                {
                                    responseWrapper.errorMessage = $"Flow Finance Response: {errorResponse.type}: {errorResponse.errorClass}";
                                }

                                if (errorResponse.message != null)
                                {
                                    responseWrapper.errorMessage = $"Flow Finance Response: Message: {errorResponse.message}";
                                }
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine($"SendRequest Parse Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
                                //Console.WriteLine($"SendRequest Response Content: {responseContent}");
                                responseWrapper.errorMessage = $"SendRequest Parse Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace} Response: {responseContent}";
                            }
                        }
                        else
                        {
                            //Console.WriteLine($"SendRequest Response Content is Empty. Response Status: {responseMessage.ReasonPhrase}");
                            responseWrapper.errorMessage = $"SendRequest Response Content is Empty. Response Status: {responseMessage.ReasonPhrase}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"SendRequest Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
                    responseWrapper.errorMessage = $"SendRequest Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}";
                }
            }

            return responseWrapper;
        }

        /// <summary>
        /// Preview loan offers.
        /// POST /api/v1/loan-preview
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> LoanPreview(decimal amount, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            Models.LoanPreviewRequest.RootObject loanPreviewRequest = new Models.LoanPreviewRequest.RootObject
            {
                amount = amount
            };

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(loanPreviewRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, FlowFinanceConstants.LoanPreview, jsonSerializedRequest, accountId.ToString());
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.LoanPreviewResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoanPreview Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// This endpoint returns whether a business entity is eligible for applying for an account.
        /// POST /api/v1/pre-qualify
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> PreQualify(string cnpj)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            Models.PreQualifyRequest.RootObject preQualifyRequest = new Models.PreQualifyRequest.RootObject
            {
                cnpj = cnpj
            };

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(preQualifyRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, FlowFinanceConstants.PreQualify, jsonSerializedRequest);
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.PreQualifyResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PreQualify Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Create a new account.
        /// POST /api/v1/accounts
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseWrapper> CreateAccount(Models.CreateAccountRequest.RootObject createAccountRequest)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(createAccountRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, FlowFinanceConstants.Accounts, jsonSerializedRequest);
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.CreateAccountResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateAccount Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Retrieve the details of an existing account by account id.
        /// GET /api/v1/accounts/{account-id}
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> RetrieveAccountById(int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Get, $"{FlowFinanceConstants.Accounts}/{accountId}");
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.RetrieveAccountByIdResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RetrieveAccountById Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Retrieve the details of an existing loa by id.
        /// GET /api/v1/loans/{id}
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> RetrieveLoanById(string loanId, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Get, $"{FlowFinanceConstants.Loans}/{loanId}", null, accountId.ToString());
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.RetrieveLoanByIdResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RetrieveLoanById Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Retrieve all connected accounts.
        /// GET /api/v1/accounts
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> ListAccounts(int page, int limit)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Get, $"{FlowFinanceConstants.Accounts}?page={page}&limit={limit}");
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.ListAccountsResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ListAccounts Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Retrieve all persons associated with the account.
        /// GET /api/v1/accounts/{account-id}/persons
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> ListPersons(int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Get, $"{FlowFinanceConstants.Accounts}/{accountId}/{FlowFinanceConstants.Persons}");
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.ListPersonsResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ListPersons Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Create a loan
        /// POST /api/v1/loans
        /// </summary>
        /// <param name="offerToken"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> CreateLoan(string offerToken, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            Models.CreateLoanRequest.RootObject createLoanRequest = new Models.CreateLoanRequest.RootObject
            {
                offer_token = offerToken
            };

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(createLoanRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, FlowFinanceConstants.Loans, jsonSerializedRequest, accountId.ToString());
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.CreateLoanResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PreQualify Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Sign a loan.
        /// PATCH /api/v1/loans/{id}
        /// </summary>
        /// <param name="signLoanRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> SignLoan(Models.SignLoanRequest.RootObject signLoanRequest, string id, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(signLoanRequest);
                responseWrapper = await SendRequest(HttpMethod.Patch, $"{FlowFinanceConstants.Loans}/{id}", jsonSerializedRequest, accountId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignLoan Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Retrieve all your webhook endpoints.
        /// GET /api/v1/webhook-endpoints
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseWrapper> RetrieveWebhookEndpoints()
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Get, $"{FlowFinanceConstants.WebhookEndpoints}");
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.RetrieveWebhookEndpointsResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RetrieveWebhookEndpoints Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Create a new webhook endpoint.
        /// POST /api/v1/webhook-endpoints
        /// </summary>
        /// <param name="createWebhookEndpointRequest"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> CreateWebhookEndpoint(Models.CreateWebhookEndpointRequest.RootObject createWebhookEndpointRequest)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(createWebhookEndpointRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, FlowFinanceConstants.WebhookEndpoints, jsonSerializedRequest);
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.CreateWebhookEndpointResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateWebhookEndpoint Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Delete a webhook endpoint by id.
        /// DELETE /api/v1/webhook-endpoints/{webhook-id}
        /// </summary>
        /// <param name="webhookId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> DeleteWebhookEndpoint(int webhookId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Delete, $"{FlowFinanceConstants.WebhookEndpoints}/{webhookId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteWebhookEndpoint Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Delete an account by id.
        /// DELETE /api/v1/accounts/{account-id}
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> DeleteAccount(int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Delete, $"{FlowFinanceConstants.Accounts}/{accountId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteAccount Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Delete a person.
        /// DELETE /api/v1/accounts/{account-id}/persons/{person-id}
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> DeletePerson(int accountId, string personId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                responseWrapper = await SendRequest(HttpMethod.Delete, $"{FlowFinanceConstants.Accounts}/{accountId}/{FlowFinanceConstants.Persons}/{personId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeletePerson Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Generate token pair given username and password in the 'Authorization' header
        /// POST /api/v1/oauth/login
        /// </summary>
        /// <returns></returns>
        public async Task<Models.OAuthResponse.RootObject> OAuthLogin()
        {
            //Console.WriteLine("--] OAuthLogin [--");

            Models.OAuthResponse.RootObject oAuthResponse = null;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{flowFinaceApiUrl}{FlowFinanceConstants.OAuth}"),
                };

                // Flow Headers
                request.Headers.Add(FlowFinanceConstants.AUTHORIZATION_HEADER_NAME, "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:{client_secret}")));

                // Vtex headers
                request.Headers.Add(FlowFinanceConstants.USE_HTTPS_HEADER_NAME, "true");
                //request.Headers.Add(PROXY_TO_HEADER_NAME, flowFinaceApiUrlSecure);
                request.Headers.Add(FlowFinanceConstants.PROXY_AUTHORIZATION_HEADER_NAME, _httpContextAccessor.HttpContext.Request.Headers[FlowFinanceConstants.HEADER_VTEX_CREDENTIAL].ToString());

                HttpClient client = _clientFactory.CreateClient();
                HttpResponseMessage responseMessage = await client.SendAsync(request);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                //Console.WriteLine($"OAuthLogin Response Content {responseContent}");
                oAuthResponse = JsonConvert.DeserializeObject<Models.OAuthResponse.RootObject>(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OAuthLogin Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return oAuthResponse;
        }

        public async Task<string> GetAccessToken()
        {
            string token = string.Empty;

            Models.OAuthResponse.RootObject oAuthResponse = await OAuthLogin();
            if(oAuthResponse != null && oAuthResponse.data != null && !string.IsNullOrEmpty(oAuthResponse.data.access_token))
            {
                token = $"{oAuthResponse.data.token_type} {oAuthResponse.data.access_token}";

                //Console.WriteLine($"GetAccessToken Success. {oAuthResponse.data.token_type} {oAuthResponse.data.expires_in}");
            }
            else
            {
                Console.WriteLine("GetAccessToken Failure.");
                //throw new Exception("GetAccessToken Failure.");
            }

            return token;
        }

        /// <summary>
        /// Create a new person.
        /// POST /api/v1/accounts/{account-id}/persons
        /// </summary>
        /// <param name="createPersonRequest"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> CreatePerson(Models.CreatePersonRequest.RootObject createPersonRequest, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(createPersonRequest);
                responseWrapper = await SendRequest(HttpMethod.Post, $"{FlowFinanceConstants.Accounts}/{accountId}/{FlowFinanceConstants.Persons}", jsonSerializedRequest);
                if (responseWrapper.success)
                {
                    responseWrapper.responseObject = JsonConvert.DeserializeObject<Models.CreatePersonResponse.RootObject>(responseWrapper.responseMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreatePerson Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Update one or many fields of an existing person.
        /// PATCH /api/v1/accounts/{account-id}/persons/{person-id}
        /// </summary>
        /// <param name="updatePersonRequest"></param>
        /// <param name="accountId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> UpdatePerson(Models.CreatePersonRequest.RootObject updatePersonRequest, int accountId, int personId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(updatePersonRequest);
                responseWrapper = await SendRequest(HttpMethod.Patch, $"{FlowFinanceConstants.Accounts}/{accountId}/{FlowFinanceConstants.Persons}/{personId}", jsonSerializedRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdatePerson Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }

        /// <summary>
        /// Update one or many fields of an existing account.
        /// PATCH /api/v1/accounts/{account-id}
        /// </summary>
        /// <param name="updateAccountRequest"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseWrapper> UpdateAccount(Models.UpdateAccountRequest.RootObject updateAccountRequest, int accountId)
        {
            ResponseWrapper responseWrapper = new ResponseWrapper();

            try
            {
                var jsonSerializedRequest = JsonConvert.SerializeObject(updateAccountRequest);
                responseWrapper = await SendRequest(HttpMethod.Patch, $"{FlowFinanceConstants.Accounts}/{accountId}", jsonSerializedRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateAccount Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return responseWrapper;
        }
    }
}