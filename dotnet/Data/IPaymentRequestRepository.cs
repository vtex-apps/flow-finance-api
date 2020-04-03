﻿namespace FlowFinance.Data
{
    using FlowFinance.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPaymentRequestRepository
    {
        Task<CreatePaymentRequest> GetPaymentRequestAsync(string paymentIdentifier);

        Task SavePaymentRequestAsync(string paymentIdentifier, CreatePaymentRequest createPaymentRequest);

        Task SetMerchantSettings(MerchantSettings merchantSettings);

        Task<MerchantSettings> GetMerchantSettings();

        Task PostCallbackResponse(string callbackUrl, CreatePaymentResponse createPaymentResponse);

        Task<IList<FlowFinanceShopper>> GetFlowFinanceShoppers();

        Task<bool> SaveFlowFinanceShoppers(IList<FlowFinanceShopper> shoppers);

        Task<OrderInformation> GetOrderInformation(string orderId);
    }
}