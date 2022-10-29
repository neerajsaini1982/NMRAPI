// <copyright file="UserService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.ServiceImplementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.OracleContext;
    using NMRAPIs.Infrastructure.OracleQuery;

    /// <summary>
    /// Implement the <see cref="IUserService"/> interface.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly OracleDBContext oracleDBContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">Oracle DB Context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public UserService(OracleDBContext context, IMapper mapper)
        {
            this.oracleDBContext = context;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                UserQuery.GetAllUsers,
                (reader) =>
                   {
                       if (reader.HasRows)
                       {
                           while (reader.Read())
                           {
                               users.Add(new UserViewModel()
                               {
                                   Id = Convert.ToInt32(reader["contactiid"].ToString()),
                                   FirstName = reader["name"].ToString(),
                                   Email = reader["email"].ToString(),
                               });
                           }
                       }
                   });

            return users;
        }
    }
}
