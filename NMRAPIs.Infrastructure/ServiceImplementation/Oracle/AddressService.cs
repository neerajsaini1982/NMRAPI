// <copyright file="AddressService.cs" company="NMRAPIs">
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
    using NMRAPIs.BusinessLogic.Features.Oracle.Address;
    using NMRAPIs.BusinessLogic.Features.Oracle.Address.ViewModel;
    using NMRAPIs.BusinessLogic.Features.Profile.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.EntityClass;
    using NMRAPIs.Infrastructure.OracleContext;
    using NMRAPIs.Infrastructure.OracleQuery;

    /// <summary>
    /// Implement the <see cref="IAddressService"/> interface.
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly OracleDBContext oracleDBContext;
        private readonly IMapper mapper;
        private readonly NMRAPIsContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressService"/> class.
        /// </summary>
        /// <param name="oracleDBContext">Oracle DB Context.</param>
        /// <param name="context">DB Context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public AddressService(OracleDBContext oracleDBContext, NMRAPIsContext context, IMapper mapper)
        {
            this.oracleDBContext = oracleDBContext;
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<AddressViewModel>> GetAllAddresses(GetAllAddressesCommand command)
        {
            List<AddressViewModel> addresses = new List<AddressViewModel>();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                string.Format(UserQuery.GetAllAddresses, command.searchPrefix),
                (reader) =>
                   {
                       if (reader.HasRows)
                       {
                           while (reader.Read())
                           {
                               addresses.Add(new AddressViewModel()
                               {
                                   AddressId = Convert.ToInt32(reader["addressiid"].ToString()),
                                   FullAddress = reader["FullAddress"].ToString(),
                               });
                           }
                       }
                   });

            return addresses;
        }

        /// <inheritdoc/>
        public async Task<AddressViewModel> GetAddressById(GetAddressByIdCommand command)
        {
            AddressViewModel address = new AddressViewModel();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                string.Format(UserQuery.GetAddressById, Convert.ToString(command.Addressid)),
                (reader) =>
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            address.AddressId = Convert.ToInt32(reader["addressiid"].ToString());
                            address.FullAddress = reader["FullAddress"].ToString();
                        }
                    }
                });

            return address;
        }

        /// <inheritdoc/>
        public async Task<int> CreateAddress(CreateAddressCommand command)
        {
            Address address = new Address(addressLine: command.AddressLine, city: command.City, state: command.State, zip: command.Zip);
            this.context.Addresses.Add(address);
            return this.context.SaveChanges();
        }

    }
}
