using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.Models.ErrorResponse
{
    public class Error
    {
        public string title { get; set; }
        public string field { get; set; }
        public object message { get; set; }
    }

    public class RootObject
    {
        //public List<Error> errors { get; set; }
        public object errors { get; set; }
        public string error { get; set; }

        public object schema { get; set; }
        public object value { get; set; }

        [JsonProperty("class")]
        public string errorClass { get; set; }
        public string type { get; set; }

        public string message { get; set; }
    }
}
