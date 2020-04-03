using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("ApplicationInput")]
    public class ApplicationInputType : InputObjectGraphType<ApplicationInput>
    {
        public ApplicationInputType()
        {
            Name = "ApplicationInput";

            Field<BusinessInfoType>("businessInfo", "Business Info");
            Field<PersonalnfoType>("personalInfo", "Personal Info");
            Field<TosAcceptanceType>("tosAcceptance", "TOS");
        }
    }
}