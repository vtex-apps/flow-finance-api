using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    public class BusinessInfo
    {
        public string businessId { get; set; }
        public string name { get; set; }
        public string legalName { get; set; }
        public Address address { get; set; }
        public ContactInfo contactInfo { get; set; }
        public Documents documents { get; set; }
    }

    public class PersonalInfo
    {
        public string idNumber { get; set; }
        public string maritalStatus { get; set; }
        public bool pep { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public Documents documents { get; set; }
        public ContactInfo contactInfo { get; set; }
        public bool accountOpener { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public string streetName { get; set; }
        public string streetNumber { get; set; }
        public string postalCode { get; set; }
        public string district { get; set; }
        public string stateCode { get; set; }
        public string country { get; set; }
        public string extraAddressInfo { get; set; }
        public string city { get; set; }
    }

    public class ContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }

    public class Documents
    {
        public List<PhysicalDocument> physicalDocument { get; set; }
        public List<VirtualDocument> virtualDocument { get; set; }
    }

    public class PhysicalDocument
    {
        public string type { get; set; }
    }

    public class VirtualDocument
    {
        public string type { get; set; }
        public string value { get; set; }
        public string exp { get; set; }
        public string issuer { get; set; }
    }

    public class TOS
    {
        //public string date { get; set; }
        //public string ip { get; set; }
        public string userAgent { get; set; }
    }

    public class ApplicationInput
    {
        public BusinessInfo businessInfo { get; set; }
        public PersonalInfo personalInfo { get; set; }
        public TOS tosAcceptance { get; set; }
    }
}
