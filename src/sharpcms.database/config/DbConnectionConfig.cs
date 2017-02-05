namespace sharpcms.database.config
{
    public class DbConnectionConfig : IDbConnectionConfig
    {
        public string Name { get; set; } = null;

        public string ConnectionString { get; set; } = null;

        public string Provider { get; set; } = null;
    }
}