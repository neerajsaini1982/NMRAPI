// <copyright file="GetEventByIdCommand.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.Event
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using NMRAPIs.BusinessLogic.Features.Oracle.Event.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// GetAll playbooks By Company Id for particular roles MediatR Query.
    /// </summary>
    public class GetEventByIdCommand : Request, IGetEntityCommand<EventViewModel>
    {
        /// <summary>
        /// Gets or sets EventId.
        /// </summary>
        public int EventId { get; set; }
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetEventByIdCommandAuthorization"/> command.
    /// </summary>
    public class GetEventByIdCommandAuthorization : IAuthorize<GetEventByIdCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetEventByIdCommand>.Authorize(GetEventByIdCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetEventByIdCommand"/> command.
    /// </summary>
    public class GetEventByIdCommandValidator : AbstractValidator<GetEventByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventByIdCommandValidator "/> class.
        /// </summary>
        public GetEventByIdCommandValidator()
        {
            this.RuleFor(x => x.EventId).GreaterThan(0);
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetEventByIdCommand"/> Query.
    /// </summary>
    public class GetEventByIdCommandHandler : IRequestHandler<GetEventByIdCommand, EventViewModel>
    {
        private readonly IEventService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventByIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IEventService"/> service.</param>
        public GetEventByIdCommandHandler(IEventService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<EventViewModel> Handle(GetEventByIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetEventById(request);
        }
    }
}
