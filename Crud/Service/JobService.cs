using Microsoft.Data.SqlClient;
using Crud.Contracts;
namespace Crud.Service
{
    public class JobService : IJobService
    {
        private readonly IConfiguration _configuration;

        public JobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task BackupDatabaseAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var builder = new SqlConnectionStringBuilder(_configuration.GetConnectionString("DefaultConnection"));

            // adjust db + backup path
            var databaseName = builder.InitialCatalog;
            var backupPath = $"C:\\Backups\\{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var commandText = $@"
            BACKUP DATABASE [{databaseName}]
            TO DISK = '{backupPath}'
            WITH FORMAT, INIT, 
            NAME = 'Full Backup of {databaseName}', 
            SKIP, NOREWIND, NOUNLOAD, STATS = 10";

            using var command = new SqlCommand(commandText, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
