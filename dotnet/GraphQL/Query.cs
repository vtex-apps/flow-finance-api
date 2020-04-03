using FlowFinance.GraphQL.Types;
using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL
{
    [GraphQLMetadata("Query")]
    public class Query : ObjectGraphType<object>
    {
        public Query(IFlowFinancePaymentService flowFinancePaymentService)
        {
            Name = "Query";

        }
    }
}
