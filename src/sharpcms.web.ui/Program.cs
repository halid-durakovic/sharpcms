using sharpcms.startup;

namespace sharpcms.web.ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.Run<Startup>("http://0.0.0.0:5002", args);
        }
    }
}
