using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("ApplicationResult")]
    public class ApplicationResultType : ObjectGraphType<ApplicationResult>
    {
        public ApplicationResultType(IFlowFinancePaymentService flowFinancePaymentService)
        {
            Name = "ApplicationResult";

            //Field(b => b.Id).Description("The id of the review.");
            Field(b => b.error).Description("Error message (if any)");
            Field(b => b.success).Description("Success (true/false)");
        }
    }
}
