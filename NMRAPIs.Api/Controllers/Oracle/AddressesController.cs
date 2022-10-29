﻿// <copyright file="AddressesController.cs" company="NMRAPIs">
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
    using NMRAPIs.BusinessLogic.Features.Oracle.Address;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Core.Constants;
    using NMRAPIs.Core.Wrappers;

    /// <summary>
    /// Controller to perform related functions of <see cref="IUserService"/> service.
    /// </summary>
    [Route("r2api")]
    [ApiController]
    public class AddressesController : Controller
    {
        private readonly IMediator mediator;
        private readonly IOptions<AzureAdSettings> azureSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressesController"/> class.
        /// </summary>
        /// <param name="mediator">Instance of <see cref="IMediator"/> class.</param>
        public AddressesController(IMediator mediator, IOptions<AzureAdSettings> azureSettings)
        {
            this.mediator = mediator;
            this.azureSettings = azureSettings;
        }

        /// <summary>
        /// Returns the list of all address.
        /// </summary>
        /// <returns>List of users .</returns>
        [HttpGet("addresses")]
        [ProducesResponseType(typeof(ApiResponse<List<AddressViewModel>>), 200)]
        public async Task<IActionResult> GetAllAddresses(string searchPrefix)
        {
            //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var decodedValue = handler.ReadJwtToken(_bearer_token);
            //var tid = decodedValue.Claims.FirstOrDefault(c => c.Type == "tid").Value;

            //if (tid != this.azureSettings.Value.TenantId)
            //{
            //    throw new ApiException("UnAuthorized", 401);
            //}

            return this.Ok(await this.mediator.Send(new GetAllAddressesCommand(searchPrefix)));
        }

        /// <summary>
        /// Returns the list of all address.
        /// </summary>
        /// <returns>List of users .</returns>
        [HttpGet("addresses/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AddressViewModel>), 200)]
        public async Task<IActionResult> GetAddressById(int id)
        {
            //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var decodedValue = handler.ReadJwtToken(_bearer_token);
            //var tid = decodedValue.Claims.FirstOrDefault(c => c.Type == "tid").Value;

            //if (tid != this.azureSettings.Value.TenantId)
            //{
            //    throw new ApiException("UnAuthorized", 401);
            //}

            return this.Ok(await this.mediator.Send(new GetAddressByIdCommand() { Addressid = id }));
        }
    }
}
