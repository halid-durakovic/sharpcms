using System.Collections.Generic;
using System.Linq;

namespace sharpcms.database.config.parser
{
    public class DbConnectionParser
    {
        private readonly IDbConnectionConfig _config;

        public DbConnectionParser(IDbConnectionConfig config)
        {
            _config = config;
        }

        public Dictionary<string, string> ParseConnectionString()
        {
            var parsedValues = new Dictionary<string, string>();

            var splitNameValueParts = GetNameValueParts();

            foreach (var nameValuePart in splitNameValueParts)
            {
                var nameValueParts = nameValuePart.Split('=');

                var name = nameValueParts[0];

                var value = nameValueParts[1];

                parsedValues[name] = value;
            }

            return parsedValues;
        }

        public IDbConnectionConfig ParseConnectionConfig(Dictionary<string,string> values)
        {
            var dbConnectionString = string.Empty;

            foreach (var key in values.Keys)
            {
                var value = values[key];

                dbConnectionString += $"{key}={value};";
            }

            var parsedConfig = new DbConnectionConfig
            {
                Name = _config.Name,

                Provider = _config.Provider,

                ConnectionString = dbConnectionString
            };

            return parsedConfig;
        }

        private IEnumerable<string> GetNameValueParts()
        {
            return _config
                .ConnectionString
                .Split(';')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim())
                .ToArray();
        }
    }
}