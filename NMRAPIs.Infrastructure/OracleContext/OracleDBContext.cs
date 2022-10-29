// <copyright file="OracleDBContext.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.OracleContext
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// Oracle DB Context.
    /// </summary>
    public class OracleDBContext : DbContext
    {
        private IConfiguration config;
        private OracleConnection connection;
        private OracleCommand cmd;
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDBContext"/> class.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        public OracleDBContext(IConfiguration configuration)
        {
            this.config = configuration;
            this.connectionString = this.config.GetConnectionString("OracleConnection");
        }

        /// <summary>
        /// Get Connection of oracle.
        /// </summary>
        /// <returns>Oracle Connection.</returns>
        public OracleConnection GetConn()
        {
            this.connection = new OracleConnection(this.connectionString);
            return this.connection;
        }

        /// <summary>
        /// Get Command of oracle db.
        /// </summary>
        /// <returns>Oracle Command.</returns>
        public OracleCommand GetCommand()
        {
            this.cmd = this.connection.CreateCommand();
            return this.cmd;
        }
    }
}
