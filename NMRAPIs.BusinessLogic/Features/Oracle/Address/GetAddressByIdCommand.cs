// <copyright file="GetAddressByIdCommand.cs" company="NMRAPIs">
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
    using MediatR;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// GetAll playbooks By Company Id for particular roles MediatR Query.
    /// </summary>
    public class GetAddressByIdCommand : Request, IGetEntityCommand<AddressViewModel>
    {
        /// <summary>
        /// Get or set AddressId.
        /// </summary>
        public int Addressid { get; set; }
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetAddressByIdCommandAuthorization"/> command.
    /// </summary>
    public class GetAddressByIdCommandAuthorization : IAuthorize<GetAddressByIdCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAddressByIdCommand>.Authorize(GetAddressByIdCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetAddressByIdCommand"/> command.
    /// </summary>
    public class GetAddressByIdCommandValidator : AbstractValidator<GetAddressByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAddressByIdCommandValidator "/> class.
        /// </summary>
        public GetAddressByIdCommandValidator()
        {
            this.RuleFor(x => x.Addressid).GreaterThan(0);
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetAddressByIdCommand"/> Query.
    /// </summary>
    public class GetAddressByIdCommandHandler : IRequestHandler<GetAddressByIdCommand, AddressViewModel>
    {
        private readonly IAddressService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAddressByIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IAddressService"/> service.</param>
        public GetAddressByIdCommandHandler(IAddressService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<AddressViewModel> Handle(GetAddressByIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAddressById(request);
        }
    }
}
