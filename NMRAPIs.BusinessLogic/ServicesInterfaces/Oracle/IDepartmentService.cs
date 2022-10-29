// <copyright file="IDepartmentService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department.ViewModel;

    /// <summary>
    /// Interface for the Department Service.
    /// </summary>
    public interface IDepartmentService : IBaseService
    {
        /// <summary>
        /// Get All Departments.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<DepartmentViewModel>> GetAllDepartments();

        /// <summary>
        /// Get Department by id.
        /// </summary>
        /// <param name="command">Command to get address by Id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<DepartmentViewModel> GetDepartmentById(GetDepartmentByIdCommand command);
    }
}
