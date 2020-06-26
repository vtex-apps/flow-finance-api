using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("PhysicalDocument")]
    public class PhysicalDocumentType : InputObjectGraphType<PhysicalDocument>
    {
        public PhysicalDocumentType()
        {
            Name = "PhysicalDocument";

            Field(b => b.type).Description("Physical Documents Type");
            //Field(b => b.value).Description("Physical Documents Value");
            //Field<UploadGraphType>("value");
        }
    }
}
