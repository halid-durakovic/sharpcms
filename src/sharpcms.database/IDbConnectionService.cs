namespace sharpcms.database
{
    public interface IDbConnectionService
    {
        void CreateIfDoesNotExist(string name);
        void DeleteIfDoesExist(string name);
        DbConnection GetConnection(string name);
    }
}