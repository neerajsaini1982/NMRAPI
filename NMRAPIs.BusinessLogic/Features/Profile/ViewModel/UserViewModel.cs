// <copyright file="UserViewModel.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

using System;

namespace NMRAPIs.BusinessLogic.Features.Profile.ViewModel
{
    /// <summary>
    /// View model to UserViewModel.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get;  set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string FirstName { get;  set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        public string LastName { get;  set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get;  set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get;  set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdatedAt.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdatedBy.
        /// </summary>
        public int LastUpdatedBy { get; set; }
    }
}
