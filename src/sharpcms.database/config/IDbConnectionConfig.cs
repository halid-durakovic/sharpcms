namespace sharpcms.database.config
{
    public interface IDbConnectionConfig
    {
        string Name { get; set; }

        string ConnectionString { get; set; }

        string Provider { get; set; }
    }
}