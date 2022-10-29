// <copyright file="AddressViewModel.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel

{
    using System;

    /// <summary>
    /// View model to AddressViewModel.
    /// </summary>
    public class AddressViewModel
    {
        /// <summary>
        /// Gets or sets the AddressId.
        /// </summary>
        public int AddressId { get;  set; }

        /// <summary>
        /// Gets or sets the FullAddress.
        /// </summary>
        public string FullAddress { get;  set; }
    }
}
