using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.WebhookPayload
{

    public class RootObject
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("entity_type")]
        public string EntityType { get; set; }

        [JsonProperty("entity")]
        public Entity Entity { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class Entity
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("line_of_credit")]
        public string LineOfCredit { get; set; }

        [JsonProperty("available_credit")]
        public string AvailableCredit { get; set; }

        [JsonProperty("business")]
        public Business Business { get; set; }

        [JsonProperty("tos_acceptance")]
        public TosAcceptance TosAcceptance { get; set; }
    }

    public class TosAcceptance
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }
    }

    public class Business
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("business_id")]
        public string BusinessId { get; set; }

        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact_info")]
        public ContactInfo ContactInfo { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("documents")]
        public Documents Documents { get; set; }

        [JsonProperty("legal_name")]
        public string LegalName { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class Address
    {
        [JsonProperty("street_name")]
        public string StreetName { get; set; }

        [JsonProperty("street_number")]
        public string StreetNumber { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("extra_address_info")]
        public string ExtraAddressInfo { get; set; }
    }

    public class ContactInfo
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class Documents
    {
        [JsonProperty("physical")]
        public List<Physical> Physical { get; set; }
    }

    public class Physical
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }
    }
}
