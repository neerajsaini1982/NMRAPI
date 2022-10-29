// <copyright file="Profile.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.EntityClass
{
    using System;
    using NMRAPIs.Core.Common;

    /// <summary>
    /// Initializes a new instance of the <see cref="Profile"/> class.
    /// </summary>
    public class Profile : ITrackable, IDeletable
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string FirstName { get; protected set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        public string LastName { get; protected set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; protected set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime CreatedAt { get; set; }

        /// <inheritdoc/>
        public int CreatedBy { get; set; }

        /// <inheritdoc/>
        public DateTime LastUpdatedAt { get; set; }

        /// <inheritdoc/>
        public int LastUpdatedBy { get; set; }
    }
}
