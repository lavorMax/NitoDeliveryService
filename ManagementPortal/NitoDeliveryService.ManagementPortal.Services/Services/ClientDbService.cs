using Microsoft.Data.SqlClient;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.Services
{
    public class ClientDbService : IClientDbService
    {
        private readonly PlaceDBServerOptions _options;

        public ClientDbService(PlaceDBServerOptions options)
        {
            _options = options;
        }

        public async Task CreateDb(int clientId)
        {
            string connectionString = _options.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string databaseName = $"ClientDB{clientId}";
                string createDatabaseQuery = $"CREATE DATABASE {databaseName}";
                using (SqlCommand command = new(createDatabaseQuery, connection))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task RemoveDb(int clientId)
        {
            string databaseName = $"ClientDB{clientId}";

            string connectionString = _options.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlCommand command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    object result = await command.ExecuteScalarAsync().ConfigureAwait(false);

                    if (result != null && int.TryParse(result.ToString(), out int databaseId))
                    {
                        using (SqlCommand deleteCommand = new SqlCommand($"DROP DATABASE [{databaseName}]", connection))
                        {
                            await deleteCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                        }
                    }
                    else
                    {
                        throw new Exception("Database don`t exist");
                    }
                }
            }
        }
    }
}
