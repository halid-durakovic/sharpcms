namespace sharpcms.database.config
{
    public interface IDbConnectionConfigService
    {
        IDbConnectionConfig GetConnectionConfig(string name);
        string GetConnectionConfigDbName(string name);
        IDbConnectionConfig GetMasterConnectionConfig(string name);
    }
}