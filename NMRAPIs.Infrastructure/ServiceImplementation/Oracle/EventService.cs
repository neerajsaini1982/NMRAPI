// <copyright file="EventService.cs" company="NMRAPIs">
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
    using NMRAPIs.BusinessLogic.Features.Oracle.Event;
    using NMRAPIs.BusinessLogic.Features.Oracle.Event.ViewModel;
    using NMRAPIs.BusinessLogic.ServicesInterfaces.Oracle;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.OracleContext;
    using NMRAPIs.Infrastructure.OracleQuery;

    /// <summary>
    /// Implement the <see cref="IEventService"/> interface.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly OracleDBContext oracleDBContext;
        private readonly IMapper mapper;
        private readonly NMRAPIsContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="oracleDBContext">Oracle DB Context.</param>
        /// <param name="context">DB Context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public EventService(OracleDBContext oracleDBContext, NMRAPIsContext context, IMapper mapper)
        {
            this.oracleDBContext = oracleDBContext;
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<EventViewModel> GetEventById(GetEventByIdCommand command)
        {
            EventViewModel eventmodel = new EventViewModel();

            OracleDBExtensions.ExecuteReader(
                this.oracleDBContext,
                string.Format(UserQuery.GetEventById, Convert.ToString(command.EventId)),
                (reader) =>
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            eventmodel.Contractid = Convert.ToInt32(reader["contractid"].ToString());
                            eventmodel.Type = reader["type"].ToString();
                            eventmodel.ContractDescription = reader["contractdescription"].ToString();
                            eventmodel.EventId = reader["eventid"].ToString();
                            eventmodel.SalesPerson = reader["salesperson"].ToString();
                            eventmodel.SiteName = reader["sitename"].ToString();
                            eventmodel.City = reader["City"].ToString();
                            eventmodel.LocDescription = reader["locdescription"].ToString();
                            eventmodel.NotifyColor = reader["NotifyColor"].ToString();
                            eventmodel.DateCreated = Convert.ToDateTime(reader["datecreated"].ToString());
                            eventmodel.LoadinDate = Convert.ToDateTime(reader["loadindate"].ToString());
                            eventmodel.PickupDate = Convert.ToDateTime(reader["pickupdate"].ToString());
                            eventmodel.LastUpdated = Convert.ToDateTime(reader["lastupdated"].ToString());
                            eventmodel.ShippingBoothroomNo = Convert.ToInt32(reader["shippingboothroomno"].ToString());
                            eventmodel.TravelExpenses = Convert.ToInt32(reader["TravelExpenses"].ToString());
                            eventmodel.NonUnionLabor = Convert.ToInt32(reader["NonUnionLabor"].ToString());
                            eventmodel.UnionLabor = Convert.ToInt32(reader["UnionLabor"].ToString());
                            eventmodel.LaborTotal = Convert.ToInt32(reader["LaborTotal"].ToString());
                            eventmodel.FreightTotal = Convert.ToInt32(reader["FreightTotal"].ToString());
                            eventmodel.OrderTotal = Convert.ToInt32(reader["OrderTotal"].ToString());
                            eventmodel.LaborFilled = Convert.ToInt32(reader["LaborFilled"].ToString());
                            eventmodel.LaborConfirmed = Convert.ToInt32(reader["LaborConfirmed"].ToString());
                            eventmodel.FilledPercentage = Convert.ToDouble(reader["FilledPercentage"].ToString());
                        }
                    }
                });

            return eventmodel;
        }
    }
}
