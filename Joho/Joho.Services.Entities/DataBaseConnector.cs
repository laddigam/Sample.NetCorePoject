using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Joho.Services.Entities
{
    public class DataBaseConnector
    {

        /// <summary>
        /// The connection
        /// </summary>
        public NpgsqlConnection Connection;
        /// <summary>
        /// The command
        /// </summary>
        NpgsqlCommand cmd;
        /// <summary>
        /// The connect
        /// </summary>
        NpgsqlConnection connect;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseConnector"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DataBaseConnector(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);

        }

        /// <summary>
        /// Executes the select operation asynchronous.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public async Task<dynamic> ExcuteSelectOperationAsync(string storedProcedure, List<NpgsqlParameter> parameters, dynamic destination = null)
        {
            dynamic jsonResponse = null;
            try
            {
                using (connect = new NpgsqlConnection(Connection.ConnectionString))
                {
                    cmd = new NpgsqlCommand(storedProcedure, Connection);

                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (dr.Read())
                        {
                            string doc = string.Empty;
                            if (!dr.IsDBNull(0))
                            {
                                doc = dr.GetString(0);
                            }
                            jsonResponse = destination == null ? doc : JsonConvert.DeserializeObject(doc, destination.GetType());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (cmd.Connection != null)
                {
                    cmd.Connection.Close();
                }
                connect.Close();
                cmd.Dispose();
            }
            return jsonResponse;
        }


        /// <summary>
        /// excute inline select statement
        /// </summary>
        /// <param name="query"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public async Task<dynamic> ExcuteInLineQueryOperationAsync(string query, dynamic destination = null)
        {

            query = "SELECT array_to_json(array_agg(row_to_json(t))) from(" + query + ")t;";



            dynamic jsonResponse = null;
            try
            {
                using (connect = new NpgsqlConnection(Connection.ConnectionString))
                {
                    cmd = new NpgsqlCommand(query, Connection);


                    cmd.CommandType = CommandType.Text;
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (dr.Read())
                        {

                            if (!dr.IsDBNull(0))
                            {
                                jsonResponse = destination == null ? dr.GetString(0) : JsonConvert.DeserializeObject(dr.GetString(0), destination.GetType());

                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    cmd.Connection.Close();
                }
                connect.Close();
                cmd.Dispose();
            }
            return jsonResponse;
        }
        /// <summary>
        /// Executes the crud operation asynchronous.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExcuteCRUDOperationAsync(string storedProcedure, List<NpgsqlParameter> parameters)
        {
            int recs = 0;
            try
            {
                using (connect = new NpgsqlConnection(Connection.ConnectionString))
                    cmd = new NpgsqlCommand(storedProcedure, Connection);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters.ToArray());

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                recs = Convert.ToInt32(await cmd.ExecuteScalarAsync());


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    cmd.Connection.Close();
                }
                connect.Close();
                cmd.Dispose();
            }
            return recs;
        }
        public void Dispose()
        {
            Connection.CloseAsync().ConfigureAwait(false);
        }

        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    var type = pro.PropertyType;
                    if (Nullable.GetUnderlyingType(type) != null)
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], type), null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

    }
}
