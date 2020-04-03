using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("BusinessInfo")]
    public class BusinessInfoType : InputObjectGraphType<BusinessInfo>
    {
        public BusinessInfoType()
        {
            Name = "BusinessInfo";

            Field<AddressType>("address");
            Field(b => b.businessId).Description("Business Info Id");
            Field<ContactInfoType>("contactInfo");
            Field<DocumentType>("documents");
            Field(b => b.legalName).Description("Business Legal Name");
            Field(b => b.name).Description("Business Name");
        }
    }
}
