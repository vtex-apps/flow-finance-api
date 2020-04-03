using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    [GraphQLMetadata("Address")]
    public class AddressType : InputObjectGraphType<Address>
    {
        public AddressType()
        {
            Name = "Address";

            Field(b => b.country).Description("Country");
            Field(b => b.district).Description("District");
            Field(b => b.extraAddressInfo).Description("Extra");
            Field(b => b.postalCode).Description("Postal Code");
            Field(b => b.stateCode).Description("State");
            Field(b => b.streetName).Description("Street");
            Field(b => b.streetNumber).Description("Street Number");
            Field(b => b.city).Description("City");
        }
    }
}
