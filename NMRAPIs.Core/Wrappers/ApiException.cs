using System;
using System.Collections.Generic;
using System.Text;

namespace NMRAPIs.Core.Wrappers
{
    public class ApiException : Exception
    {
        public ApiException(
           string message,
           int statusCode = 500,
           IEnumerable<ValidationError> errors = null)
           : base(message)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public ApiException(Exception ex, int statusCode = 500)
            : base(ex.Message)
        {
            this.StatusCode = statusCode;
        }

        public IEnumerable<ValidationError> Errors { get; set; }

        public int StatusCode { get; set; }
    }
}
