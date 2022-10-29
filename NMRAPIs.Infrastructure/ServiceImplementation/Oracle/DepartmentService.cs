// <copyright file="DepartmentService.cs" company="NMRAPIs">
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
    using NMRAPIs.BusinessLogic.Features.Oracle.Department;
    using NMRAPIs.BusinessLogic.Features.Oracle.Department.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.OracleContext;
    using NMRAPIs.Infrastructure.OracleQuery;

    /// <summary>
    /// Implement the <see cref="IDepartmentService"/> interface.
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly OracleDBContext oracleDBContext;
        private readonly IMapper mapper;
        private readonly NMRAPIsContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentService"/> class.
        /// </summary>
        /// <param name="oracleDBContext">Oracle DB Context.</param>
        /// <param name="context">DB Context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public DepartmentService(OracleDBContext oracleDBContext, NMRAPIsContext context, IMapper mapper)
        {
            this.oracleDBContext = oracleDBContext;
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<DepartmentViewModel>> GetAllDepartments()
        {
            List<DepartmentViewModel> departments = new List<DepartmentViewModel>();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                UserQuery.GetAllDepartments,
                (reader) =>
                   {
                       if (reader.HasRows)
                       {
                           while (reader.Read())
                           {
                               departments.Add(new DepartmentViewModel()
                               {
                                   Groupiid = Convert.ToInt32(reader["Groupiid"].ToString()),
                                   Groupid = reader["Groupid"].ToString(),
                                   Description = reader["Description"].ToString(),
                               });
                           }
                       }
                   });

            return departments;
        }

        /// <inheritdoc/>
        public async Task<DepartmentViewModel> GetDepartmentById(GetDepartmentByIdCommand command)
        {
            DepartmentViewModel Department = new DepartmentViewModel();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                string.Format(UserQuery.GetDepartmentById, Convert.ToString(command.DepartmentId)),
                (reader) =>
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Department.Groupiid = Convert.ToInt32(reader["Groupiid"].ToString());
                            Department.Groupid = reader["Groupid"].ToString();
                            Department.Description = reader["Description"].ToString();
                        }
                    }
                });

            return Department;
        }
    }
}
