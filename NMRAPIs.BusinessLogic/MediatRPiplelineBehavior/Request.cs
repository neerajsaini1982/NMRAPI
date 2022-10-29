using Newtonsoft.Json;
using System.Security.Claims;

namespace NMRAPIs.BusinessLogic.MediatRPiplelineBehavior
{
    public class Request
    {
        /// <summary>
        /// Gets or sets the current login user.
        /// </summary>
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; }
    }
}
