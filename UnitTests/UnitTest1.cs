using FlowFinance.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private const string FlowFinaceStgApiUrl = "https://stg-gpp.flowfinance.com.br/api/v1/";
        private const string APPLICATION_JSON = "application/json";
        private const string AUTHORIZATION_HEADER_NAME = "Authorization";
        string client_id = "3tuR1yohTkr8e4XDHAg9t";
        string client_secret = "422285fc6494dc1e5d26925693278fe6";
        //string cnpj = "56547844000101";
        //string cnpj = "66.005.388 / 0001-21";
        //string cnpj = "66005388000121";
        //string cnpj = "93909561000199";
        //string business_id = "969.303.065.007";

        string cnpj = "20.508.401/0001-64";
        string business_id = "969.576.762.929";

        string accessToken = "Bearer eyJhbGciOiJIUzUxMiJ9.eyJpZCI6MSwidHlwZSI6ImNsaWVudCIsImV4cCI6MTU4NTE3MjEyOX0.v-EBu9nb1qEyCSuth94fml1nBulavjoXM3IkiyGTXCeecwRr-8bPFs8PjGpUWlavylTfXcMejVVrwe9Lqn8XiA";

        [TestMethod]
        public void PreQualifyTestMethod()
        {
            FlowFinance.Models.PreQualifyRequest.RootObject preQualifyRequest = new FlowFinance.Models.PreQualifyRequest.RootObject
            {
                cnpj = cnpj
            };

            FlowFinance.Models.PreQualifyResponse.RootObject preQualifyResponse = new FlowFinance.Models.PreQualifyResponse.RootObject();

            string message = JsonConvert.SerializeObject(preQualifyRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{FlowFinaceStgApiUrl}{FlowFinanceConstants.PreQualify}"),
                Content = new StringContent(message, Encoding.UTF8, APPLICATION_JSON)
            };

            // Flow Headers
            request.Headers.Add(AUTHORIZATION_HEADER_NAME, GetToken());

            Console.WriteLine($" <=]|[=> Request : {request.Headers} : {request.Content.Headers} : {request.Method} : {request.Properties} : {request.RequestUri} <=]|[=> ");

            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;
            Console.WriteLine($"StatusCode: {response.StatusCode}");
            var reply = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Reply: {reply}");
            if (response.IsSuccessStatusCode)
            {
                preQualifyResponse = JsonConvert.DeserializeObject<FlowFinance.Models.PreQualifyResponse.RootObject>(reply);
                Console.WriteLine($"Eligible? {preQualifyResponse.data.eligible}");
            }
            else
            {
                FlowFinance.Models.ErrorResponse.RootObject errorResponse = JsonConvert.DeserializeObject<FlowFinance.Models.ErrorResponse.RootObject>(reply);
                if(errorResponse.error != null)
                {
                    Console.WriteLine($"Error: {errorResponse.error}");
                }

                //if(errorResponse.errors != null)
                //{
                //    foreach(FlowFinance.Models.ErrorResponse.Error error in errorResponse.errors)
                //    {
                //        Console.WriteLine($"Error(s): {error.title} {error.field} {error.message}");
                //    }
                //}

                Assert.Fail();
            }
        }

        [TestMethod]
        public void CreateAccountTestMethod()
        {
            FlowFinance.Models.CreateAccountResponse.RootObject createAccountResponse = new FlowFinance.Models.CreateAccountResponse.RootObject();

            FlowFinance.Models.CreateAccountRequest.RootObject createAccountRequest = new FlowFinance.Models.CreateAccountRequest.RootObject
            {
                //tos_acceptance = new FlowFinance.Models.CreateAccountRequest.TosAcceptance
                //{
                //    date = DateTime.Now,
                //    ip = "192.1.1.1",
                //    user_agent = "whatever"
                //},
                business = new FlowFinance.Models.CreateAccountRequest.Business
                {
                    address = new FlowFinance.Models.CreateAccountRequest.Address
                    {
                        city = "Santo André",
                        country = "BR",
                        district = "Silveira",
                        extra_address_info = "",
                        postal_code = "09121-360",
                        state_code = "SP",
                        street_name = "Rua Vinte e Um de Abril",
                        street_number = "1"
                    },
                    business_id = business_id,
                    contact_info = new FlowFinance.Models.CreateAccountRequest.ContactInfo
                    {
                        email = "contato@rafaeleraultelecomltda.com.br",
                        phone_number = "(11) 2840-2241"
                    },
                    documents = new FlowFinance.Models.CreateAccountRequest.Documents
                    {
                        physicalDocuments = new System.Collections.Generic.List<FlowFinance.Models.CreateAccountRequest.Physical>(),
                        virtualDocuments = new System.Collections.Generic.List<FlowFinance.Models.CreateAccountRequest.Virtual>()
                    },
                    legal_name = "Rafael e Raul Telecom Ltda",
                    name = "Rafael e Raul Telecom Ltda"
                }
            };

            string message = JsonConvert.SerializeObject(createAccountRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{FlowFinaceStgApiUrl}{FlowFinanceConstants.Accounts}"),
                Content = new StringContent(message, Encoding.UTF8, APPLICATION_JSON)
            };

            // Flow Headers
            request.Headers.Add(AUTHORIZATION_HEADER_NAME, GetToken());

            Console.WriteLine(request);

            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.StatusCode);
            var reply = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(reply);
            if(response.IsSuccessStatusCode)
            {
                createAccountResponse = JsonConvert.DeserializeObject<FlowFinance.Models.CreateAccountResponse.RootObject>(reply);
            }
            else
            {
                //FlowFinance.Models.ErrorResponse.ErrorResponse errorResponse = JsonConvert.DeserializeObject<FlowFinance.Models.ErrorResponse.ErrorResponse>(reply);
                //if (errorResponse.error != null)
                //{
                //    Console.WriteLine($"Error: {errorResponse.error}");
                //}

                //if (errorResponse.errors != null)
                //{
                //    foreach (FlowFinance.Models.ErrorResponse.Error error in errorResponse.errors)
                //    {
                //        Console.WriteLine($"Error(s): {error.title} {error.field} {error.message}");
                //    }
                //}

                Assert.Fail();
            }
        }

        [TestMethod]
        public void LoanOffersTest()
        {
            FlowFinance.Models.LoanPreviewResponse.RootObject loanPreviewResponse = new FlowFinance.Models.LoanPreviewResponse.RootObject();

            FlowFinance.Models.LoanPreviewRequest.RootObject loanPreviewRequest = new FlowFinance.Models.LoanPreviewRequest.RootObject
            {
                amount = 255.50m
            };

            string message = JsonConvert.SerializeObject(loanPreviewRequest);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{FlowFinaceStgApiUrl}{FlowFinanceConstants.LoanPreview}"),
                Content = new StringContent(message, Encoding.UTF8, APPLICATION_JSON)
            };

            // Flow Headers
            request.Headers.Add(AUTHORIZATION_HEADER_NAME, GetToken());
            request.Headers.Add("account-id", "7");

            //Console.WriteLine(request);

            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.StatusCode);
            var reply = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(reply);
            if (response.IsSuccessStatusCode)
            {
                loanPreviewResponse = JsonConvert.DeserializeObject<FlowFinance.Models.LoanPreviewResponse.RootObject>(reply);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LoginTestMethod()
        {
            FlowFinance.Models.OAuthResponse.RootObject oAuthResponse = new FlowFinance.Models.OAuthResponse.RootObject();

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{FlowFinaceStgApiUrl}{FlowFinanceConstants.OAuth}"),
                };

                // Flow Headers
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:{client_secret}")));

                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;
                Console.WriteLine(response.StatusCode);
                var reply = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(reply);
                if (response.IsSuccessStatusCode)
                {
                    oAuthResponse = JsonConvert.DeserializeObject<FlowFinance.Models.OAuthResponse.RootObject>(reply);
                    string accessToken = $"{oAuthResponse.data.token_type} {oAuthResponse.data.access_token}";
                    Console.WriteLine($"Access Token = {accessToken}");
                }
                else
                {
                    FlowFinance.Models.ErrorResponse.RootObject errorResponse = JsonConvert.DeserializeObject<FlowFinance.Models.ErrorResponse.RootObject>(reply);
                    if (errorResponse.error != null)
                    {
                        Console.WriteLine($"Error: {errorResponse.error}");
                    }

                    //if (errorResponse.errors != null)
                    //{
                    //    foreach (FlowFinance.Models.ErrorResponse.Error error in errorResponse.errors)
                    //    {
                    //        Console.WriteLine($"Error(s): {error.title} {error.field} {error.message}");
                    //    }
                    //}
                    Assert.Fail();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OAuthLogin Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestCreditParse()
        {
            string credit = "BRL 5000.00";
            decimal parsedCredit = 0m;

            if (!string.IsNullOrEmpty(credit))
            {
                string[] arrayAvailableCredit = credit.Split(' ');
                if(decimal.TryParse(arrayAvailableCredit[1], out parsedCredit))
                {
                    Console.WriteLine($"'{credit}' parsed to '{parsedCredit}'");
                }
                else
                {
                    Console.WriteLine($"Coould not parse '{credit}'");
                }
            }

            //CultureInfo cultureInfo = new CultureInfo("pt-BR");
            //if(decimal.TryParse(credit, NumberStyles.AllowCurrencySymbol, cultureInfo, out parsedCredit))
            //{
            //    Console.WriteLine($"'{credit}' parsed to '{parsedCredit}'");
            //}
            //else
            //{
            //    Console.WriteLine($"Coould not parse '{credit}'");
            //}
        }

        private string GetToken()
        {
            FlowFinance.Models.OAuthResponse.RootObject oAuthResponse = new FlowFinance.Models.OAuthResponse.RootObject();
            string accessToken = string.Empty;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{FlowFinaceStgApiUrl}{FlowFinanceConstants.OAuth}"),
                };

                // Flow Headers
                request.Headers.Add(AUTHORIZATION_HEADER_NAME, "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:{client_secret}")));

                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;
                Console.WriteLine(response.StatusCode);
                var reply = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(reply);
                if (response.IsSuccessStatusCode)
                {
                    oAuthResponse = JsonConvert.DeserializeObject<FlowFinance.Models.OAuthResponse.RootObject>(reply);
                    accessToken = $"{oAuthResponse.data.token_type} {oAuthResponse.data.access_token}";
                    Console.WriteLine($"Access Token = {accessToken}");
                }
                else
                {
                    FlowFinance.Models.ErrorResponse.RootObject errorResponse = JsonConvert.DeserializeObject<FlowFinance.Models.ErrorResponse.RootObject>(reply);
                    if (errorResponse.error != null)
                    {
                        Console.WriteLine($"Error: {errorResponse.error}");
                    }

                    //if (errorResponse.errors != null)
                    //{
                    //    foreach (FlowFinance.Models.ErrorResponse.Error error in errorResponse.errors)
                    //    {
                    //        Console.WriteLine($"Error(s): {error.title} {error.field} {error.message}");
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OAuthLogin Error: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}");
            }

            return accessToken;
        }
    }
}
