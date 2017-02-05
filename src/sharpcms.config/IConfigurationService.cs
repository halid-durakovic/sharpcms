namespace sharpcms.config
{
    public interface IConfigurationService
    {
        string Get(string name);

        T Get<T>(string alias = null) where T : new();
    }
}