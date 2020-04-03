using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("VirtualDocument")]
    public class VirtualDocumentType : InputObjectGraphType<VirtualDocument>
    {
        public VirtualDocumentType()
        {
            Name = "VirtualDocument";

            Field(b => b.type).Description("Virtual Documents Type");
            Field(b => b.value).Description("Virtual Documents Value");
            Field(b => b.exp).Description("Virtual Documents Exp");
            Field(b => b.issuer).Description("Virtual Documents Issuer");
        }
    }
}
