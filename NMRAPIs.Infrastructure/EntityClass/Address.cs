// <copyright file="Address.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.EntityClass
{
    using System;
    using NMRAPIs.Core.Common;

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    public class Address : ITrackable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        public Address()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        /// <param name="addressLine">Address Line.</param>
        /// <param name="city">City.</param>
        /// <param name="state">State.</param>
        /// <param name="zip">Zip.</param>
        public Address(
            string addressLine,
            string city,
            string state,
            string zip)
        {
            this.AddressLine = addressLine;
            this.City = city;
            this.State = state;
            this.Zip = zip;
        }

        /// <summary>
        /// Gets or sets the AddressId.
        /// </summary>
        public int AddressId { get; protected set; }

        /// <summary>
        /// Gets or sets the AddressLine.
        /// </summary>
        public string AddressLine { get; protected set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get; protected set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        public string State { get; protected set; }

        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        public string Zip { get; protected set; }

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
