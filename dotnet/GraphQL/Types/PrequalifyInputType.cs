using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("PrequalifyInputType")]
    public class PrequalifyInputType : InputObjectGraphType<Models.PreQualifyRequest.RootObject>
    {
        public PrequalifyInputType()
        {
            Name = "PrequalifyInput";

            Field(x => x.cnpj).Description("CNPJ");
        }
    }
}
