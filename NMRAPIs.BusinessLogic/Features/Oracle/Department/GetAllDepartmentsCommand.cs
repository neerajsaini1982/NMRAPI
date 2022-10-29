// <copyright file="GetAllDepartmentsCommand.cs" company="NMRAPIs">
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
    using NMRAPIs.BusinessLogic.Features.Oracle.Department.ViewModel;
    using NMRAPIs.BusinessLogic.MediatRPiplelineBehavior;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;

    /// <summary>
    /// Get All Departments.
    /// </summary>
    public class GetAllDepartmentsCommand : Request, IGetAllEntityCommand<DepartmentViewModel>
    {
    }

    /// <summary>
    /// Authorization class for the MediatR <see cref="GetAllDepartmentsCommandAuthorization"/> command.
    /// </summary>
    public class GetAllDepartmentsCommandAuthorization : IAuthorize<GetAllDepartmentsCommand>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllDepartmentsCommand>.Authorize(GetAllDepartmentsCommand request)
        {
            var authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Validation class for the MediatR <see cref="GetAllDepartmentsCommand"/> command.
    /// </summary>
    public class GetAllDepartmentsCommandValidator : AbstractValidator<GetAllDepartmentsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllDepartmentsCommandValidator "/> class.
        /// </summary>
        public GetAllDepartmentsCommandValidator()
        {
        }
    }

    /// <summary>
    /// Handler for MediatR <see cref="GetAllDepartmentsCommand"/> Query.
    /// </summary>
    public class GetAllDepartmentsCommandCommandHandler : IGetAllEntityCommandHandler<GetAllDepartmentsCommand, DepartmentViewModel>
    {
        private readonly IDepartmentService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllDepartmentsCommandCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="IDepartmentService"/> service.</param>
        public GetAllDepartmentsCommandCommandHandler(IDepartmentService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<List<DepartmentViewModel>> Handle(GetAllDepartmentsCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllDepartments();
        }
    }
}
