using System.Data.SqlClient;
using sharpcms.database.config;
using sharpcms.database.config.management;

namespace sharpcms.database
{
    public class DbConnectionService : IDbConnectionService
    {
        private readonly DbProvider _dbProvider;

        private readonly DbConnectionConfigService _configurationService;

        public DbConnectionService() : this(new DbConnectionConfigService(), new DbProvider())
        {
        }

        public DbConnectionService(DbConnectionConfigService configurationService, DbProvider dbProvider)
        {
            _configurationService = configurationService;

            _dbProvider = dbProvider;
        }

        public void CreateIfDoesNotExist(string name)
        {
            var originalDbName = _configurationService.GetConnectionConfigDbName(name);

            var masterDbConnectionConfig = _configurationService.GetMasterConnectionConfig(name);

            using (var dbConnection = new SqlConnection(masterDbConnectionConfig.ConnectionString))
            {
                dbConnection.Open();

                _dbProvider.Create(dbConnection, originalDbName);
            }
        }

        public void DeleteIfDoesExist(string name)
        {
            var originalDbName = _configurationService.GetConnectionConfigDbName(name);

            var masterDbConnectionConfig = _configurationService.GetMasterConnectionConfig(name);

            using (var dbConnection = new SqlConnection(masterDbConnectionConfig.ConnectionString))
            {
                dbConnection.Open();

                _dbProvider.Delete(dbConnection, originalDbName);
            }
        }

        public DbConnection GetConnection(string name)
        {
            var dbConnectionConfig = _configurationService.GetConnectionConfig(name);

            var sqlConnection = new SqlConnection(dbConnectionConfig.ConnectionString);

            var dbConnection = new DbConnection(sqlConnection, dbConnectionConfig);

            sqlConnection.Open();

            return dbConnection;
        }
        
    }
}