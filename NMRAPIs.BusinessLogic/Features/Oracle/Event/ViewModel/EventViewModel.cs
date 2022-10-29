// <copyright file="EventViewModel.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.Event.ViewModel
{
    using System;

    /// <summary>
    /// View model for Event.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Gets or sets the Contractid.
        /// </summary>
        public int Contractid { get; set; }

        /// <summary>
        /// Gets or sets the Type .
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the ContractDescription .
        /// </summary>
        public string ContractDescription { get; set; }

        /// <summary>
        /// Gets or sets the EventId .
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the datecreated .
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the loadindate .
        /// </summary>
        public DateTime LoadinDate { get; set; }

        /// <summary>
        /// Gets or sets the PickupDate .
        /// </summary>
        public DateTime PickupDate { get; set; }

        /// <summary>
        /// Gets or sets the SalesPerson .
        /// </summary>
        public string SalesPerson { get; set; }

        /// <summary>
        /// Gets or sets the SiteName .
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets the City .
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the LocDescription .
        /// </summary>
        public string LocDescription { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdated .
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the ShippingBoothroomNo .
        /// </summary>
        public int ShippingBoothroomNo { get; set; }

        /// <summary>
        /// Gets or sets the TravelExpenses .
        /// </summary>
        public int TravelExpenses { get; set; }

        /// <summary>
        /// Gets or sets the NonUnionLabor .
        /// </summary>
        public int NonUnionLabor { get; set; }

        /// <summary>
        /// Gets or sets the UnionLabor .
        /// </summary>
        public int UnionLabor { get; set; }

        /// <summary>
        /// Gets or sets the LaborTotal .
        /// </summary>
        public int LaborTotal { get; set; }

        /// <summary>
        /// Gets or sets the FreightTotal .
        /// </summary>
        public int FreightTotal { get; set; }

        /// <summary>
        /// Gets or sets the OrderTotal .
        /// </summary>
        public int OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the NotifyColor .
        /// </summary>
        public string NotifyColor { get; set; }

        /// <summary>
        /// Gets or sets the LaborFilled .
        /// </summary>
        public int LaborFilled { get; set; }

        /// <summary>
        /// Gets or sets the LaborConfirmed .
        /// </summary>
        public int LaborConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the FilledPercentage .
        /// </summary>
        public double FilledPercentage { get; set; }
    }
}
