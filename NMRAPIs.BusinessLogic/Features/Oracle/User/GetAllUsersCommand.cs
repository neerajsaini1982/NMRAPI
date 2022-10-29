// <copyright file="GetAllUsersCommand.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.User
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// GetAll playbooks By Company Id for particular roles MediatR Query.
    /// </summary>
    public class GetAllUsersCommand : Request, IGetAllEntityCommand<UserViewModel>
    {
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetAllUsersCommandAuthorization"/> command.
    /// </summary>
    public class GetAllUsersCommandAuthorization : IAuthorize<GetAllUsersCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllUsersCommand>.Authorize(GetAllUsersCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetAllUsersCommand"/> command.
    /// </summary>
    public class GetAllUsersCommandValidator : AbstractValidator<GetAllUsersCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUsersCommandValidator "/> class.
        /// </summary>
        public GetAllUsersCommandValidator()
        {
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetAllUsersCommand"/> Query.
    /// </summary>
    public class GetAllUsersCommandCommandHandler : IGetAllEntityCommandHandler<GetAllUsersCommand, UserViewModel>
    {
        private readonly IUserService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUsersCommandCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IUserService"/> service.</param>
        public GetAllUsersCommandCommandHandler(IUserService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<List<UserViewModel>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllUsers();
        }
    }
}
