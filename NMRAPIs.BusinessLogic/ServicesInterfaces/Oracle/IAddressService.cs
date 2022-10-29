// <copyright file="IAddressService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;

    /// <summary>
    /// Interface for the Profile Service.
    /// </summary>
    public interface IAddressService : IBaseService
    {
        /// <summary>
        /// Get All Addresses.
        /// </summary>
        /// <param name="command">Command to get all address.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<AddressViewModel>> GetAllAddresses(GetAllAddressesCommand command);

        /// <summary>
        /// Get Address by id.
        /// </summary>
        /// <param name="command">Command to get address by Id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<AddressViewModel> GetAddressById(GetAddressByIdCommand command);

        /// <summary>
        /// Create Address.
        /// </summary>
        /// <param name="command">Command to create address.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> CreateAddress(CreateAddressCommand command);
    }
}
