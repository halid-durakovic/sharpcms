using System.Data;
using System.IO;
using System.Linq;
using Dapper;

namespace sharpcms.database.config.management
{
    public class DbProvider
    {
        private readonly DbPathProvider _pathProvider;

        public DbProvider() : this(new DbPathProvider())
        {
        }

        private DbProvider(DbPathProvider pathProvider)
        {
            _pathProvider = pathProvider;
        }

        public void Create(IDbConnection dbConnection, string dbName)
        {
            if (Exists(dbConnection, dbName)) return;

            var dbDataFilePath = _pathProvider.GetDbDataFilePath(dbName);

            tryToDeleteFile(dbDataFilePath);

            var dbLogFilePath = _pathProvider.GetDbLogFilePath(dbName);

            tryToDeleteFile(dbLogFilePath);

            var createDbCommand = getCreateDbCommand(dbName, dbDataFilePath, dbLogFilePath);

            dbConnection.Execute(createDbCommand);
        }

        public void Delete(IDbConnection dbConnection, string dbName)
        {
            if (!Exists(dbConnection, dbName)) return;

            var singleUserDbCommand = getSingleUserDbCommand(dbName);

            var deleteDbCommand = getDeleteDbCommand(dbName);

            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = $"{singleUserDbCommand};{deleteDbCommand};";

                dbCommand.ExecuteNonQuery();
            }
        }

        public bool Exists(IDbConnection dbConnection, string dbName)
        {
            var existsDbCommand = getExistsDbCommand(dbName);

            return dbConnection.Query<int>(existsDbCommand).First() == 1;
        }

        private void tryToDeleteFile(string dbFilePath)
        {
            if (File.Exists(dbFilePath))
            {
                try
                {
                    File.SetAttributes(dbFilePath, FileAttributes.Normal);

                    File.Delete(dbFilePath);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private string getCreateDbCommand(string dbName, string dbDataFilePath, string dbLogFilePath)
        {
            return $@"CREATE DATABASE [{dbName}]
                CONTAINMENT = NONE
                ON PRIMARY 
                ( 
                    NAME = N'{dbName}', 
                    FILENAME = N'{dbDataFilePath}', 
                    SIZE = 8192KB, 
                    MAXSIZE = UNLIMITED, 
                    FILEGROWTH = 65536KB 
                )
                LOG ON 
                ( 
                    NAME = N'{dbName}_log', 
                    FILENAME = N'{dbLogFilePath}', 
                    SIZE = 8192KB, 
                    MAXSIZE = 2048GB, 
                    FILEGROWTH = 65536KB 
                )";
        }

        private string getExistsDbCommand(string dbName)
        {
            return $@"IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE (name = N'{dbName}')))
                BEGIN
	                SELECT 1
                END
                ELSE
                BEGIN
	                SELECT 0
                END";
        }

        private string getDeleteDbCommand(string dbName)
        {
            return $"DROP DATABASE [{dbName}]";
        }

        private string getSingleUserDbCommand(string dbName)
        {
            return $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
        }

        private string getOfflineDbCommand(string dbName)
        {
            return $"ALTER DATABASE [{dbName}] SET OFFLINE WITH ROLLBACK IMMEDIATE;";
        }
    }
}