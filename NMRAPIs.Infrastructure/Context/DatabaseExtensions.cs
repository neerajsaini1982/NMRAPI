namespace NMRAPIs.Infrastructure.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.Data.Errors;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// Db Extensions.
    /// </summary>
    public static class DBExtensions
    {
        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="context">Current Db Context.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="commandText">Command Text.</param>
        /// <param name="callback">Callback delegate which will be executed before closing connection.</param>
        /// <param name="sqlParameters">Sql Parameters.</param>
        /// <param name="commandTimeout">Command time out.</param>
        public static void ExecuteReader(this NMRAPIsContext context, CommandType commandType, string commandText, Action<DbDataReader> callback, List<SqlParameter> sqlParameters = null, int commandTimeout = 120)
        {
            try
            {
                var connection = context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        context.Database.OpenConnection();
                    }

                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    command.CommandTimeout = commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count > 0)
                    {
                        foreach (var parameter in sqlParameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var reader = command.ExecuteReader();
                    callback?.Invoke(reader);
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                AddToErrorLogs(ex, context, "ExecuteReader", sqlParameters, commandText);
            }
            finally
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    context.Database.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Executes scalar.
        /// </summary>
        /// <param name="context">Current Db Context.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="commandText">Command Text.</param>
        /// <param name="sqlParameters">Sql Parameters.</param>
        /// <param name="commandTimeout">Command time out.</param>
        /// <returns>Object depending upon the type of the execution.</returns>
        public static object ExecuteScalar(this NMRAPIsContext context, CommandType commandType, string commandText, List<SqlParameter> sqlParameters = null, int commandTimeout = 120)
        {
            object returnValue = null;
            try
            {
                var connection = context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        context.Database.OpenConnection();
                    }

                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    command.CommandTimeout = commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count > 0)
                    {
                        foreach (var parameter in sqlParameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    returnValue = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                AddToErrorLogs(ex, context, "ExecuteScalar", sqlParameters, commandText);
            }
            finally
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    context.Database.CloseConnection();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Executes NonQuery.
        /// </summary>
        /// <param name="context">Current Db Context.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="commandText">Command Text.</param>
        /// <param name="sqlParameters">Sql Parameters.</param>
        /// <param name="commandTimeout">Command time out.</param>
        /// <returns>Affected rows.</returns>
        public static int ExecuteNonQuery(this NMRAPIsContext context, CommandType commandType, string commandText, List<SqlParameter> sqlParameters = null, int commandTimeout = 120)
        {
            int affectedRows = 0;
            try
            {
                var connection = context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        context.Database.OpenConnection();
                    }

                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    command.CommandTimeout = commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count > 0)
                    {
                        foreach (var parameter in sqlParameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    affectedRows = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                AddToErrorLogs(ex, context, "ExecuteNonQuery", sqlParameters, commandText);
            }
            finally
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    context.Database.CloseConnection();
                }
            }

            return affectedRows;
        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="context">Current Db Context.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="commandText">Command Text.</param>
        /// <param name="callback">Callback after filling dataset.</param>
        /// <param name="sqlParameters">Sql Parameters.</param>
        /// <param name="commandTimeout">Command time out.</param>
        public static void ExecuteProcedure(this NMRAPIsContext context, CommandType commandType, string commandText, Action<DataSet> callback, List<SqlParameter> sqlParameters = null, int commandTimeout = 120)
        {
            var ds = new DataSet();

            try
            {
                var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
                using (SqlCommand command = connection.CreateCommand())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        context.Database.OpenConnection();
                    }

                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    command.CommandTimeout = commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count > 0)
                    {
                        foreach (var parameter in sqlParameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                        adapter.Dispose();
                        callback?.Invoke(ds);
                        ds.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                AddToErrorLogs(ex, context, "ExecuteProcedure", sqlParameters, commandText);
            }
            finally
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    context.Database.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Returns Object List.
        /// </summary>
        /// <typeparam name="T">Type of object that you want to convert.</typeparam>
        /// <param name="dt">Datatable. </param>
        /// <param name="ignoreList">Properties which you want to ignore.</param>
        /// <returns>Object List.</returns>
        public static List<T> ConvertToList<T>(this DataTable dt, string[] ignoreList = null)
        {
            if (ignoreList == null)
            {
                ignoreList = new string[] { };
            }

            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name) && !ignoreList.Contains(pro.Name))
                    {
                        if (row[pro.Name] == DBNull.Value)
                        {
                            pro.SetValue(objT, null);
                        }
                        else
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                    }
                }

                return objT;
            }).ToList();
        }

        /// <summary>
        /// Checks whether the data record has column or not.
        /// </summary>
        /// <param name="dr">Data record.</param>
        /// <param name="columnName">Column Name.</param>
        /// <returns>A Boolean indicating whether record has column or not.</returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add error logs to database.
        /// </summary>
        /// <param name="ex">Exceptions.</param>
        /// <param name="context"> Db context.</param>
        /// <param name="methodName"> Method name.</param>
        /// <param name="parameters"> Parameters.</param>
        /// <param name="spName"> SP Name.</param>
        public static void AddToErrorLogs(Exception ex, NMRAPIsContext context, string methodName, List<SqlParameter> parameters, string spName)
        {
            try
            {
                CodeErrorLogs errorLogs = new CodeErrorLogs()
                {
                    Arguments = JsonConvert.SerializeObject(parameters.Select(x => x.Value)),
                    CreatedDate = DateTime.Now,
                    Message = ex.Message,
                    Method = methodName,
                    StackTrace = ex.StackTrace,
                    StoredProcedure = spName
                };

                context.CodeErrorLogs.Add(errorLogs);
                context.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}
