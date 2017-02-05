using System;
using System.Collections.Generic;

namespace sharpcms.database.config
{
    public class DbConnectionConfigNotFoundException : Exception
    {
        public DbConnectionConfigNotFoundException(string searchName) : base($"Could not find '{searchName}' but you can define something like '{getExampleCfg()}'.")
        {
        }

        public DbConnectionConfigNotFoundException(string searchName, IEnumerable<string> existingNames) : base($"Could not find '{searchName}' but you can use {string.Join(",", existingNames)}.")
        {
        }

        private static string getExampleCfg()
        {
            return @"{
  ""DbConfig"": {
    ""ConnectionStrings"": [
      {
        ""Name"": ""db1"",
        ""ConnectionString"": ""Server=localhost;Database=sharpcms.db.tests.db1;User Id=sharpcms; Password=password;MultipleActiveResultSets=true;"",
        ""Provider"": ""SqlServer""
      }
    ]
  }
}";
        }
    }
}