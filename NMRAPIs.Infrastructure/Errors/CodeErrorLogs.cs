// <copyright file="CodeErrorLogs.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.Data.Errors
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Code error logs db model.
    /// </summary>
    public class CodeErrorLogs
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets StackTrace.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Arguments.
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets ProfileId.
        /// </summary>
        public int? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets stored procedure name.
        /// </summary>
        public string StoredProcedure { get; set; }
    }
}
