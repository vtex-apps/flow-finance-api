﻿using FlowFinance.Data;
using FlowFinance.GraphQL.Types;
using FlowFinance.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlowFinance.Services
{
    public class FlowFinancePaymentService : IFlowFinancePaymentService
    {
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FlowFinancePaymentService(IPaymentRequestRepository paymentRequestRepository, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {
            this._paymentRequestRepository = paymentRequestRepository ??
                                            throw new ArgumentNullException(nameof(paymentRequestRepository));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                                  throw new ArgumentNullException(nameof(clientFactory));
        }

        /// <summary>
        /// Creates a new payment and/or initiates the payment flow.
        /// </summary>
        /// <param name="createPaymentRequest"></param>
        /// <returns></returns>
        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest)
        {
            CreatePaymentResponse paymentResponse = new CreatePaymentResponse
            {
                status = FlowFinanceConstants.Vtex.Undefined,
                acquirer = FlowFinanceConstants.Acquirer,
                paymentId = createPaymentRequest.paymentId,
                paymentAppData = new PaymentAppData
                {
                    appName = FlowFinanceConstants.PaymentApp,
                    payload = JsonConvert.SerializeObject(new Payload
                    {
                        inboundRequestsUrl = createPaymentRequest.inboundRequestsUrl,
                        callbackUrl = createPaymentRequest.callbackUrl,
                        //paymentIdentifier = paymentIdentifier,
                        //publicKey = publicKey
                    })
                }
            };

            OrderInformation orderInformation = await _paymentRequestRepository.GetOrderInformation(createPaymentRequest.orderId);
            if (string.IsNullOrEmpty(orderInformation.offerToken))
            {
                paymentResponse.message = "Missing Offer Token";
            }
            else
            {
                FlowFinanceShopper shopper = await this.GetFlowFinanceShopperByEmail(orderInformation.email);
                if (shopper != null && !string.IsNullOrEmpty(shopper.accountId))
                {
                    int accountId = int.Parse(shopper.accountId);
                    MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
                    IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
                    ResponseWrapper responseWrapper = await flowFinanceAPI.CreateLoan(orderInformation.offerToken, accountId);
                    if (responseWrapper.success)
                    {
                        Models.CreateLoanResponse.RootObject createLoanResponse = (Models.CreateLoanResponse.RootObject)responseWrapper.responseObject;
                        paymentResponse.authorizationId = createLoanResponse.data.id;
                        paymentResponse.code = createLoanResponse.data.account_id.ToString();
                        paymentResponse.message = createLoanResponse.data.details.pt_br.ToString();
                        //paymentResponse.nsu
                        paymentResponse.status = FlowFinanceConstants.Vtex.Approved;
                        //paymentResponse.tid
                    }
                    else
                    {
                        paymentResponse.message = responseWrapper.errorMessage;
                        //paymentResponse.status = FlowFinanceConstants.Vtex.Undefined;
                    }
                }
                else
                {
                    paymentResponse.message = "Could not find account id.";
                }
            }

            return paymentResponse;
        }

        /// <summary>
        /// Cancels a payment that was not yet approved or captured (settled).
        /// </summary>
        /// <param name="cancelPaymentRequest"></param>
        /// <returns></returns>
        public async Task<CancelPaymentResponse> CancelPaymentAsync(CancelPaymentRequest cancelPaymentRequest)
        {
            CancelPaymentResponse cancelPaymentResponse = null;

            return cancelPaymentResponse;
        }

        /// <summary>
        /// Captures (settle) a payment that was previously approved.
        /// </summary>
        /// <param name="capturePaymentRequest"></param>
        /// <returns></returns>
        public async Task<CapturePaymentResponse> CapturePaymentAsync(CapturePaymentRequest capturePaymentRequest)
        {
            CapturePaymentResponse capturePaymentResponse = null;

            // Load request from storage for order id
            CreatePaymentRequest paymentRequest = await this._paymentRequestRepository.GetPaymentRequestAsync(capturePaymentRequest.paymentId);

            //if (capturePaymentRequest.authorizationId == null)
            //{
            //    // Get id from storage
            //    capturePaymentRequest.authorizationId = paymentRequest.transactionId;
            //}

            return capturePaymentResponse;
        }

        /// <summary>
        /// Refunds a payment that was previously captured (settled). You can expect partial refunds.
        /// </summary>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        public async Task<RefundPaymentResponse> RefundPaymentAsync(RefundPaymentRequest refundPaymentRequest)
        {
            //if (refundPaymentRequest.authorizationId == null)
            //{
            //    // Get id from storage
            //    CreatePaymentRequest paymentRequest = await this._paymentRequestRepository.GetPaymentRequestAsync(refundPaymentRequest.paymentId);
            //    refundPaymentRequest.authorizationId = paymentRequest.transactionId;
            //}

            //int amount = decimal.ToInt32(refundPaymentRequest.value * 100);

            RefundPaymentResponse refundPaymentResponse = null;

            return refundPaymentResponse;
        }

        /// <summary>
        /// Retrieve saved Payment Request
        /// </summary>
        /// <param name="paymentIdentifier"></param>
        /// <returns></returns>
        public async Task<CreatePaymentRequest> GetCreatePaymentRequestAsync(string paymentIdentifier)
        {
            CreatePaymentRequest paymentRequest = await this._paymentRequestRepository.GetPaymentRequestAsync(paymentIdentifier);

            return paymentRequest;
        }

        /// <summary>
        /// After completing the checkout flow and receiving the checkout token, authorize the charge.
        /// Authorizing generates a charge ID that you’ll use to reference the charge moving forward.
        /// You must authorize a charge to fully create it. A charge is not visible in the Read response,
        /// nor in the merchant dashboard until you authorize it.
        /// </summary>
        /// <param name="paymentIdentifier"></param>
        /// <param name="token"></param>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public async Task<CreatePaymentResponse> AuthorizeAsync(string paymentIdentifier, string token, string callbackUrl, int amount, string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                orderId = paymentIdentifier;
            }

            string paymentStatus = FlowFinanceConstants.Vtex.Denied;

            //if (flowFinanceResponse)
            //{
            //    paymentStatus = FlowFinanceConstants.Vtex.Approved;
            //}

            CreatePaymentResponse paymentResponse = null;

            return paymentResponse;
        }

        public async Task<CreatePaymentResponse> ReadChargeAsync(string paymentId)
        {

            return null;
        }

        public async Task<MerchantSettings> GetSettingsAsync()
        {
            MerchantSettings settings = await this._paymentRequestRepository.GetMerchantSettings();

            return settings;
        }

        public async Task<bool> PreQualify(string cnpj)
        {
            bool eligible = false;
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.PreQualify(cnpj);
            if (responseWrapper.success)
            {
                Models.PreQualifyResponse.RootObject flowFinanceResponse = (Models.PreQualifyResponse.RootObject)responseWrapper.responseObject;
                eligible = flowFinanceResponse.data.eligible;
            }
            else
            {
                Console.WriteLine($"PreQualify Failed.  {responseWrapper.errorMessage}");
            }

            return eligible;
        }

        public async Task<Models.CreateAccountResponse.RootObject> CreateAccount(Models.CreateAccountRequest.RootObject createAccountRequest)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.CreateAccount(createAccountRequest);
            Models.CreateAccountResponse.RootObject createAccountResponse = (Models.CreateAccountResponse.RootObject)responseWrapper.responseObject;

            return createAccountResponse;
        }

        public async Task<Models.LoanPreviewResponse.RootObject> LoanPreview(int amount, int accountId)
        {
            Models.LoanPreviewResponse.RootObject loanPreviewResponse = null;
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.LoanPreview(amount, accountId);
            if (responseWrapper.success)
            {
                loanPreviewResponse = (Models.LoanPreviewResponse.RootObject)responseWrapper.responseObject;
            }
            else
            {
                Console.WriteLine($"Loan Preview Failed.  {responseWrapper.errorMessage}");
            }

            return loanPreviewResponse;
        }

        public async Task<Models.CreateLoanResponse.RootObject> CreateLoan(string offerToken, int accountId)
        {
            Models.CreateLoanResponse.RootObject createLoanResponse = null;
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.CreateLoan(offerToken, accountId);
            if (responseWrapper.success)
            {
                createLoanResponse = (Models.CreateLoanResponse.RootObject)responseWrapper.responseObject;
            }
            else
            {
                Console.WriteLine($"Create Loan Failed.  {responseWrapper.errorMessage}");
            }

            return createLoanResponse;
        }

        /// <summary>
        /// Checks VBASE to see if the shopper has a FF account ID 
        /// If they do, send their FF account ID to the FF API to check their available credit
        /// Use the FF API to get the loan options
        /// Return a response
        /// </summary>
        /// <param name="getLoanOptionsRequest"></param>
        /// <returns></returns>
        public async Task<GetLoanOptionsResponse> GetLoanOptions(GetLoanOptionsRequest getLoanOptionsRequest)
        {
            GetLoanOptionsResponse getLoanOptionsResponse = new GetLoanOptionsResponse
            {
                availableCredit = 0m,
                accountStatus = FlowFinanceConstants.LoanStatus.None,
                loanOptions = new List<LoanOption>()
            };

            FlowFinanceShopper shopper = await this.GetFlowFinanceShopperByEmail(getLoanOptionsRequest.email);
            if (shopper != null && !string.IsNullOrEmpty(shopper.accountId))
            {
                int accountId = int.Parse(shopper.accountId);
                Console.WriteLine($"GetLoanOptions: Account Id for '{shopper.email}' is '{shopper.accountId}'");
                MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
                IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
                ResponseWrapper responseWrapper = await flowFinanceAPI.RetrieveAccountById(accountId);
                if (responseWrapper.success)
                {
                    Models.RetrieveAccountByIdResponse.RootObject accountResponse = (Models.RetrieveAccountByIdResponse.RootObject)responseWrapper.responseObject;
                    switch(accountResponse.data.status)
                    {
                        case FlowFinanceConstants.FlowFinanceStatus.Approved:
                            getLoanOptionsResponse.accountStatus = FlowFinanceConstants.LoanStatus.Approved;
                            break;
                        case FlowFinanceConstants.FlowFinanceStatus.Denied:
                            getLoanOptionsResponse.accountStatus = FlowFinanceConstants.LoanStatus.Denied;
                            break;
                        case FlowFinanceConstants.FlowFinanceStatus.Pending:
                        case FlowFinanceConstants.FlowFinanceStatus.UnderReview:
                            getLoanOptionsResponse.accountStatus = FlowFinanceConstants.LoanStatus.Pending;
                            break;
                        default:
                            getLoanOptionsResponse.accountStatus = FlowFinanceConstants.LoanStatus.None;
                            break;
                    }

                    decimal availableCredit = 0m;
                    string rawAvailableCredit = accountResponse.data.available_credit;
                    if (!string.IsNullOrEmpty(rawAvailableCredit))
                    {
                        string[] arrayAvailableCredit = rawAvailableCredit.Split(' ');
                        decimal.TryParse(arrayAvailableCredit[1], out availableCredit);
                    }

                    getLoanOptionsResponse.availableCredit = availableCredit;
                    if (availableCredit >= getLoanOptionsRequest.total)
                    {
                        responseWrapper = await flowFinanceAPI.LoanPreview(getLoanOptionsRequest.total, accountId);
                        if (responseWrapper.success)
                        {
                            Models.LoanPreviewResponse.RootObject loanPreviewResponse = (Models.LoanPreviewResponse.RootObject)responseWrapper.responseObject;
                            foreach (Models.LoanPreviewResponse.Datum offer in loanPreviewResponse.data)
                            {
                                LoanOption loanOption = new LoanOption
                                {
                                    amount = offer.amount ?? 0m,
                                    installment_amount = offer.installment_amount ?? 0m,
                                    interest_rate = offer.interest_rate ?? 0m,
                                    offer_token = offer.offer_token,
                                    term = offer.term,
                                    total_debt = offer.total_debt ?? 0m
                                };

                                getLoanOptionsResponse.loanOptions.Add(loanOption);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Loan Preview Failed {responseWrapper.errorMessage}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Retrieve Account By Id Failed {responseWrapper.errorMessage}");
                }
            }

            return getLoanOptionsResponse;
        }

        public async Task<Models.ListAccountsResponse.RootObject> ListAccounts(int page, int limit)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.ListAccounts(page, limit);
            Models.ListAccountsResponse.RootObject listAccountsResponse = (Models.ListAccountsResponse.RootObject)responseWrapper.responseObject;

            return listAccountsResponse;
        }

        public async Task<Models.ListPersonsResponse.RootObject> ListPersons(int accountId)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.ListPersons(accountId);
            Models.ListPersonsResponse.RootObject listPersonsResponse = (Models.ListPersonsResponse.RootObject)responseWrapper.responseObject;

            return listPersonsResponse;
        }

        public async Task SetAccountId(FlowFinanceShopper shopper)
        {
            await this.AppendFlowFinanceShopper(shopper);
        }

        public async Task ProcessCallback(Models.WebhookPayload.RootObject callbackPayload)
        {
            Console.WriteLine($"|[ Event: {callbackPayload.data.eventType} ]|[ Type: {callbackPayload.data.entity_type} ]|");
            Console.WriteLine($"|[ Id: {callbackPayload.data.entity.id} ]|[ Status: {callbackPayload.data.entity.status} ]|");
            string message = string.Empty;
            FlowFinanceShopper shopper = await this.GetFlowFinanceShopperById(callbackPayload.data.entity.id.ToString());
            string email = shopper.email;

            switch (callbackPayload.data.eventType)
            {
                case FlowFinanceConstants.WebHookNotification.AccountCreated:
                    message = await this.SendEmail(email, MailTemplateType.Submitted);
                    break;
                case FlowFinanceConstants.WebHookNotification.AccountUpdated:
                    if(callbackPayload.data.entity.status.Equals(FlowFinanceConstants.FlowFinanceStatus.Approved))
                    {
                        message = await this.SendEmail(email, MailTemplateType.Approved);
                    }
                    else if(callbackPayload.data.entity.status.Equals(FlowFinanceConstants.FlowFinanceStatus.Denied))
                    {
                        message = await this.SendEmail(email, MailTemplateType.Denied);
                    }
                    break;
            }

            Console.WriteLine($"SendMail Result: {message}");
        }

        public async Task<ApplicationResult> ProcessApplication(ApplicationInput applicationInput)
        {
            //Console.WriteLine($"ProcessApplication {applicationInput.businessInfo.name}");

            int accountId = 0;
            ApplicationResult applicationResult = new ApplicationResult()
            {
                success = false
            };

            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);

            Models.CreateAccountRequest.RootObject createAccountRequest = new Models.CreateAccountRequest.RootObject
            {
                //tos_acceptance = new Models.CreateAccountRequest.TosAcceptance
                //{
                //    date = DateTime.Parse(applicationInput.tosAcceptance.date),
                //    ip = applicationInput.tosAcceptance.ip,
                //    user_agent = applicationInput.tosAcceptance.userAgent
                //},
                business = new Models.CreateAccountRequest.Business
                {
                    address = new Models.CreateAccountRequest.Address
                    {
                        city = applicationInput.businessInfo.address.city,
                        country = applicationInput.businessInfo.address.country,
                        district = applicationInput.businessInfo.address.district,
                        extra_address_info = applicationInput.businessInfo.address.extraAddressInfo,
                        postal_code = applicationInput.businessInfo.address.postalCode,
                        state_code = applicationInput.businessInfo.address.stateCode,
                        street_name = applicationInput.businessInfo.address.streetName,
                        street_number = applicationInput.businessInfo.address.streetNumber
                    },
                    business_id = applicationInput.businessInfo.businessId,
                    contact_info = new Models.CreateAccountRequest.ContactInfo
                    {
                        email = applicationInput.businessInfo.contactInfo.email,
                        phone_number = applicationInput.businessInfo.contactInfo.phoneNumber
                    },
                    documents = new Models.CreateAccountRequest.Documents
                    {
                        physicalDocuments = new List<Models.CreateAccountRequest.Physical>(),
                        virtualDocuments = new List<Models.CreateAccountRequest.Virtual>()
                    },
                    legal_name = applicationInput.businessInfo.legalName,
                    name = applicationInput.businessInfo.name
                }
            };

            foreach(PhysicalDocument physicalDocument in applicationInput.businessInfo.documents.physicalDocument)
            {
                createAccountRequest.business.documents.physicalDocuments.Add(
                    new Models.CreateAccountRequest.Physical()
                    {
                        type = physicalDocument.type,
                        value = physicalDocument.value
                    });
            }

            foreach (VirtualDocument virtualDocument in applicationInput.businessInfo.documents.virtualDocument)
            {
                createAccountRequest.business.documents.virtualDocuments.Add(
                    new Models.CreateAccountRequest.Virtual()
                    {
                        type = virtualDocument.type,
                        value = virtualDocument.value,
                        exp = virtualDocument.exp,
                        issuer = virtualDocument.issuer
                    });
            }

            ResponseWrapper responseWrapperCreateAccount = await flowFinanceAPI.CreateAccount(createAccountRequest);
            ResponseWrapper responseWrapperUpdateAccount = new ResponseWrapper();
            ResponseWrapper responseWrapperCreatePerson = new ResponseWrapper();
            if (responseWrapperCreateAccount.success)
            {
                Models.CreateAccountResponse.RootObject createAccountResponse = (Models.CreateAccountResponse.RootObject)responseWrapperCreateAccount.responseObject;
                accountId = createAccountResponse.data.id;

                Console.WriteLine($"->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->->-> Account Id = [{accountId}]");

                Models.CreatePersonRequest.RootObject createPersonRequest = new Models.CreatePersonRequest.RootObject()
                {
                    account_opener = applicationInput.personalInfo.accountOpener,
                    address = new Models.CreatePersonRequest.Address
                    {
                        city = applicationInput.businessInfo.address.city,
                        country = applicationInput.businessInfo.address.country,
                        district = applicationInput.businessInfo.address.district,
                        extra_address_info = applicationInput.businessInfo.address.extraAddressInfo,
                        postal_code = applicationInput.businessInfo.address.postalCode,
                        state_code = applicationInput.businessInfo.address.stateCode,
                        street_name = applicationInput.businessInfo.address.streetName,
                        street_number = applicationInput.businessInfo.address.streetNumber
                    },
                    contact_info = new Models.CreatePersonRequest.ContactInfo
                    {
                        email = applicationInput.personalInfo.contactInfo.email,
                        phone_number = applicationInput.personalInfo.contactInfo.phoneNumber
                    },
                    documents = new Models.CreatePersonRequest.Documents
                    {
                        physicalDocuments = new List<Models.CreatePersonRequest.Physical>(),
                        virtualDocuments = new List<Models.CreatePersonRequest.Virtual>()
                    },
                    first_name = applicationInput.personalInfo.firstName,
                    last_name = applicationInput.personalInfo.lastName,
                    id_number = applicationInput.personalInfo.idNumber,
                    marital_status = applicationInput.personalInfo.maritalStatus,
                    pep = applicationInput.personalInfo.pep
                };

                foreach (PhysicalDocument physicalDocument in applicationInput.personalInfo.documents.physicalDocument)
                {
                    createPersonRequest.documents.physicalDocuments.Add(
                        new Models.CreatePersonRequest.Physical()
                        {
                            type = physicalDocument.type,
                            value = physicalDocument.value
                        });
                }

                foreach (VirtualDocument virtualDocument in applicationInput.personalInfo.documents.virtualDocument)
                {
                    createPersonRequest.documents.virtualDocuments.Add(
                        new Models.CreatePersonRequest.Virtual()
                        {
                            type = virtualDocument.type,
                            value = virtualDocument.value,
                            exp = virtualDocument.exp,
                            issuer = virtualDocument.issuer
                        });
                }

                responseWrapperCreatePerson = await flowFinanceAPI.CreatePerson(createPersonRequest, accountId);
                if (responseWrapperCreatePerson.success)
                {
                    Models.CreatePersonResponse.RootObject createPersonResponse = (Models.CreatePersonResponse.RootObject)responseWrapperCreatePerson.responseObject;

                    Models.UpdateAccountRequest.RootObject updateAccountRequest = new Models.UpdateAccountRequest.RootObject
                    {
                        tos_acceptance = new Models.UpdateAccountRequest.TosAcceptance
                        {
                            date = DateTime.Parse(applicationInput.tosAcceptance.date),
                            ip = applicationInput.tosAcceptance.ip,
                            user_agent = applicationInput.tosAcceptance.userAgent
                        },
                        business = new Models.UpdateAccountRequest.Business
                        {
                            address = new Models.UpdateAccountRequest.Address
                            {
                                city = applicationInput.businessInfo.address.city,
                                country = applicationInput.businessInfo.address.country,
                                district = applicationInput.businessInfo.address.district,
                                extra_address_info = applicationInput.businessInfo.address.extraAddressInfo,
                                postal_code = applicationInput.businessInfo.address.postalCode,
                                state_code = applicationInput.businessInfo.address.stateCode,
                                street_name = applicationInput.businessInfo.address.streetName,
                                street_number = applicationInput.businessInfo.address.streetNumber
                            },
                            business_id = applicationInput.businessInfo.businessId,
                            contact_info = new Models.UpdateAccountRequest.ContactInfo
                            {
                                email = applicationInput.businessInfo.contactInfo.email,
                                phone_number = applicationInput.businessInfo.contactInfo.phoneNumber
                            },
                            documents = new Models.UpdateAccountRequest.Documents
                            {
                                physicalDocuments = new List<Models.UpdateAccountRequest.Physical>(),
                                virtualDocuments = new List<Models.UpdateAccountRequest.Virtual>()
                            },
                            legal_name = applicationInput.businessInfo.legalName,
                            name = applicationInput.businessInfo.name
                        }
                    };

                    responseWrapperUpdateAccount = await flowFinanceAPI.UpdateAccount(updateAccountRequest, accountId);

                    // Save shopper in vbase
                    FlowFinanceShopper shopper = new FlowFinanceShopper
                    {
                        accountId = createPersonResponse.data.account_id.ToString(), // accountId.ToString(),
                        email = createPersonResponse.data.contact_info.email, // applicationInput.personalInfo.contactInfo.email,
                        createdAt = createPersonResponse.data.created_at,
                        firstName = createPersonResponse.data.first_name,
                        lastName = createPersonResponse.data.last_name,
                        idNumber = createPersonResponse.data.id_number
                    };

                    await this.AppendFlowFinanceShopper(shopper);
                }
            }

            applicationResult.success = responseWrapperCreateAccount.success && responseWrapperUpdateAccount.success && responseWrapperCreatePerson.success;
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(responseWrapperCreateAccount.errorMessage))
                sb.AppendLine($"Create Account Error: {responseWrapperCreateAccount.errorMessage}");
            if (!string.IsNullOrEmpty(responseWrapperUpdateAccount.errorMessage))
                sb.AppendLine($"Update Account Error: {responseWrapperUpdateAccount.errorMessage}");
            if (!string.IsNullOrEmpty(responseWrapperCreatePerson.errorMessage))
                sb.AppendLine($"Create Person Error: {responseWrapperCreatePerson.errorMessage}");
            applicationResult.error = sb.ToString();

            return applicationResult;
        }

        public async Task<string> SignLoan(Models.SignLoanRequest.RootObject signLoanRequest, string loanId, int accountId)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.SignLoan(signLoanRequest, loanId, accountId);
            string signLoanResponse = responseWrapper.success ? responseWrapper.responseMessage : responseWrapper.errorMessage;

            return signLoanResponse;
        }

        public async Task<Models.CreateWebhookEndpointResponse.RootObject> CreateWebhook(Models.CreateWebhookEndpointRequest.RootObject createWebhookRequest)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.CreateWebhookEndpoint(createWebhookRequest);
            Models.CreateWebhookEndpointResponse.RootObject createWebhookResponse = (Models.CreateWebhookEndpointResponse.RootObject)responseWrapper.responseObject;

            return createWebhookResponse;
        }

        public async Task<string> DeleteWebhookEndpoint(int webhookId)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.DeleteWebhookEndpoint(webhookId);
            string deleteWebhookResponse = responseWrapper.success ? responseWrapper.responseMessage : responseWrapper.errorMessage;

            return deleteWebhookResponse;
        }

        public async Task<string> InitWebhooks(string siteRoot)
        {
            string initWebhookResponse = string.Empty;
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.RetrieveWebhookEndpoints();
            if (responseWrapper.success)
            {
                StringBuilder sb = new StringBuilder();
                Models.RetrieveWebhookEndpointsResponse.RootObject retrieveWebhookEndpointsResponse = (Models.RetrieveWebhookEndpointsResponse.RootObject)responseWrapper.responseObject;
                foreach(Models.RetrieveWebhookEndpointsResponse.Datum webhook in retrieveWebhookEndpointsResponse.data)
                {
                    responseWrapper = await flowFinanceAPI.DeleteWebhookEndpoint(webhook.id);
                    if(responseWrapper.success)
                    {
                        sb.AppendLine($"Webhook {webhook.id} Deleted. {responseWrapper.responseMessage}");
                    }
                    else
                    {
                        sb.AppendLine($"Webhook {webhook.id} Error. {responseWrapper.errorMessage}");
                    }
                }

                Models.CreateWebhookEndpointRequest.RootObject createWebhookEndpointRequest = new Models.CreateWebhookEndpointRequest.RootObject
                {
                    events = new List<string>
                    {
                        FlowFinanceConstants.WebHookNotification.AccountUpdated,
                        FlowFinanceConstants.WebHookNotification.AccountCreated
                    },
                    url = $"https://{siteRoot}/flow-finance/callback"
                };

                responseWrapper = await flowFinanceAPI.CreateWebhookEndpoint(createWebhookEndpointRequest);
                if (responseWrapper.success)
                {
                    sb.AppendLine($"Webhook Created. {responseWrapper.responseMessage}");
                }
                else
                {
                    sb.AppendLine($"Webhook Creation Error. {responseWrapper.errorMessage}");
                }

                initWebhookResponse = sb.ToString();
            }
            else
            {
                initWebhookResponse = $"Retrieve Error: {responseWrapper.errorMessage}";
            }

            return initWebhookResponse;
        }

        public async Task<FlowFinanceShopper> GetFlowFinanceShopperByEmail(string email)
        {
            IList<FlowFinanceShopper> currentShoppers = await this._paymentRequestRepository.GetFlowFinanceShoppers();
            return currentShoppers.Where(n => n.email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public async Task<FlowFinanceShopper> GetFlowFinanceShopperById(string id)
        {
            IList<FlowFinanceShopper> currentShoppers = await this._paymentRequestRepository.GetFlowFinanceShoppers();
            return currentShoppers.Where(n => n.accountId.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public async Task<Models.RetrieveLoanByIdResponse.RootObject> RetrieveLoanById(string loanId, int accountId)
        {
            MerchantSettings merchantSettings = await this._paymentRequestRepository.GetMerchantSettings();
            IFlowFinanceAPI flowFinanceAPI = new FlowFinanceAPI(_httpContextAccessor, _clientFactory, merchantSettings);
            ResponseWrapper responseWrapper = await flowFinanceAPI.RetrieveLoanById(loanId, accountId);
            Models.RetrieveLoanByIdResponse.RootObject retrieveLoanByIdResponse = (Models.RetrieveLoanByIdResponse.RootObject)responseWrapper.responseObject;

            return retrieveLoanByIdResponse;
        }

        public async Task<IList<FlowFinanceShopper>> AppendFlowFinanceShopper(FlowFinanceShopper shopper)
        {
            IList<FlowFinanceShopper> currentShoppers = await this._paymentRequestRepository.GetFlowFinanceShoppers();

            if (currentShoppers == null)
            {
                currentShoppers = new List<FlowFinanceShopper>();
            }

            if (currentShoppers.Any(n => n.email.Equals(shopper.email, StringComparison.InvariantCultureIgnoreCase)))
            {
                // If the shopper already has an account, what should we do?  For testing, overwrite.
                Console.WriteLine($"Email {shopper.email} exists.  Replacing.");
                currentShoppers.Remove(currentShoppers.Where(n => n.email.Equals(shopper.email, StringComparison.InvariantCultureIgnoreCase)).First());
            }

            currentShoppers.Add(shopper);

            await this._paymentRequestRepository.SaveFlowFinanceShoppers(currentShoppers);

            return currentShoppers;
        }

        public async Task<IList<FlowFinanceShopper>> ListShoppers()
        {
            return await this._paymentRequestRepository.GetFlowFinanceShoppers();
        }

        /// <summary>
        /// POST to  http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail?an={!accountName}
        /// Headers:
        /// "content-type": "application/json",
        /// "X-VTEX-API-AppKey": "APPKEY"
        /// "X-VTEX-API-AppToken": "APPTOKEN"
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendEmail(string to, MailTemplateType templateType)
        {
            string templateName = string.Empty;
            switch(templateType)
            {
                case MailTemplateType.Approved:
                    templateName = FlowFinanceConstants.TEMPLATE_NAME_APPROVED;
                    break;
                case MailTemplateType.Denied:
                    templateName = FlowFinanceConstants.TEMPLATE_NAME_DENIED;
                    break;
                case MailTemplateType.Submitted:
                    templateName = FlowFinanceConstants.TEMPLATE_NAME_SUBMITTED;
                    break;
            }

            EmailMessage emailMessage = new EmailMessage
            {
                templateName = templateName,
                providerName = FlowFinanceConstants.Acquirer,
                jsonData = new JsonData
                {
                    to = to
                }
            };

            string accountName = _httpContextAccessor.HttpContext.Request.Headers[FlowFinanceConstants.VTEX_ACCOUNT_HEADER_NAME].ToString();
            string message = JsonConvert.SerializeObject(emailMessage);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{FlowFinanceConstants.MailService}?an={accountName}")
            };

            request.Content = new StringContent(message, Encoding.UTF8, FlowFinanceConstants.APPLICATION_JSON);
            request.Headers.Add(FlowFinanceConstants.AUTHORIZATION_HEADER_NAME, _httpContextAccessor.HttpContext.Request.Headers[FlowFinanceConstants.HEADER_VTEX_CREDENTIAL].ToString());
            HttpClient client = _clientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            return $"[{responseMessage.StatusCode}] {responseContent}";
        }
    }
}