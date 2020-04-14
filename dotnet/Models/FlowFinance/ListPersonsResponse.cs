using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.ListPersonsResponse
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

    public class Virtual
    {
        public string id { get; set; }
        public string value { get; set; }
        public DateTime timestamp { get; set; }
        public string type { get; set; }
        public string exp { get; set; }
        public string issuer { get; set; }
    }

    public class Physical
    {
        public string id { get; set; }
        public DateTime timestamp { get; set; }
        public string type { get; set; }
        public string extension { get; set; }
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

    public class Datum
    {
        public string id_number { get; set; }
        public string marital_status { get; set; }
        public bool pep { get; set; }
        public int id { get; set; }
        public DateTime updated_at { get; set; }
        public Address address { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public Documents documents { get; set; }
        public ContactInfo contact_info { get; set; }
        public DateTime created_at { get; set; }
        public bool account_opener { get; set; }
        public int account_id { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
    }
}
