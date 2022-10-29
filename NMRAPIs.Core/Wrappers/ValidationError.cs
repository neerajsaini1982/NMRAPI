using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NMRAPIs.Core.Wrappers
{
    public class ValidationError
    {
        public ValidationError(string field, string message)
        {
            this.Field = field != string.Empty ? field : null;
            this.Message = message;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }
    }
}
