﻿namespace FlowFinance.Controllers
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
        public async Task<IActionResult> CreatePaymentAsync()
        {
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CreatePaymentRequest createPaymentRequest = JsonConvert.DeserializeObject<CreatePaymentRequest>(bodyAsText);
            var paymentResponse = await this._flowFinancePaymentService.CreatePaymentAsync(createPaymentRequest);

            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentResponse);
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/cancellations
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="cancelPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelPaymentAsync(string paymentId)
        {
            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CancelPaymentRequest cancelPaymentRequest = JsonConvert.DeserializeObject<CancelPaymentRequest>(bodyAsText);

            //if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    return BadRequest();
            //}
            //else
            //{
                var cancelResponse = await this._flowFinancePaymentService.CancelPaymentAsync(cancelPaymentRequest);

                return Json(cancelResponse);
            //}
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/settlements
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="capturePaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CapturePaymentAsync(string paymentId)
        {
            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CapturePaymentRequest capturePaymentRequest = JsonConvert.DeserializeObject<CapturePaymentRequest>(bodyAsText);

            //if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    return BadRequest();
            //}
            //else
            //{
                var captureResponse = await this._flowFinancePaymentService.CapturePaymentAsync(capturePaymentRequest);

                return Json(captureResponse);
            //}
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/refunds
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> RefundPaymentAsync(string paymentId)
        {
            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            RefundPaymentRequest refundPaymentRequest = JsonConvert.DeserializeObject<RefundPaymentRequest>(bodyAsText);

            //if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    return BadRequest();
            //}
            //else
            //{
                var refundResponse = await this._flowFinancePaymentService.RefundPaymentAsync(refundPaymentRequest);

                return Json(refundResponse);
            //}
        }

        /// <summary>
        /// Retrieve stored payment request
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> GetPaymentRequestAsync(string paymentIdentifier)
        {
            var paymentRequest = await this._flowFinancePaymentService.GetCreatePaymentRequestAsync(paymentIdentifier);

            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentRequest);
        }

        /// <summary>
        /// After completing the checkout flow and receiving the checkout token, authorize the charge.
        /// Authorizing generates a charge ID that you’ll use to reference the charge moving forward.
        /// You must authorize a charge to fully create it. A charge is not visible in the Read response,
        /// nor in the merchant dashboard until you authorize it.
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthorizeAsync(string paymentIdentifier, string token, string callbackUrl, int orderTotal)
        {
            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];

            //if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    return BadRequest();
            //}
            //else
            //{
                var paymentRequest = await this._flowFinancePaymentService.AuthorizeAsync(paymentIdentifier, token, callbackUrl, orderTotal, string.Empty);
                Response.Headers.Add("Cache-Control", "private");

                return Json(paymentRequest);
            //}
        }

        /// <summary>
        /// Read the charge information, current charge status, and checkout data
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReadChargeAsync(string paymentIdentifier)
        {
            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];

            //if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    return BadRequest();
            //}
            //else
            //{
                var paymentRequest = await this._flowFinancePaymentService.ReadChargeAsync(paymentIdentifier);
                Response.Headers.Add("Cache-Control", "private");

                return Json(paymentRequest);
            //}
        }

        public async Task<IActionResult> InboundAsync(string actiontype)
        {
            Console.WriteLine($"InboundAsync action = {actiontype}");

            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            string responseStatusCode = string.Empty;
            string responseBody = string.Empty;

            //string privateKey = HttpContext.Request.Headers[FlowFinanceConstants.PrivateKeyHeader];
            //string publicKey = HttpContext.Request.Headers[FlowFinanceConstants.PublicKeyHeader];
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
            //else if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            //{
            //    responseStatusCode = StatusCodes.Status400BadRequest.ToString();
            //    responseMessage = "Missing keys.";
            //}
            else
            {
                switch(actiontype)
                {
                    case FlowFinanceConstants.Inbound.ActionAuthorize:
                        string token = inboundRequestBody.token;
                        string callbackUrl = inboundRequestBody.callbackUrl;
                        int amount = inboundRequestBody.orderTotal;
                        string orderId = inboundRequestBody.orderId;
                        if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(callbackUrl))
                        {
                            responseStatusCode = StatusCodes.Status400BadRequest.ToString();
                            responseMessage = "Missing parameters.";
                        }
                        else
                        {
                            var paymentRequest = await this._flowFinancePaymentService.AuthorizeAsync(paymentId, token, callbackUrl, amount, orderId);
                            Response.Headers.Add("Cache-Control", "private");

                            responseBody = JsonConvert.SerializeObject(paymentRequest);
                            responseStatusCode = StatusCodes.Status200OK.ToString();
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
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            GetLoanOptionsRequest getLoanOptionsRequest = JsonConvert.DeserializeObject<GetLoanOptionsRequest>(bodyAsText);
            GetLoanOptionsResponse getLoanOptionsResponse = await this._flowFinancePaymentService.GetLoanOptions(getLoanOptionsRequest);
            Response.Headers.Add("Cache-Control", "private");

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

        public async Task<IActionResult> InitWebhooks(string siteRoot)
        {
            return Json(await this._flowFinancePaymentService.InitWebhooks(siteRoot));
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