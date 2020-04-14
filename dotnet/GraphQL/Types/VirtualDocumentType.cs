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

            Field(b => b.type, nullable: true).Description("Virtual Documents Type");
            Field(b => b.value, nullable: true).Description("Virtual Documents Value");
            Field(b => b.exp, nullable: true).Description("Virtual Documents Exp");
            Field(b => b.issuer, nullable: true).Description("Virtual Documents Issuer");
        }
    }
}
