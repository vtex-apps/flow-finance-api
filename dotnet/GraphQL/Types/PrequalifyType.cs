using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("Prequalify")]
    public class PrequalifyType : ObjectGraphType<Models.PreQualifyResponse.RootObject>
    {
        public PrequalifyType(IFlowFinancePaymentService flowFinancePaymentService)
        {
            Name = "Prequalify";

            Field(b => b.data.cnpj).Description("CNPJ");
            Field(b => b.data.eligible).Description("Eligible");
        }
    }
}
