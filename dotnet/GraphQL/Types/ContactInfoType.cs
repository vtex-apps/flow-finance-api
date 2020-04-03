using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("ContactInfo")]
    public class ContactInfoType : InputObjectGraphType<ContactInfo>
    {
        public ContactInfoType()
        {
            Name = "ContactInfo";

            Field(b => b.email).Description("Email");
            Field(b => b.phoneNumber).Description("Phone");
        }
    }
}
