using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("PersonalInfo")]
    public class PersonalnfoType : InputObjectGraphType<PersonalInfo>
    {
        public PersonalnfoType()
        {
            Name = "PersonalInfo";

            Field(b => b.accountOpener).Description("Personal Account Opener");
            Field<ContactInfoType>("contactInfo");
            Field<DocumentType>("documents");
            Field(b => b.firstName).Description("Personal First Name");
            Field(b => b.lastName).Description("Personal Last Name");
            Field(b => b.idNumber).Description("Personal Id");
            Field(b => b.maritalStatus).Description("Personal Marital Status");
            Field(b => b.pep).Description("Personal Exposed Person");
        }
    }
}
