using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.FlowFinance.Common
{
    public class Business
    {
        public string legal_name { get; set; }
        public string name { get; set; }
        public DateTime updated_at { get; set; }
        public Address address { get; set; }
        public Documents documents { get; set; }
        public ContactInfo contact_info { get; set; }
        public DateTime created_at { get; set; }
        public int account_id { get; set; }
        public string business_id { get; set; }
    }
}
