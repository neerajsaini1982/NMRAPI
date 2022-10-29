// <copyright file="GetAllAddressesCommand.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.Address
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// GetAll playbooks By Company Id for particular roles MediatR Query.
    /// </summary>
    public class GetAllAddressesCommand : Request, IGetAllEntityCommand<AddressViewModel>
    {
        /// <summary>
        /// Get or set searchPrefix.
        /// </summary>
        public string searchPrefix { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllAddressesCommand"/> class.
        /// </summary>
        /// <param name="searchPrefixValue">searchPrefix</param>
        public GetAllAddressesCommand(string searchPrefixValue)
        {
            this.searchPrefix = searchPrefixValue;
        }
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetAllAddressesCommandAuthorization"/> command.
    /// </summary>
    public class GetAllAddressesCommandAuthorization : IAuthorize<GetAllAddressesCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllAddressesCommand>.Authorize(GetAllAddressesCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetAllAddressesCommand"/> command.
    /// </summary>
    public class GetAllAddressesCommandValidator : AbstractValidator<GetAllAddressesCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllAddressesCommandValidator "/> class.
        /// </summary>
        public GetAllAddressesCommandValidator()
        {
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetAllAddressesCommand"/> Query.
    /// </summary>
    public class GetAllAddressesCommandCommandHandler : IGetAllEntityCommandHandler<GetAllAddressesCommand, AddressViewModel>
    {
        private readonly IAddressService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllAddressesCommandCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IAddressService"/> service.</param>
        public GetAllAddressesCommandCommandHandler(IAddressService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<List<AddressViewModel>> Handle(GetAllAddressesCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllAddresses(request);
        }
    }
}
