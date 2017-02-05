using System;
using System.Data;
using sharpcms.database.config;

namespace sharpcms.database
{
    public class DbConnection : IDisposable
    {
        private readonly object _synchronise = new object();

        public DbConnection(IDbConnection connection, IDbConnectionConfig connectionConfig)
        {
            Disposing = false;

            Connection = connection;

            ConnectionConfig = connectionConfig;
        }


        private bool Disposing { get; set; }

        public IDbConnection Connection { get; private set; }

        public IDbConnectionConfig ConnectionConfig { get; }

        public void Dispose()
        {
            if (Disposing) return;

            lock (_synchronise)
            {
                if (Disposing) return;

                Disposing = true;

                Connection.Dispose();

                Connection = null;
            }
        }
    }
}