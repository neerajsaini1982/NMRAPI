// <copyright file="IUserService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;

    /// <summary>
    /// Interface for the Profile Service.
    /// </summary>
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// Get All Users.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<UserViewModel>> GetAllUsers();
    }
}
