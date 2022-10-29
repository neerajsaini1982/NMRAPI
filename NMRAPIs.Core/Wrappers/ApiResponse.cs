using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace NMRAPIs.Core.Wrappers
{
    [DataContract]
    public class ApiResponse<T>
    {
        public ApiResponse(
           int statusCode = (int)HttpStatusCode.OK,
           string message = "",
           T result = default(T),
           ApiError apiError = null,
           string apiVersion = "1.0")
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Result = result;
            this.ResponseException = apiError;
            this.Version = apiVersion;
        }

        /// <summary>
        /// Gets or sets response message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets response exception.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public ApiError ResponseException { get; set; }

        /// <summary>
        /// Gets or sets response result.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }

        /// <summary>
        /// Gets or sets response http status code.
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets response version.
        /// </summary>
        [DataMember]
        public string Version { get; set; }
    }
}
