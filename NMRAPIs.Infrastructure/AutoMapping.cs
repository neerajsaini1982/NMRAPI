// <copyright file="AutoMapping.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;

    /// <summary>
    /// AutoMapper  Class.
    /// </summary>
    public class AutoMapping : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapping"/> class.
        /// </summary>
        public AutoMapping()
        {
            this.CreateMap<EntityClass.Profile, UserViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
