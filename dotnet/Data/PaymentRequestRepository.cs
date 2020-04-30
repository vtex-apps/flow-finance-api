namespace FlowFinance.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using FlowFinance.Models;
    using FlowFinance.Services;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Concrete implementation of <see cref="IPaymentRequestRepository"/> for persisting data to/from vbase.
    /// </summary>
    public class PaymentRequestRepository : IPaymentRequestRepository
    {
        private const string SETTINGS_NAME = "merchantSettings";
        private const string BUCKET = "paymentRequest";
        private const string SHOPPER_BUCKET = "shoppers";
        private const string HEADER_VTEX_CREDENTIAL = "X-Vtex-Credential";
        private const string HEADER_VTEX_WORKSPACE = "X-Vtex-Workspace";
        private const string HEADER_VTEX_ACCOUNT = "X-Vtex-Account";
        private const string APPLICATION_JSON = "application/json";
        private const string APP_SETTINGS = "vtex.flow-finance-api";
        private const string ENVIRONMENT = "vtexcommercestable";
        private const string AUTHORIZATION_HEADER_NAME = "Authorization";
        private const string VTEX_ID_HEADER_NAME = "VtexIdclientAutCookie";
        private readonly IVtexEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _applicationName;


        public PaymentRequestRepository(IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {
            this._environmentVariableProvider = environmentVariableProvider ??
                                                throw new ArgumentNullException(nameof(environmentVariableProvider));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                               throw new ArgumentNullException(nameof(clientFactory));

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";
        }


        public async Task<CreatePaymentRequest> GetPaymentRequestAsync(string paymentIdentifier)
        {
            Console.WriteLine($"GetPaymentRequestAsync called with {this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]},master,{this._environmentVariableProvider.ApplicationName},{this._environmentVariableProvider.ApplicationVendor},{this._environmentVariableProvider.Region}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/master/buckets/{this._applicationName}/{BUCKET}/files/{paymentIdentifier}"),
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // A helper method is in order for this as it does not return the stack trace etc.
            response.EnsureSuccessStatusCode();

            CreatePaymentRequest paymentRequest =  JsonConvert.DeserializeObject<CreatePaymentRequest>(responseContent);
            return paymentRequest;
        }

        public async Task SavePaymentRequestAsync(string paymentIdentifier, CreatePaymentRequest createPaymentRequest)
        {
            if (createPaymentRequest == null)
            {
                createPaymentRequest = new CreatePaymentRequest();
            }

            var jsonSerializedCreatePaymentRequest = JsonConvert.SerializeObject(createPaymentRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/master/buckets/{this._applicationName}/{BUCKET}/files/{paymentIdentifier}"),
                Content = new StringContent(jsonSerializedCreatePaymentRequest, Encoding.UTF8, APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task PostCallbackResponse(string callbackUrl, CreatePaymentResponse createPaymentResponse)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                // Internal vtex posts must be to http with use https header
                //callbackUrl = callbackUrl.Replace("https", "http");

                var jsonSerializedPaymentResponse = JsonConvert.SerializeObject(createPaymentResponse);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(callbackUrl),
                    Content = new StringContent(jsonSerializedPaymentResponse, Encoding.UTF8, APPLICATION_JSON)
                };

                string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
                if (authToken != null)
                {
                    request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                    request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
                }

                var client = _clientFactory.CreateClient();
                response = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PostCallbackResponse to '{callbackUrl}' Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }


            Console.WriteLine($"PostCallbackResponse to '{callbackUrl}' status [{response.StatusCode}]");
            //response.EnsureSuccessStatusCode();
        }

        public async Task<MerchantSettings> GetMerchantSettings()
        {
            // Load merchant settings
            // 'http://apps.${region}.vtex.io/${account}/${workspace}/apps/${vendor.appName}/settings'
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_WORKSPACE]}/apps/{APP_SETTINGS}/settings"),
            };

            Console.WriteLine($"Request URL = {request.RequestUri}");

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Response = {responseContent}");

            return JsonConvert.DeserializeObject<MerchantSettings>(responseContent);
        }

        public async Task SetMerchantSettings(MerchantSettings merchantSettings)
        {
            if (merchantSettings == null)
            {
                merchantSettings = new MerchantSettings();
            }

            var jsonSerializedMerchantSettings = JsonConvert.SerializeObject(merchantSettings);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_WORKSPACE]}/apps/{APP_SETTINGS}/settings"),
                Content = new StringContent(jsonSerializedMerchantSettings, Encoding.UTF8, APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<IList<FlowFinanceShopper>> GetFlowFinanceShoppers()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/master/buckets/{this._applicationName}/{SHOPPER_BUCKET}/files/{SHOPPER_BUCKET}"),
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // A helper method is in order for this as it does not return the stack trace etc.
            response.EnsureSuccessStatusCode();

            IList<FlowFinanceShopper> shoppers = JsonConvert.DeserializeObject<IList<FlowFinanceShopper>>(responseContent);

            return shoppers;
        }

        public async Task<bool> SaveFlowFinanceShoppers(IList<FlowFinanceShopper> shoppers)
        {
            if (shoppers == null)
            {
                shoppers = new List<FlowFinanceShopper>();
            }

            var jsonSerializedShopper = JsonConvert.SerializeObject(shoppers);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}/master/buckets/{this._applicationName}/{SHOPPER_BUCKET}/files/{SHOPPER_BUCKET}"),
                Content = new StringContent(jsonSerializedShopper, Encoding.UTF8, APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<OrderInformation> GetOrderInformation(string orderId)
        {
            ///http://{{accountName}}.vtexcommercestable.com.br/api/checkout/pvt/orders/order-group/727553409798
            ///http://{{accountName}}.vtexcommercestable.com.br/api/checkout/pvt/orders/604441449285-01

            OrderInformation orderInformation = new OrderInformation { hasError = true, message = "Unknown Error." };
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}.{ENVIRONMENT}.com.br/api/checkout/pvt/orders/{orderId}-01")
                    //RequestUri = new Uri($"https://{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}.{ENVIRONMENT}.com.br/api/checkout/pvt/orders/order-group/{orderId}")
                };

                string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
                if (authToken != null)
                {
                    request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                    request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
                }

                //StringBuilder sb = new StringBuilder();

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    orderInformation.hasError = false;

                    Models.VtexOrder.VtexOrder vtexOrder = JsonConvert.DeserializeObject<Models.VtexOrder.VtexOrder>(responseContent);
                    if (vtexOrder.customData != null)
                    {
                        Models.VtexOrder.CustomApp customApp = vtexOrder.customData.customApps.Where(c => c.id.Equals(FlowFinanceConstants.CustomTokenId)).FirstOrDefault();
                        if (customApp != null)
                        {
                            orderInformation.offerToken = customApp.fields.chosenLoanToken;
                            orderInformation.email = vtexOrder.clientProfileData.email;
                        }
                        else
                        {
                            orderInformation.hasError = true;
                            orderInformation.message = $"Custom App not Found. {JsonConvert.SerializeObject(vtexOrder.customData)}";
                        }
                    }
                    else
                    {
                        orderInformation.hasError = true;
                        orderInformation.message = "Custom Data field is Null.";
                    }
                }
                else
                {
                    orderInformation.hasError = true;
                    orderInformation.message = $"Reason: '{response.ReasonPhrase}'";
                }
            }
            catch(Exception ex)
            {
                orderInformation.hasError = true;
                orderInformation.message = $"Error: {ex.Message} {ex.InnerException}";
            }

            return orderInformation;
        }

        /// <summary>
        /// Returns the current order configuration as a json string
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrderConfiguration()
        {
            // https://{{accountName}}.vtexcommercestable.com.br/api/checkout/pvt/configuration/orderForm
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}.{ENVIRONMENT}.com.br/api/checkout/pvt/configuration/orderForm"),
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // A helper method is in order for this as it does not return the stack trace etc.
            response.EnsureSuccessStatusCode();

            return responseContent;
        }

        public async Task<bool> SetOrderConfiguration(string jsonSerializedOrderConfig)
        {
            // https://{{accountName}}.vtexcommercestable.com.br/api/checkout/pvt/configuration/orderForm
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_ACCOUNT]}.{ENVIRONMENT}.com.br/api/checkout/pvt/configuration/orderForm"),
                Content = new StringContent(jsonSerializedOrderConfig, Encoding.UTF8, APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }
    }
}
