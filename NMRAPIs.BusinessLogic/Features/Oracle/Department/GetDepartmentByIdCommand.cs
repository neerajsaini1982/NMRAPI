// <copyright file="GetDepartmentByIdCommand.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.Features.Oracle.Department
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// GetAll playbooks By Company Id for particular roles MediatR Query.
    /// </summary>
    public class GetDepartmentByIdCommand : Request, IGetEntityCommand<DepartmentViewModel>
    {
        /// <summary>
        /// Get or set DepartmentId.
        /// </summary>
        public int DepartmentId { get; set; }
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetDepartmentByIdCommandAuthorization"/> command.
    /// </summary>
    public class GetDepartmentByIdCommandAuthorization : IAuthorize<GetDepartmentByIdCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetDepartmentByIdCommand>.Authorize(GetDepartmentByIdCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetDepartmentByIdCommand"/> command.
    /// </summary>
    public class GetDepartmentByIdCommandValidator : AbstractValidator<GetDepartmentByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDepartmentByIdCommandValidator "/> class.
        /// </summary>
        public GetDepartmentByIdCommandValidator()
        {
            this.RuleFor(x => x.DepartmentId).GreaterThan(0);
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetDepartmentByIdCommand"/> Query.
    /// </summary>
    public class GetDepartmentByIdCommandHandler : IRequestHandler<GetDepartmentByIdCommand, DepartmentViewModel>
    {
        private readonly IDepartmentService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDepartmentByIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IDepartmentService"/> service.</param>
        public GetDepartmentByIdCommandHandler(IDepartmentService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<DepartmentViewModel> Handle(GetDepartmentByIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetDepartmentById(request);
        }
    }
}
