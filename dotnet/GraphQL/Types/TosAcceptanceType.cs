using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("TosAcceptance")]
    class TosAcceptanceType : InputObjectGraphType<TOS>
    {
        public TosAcceptanceType()
        {
            Name = "TosAcceptance";

            //Field(b => b.date).Description("TOS Acceptance Date");
            //Field(b => b.ip).Description("TOS Acceptance IP");
            Field(b => b.userAgent).Description("TOS User Agent");
        }
    }
}
