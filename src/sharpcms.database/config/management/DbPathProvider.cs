using System.IO;

namespace sharpcms.database.config.management
{
    public class DbPathProvider
    {
        public string GetDbDataFilePath(string dbName)
        {
            return Path.Combine(getApplicationBasePath(), $"{dbName}.mdf");
        }

        public string GetDbLogFilePath(string dbName)
        {
            return Path.Combine(getApplicationBasePath(), $"{dbName}_log.ldf");
        }

        private string getApplicationBasePath()
        {
            return System.AppContext.BaseDirectory;
        }
    }
}