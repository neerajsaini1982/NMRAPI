// <copyright file="ProfileController.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Api.Controllers
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;
    using NMRAPIs.BusinessLogic.Features.Profile;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.Core.Constants;
    using NMRAPIs.Core.Wrappers;

    /// <summary>
    /// Controller to perform related functions of <see cref="IProfileService"/> service.
    /// </summary>
    [Route("r2api/sql")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IMediator mediator;
        private readonly IOptions<AzureAdSettings> azureSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="mediator">Instance of <see cref="IMediator"/> class.</param>
        public ProfileController(IMediator mediator, IOptions<AzureAdSettings> azureSettings)
        {
            this.mediator = mediator;
            this.azureSettings = azureSettings;
        }

        /// <summary>
        /// Returns the list of the Users.
        /// </summary>
        /// <returns>List of users .</returns>
        [HttpGet("Users")]
        [ProducesResponseType(typeof(ApiResponse<List<UserViewModel>>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(_bearer_token);
            var tid = decodedValue.Claims.FirstOrDefault(c => c.Type == "tid").Value;

            if (tid != this.azureSettings.Value.TenantId)
            {
                throw new ApiException("UnAuthorized", 401);
            }

            return this.Ok(await this.mediator.Send(new GetAllUsersCommand()));
        }
    }
}
