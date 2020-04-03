using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("Documents")]
    public class DocumentType : InputObjectGraphType<Documents>
    {
        public DocumentType()
        {
            Field<ListGraphType<PhysicalDocumentType>>("physicalDocument");
            Field<ListGraphType<VirtualDocumentType>>("virtualDocument");
        }
    }
}
