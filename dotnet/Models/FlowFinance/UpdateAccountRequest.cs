using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.UpdateAccountRequest
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
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

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ContactInfo
    {
        public string email { get; set; }
        public string phone_number { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Physical
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Virtual
    {
        public string value { get; set; }
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string exp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string issuer { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Documents
    {
        [JsonProperty("virtual", NullValueHandling = NullValueHandling.Ignore)]
        public List<Virtual> virtualDocuments { get; set; }

        [JsonProperty("physical", NullValueHandling = NullValueHandling.Ignore)]
        public List<Physical> physicalDocuments { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Business
    {
        public string business_id { get; set; }
        public string name { get; set; }
        public string legal_name { get; set; }
        public Address address { get; set; }
        public ContactInfo contact_info { get; set; }
        public Documents documents { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TosAcceptance
    {
        public DateTime date { get; set; }
        public string ip { get; set; }
        public string user_agent { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RootObject
    {
        public Business business { get; set; }
        public TosAcceptance tos_acceptance { get; set; }
    }
}
