using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.CreatePersonRequest
{
    public class Address
    {
        public string street_name { get; set; }
        public string street_number { get; set; }
        public string postal_code { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string state_code { get; set; }
        public string country { get; set; }
        public string extra_address_info { get; set; }
    }

    public class Physical
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Virtual
    {
        public string value { get; set; }
        public string type { get; set; }
        public string exp { get; set; }
        public string issuer { get; set; }
    }

    public class Documents
    {
        [JsonProperty("virtual", NullValueHandling = NullValueHandling.Ignore)]
        public List<Virtual> virtualDocuments { get; set; }

        [JsonProperty("physical", NullValueHandling = NullValueHandling.Ignore)]
        public List<Physical> physicalDocuments { get; set; }
    }

    public class ContactInfo
    {
        public string email { get; set; }
        public string phone_number { get; set; }
    }

    public class RootObject
    {
        public string id_number { get; set; }
        public string marital_status { get; set; }  // ['married', 'legally-separated', 'single', 'other', 'divorced', 'widowed'],
        public bool pep { get; set; }
        public Address address { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public Documents documents { get; set; }
        public ContactInfo contact_info { get; set; }
        public bool account_opener { get; set; }
    }
}
