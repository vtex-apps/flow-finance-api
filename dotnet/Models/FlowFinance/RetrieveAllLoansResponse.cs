using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.RetrieveAllLoansResponse
{
    public class RootObject
    {
        public int page { get; set; }
        public int total { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string offer_token { get; set; }
        public int account_id { get; set; }
        public string ccb_url { get; set; }
        public string balance { get; set; }
        public DateTime created_at { get; set; }
        public Details details { get; set; }
        public Signature signature { get; set; }
    }

    public class Details
    {
        public Pt_Br pt_br { get; set; }
    }

    public class Pt_Br
    {
        public decimal vlr_financiado { get; set; }
        public decimal qtde_parcelas { get; set; }
        public decimal perc_juros_mensal { get; set; }
        public decimal perc_cet_mensal { get; set; }
        public decimal vlr_parcela { get; set; }
        public decimal vlr_total_divida { get; set; }
    }

    public class Signature
    {
        public DateTime date { get; set; }
        public string ip { get; set; }
        public string user_agent { get; set; }
    }
}
