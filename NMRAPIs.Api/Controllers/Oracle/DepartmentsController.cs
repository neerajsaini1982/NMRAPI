// <copyright file="DepartmentsController.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Api.Controllers.Oracle
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Core.Constants;
    using NMRAPIs.Core.Wrappers;

    /// <summary>
    /// Controller to perform related functions of <see cref="IUserService"/> service.
    /// </summary>
    [Route("r2api")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IOptions<AzureAdSettings> azureSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentsController"/> class.
        /// </summary>
        /// <param name="mediator">Instance of <see cref="IMediator"/> class.</param>
        public DepartmentsController(IMediator mediator, IOptions<AzureAdSettings> azureSettings)
        {
            this.mediator = mediator;
            this.azureSettings = azureSettings;
        }

        /// <summary>
        /// Returns the list of all departments.
        /// </summary>
        /// <returns>List of departments .</returns>
        [HttpGet("departments")]
        [ProducesResponseType(typeof(ApiResponse<List<DepartmentViewModel>>), 200)]
        public async Task<IActionResult> GetAllDepartments()
        {
            //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var decodedValue = handler.ReadJwtToken(_bearer_token);
            //var tid = decodedValue.Claims.FirstOrDefault(c => c.Type == "tid").Value;

            //if (tid != this.azureSettings.Value.TenantId)
            //{
            //    throw new ApiException("UnAuthorized", 401);
            //}

            return this.Ok(await this.mediator.Send(new GetAllDepartmentsCommand()));
        }

        /// <summary>
        /// Returns the department.
        /// </summary>
        /// <returns>department.</returns>
        [HttpGet("departments/{id}")]
        [ProducesResponseType(typeof(ApiResponse<DepartmentViewModel>), 200)]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var decodedValue = handler.ReadJwtToken(_bearer_token);
            //var tid = decodedValue.Claims.FirstOrDefault(c => c.Type == "tid").Value;

            //if (tid != this.azureSettings.Value.TenantId)
            //{
            //    throw new ApiException("UnAuthorized", 401);
            //}

            return this.Ok(await this.mediator.Send(new GetDepartmentByIdCommand() { DepartmentId = id }));
        }
    }
}
