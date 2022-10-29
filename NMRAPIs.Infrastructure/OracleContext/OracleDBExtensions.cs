// <copyright file="OracleDBExtensions.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.OracleContext
{
    using System;
    using System.Data.Common;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// Oracle DB Extensions.
    /// </summary>
    public static class OracleDBExtensions
    {
        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="context">Oracle context.</param>
        /// <param name="commandText">Query to execute.</param>
        /// <param name="callback">Callback after filling dataset.</param>
        public static void ExecuteReader(this OracleDBContext context, string commandText, Action<DbDataReader> callback)
        {
            using (OracleConnection con = context.GetConn())
            {
                using (OracleCommand cmd = context.GetCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        cmd.CommandText = commandText;

                        OracleDataReader reader = cmd.ExecuteReader();

                        callback?.Invoke(reader);
                        if (reader != null)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
    }
}
