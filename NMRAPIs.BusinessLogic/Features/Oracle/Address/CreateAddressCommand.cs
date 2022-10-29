// <copyright file="CreateAddressCommand.cs" company="NMRAPIs">
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
    public class CreateAddressCommand : Request, ICreateUpdateCommand
    {
        /// <summary>
        /// Gets or sets the AddressLine.
        /// </summary>
        public string AddressLine { get;  set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get;  set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        public string State { get;  set; }

        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        public string Zip { get;  set; }
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="CreateAddressCommandAuthorization"/> command.
    /// </summary>
    public class CreateAddressCommandAuthorization : IAuthorize<CreateAddressCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<CreateAddressCommand>.Authorize(CreateAddressCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="CreateAddressCommand"/> command.
    /// </summary>
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAddressCommandValidator "/> class.
        /// </summary>
        public CreateAddressCommandValidator()
        {
            this.RuleFor(x => x.AddressLine).NotNull().NotEmpty().MaximumLength(500);
            this.RuleFor(x => x.City).NotNull().NotEmpty().MaximumLength(200);
            this.RuleFor(x => x.State).NotNull().NotEmpty().MaximumLength(200);
            this.RuleFor(x => x.Zip).NotNull().NotEmpty().MaximumLength(100);

        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="CreateAddressCommand"/> Query.
    /// </summary>
    public class CreateAddressCommandHandler : ICreateUpdateCommandHandler<CreateAddressCommand>
    {
        private readonly IAddressService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAddressCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IAddressService"/> service.</param>
        public CreateAddressCommandHandler(IAddressService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            return await this.service.CreateAddress(request);
        }
    }
}
