// <copyright file="ProfileService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.ServiceImplementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.Infrastructure.Context;

    /// <summary>
    /// Implement the <see cref="IProfileService"/> interface.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly NMRAPIsContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="context">Application DB Context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public ProfileService(NMRAPIsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            return this.mapper.Map<List<UserViewModel>>(this.context.Profiles.ToList());
        }
    }
}
