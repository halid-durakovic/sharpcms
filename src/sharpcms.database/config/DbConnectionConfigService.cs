using System.Collections.Generic;
using System.Linq;
using sharpcms.config;
using sharpcms.database.config.parser;

namespace sharpcms.database.config
{
    public class DbConnectionConfigService : IDbConnectionConfigService
    {
        private readonly IConfigurationService _configurationService;

        public DbConnectionConfigService() : this(new ConfigurationService())
        {
        }

        public DbConnectionConfigService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IDbConnectionConfig GetConnectionConfig(string name)
        {
            var dbConnectionConfig = FindOrThrowIfDoesNotExist(name);

            return dbConnectionConfig;
        }

        public string GetConnectionConfigDbName(string name)
        {
            var dbConnectionConfig = FindOrThrowIfDoesNotExist(name);

            var dbConnectionParser = new DbConnectionParser(dbConnectionConfig);

            var dbConnectionConfigValues = dbConnectionParser.ParseConnectionString();

            return dbConnectionConfigValues["Database"];
        }

        public IDbConnectionConfig GetMasterConnectionConfig(string name)
        {
            var dbConnectionConfig = FindOrThrowIfDoesNotExist(name);

            var dbConnectionParser = new DbConnectionParser(dbConnectionConfig);

            var dbConnectionConfigValues = dbConnectionParser.ParseConnectionString();

            dbConnectionConfigValues["Database"] = "master";

            var masterDbConnectionConfig = dbConnectionParser.ParseConnectionConfig(dbConnectionConfigValues);

            return masterDbConnectionConfig;
        }

        private DbConnectionConfig FindOrThrowIfDoesNotExist(string name)
        {
            var dbConfig = _configurationService.Get<DbConfig>();

            if (dbConfig.ConnectionStrings.Any(x => x.Name == name))

                return dbConfig.ConnectionStrings.First(x => x.Name == name);

            ThrowBecauseItDoesNotExist(name, dbConfig.ConnectionStrings.Select(x => x.Name));

            return null;
        }

        private void ThrowBecauseItDoesNotExist(string name, IEnumerable<string> available)
        {
            if (!available.Any())

                throw new DbConnectionConfigNotFoundException(name);

            throw new DbConnectionConfigNotFoundException(name, available);
        }
    }
}