namespace FlowFinance.Controllers
{
    using FlowFinance.Data;
    using FlowFinance.Models;
    using FlowFinance.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoutesController : Controller
    {
        private readonly IFlowFinancePaymentService _flowFinancePaymentService;

        public RoutesController(IFlowFinancePaymentService flowFinancePaymentService)
        {
            this._flowFinancePaymentService = flowFinancePaymentService ?? throw new ArgumentNullException(nameof(flowFinancePaymentService));
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments
        /// Creates a new payment and/or initiates the payment flow.
        /// </summary>
        /// <param name="createPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreatePayment()
        {
            try
            {
                //Console.WriteLine("-][--][--][--][--][--][--][--][--][--][--][- CreatePaymentAsync -][--][--][--][--][--][--][--][--][--][--][-");
                var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                CreatePaymentRequest createPaymentRequest = JsonConvert.DeserializeObject<CreatePaymentRequest>(bodyAsText);
                var paymentResponse = await this._flowFinancePaymentService.CreatePaymentAsync(createPaymentRequest);

                Response.Headers.Add("Cache-Control", "private");
                return Json(paymentResponse);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/cancellations
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="cancelPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelPayment(string paymentId)
        {
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CancelPaymentRequest cancelPaymentRequest = JsonConvert.DeserializeObject<CancelPaymentRequest>(bodyAsText);
            var cancelResponse = await this._flowFinancePaymentService.CancelPaymentAsync(cancelPaymentRequest);

            return Json(cancelResponse);
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/settlements
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="capturePaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CapturePayment(string paymentId)
        {
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CapturePaymentRequest capturePaymentRequest = JsonConvert.DeserializeObject<CapturePaymentRequest>(bodyAsText);
            var captureResponse = await this._flowFinancePaymentService.CapturePaymentAsync(capturePaymentRequest);

            return Json(captureResponse);
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/refunds
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> RefundPayment(string paymentId)
        {
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            RefundPaymentRequest refundPaymentRequest = JsonConvert.DeserializeObject<RefundPaymentRequest>(bodyAsText);
            var refundResponse = await this._flowFinancePaymentService.RefundPaymentAsync(refundPaymentRequest);

            return Json(refundResponse);
        }

        /// <summary>
        /// Retrieve stored payment request
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> GetPaymentRequest(string paymentIdentifier)
        {
            var paymentRequest = await this._flowFinancePaymentService.GetCreatePaymentRequestAsync(paymentIdentifier);

            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentRequest);
        }

        /// <summary>
        /// Read the charge information, current charge status, and checkout data
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReadCharge(string paymentIdentifier)
        {
            var paymentRequest = await this._flowFinancePaymentService.ReadChargeAsync(paymentIdentifier);
            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentRequest);
        }

        public async Task<IActionResult> Inbound(string actiontype)
        {
            Console.WriteLine($"InboundAsync action = {actiontype}");

            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            string responseStatusCode = string.Empty;
            string responseBody = string.Empty;

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            InboundRequest inboundRequest = JsonConvert.DeserializeObject<InboundRequest>(bodyAsText);
            dynamic inboundRequestBody = null;
            try
            {
                inboundRequestBody = JsonConvert.DeserializeObject(inboundRequest.requestData.body);
            }
            catch(Exception ex)
            {
                responseMessage = ex.Message;
            }

            string paymentId = inboundRequest.paymentId;
            string requestId = inboundRequest.requestId;

            if(inboundRequestBody == null)
            {
                responseStatusCode = StatusCodes.Status400BadRequest.ToString();
            }
            else
            {
                switch(actiontype)
                {
                    case FlowFinanceConstants.Inbound.ActionLoanAcceptance:
                        Models.SignLoanRequest.RootObject signLoanRequest = new Models.SignLoanRequest.RootObject
                        {
                            signature = new Models.SignLoanRequest.Signature
                            {
                                date = DateTime.Now,
                                userAgent = inboundRequestBody.userAgent,
                                ip = await this._flowFinancePaymentService.GetShopperIp()
                            }
                        };

                        responseBody = await this._flowFinancePaymentService.SignLoan(signLoanRequest, inboundRequestBody.loanId, inboundRequestBody.accountId);
                        if (responseBody.Equals(FlowFinanceConstants.Success))
                        {
                            responseStatusCode = StatusCodes.Status200OK.ToString();
                            // Verify that the loan is signed and update the status with Vtex Payment
                            this._flowFinancePaymentService.VerifyLoanAsync(paymentId, inboundRequestBody.loanId, inboundRequestBody.accountId, inboundRequestBody.callbackUrl, inboundRequestBody.amount);
                        }

                        break;
                    default:
                        responseStatusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                        responseMessage = $"Action '{actiontype}' is not supported.";
                        break;
                }
            }

            InboundResponse response = new InboundResponse
            {
                code = responseCode,
                message = responseMessage,
                paymentId = paymentId,
                requestId = requestId,
                responseData = new ResponseData
                {
                    body = responseBody,
                    statusCode = responseStatusCode
                }
            };

            return Json(response);
        }

        public async Task<IActionResult> GetLoanOptions()
        {
            GetLoanOptionsResponse getLoanOptionsResponse = null;
            string bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(bodyAsText))
            {
                GetLoanOptionsRequest getLoanOptionsRequest = JsonConvert.DeserializeObject<GetLoanOptionsRequest>(bodyAsText);
                getLoanOptionsResponse = await this._flowFinancePaymentService.GetLoanOptions(getLoanOptionsRequest);
                Response.Headers.Add("Cache-Control", "private");
            }

            return Json(getLoanOptionsResponse);
        }

        public async Task<IActionResult> ListAccounts(int page, int limit)
        {
            //Console.WriteLine($"ListAccounts page={page} limit={limit}");
            Models.ListAccountsResponse.RootObject listAccountsResponse = await this._flowFinancePaymentService.ListAccounts(page, limit);
            Response.Headers.Add("Cache-Control", "private");

            return Json(listAccountsResponse);
        }

        public async Task<IActionResult> ListPersons(int accountId)
        {
            Console.WriteLine("ListPersons");
            Models.ListPersonsResponse.RootObject listPersonsResponse = await this._flowFinancePaymentService.ListPersons(accountId);
            Response.Headers.Add("Cache-Control", "private");

            return Json(listPersonsResponse);
        }

        public async Task<IList<FlowFinanceShopper>> ListShoppers()
        {
            return await this._flowFinancePaymentService.ListShoppers();
        }

        public async Task<IActionResult> GetAppSettings()
        {
            MerchantSettings paymentRequest = await this._flowFinancePaymentService.GetSettingsAsync();

            return Json(paymentRequest);
        }

        public async Task ProcessCallback()
        {
            Console.WriteLine($"ProcessCallback");
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //Console.WriteLine($"Body=[{bodyAsText}]");
            //dynamic callbackPayload = JsonConvert.DeserializeObject(bodyAsText);
            Models.WebhookPayload.RootObject callbackPayload = JsonConvert.DeserializeObject<Models.WebhookPayload.RootObject>(bodyAsText);
            //Console.WriteLine($"payload=[{callbackPayload}]");
            //string callbackEvent = callbackPayload.Data.Event;
            //string entityType = callbackPayload.Data.Entity_type;
            //dynamic entity = callbackPayload.Data.Entity;
            await this._flowFinancePaymentService.ProcessCallback(callbackPayload);
        }

        public async Task<IActionResult> CreateWebhook()
        {
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Models.CreateWebhookEndpointRequest.RootObject createWebhookEndpointRequest = JsonConvert.DeserializeObject<Models.CreateWebhookEndpointRequest.RootObject>(bodyAsText);
            //Models.CreateWebhookEndpointResponse.RootObject createWebhookEndpointResponse = await this._flowFinancePaymentService.CreateWebhook(createWebhookEndpointRequest)
            return Json(await this._flowFinancePaymentService.CreateWebhook(createWebhookEndpointRequest));
        }

        public async Task<IActionResult> DeleteWebhook(int webhookId)
        {
            return Json(await this._flowFinancePaymentService.DeleteWebhookEndpoint(webhookId));
        }

        public async Task<IActionResult> InitWebhooks()
        {
            return Json(await this._flowFinancePaymentService.InitWebhooks());
        }

        public async Task<IActionResult> InitConfiguration()
        {
            return Json(await this._flowFinancePaymentService.InitConfiguration());
        }

        public async Task<IActionResult> CreateLoan(string offerToken, int accountId)
        {
            Console.WriteLine("-> CreateLoan <-");
            Response.Headers.Add("Cache-Control", "private");
            return Json(await this._flowFinancePaymentService.CreateLoan(offerToken, accountId));
        }

        public async Task<IActionResult> SignLoan(string loanId, int accountId)
        {
            Console.WriteLine("-> CreateLoan <-");
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //Console.WriteLine($"Body=[{bodyAsText}]");
            //dynamic callbackPayload = JsonConvert.DeserializeObject(bodyAsText);
            Models.SignLoanRequest.RootObject signLoanRequest = JsonConvert.DeserializeObject<Models.SignLoanRequest.RootObject>(bodyAsText);
            Response.Headers.Add("Cache-Control", "private");
            return Json(await this._flowFinancePaymentService.SignLoan(signLoanRequest, loanId, accountId));
        }

        public async Task<IActionResult> RetrieveLoanById(string loanId, int accountId)
        {
            Console.WriteLine("-> RetrieveLoanById <-");
            Response.Headers.Add("Cache-Control", "private");
            return Json(await this._flowFinancePaymentService.RetrieveLoanById(loanId, accountId));
        }

        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            Console.WriteLine($"-> DeleteAccount {accountId} <-");
            Response.Headers.Add("Cache-Control", "private");
            return Json(await this._flowFinancePaymentService.DeleteAccount(accountId));
        }

        public async Task<IActionResult> DeletePerson(int accountId, string personId)
        {
            Console.WriteLine($"-> DeletePerson {personId} <-");
            Response.Headers.Add("Cache-Control", "private");
            return Json(await this._flowFinancePaymentService.DeletePerson(accountId, personId));
        }

        public JsonResult PaymentMethods()
        {
            PaymentMethods methods = new PaymentMethods();
            methods.paymentMethods = new List<string>();
            methods.paymentMethods.Add("FlowFinance");
            methods.paymentMethods.Add("Promissories");

            Response.Headers.Add("Cache-Control", "private");

            return Json(methods);
        }

        public string PrintHeaders()
        {
            string headers = "--->>> Headers <<<---\n";
            foreach (var header in HttpContext.Request.Headers)
            {
                headers += $"{header.Key}: {header.Value}\n";
            }
            return headers;
        }
    }
}
